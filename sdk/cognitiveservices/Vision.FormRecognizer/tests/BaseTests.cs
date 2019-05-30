using Microsoft.Azure.CognitiveServices.FormRecognizer;
using Microsoft.Azure.Test.HttpRecorder;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace FormRecognizerSDK.Tests
{
    public abstract class BaseTests
    {
        public static bool IsTestTenant = false;
        private static readonly string FormRecognizerSubscriptionKey;
        private static Lazy<string> TestBaseUrl = new Lazy<string>(() =>
        {
            string user = "Azure";
            const string branch = "psSdkJson6";

            if (HttpMockServer.Mode == HttpRecorderMode.Record)
            {
                // The test images are checked in to git, so we see if can determine the URL to these.
                // To allow for those working in forks to also test, we try and figure out the base URL using git.
                // When all else fails, fall back to master.

                try
                {
                    var git = new Process();
                    git.StartInfo.FileName = "git";
                    git.StartInfo.Arguments = "config --local --list";
                    git.StartInfo.RedirectStandardOutput = true;
                    git.StartInfo.CreateNoWindow = true;
                    git.Start();
                    var stdout = git.StandardOutput.ReadToEnd();
                    git.WaitForExit();

                    var configRegex = new Regex(@"remote\.origin\.url=https://github.com/(\S*)/azure-sdk-for-net.git");
                    var userMatch = configRegex.Match(stdout);
                    if (userMatch.Success)
                    {
                        user = userMatch.Groups[1].Value;
                    }
                }
                catch
                {
                    /* ignore exceptions, fall back to default */
                }
            }

            return $"https://raw.githubusercontent.com/{user}/azure-sdk-for-net/{branch}/src/SDKs/CognitiveServices/dataPlane/Vision/ComputerVision/ComputerVision.Tests/TestImages/";
        });

        static BaseTests()
        {
            // Retrieve the configuration information.
            FormRecognizerSubscriptionKey = "";
        }

        protected IFormRecognizerClient GetFormRecognizerClient(params DelegatingHandler[] handler)
        {
            IFormRecognizerClient client = new FormRecognizerClient(new ApiKeyServiceClientCredentials(FormRecognizerSubscriptionKey), handlers: handler)
            {
                Endpoint = @"https://westus.api.cognitive.microsoft.com"
            };

            return client;
        }

        /// <summary>
        /// Return the relative path to the local test image, for stream-based APIs.
        /// </summary>
        protected string GetTestImagePath(string name)
        {
            return Path.Combine("TestImages", name);
        }

        /// <summary>
        /// Return the URL for the test image, for URL-based APIs.
        /// The URL is for the project test image served out of github.
        /// </summary>
        protected string GetTestImageUrl(string name)
        {
            return TestBaseUrl.Value + name;
        }
    }
}
