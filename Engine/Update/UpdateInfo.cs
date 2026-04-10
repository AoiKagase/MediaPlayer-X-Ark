using System;

namespace MediaPlayer_X_Ark.Engine.Update
{
	public class UpdateInfo
	{
		public string Version { get; set; }
		public string ReleaseDate { get; set; }
		public string ReleaseNotes { get; set; }
		public string DownloadUrl { get; set; }

		public bool IsNewerThan(Version current)
		{
			if (!System.Version.TryParse(Version, out var remote))
				return false;
			return remote > current;
		}
	}
}
