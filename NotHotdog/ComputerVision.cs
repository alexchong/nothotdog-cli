using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace NotHotdog
{
    public class ComputerVision
    {
        /// <summary>
        /// Initialize client connection with Computer Vision API
        /// </summary>
        public static ComputerVisionClient Client { get; } = Connect.AuthenticateSession();
        // Property for validated URL
        public static string ValidatedURL { get; private set; }
        // Property for image description caption
        public static string ImageDescription { get; private set; }

        /// <summary>
        /// Initialize List to store items from image features
        /// </summary>
        public static List<VisualFeatureTypes?> features = new List<VisualFeatureTypes?>()
            {
                VisualFeatureTypes.Description, VisualFeatureTypes.Categories
            };

        // Declare variable that stores async GET response of image analysis
        public static ImageAnalysis results;

        /// <summary>
        ///  Method to validate if url has an image header
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static bool IsRemoteImageUrl(string url)
        {
            try
            {
                // Initialize web request for url
                var request = (HttpWebRequest)WebRequest.Create(url);

                // Assign the web request method to GET HTTP header
                request.Method = "HEAD";

                // Invoke the GET method and return true if url header starts with `image/`
                using (var response = request.GetResponse())
                {
                    return response.ContentType.ToLower().StartsWith("image/", StringComparison.OrdinalIgnoreCase);
                }
            }
            catch (WebException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        /// <summary>
        /// Helper method to invoke remote image analysis w/ client connection
        /// </summary>
        public static void AnalyzeRemoteImageHelper(string url)
        {
            AnalyzeRemoteImage(Client, url).Wait();
        }

        /// <summary>
        /// Method to execute remote image analysis and store Computer Vision response
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private static async Task AnalyzeRemoteImage(ComputerVisionClient client, string url)
        {
            // Assign GET response list items if food image URL is finally validated
            if (IsRemoteImageUrl(url))
                ValidatedURL = url;
            else
                return;

            // Assign expression to invoke remote image analysis
            results = await client.AnalyzeImageAsync(ValidatedURL, features);

            // Assign image description caption
            ImageDescription = results.Description.Captions[0].Text;

        }

        /// <summary>
        /// Iterate through features list to check if remote image is food or not
        /// </summary>
        /// <returns></returns>
        public static bool IsRemoteImageCategory()
        {
            foreach (var category in results.Categories)
            {
                if (!category.Name.Contains("food"))
                {
                    // Recursively invoke `AnalyzeRemoteImage` to assist with 
                    AnalyzeRemoteImageHelper(ValidatedURL);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Check image description if it matches keyword (i.e. if the caption contains "hotdog")
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static bool IsRemoteImageDescription(string keyword)
        {

            // normalize description caption (e.g. "Hot dog" to "hotdog")
            var caption = Regex.Replace(results.Description.Captions[0].Text.ToLower(), @"\s+", "");
            
            // false negative benchmark
            var confidence = results.Description.Captions[0].Confidence;

            // normalize arg `keyword` to match `caption`
            keyword = Regex.Replace(keyword.ToLower(), @"\s+", "");

            if (caption.Contains(keyword) && confidence > 0.45)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

    }
}