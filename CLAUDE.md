# 個人設定

## コーディングスタイル
- 2スペースインデント
- シングルクォート優先
- セミコロン省略

## 言語設定
- 全ての応答は日本語で

# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

MediaPlayer X-Ark is a Windows desktop media player built with C# (.NET 9.0-windows7.0) and Windows Forms, using the FMOD audio engine as its core. The author is Aoi.Kagase.

## Build Commands

```bash
# Build (Release)
dotnet build "MediaPlayer X-Ark.csproj" -c Release

# Build (Debug)
dotnet build "MediaPlayer X-Ark.csproj" -c Debug

# Publish as single file
dotnet publish "MediaPlayer X-Ark.csproj" -c Release -r win-x64
```

There are no automated tests or linting tools configured. Open `MediaPlayer X-Ark.sln` in Visual Studio 2022+ for IDE development.

**Post-build**: Native DLLs (`fmod.dll`, `NFluidsynth.dll`, `ATL.dll`, `DiscordRPC.dll`) are automatically moved to a `Libs/` subfolder by a post-build target in the `.csproj`.

## Architecture

The app uses a layered architecture:

```
Forms (UI) → PlayerController → IPlayerEngine (PlayerEngine) → FMOD (native)
```

### Core Layers

**Forms** (`Forms/`) — Windows Forms UI. Each form subscribes to events from `PlayerController` for reactive updates. Cross-thread marshaling is done via `SynchronizationContext.Post()`.

**PlayerController** (`Engine/Player/PlayerController.cs`) — High-level API between UI and the audio engine. Publishes events: `TrackChanged`, `PlaybackStateChanged`, `WaveformReady`, `ErrorOccurred`. Manages NonStopMix crossfade timing via a 5ms precision timer.

**PlayerEngine** (`Engine/Player/PlayerEngine.cs`, ~1265 lines) — Core FMOD wrapper. Handles all audio playback, ReplayGain normalization, AB-repeat, sleep timer, shuffle queue, and plugin loading.

**Effector System** (`Engine/Effector/`) — Audio DSP effects. `AbstractEffectorBase` is the base class; all effects inherit from it and bridge to FMOD DSP via `FmodDspBridge`. The `Effectors` container manages all active effects. Effects support preset save/load.

**Configuration** (`Engine/Config/`) — JSON-based settings persistence. `Configuration` reads/writes `ConfigurationData`, which has sections for audio output, effects, playback, display, and network (CDDB). `SupportedFormats` tracks built-in FMOD formats plus dynamically loaded codec plugins.

**Skin System** (`Engine/Skin/`) — XML-based UI theming via `.xsk` packages. `NewSkinSystem` is the current format; `OldSkinSystem` is legacy. `SkinApplicator` applies a loaded skin across all forms.

**Visualize** (`Engine/Visualize/`) — `FmodSpectrum` drives FFT spectrum analysis; `WaveformAnalyzer` generates the full-track waveform. `SpectrumAnalyzer` is a custom double-buffered WinForms control that renders on a dedicated draw thread.

**CD Support** (`Engine/CD/`) — Win32 IOCTL-based raw CD reading (`CDReader`) plus CDDB protocol client (`CddbClient`) for online metadata.

**Discord** (`Engine/Discord/DiscordPresenceService.cs`) — Discord Rich Presence integration showing now-playing info.

### Key Interfaces

| Interface | Implementation | Purpose |
|-----------|---------------|---------|
| `IPlayerEngine` | `PlayerEngine` | All playback operations |
| `IConfigService` | `Configuration` | Settings read/write |
| `IEffector` | `AbstractEffectorBase` subclasses | DSP effects |
| `INewSkinSystem` | `NewSkinSystem` | UI theming |

### Threading Model

- **UI thread (STA)**: All WinForms controls
- **Draw thread**: `SpectrumAnalyzer` rendering loop
- **Timer thread**: `PlayerController._preciseTimer` for NonStopMix (5ms)
- Always marshal back to UI thread via `SynchronizationContext.Post()` in event handlers

### Custom Controls (`Controls/`)

- `SpectrumAnalyzer` — Double-buffered spectrum with snow-block effect
- `CustomSlider`, `ColorSlider` — Skinnable sliders
- `Knob` — Rotary knob control
- `ScrollLabel` — Auto-scrolling text for long track names

### Assembly Loading

`StartUp.cs` handles custom assembly resolution so that DLLs in the `Libs/` subfolder are found at runtime. FMOD codec plugins are loaded dynamically and registered via `SupportedFormats.RegisterLoadedCodec()`.

## Key Dependencies

- **FMOD** (`fmod.dll`) — Core audio engine (native, bundled in `Resources/`)
- **FluidSynth** / `nfluidsynth` — MIDI playback with configurable soundfonts
- **ATL** (`z440.atl.core`) — Audio tag reading/writing
- **DiscordRichPresence** — Discord status integration
