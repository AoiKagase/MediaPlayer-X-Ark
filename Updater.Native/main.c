#define UNICODE
#define _UNICODE

#include <windows.h>
#include <wchar.h>
#include <stdio.h>
#include <stdarg.h>
#include <stdbool.h>

#define PATH_BUFFER_LEN 32768

typedef struct Options
{
	DWORD pid;
	wchar_t appDir[PATH_BUFFER_LEN];
	wchar_t sourceDir[PATH_BUFFER_LEN];
	wchar_t exeName[MAX_PATH];
	wchar_t logPath[PATH_BUFFER_LEN];
	wchar_t updateZipPath[PATH_BUFFER_LEN];
} Options;

static void ResetLog(const wchar_t* logPath);
static void AppendLog(const wchar_t* logPath, const wchar_t* format, ...);
static bool ParseOptions(int argc, wchar_t** argv, Options* options);
static bool WaitForTargetExit(const Options* options);
static bool ResolvePayloadDirectory(const Options* options, wchar_t* payloadDir, size_t payloadDirLen);
static bool FindExeDirectoryRecursive(const wchar_t* rootDir, const wchar_t* exeName, wchar_t* foundDir, size_t foundDirLen);
static bool CopyDirectoryRecursive(const Options* options, const wchar_t* sourceDir, const wchar_t* destinationDir);
static bool DeleteDirectoryRecursive(const wchar_t* path);
static bool LaunchMainApplication(const Options* options);
static void CombinePath(wchar_t* buffer, size_t bufferLen, const wchar_t* left, const wchar_t* right);
static bool IsDotDirectory(const wchar_t* name);

int wmain(int argc, wchar_t** argv)
{
	Options options = { 0 };
	wchar_t payloadDir[PATH_BUFFER_LEN];

	if (!ParseOptions(argc, argv, &options))
		return 1;

	ResetLog(options.logPath);
	AppendLog(options.logPath, L"updater start");
	AppendLog(options.logPath, L"pid=%lu", options.pid);
	AppendLog(options.logPath, L"appDir=%ls", options.appDir);
	AppendLog(options.logPath, L"sourceDir=%ls", options.sourceDir);

	if (!WaitForTargetExit(&options))
		return 1;

	if (!ResolvePayloadDirectory(&options, payloadDir, _countof(payloadDir)))
		return 1;

	AppendLog(options.logPath, L"payloadDir=%ls", payloadDir);

	if (!CopyDirectoryRecursive(&options, payloadDir, options.appDir))
		return 1;

	AppendLog(options.logPath, L"copy complete");

	if (!DeleteDirectoryRecursive(options.sourceDir))
		AppendLog(options.logPath, L"cleanup warning: failed to remove _update directory");

	if (DeleteFileW(options.updateZipPath))
		AppendLog(options.logPath, L"removed _update.zip");

	if (!LaunchMainApplication(&options))
		return 1;

	AppendLog(options.logPath, L"updater complete");
	return 0;
}

static void ResetLog(const wchar_t* logPath)
{
	FILE* file = NULL;
	_wfopen_s(&file, logPath, L"w, ccs=UTF-8");
	if (file != NULL)
		fclose(file);
}

static void AppendLog(const wchar_t* logPath, const wchar_t* format, ...)
{
	FILE* file = NULL;
	SYSTEMTIME st;
	va_list args;

	_wfopen_s(&file, logPath, L"a, ccs=UTF-8");
	if (file == NULL)
		return;

	GetLocalTime(&st);
	fwprintf(file,
		L"[%04d/%02d/%02d %02d:%02d:%02d.%03d] ",
		st.wYear, st.wMonth, st.wDay,
		st.wHour, st.wMinute, st.wSecond, st.wMilliseconds);

	va_start(args, format);
	vfwprintf(file, format, args);
	va_end(args);

	fwprintf(file, L"\n");
	fclose(file);
}

