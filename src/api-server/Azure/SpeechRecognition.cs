using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace TwentyFiveHours.API.Azure
{
    public class SpeechRecognitionWrapper
    {
        private readonly SpeechConfig _config;

        public SpeechRecognitionWrapper(string key, string region)
        {
            this._config = SpeechConfig.FromSubscription(key, region);
        }

        public async Task<string> RecognizeIntoFile(string audioPath)
        {
            var stop = new TaskCompletionSource<int>();

            var output = Path.GetRandomFileName() + ".txt";

            using (var outStream = new StreamWriter(output, false))
            using (var input = AudioConfig.FromWavFileInput(audioPath))
            using (var recognizer = new SpeechRecognizer(this._config, input))
            {
                recognizer.Recognized += (s, e) =>
                {
                    System.Diagnostics.Debug.WriteLine("something is here...?");
                    if (e.Result.Reason == ResultReason.RecognizedSpeech)
                    {
                        System.Diagnostics.Debug.WriteLine("it's some sort of speech: {}", e.Result.Text);
                        outStream.Write(e.Result.Text);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("something: {}", e.Result.Reason);
                    }
                };

                recognizer.Canceled += (s, e) =>
                {
                    System.Diagnostics.Debug.WriteLine($"Cancelled: {e.ErrorDetails}");
                    stop.TrySetResult(0);
                };

                recognizer.SessionStarted += (s, e) => System.Diagnostics.Debug.WriteLine("Started recognizing");

                recognizer.SessionStopped += (s, e) => stop.TrySetResult(0);

                await recognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);

                Task.WaitAny(new[] { stop.Task });

                await recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);
                System.Diagnostics.Debug.WriteLine("alright finished recognizing shits here");
            }

            return output;
        }
    }
}
