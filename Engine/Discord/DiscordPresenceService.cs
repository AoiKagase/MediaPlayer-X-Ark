using DiscordRPC;
using MediaPlayer_X_Ark.Engine.Player;
using System;
using System.IO;

namespace MediaPlayer_X_Ark.Engine.Discord
{
    /// <summary>
    /// Discord Rich Presence を管理するサービス。
    /// PlayerController のイベントを購読して状態を更新する。
    /// </summary>
    public class DiscordPresenceService : IDisposable
    {
        private DiscordRpcClient _client;
        private readonly PlayerController _controller;
        private bool _enabled;
        private bool _disposed = false;
        private string _applicationId;
        public bool Enabled
        {
            get => _enabled;
            set
            {
                if (_enabled == value) return;
                _enabled = value;
                if (_enabled) Start();
                else Stop();
            }
        }

        public DiscordPresenceService(PlayerController controller, string applicationId)
        {
            _applicationId = applicationId;
            _controller = controller;
            _controller.TrackChanged += OnTrackChanged;
            _controller.PlaybackStateChanged += OnPlaybackStateChanged;
        }

        private void Start()
        {
            if (string.IsNullOrEmpty(_applicationId)) return;
            _client = new DiscordRpcClient(_applicationId);
            _client.Initialize();
            UpdatePresence();
        }

        private void Stop()
        {
            if (_client == null) return;
            _client.ClearPresence();
            _client.Dispose();
            _client = null;
        }

        private void OnTrackChanged(int index)
        {
            if (!_enabled) return;
            UpdatePresence();
        }

        private void OnPlaybackStateChanged()
        {
            if (!_enabled) return;
            UpdatePresence();
        }

        private void UpdatePresence()
        {
            if (_client == null || !_client.IsInitialized) return;

            // 停止中
            if (!_controller.IsPlaying && _controller.PlayingIndex < 0)
            {
                _client.ClearPresence();
                return;
            }

            string title = string.Empty;
            string artist = string.Empty;
            string album = string.Empty;

            int idx = _controller.PlayingIndex;
            if (idx >= 0 && idx < _controller.Engine.PlayList.Count)
            {
                var entry = _controller.Engine.PlayList[idx];
                title = !string.IsNullOrEmpty(entry.Title)
                    ? entry.Title
                    : Path.GetFileNameWithoutExtension(entry.FileName);
                artist = entry.Artist ?? string.Empty;
                album = entry.Album ?? string.Empty;
            }

            string details = string.IsNullOrEmpty(artist)
                ? title
                : $"{title} - {artist}";
            string state = string.IsNullOrEmpty(album)
                ? (_controller.IsPlaying ? "再生中" : "一時停止中")
                : $"{album}  |  {(_controller.IsPlaying ? "再生中" : "一時停止中")}";

            _client.SetPresence(new RichPresence
            {
                Details = Truncate(details, 128),
                State = Truncate(state, 128),
                Timestamps = _controller.IsPlaying
                    ? Timestamps.Now
                    : null,
                Assets = new Assets
                {
                    LargeImageKey = "x-ark",   // Developer Portal で設定した画像キー
                    LargeImageText = "MediaPlayer X-Ark",
                },
            });
        }

        /// <summary>Discord の文字数制限（最低2文字・最大128文字）に合わせる</summary>
        private static string Truncate(string s, int max)
        {
            if (string.IsNullOrEmpty(s)) return "―";
            return s.Length <= max ? s : s.Substring(0, max);
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            _controller.TrackChanged -= OnTrackChanged;
            _controller.PlaybackStateChanged -= OnPlaybackStateChanged;
            Stop();
        }
    }
}