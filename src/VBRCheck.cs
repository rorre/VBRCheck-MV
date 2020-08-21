using MapsetParser.objects;
using MapsetParser.statics;
using MapsetVerifierFramework.objects;
using MapsetVerifierFramework.objects.attributes;
using MapsetVerifierFramework.objects.metadata;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using FFmpeg.NET;

namespace VBRCheck
{
    [Check]
    public class CheckVBRQuality : GeneralCheck
    {
        public override CheckMetadata GetMetadata() => new BeatmapCheckMetadata()
        {
            Category = "Audio",
            Message = "Too high VBR quality settings.",
            Author = "-Keitaro",

            Documentation = new Dictionary<string, string>()
            {
                {
                    "Purpose",
                    @"
                    Preventing audio quality from being too high to save file size."
                },
                {
                    "Reasoning",
                    @"
                    TODO: This."
                }
            }
        };

        public override Dictionary<string, IssueTemplate> GetTemplates()
        {
            return new Dictionary<string, IssueTemplate>()
            {
                { "Quality Setting",
                    new IssueTemplate(Issue.Level.Problem,
                        "\"{0}\" current VBR quality setting is {1}.",
                        "path", "vbr quality setting")
                    .WithCause(
                        "The VBR quality settings should be 6 or lower.") }
            };
        }

        private MetaData GetAudioMetadata(string audioPath)
        {
            // https://github.com/Naxesss/MapsetVerifierBackend/blob/master/Program.cs#L22-L24
            string appdataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                appdataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            string ffmpegPath = Path.Combine(appdataPath, "Mapset Verifier Externals", "ffmpeg", "ffmpeg.exe");
            Engine ffmpeg = new Engine(ffmpegPath);
            MediaFile audioFile = new MediaFile(audioPath);

            var task = Task.Run(async () => await ffmpeg.GetMetaDataAsync(audioFile));
            return task.Result;
        }

        public override IEnumerable<Issue> GetIssues(BeatmapSet beatmapSet)
        {
            if (beatmapSet.GetAudioFilePath() == null)
                yield break;

            string audioPath = beatmapSet.GetAudioFilePath();
            // Maybe should also check if its an actual vorbis or not.
            // Later though.
            if (!audioPath.EndsWith(".ogg"))
                yield break;
            string audioRelPath = PathStatic.RelativePath(audioPath, beatmapSet.songPath);

            MetaData metadata = GetAudioMetadata(audioPath);
            int bitrate = metadata.AudioData.BitRateKbs;
            if (bitrate > 192)
            {
                int quality = 0;
                if (bitrate < 256)
                    quality = bitrate / 32;
                else
                    quality = bitrate / 64 + 4;
                yield return new Issue(GetTemplate("Quality Setting"), null, audioRelPath, quality);
            }

        }
    }
}
