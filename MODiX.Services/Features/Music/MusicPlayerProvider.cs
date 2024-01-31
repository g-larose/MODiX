using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using YoutubeExplode;
using YoutubeExplode.Exceptions;
using YoutubeExplode.Videos;

namespace MODiX.Services.Features.Music
{
    public class MusicPlayerProvider
    {
        public async Task PlayVideoAsync()
        {
            string youtubeUrl = "https://www.youtube.com/watch?v=LVhJy-CR64Q";
            // Get the audio stream URL using YouTubeExplode
            var audioStreamUrl = await GetYouTubeAudioStreamUrl(youtubeUrl);

            if (audioStreamUrl != null)
            {
                // Play the audio stream using NAudio
                PlayAudioStream(audioStreamUrl);
            }
        }

        static async Task<string> GetYouTubeAudioStreamUrl(string youtubeUrl)
        {
            var youtube = new YoutubeClient();
            var video = await youtube.Videos.GetAsync(youtubeUrl);
            

            var streamInfoSet = await youtube.Videos.Streams.GetManifestAsync(video.Id);
            var audioStreamInfo = streamInfoSet.GetAudioOnlyStreams().FirstOrDefault();

            if (audioStreamInfo != null)
            {
                return audioStreamInfo.Url;
            }

            return null;
        }

        static void PlayAudioStream(string audioStreamUrl)
        {
            var ffmpegPath = "ffmpeg"; // Assuming ffmpeg is in the system PATH

            using (var process = new Process())
            {
                process.StartInfo.FileName = ffmpegPath;
                process.StartInfo.Arguments = $"-i {audioStreamUrl} -acodec pcm_s16le -ar 44100 -ac 2 -f wav pipe:1";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                process.OutputDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        // Handle the audio stream data (e.g., play or process it)

                        Console.WriteLine(e.Data);
                    }
                };

                process.ErrorDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        // Handle error messages
                        Console.WriteLine($"Error: {e.Data}");
                    }
                };

                process.Start();

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                
                // Wait for the process to exit
                process.WaitForExit();
            }
        }

    }
}