static bool ParseOptions(int argc, wchar_t** argv, Options* options)
{
	int i;

	if (options == NULL)
		return false;

	options->pid = 0;
	options->appDir[0] = L'\0';
	options->sourceDir[0] = L'\0';
	wcscpy_s(options->exeName, _countof(options->exeName), L"x-ark.exe");

	for (i = 1; i < argc; i++)
	{
		if (wcscmp(argv[i], L"--pid") == 0 && i + 1 < argc)
		{
			options->pid = (DWORD)_wtoi(argv[++i]);
		}
		else if (wcscmp(argv[i], L"--app-dir") == 0 && i + 1 < argc)
		{
			wcsncpy_s(options->appDir, _countof(options->appDir), argv[++i], _TRUNCATE);
		}
		else if (wcscmp(argv[i], L"--source-dir") == 0 && i + 1 < argc)
		{
			wcsncpy_s(options->sourceDir, _countof(options->sourceDir), argv[++i], _TRUNCATE);
		}
		else if (wcscmp(argv[i], L"--exe-name") == 0 && i + 1 < argc)
		{
			wcsncpy_s(options->exeName, _countof(options->exeName), argv[++i], _TRUNCATE);
		}
	}

	if (options->pid == 0 || options->appDir[0] == L'\0' || options->sourceDir[0] == L'\0')
		return false;

	CombinePath(options->logPath, _countof(options->logPath), options->appDir, L"_update_apply.log");
	CombinePath(options->updateZipPath, _countof(options->updateZipPath), options->appDir, L"_update.zip");
	return true;
}

static bool WaitForTargetExit(const Options* options)
{
	HANDLE processHandle = OpenProcess(SYNCHRONIZE, FALSE, options->pid);
	DWORD waitResult;

	if (processHandle == NULL)
	{
		AppendLog(options->logPath, L"target process already exited");
		return true;
	}

	AppendLog(options->logPath, L"waiting for pid=%lu to exit", options->pid);
	waitResult = WaitForSingleObject(processHandle, 60000);
	CloseHandle(processHandle);

	if (waitResult == WAIT_OBJECT_0)
		return true;

	AppendLog(options->logPath, L"wait failed: %lu", waitResult);
	return false;
}

static bool ResolvePayloadDirectory(const Options* options, wchar_t* payloadDir, size_t payloadDirLen)
{
	wchar_t directExePath[PATH_BUFFER_LEN];

	CombinePath(directExePath, _countof(directExePath), options->sourceDir, options->exeName);
	if (GetFileAttributesW(directExePath) != INVALID_FILE_ATTRIBUTES)
	{
		wcsncpy_s(payloadDir, payloadDirLen, options->sourceDir, _TRUNCATE);
		return true;
	}

	if (FindExeDirectoryRecursive(options->sourceDir, options->exeName, payloadDir, payloadDirLen))
		return true;

	AppendLog(options->logPath, L"payload resolve failed: %ls not found", options->exeName);
	return false;
}

static bool FindExeDirectoryRecursive(const wchar_t* rootDir, const wchar_t* exeName, wchar_t* foundDir, size_t foundDirLen)
{
	WIN32_FIND_DATAW findData;
	wchar_t searchPattern[PATH_BUFFER_LEN];
	wchar_t candidatePath[PATH_BUFFER_LEN];
	HANDLE findHandle;

	CombinePath(searchPattern, _countof(searchPattern), rootDir, L"*");
	findHandle = FindFirstFileW(searchPattern, &findData);
	if (findHandle == INVALID_HANDLE_VALUE)
		return false;

	do
	{
		if (IsDotDirectory(findData.cFileName))
			continue;

		CombinePath(candidatePath, _countof(candidatePath), rootDir, findData.cFileName);

		if ((findData.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY) != 0)
		{
			wchar_t directExePath[PATH_BUFFER_LEN];

			CombinePath(directExePath, _countof(directExePath), candidatePath, exeName);
			if (GetFileAttributesW(directExePath) != INVALID_FILE_ATTRIBUTES)
			{
				wcsncpy_s(foundDir, foundDirLen, candidatePath, _TRUNCATE);
				FindClose(findHandle);
				return true;
			}

			if (FindExeDirectoryRecursive(candidatePath, exeName, foundDir, foundDirLen))
			{
				FindClose(findHandle);
				return true;
			}
		}
	} while (FindNextFileW(findHandle, &findData));

	FindClose(findHandle);
	return false;
}

