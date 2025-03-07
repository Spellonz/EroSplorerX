using EroSplorerX.Data;
using EroSplorerX.Helpers;
using Microsoft.UI.Xaml.Controls;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;

namespace EroSplorerX.Views;

public sealed partial class VideoPage : Page
{
    public string? Source { get; set; }

    public VideoPage()
    {
        this.InitializeComponent();
        RegisterMediaPlayerEvents(VideoPlayer);
    }

    public void SetVideoPath(string path)
    {
        var eroPath = new EroPath(path);
        if (eroPath.HasVideo)
        {
            VideoPlayer.Source = MediaSource.CreateFromUri(new Uri(eroPath.VideoPath));

            _ = StartPlayingWithFunscript(eroPath);
        }
    }

    private void RegisterMediaPlayerEvents(MediaPlayerElement mediaPlayerElement)
    {
        if (mediaPlayerElement.MediaPlayer != null)
        {
            // Playback state change event
            mediaPlayerElement.MediaPlayer.PlaybackSession.PlaybackStateChanged += PlaybackStateChanged;

            // Position change event (e.g., seek)
            mediaPlayerElement.MediaPlayer.PlaybackSession.PositionChanged += PositionChanged;
        }
    }

    private void PlaybackStateChanged(MediaPlaybackSession sender, object args)
    {
        switch (sender.PlaybackState)
        {
            case MediaPlaybackState.None:
                //Console.WriteLine("Playback is not initialized or no media is loaded.");
                break;
            case MediaPlaybackState.Opening:
                //Console.WriteLine("Media is being opened.");
                break;
            case MediaPlaybackState.Buffering:
                //Console.WriteLine("Media is buffering.");
                break;
            case MediaPlaybackState.Playing:
                var currentTime = sender.Position.TotalMilliseconds;
                try 
                {
                    _ = AutoBlowHelper.SyncScriptStart(SystemConstants.AUTOBLOW_DEVICE_ID, currentTime);
                }
                catch (Exception ex)
                {
                    //ShowErrorInfoBar(ex.Message);
                    Log.Error(ex, "Failed to start AutoBlow script");
                }
                //ShowInfoInfoBar($"Resuming at {currentTime}");
                Console.WriteLine($"Playback is playing at {currentTime} ms");


                break;
            case MediaPlaybackState.Paused:
                var pauseScript = AutoBlowHelper.SyncScriptStop(SystemConstants.AUTOBLOW_DEVICE_ID);
                break;
        }
    }

    private void PositionChanged(MediaPlaybackSession sender, object args)
    {
        // Handle position change (e.g., seek)
        Console.WriteLine($"Playback position changed to: {sender.Position.TotalMilliseconds} ms");
    }

    public void UnregisterMediaPlayerEvents()
    {
        if (VideoPlayer.MediaPlayer != null)
        {
            VideoPlayer.MediaPlayer.PlaybackSession.PlaybackStateChanged -= PlaybackStateChanged;
            VideoPlayer.MediaPlayer.PlaybackSession.PositionChanged -= PositionChanged;
        }
    }

    private async Task StartPlayingWithFunscript(EroPath eroPath)
    {

        if (eroPath.HasFunscript)
        {
            var deviceId = SystemConstants.AUTOBLOW_DEVICE_ID;
            var status = await AutoBlowHelper.Connected(deviceId);
            if (status == null || !status.Connected)
            {
                ShowErrorInfoBar("AutoBlow not connected");
                return;
            }

            // Upload funscript
            var funscriptContent = await File.ReadAllBytesAsync(eroPath.FunScriptPath);
            if (funscriptContent == null || funscriptContent.Length == 0)
            {
                ShowErrorInfoBar("Failed to read funscript");
                return;
            }

            //ShowInfoInfoBar("Uploading funscript..");
            var uploadFunscript = await AutoBlowHelper.SyncScriptUploadFunscript(deviceId, funscriptContent);
            if (uploadFunscript == null)
            {
                ShowErrorInfoBar("Failed to upload funscript");
                return;
            }

            // Get the script token from the response
            var token = uploadFunscript.SyncScriptToken;
            if (string.IsNullOrEmpty(token))
            { 
                ShowErrorInfoBar("Failed to upload funscript");
                return; 
            }

            //ShowSuccessInfoBar("Funscript uploaded");
            //ShowInfoInfoBar("Loading funscript..");

            // Load the token
            var loadToken = await AutoBlowHelper.SyncScriptLoadToken(deviceId, token);
            //ShowSuccessInfoBar("Funscript loaded");

            VideoPlayer.MediaPlayer.Play();

        }
    }

    #region InfoBar Methods

    public void ShowInfoInfoBar(string message, int time = SystemConstants.INFOBAR_DURATION_VERY_SHORT)
    {
        MainInfoBar.Title = "Info";
        MainInfoBar.Severity = InfoBarSeverity.Informational;
        ShowInfoBarTemporarily(message, time);
    }

    public void ShowSuccessInfoBar(string message, int time = SystemConstants.INFOBAR_DURATION_VERY_SHORT)
    {
        MainInfoBar.Title = "Success";
        MainInfoBar.Severity = InfoBarSeverity.Success;
        ShowInfoBarTemporarily(message, time);
    }

    public void ShowWarningInfoBar(string message, int time = SystemConstants.INFOBAR_DURATION_VERY_SHORT)
    {
        MainInfoBar.Title = "Warning";
        MainInfoBar.Severity = InfoBarSeverity.Warning;
        ShowInfoBarTemporarily(message, time);
    }

    public void ShowErrorInfoBar(string message, int time = SystemConstants.INFOBAR_DURATION_VERY_SHORT)
    {
        MainInfoBar.Title = "Error";
        MainInfoBar.Severity = InfoBarSeverity.Error;
        ShowInfoBarTemporarily(message, time);
    }

    private async void ShowInfoBarTemporarily(string message, int time = SystemConstants.INFOBAR_DURATION_VERY_SHORT)
    {
        MainInfoBar.Message = message;
        MainInfoBar.IsOpen = true;

        await Task.Delay(time);
        FadeOutInfoBar.Begin();
    }

    private void MainInfoBar_CloseButtonClick(InfoBar sender, object args)
    {
        MainInfoBar.IsOpen = false;
        FadeOutInfoBar.Stop();
    }

    private void FadeOutInfoBar_Completed(object sender, object e)
    {
        MainInfoBar.IsOpen = false;
        MainInfoBar.Opacity = 1;
    }

    #endregion
}
