using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Console = Colorful.Console;

namespace NotHotdog
{
    public class ComputerVision
    {
        /// <summary>
        /// Initialize client connection with Computer Vision API
        /// </summary>
        public static ComputerVisionClient Client { get; } = Connect.AuthenticateSession();

        /// <summary>
        /// Initialize List to store items from image features
        /// </summary>
        static List<VisualFeatureTypes?> features = new List<VisualFeatureTypes?>()
            {
                VisualFeatureTypes.Description, VisualFeatureTypes.Categories
            };
        
        // Declare string for analyzed image caption description
        static string imageDescription;

        // Declare variable that stores async GET response of image analysis
        static ImageAnalysis results;

        /// <summary>
        ///  Method to validate if url has an image header
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool IsRemoteImageUrl(string url)
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
        public static void AnalyzeRemoteImageHelper()
        {
            AnalyzeRemoteImage(Client).Wait();
        }

        /// <summary>
        /// Method to execute remote image analysis and store Computer Vision response
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static async Task AnalyzeRemoteImage(ComputerVisionClient client)
        {

            string url;
            string input;


            // Assign GET response list items if food image URL is finally validated
            while (true)
            {
                Console.Write("Enter/Paste food image full url (e.g. https://*.jpg): ");
                input = Console.ReadLine();

                if (IsRemoteImageUrl(input))
                {
                    url = input;
                    break;
                }

            }

            // Assign expression to invoke remote image analysis
            results = await client.AnalyzeImageAsync(url, features);

            // Assign image description caption
            imageDescription = results.Description.Captions[0].Text;

            System.Console.WriteLine("\nAnalyzing image...\n", Color.AntiqueWhite);
            Thread.Sleep(3000);
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
                    Console.WriteLine("Subject of image is not food.");
                    Thread.Sleep(3000);
                    Console.WriteLine($"You entered an image URL of {imageDescription}.", Color.LightGoldenrodYellow);
                    Thread.Sleep(3000);
                    Console.WriteLine("\n( u_u) Please enter an image URL of food.\n");
                    Thread.Sleep(1500);
                    AnalyzeRemoteImageHelper();
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