static bool CopyDirectoryRecursive(const Options* options, const wchar_t* sourceDir, const wchar_t* destinationDir)
{
	WIN32_FIND_DATAW findData;
	wchar_t searchPattern[PATH_BUFFER_LEN];
	wchar_t sourcePath[PATH_BUFFER_LEN];
	wchar_t destinationPath[PATH_BUFFER_LEN];
	HANDLE findHandle;

	CombinePath(searchPattern, _countof(searchPattern), sourceDir, L"*");
	findHandle = FindFirstFileW(searchPattern, &findData);
	if (findHandle == INVALID_HANDLE_VALUE)
	{
		AppendLog(options->logPath, L"copy failed: unable to enumerate %ls", sourceDir);
		return false;
	}

	do
	{
		if (IsDotDirectory(findData.cFileName))
			continue;

		CombinePath(sourcePath, _countof(sourcePath), sourceDir, findData.cFileName);
		CombinePath(destinationPath, _countof(destinationPath), destinationDir, findData.cFileName);

		if ((findData.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY) != 0)
		{
			CreateDirectoryW(destinationPath, NULL);
			if (!CopyDirectoryRecursive(options, sourcePath, destinationPath))
			{
				FindClose(findHandle);
				return false;
			}
		}
		else
		{
			if (_wcsicmp(findData.cFileName, L"updater.exe") == 0)
			{
				AppendLog(options->logPath, L"skip updater.exe");
				continue;
			}

			AppendLog(options->logPath, L"copy %ls", findData.cFileName);
			if (!CopyFileW(sourcePath, destinationPath, FALSE))
			{
				AppendLog(options->logPath, L"copy failed: %ls (%lu)", destinationPath, GetLastError());
				FindClose(findHandle);
				return false;
			}
		}
	} while (FindNextFileW(findHandle, &findData));

	FindClose(findHandle);
	return true;
}

static bool DeleteDirectoryRecursive(const wchar_t* path)
{
	WIN32_FIND_DATAW findData;
	wchar_t searchPattern[PATH_BUFFER_LEN];
	wchar_t childPath[PATH_BUFFER_LEN];
	HANDLE findHandle;

	if (GetFileAttributesW(path) == INVALID_FILE_ATTRIBUTES)
		return true;

	CombinePath(searchPattern, _countof(searchPattern), path, L"*");
	findHandle = FindFirstFileW(searchPattern, &findData);
	if (findHandle == INVALID_HANDLE_VALUE)
		return RemoveDirectoryW(path) != 0;

	do
	{
		if (IsDotDirectory(findData.cFileName))
			continue;

		CombinePath(childPath, _countof(childPath), path, findData.cFileName);
		if ((findData.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY) != 0)
		{
			if (!DeleteDirectoryRecursive(childPath))
			{
				FindClose(findHandle);
				return false;
			}
		}
		else
		{
			SetFileAttributesW(childPath, FILE_ATTRIBUTE_NORMAL);
			if (!DeleteFileW(childPath))
			{
				FindClose(findHandle);
				return false;
			}
		}
	} while (FindNextFileW(findHandle, &findData));

	FindClose(findHandle);
	SetFileAttributesW(path, FILE_ATTRIBUTE_NORMAL);
	return RemoveDirectoryW(path) != 0;
}

static bool LaunchMainApplication(const Options* options)
{
	STARTUPINFOW startupInfo;
	PROCESS_INFORMATION processInfo;
	wchar_t appPath[PATH_BUFFER_LEN];
	wchar_t commandLine[PATH_BUFFER_LEN];

	ZeroMemory(&startupInfo, sizeof(startupInfo));
	ZeroMemory(&processInfo, sizeof(processInfo));
	startupInfo.cb = sizeof(startupInfo);

	CombinePath(appPath, _countof(appPath), options->appDir, options->exeName);
	swprintf_s(commandLine, _countof(commandLine), L"\"%ls\"", appPath);

	if (!CreateProcessW(
		appPath,
		commandLine,
		NULL,
		NULL,
		FALSE,
		0,
		NULL,
		options->appDir,
		&startupInfo,
		&processInfo))
	{
		AppendLog(options->logPath, L"launch failed: %ls (%lu)", appPath, GetLastError());
		return false;
	}

	CloseHandle(processInfo.hThread);
	CloseHandle(processInfo.hProcess);
	AppendLog(options->logPath, L"launch %ls", appPath);
	return true;
}

static void CombinePath(wchar_t* buffer, size_t bufferLen, const wchar_t* left, const wchar_t* right)
{
	if (left == NULL || left[0] == L'\0')
	{
		wcsncpy_s(buffer, bufferLen, right, _TRUNCATE);
		return;
	}

	swprintf_s(buffer, bufferLen, L"%ls\\%ls", left, right);
}

static bool IsDotDirectory(const wchar_t* name)
{
	return wcscmp(name, L".") == 0 || wcscmp(name, L"..") == 0;
}
