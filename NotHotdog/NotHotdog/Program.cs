using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace NotHotdog
{
    class App
    {
        //public enum Hotdog
        //{
        //    Hotdog,
        //    NotHotdog
        //}

        public static string[] SubscriptionKey { get; private set; } = new string[2];

        /// <summary>
        /// Read in text file with Computer Vision API key/endpoint
        /// </summary>
        public static void ReadSubscriptionKey()
        {

            // TODO: Retrieve path  based on user directory/environment variables
            string filePath = @"C:\Users\Alex\source\repos\NotHotdog\NotHotdog\key.txt";

            using (StreamReader sr = File.OpenText(filePath))
            {
                string line = "";
                int count = 0;
                while ((line = sr.ReadLine()) != null && count < 2)
                {
                    SubscriptionKey[count] = line;
                    count++;
                }
            }

        }

        /// <summary>
        /// Authenticate API key to create new Computer Vision client
        /// </summary>
        /// <param name="key"></param>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public static ComputerVisionClient Authenticate(string key, string endpoint)
        {
            ComputerVisionClient client = new ComputerVisionClient
                (new ApiKeyServiceClientCredentials(key))
            { Endpoint = endpoint };
            return client;
        }

        /// <summary>
        /// Checks if the argument is a valid url to an image
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool IsImageUrl(string url)
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


        public static async Task AnalyzeImage(ComputerVisionClient client)
        {
            // Initialize List for GET response items
            List<VisualFeatureTypes?> features = new List<VisualFeatureTypes?>()
            {
                VisualFeatureTypes.Description, VisualFeatureTypes.Categories
            };

            // Initialize expression variable to invoke remote image analysis
            ImageAnalysis results;

            string url;

            // Assign GET response list items if food image URL is finally validated
            while (true)
            {
                Console.Write("Enter food image full url (e.g. https://...): ");
                var input = Console.ReadLine();

                if (IsImageUrl(input))
                {
                    url = input;
                    // Assign expression to invoke remote image analysis
                    results = await client.AnalyzeImageAsync(url, features);

                    foreach (var category in results.Categories)
                    {
                        if (!category.Name.Contains("food"))
                        {
                            Console.WriteLine("Subject of image is not food. Please upload an image of food");
                        }
                        else
                        {
                            // Terminate indefinite while loop upon total food image validation
                            break;
                        }
                    }
                }

            }

            // TODO: If image is not hotdog, let the user know the image is literally not a hotdog


            //// NOTE: Testing get methods for image captions
            //foreach (var caption in results.Description.Captions)
            //{
            //    Console.WriteLine($"{caption.Text} with confidence {caption.Confidence}");
            //    //if (caption.Text.Contains("hotdog") || caption.Text.Contains("hot dog")) { Console.WriteLine("Hotdog"); }
            //    //else { Console.WriteLine("Not Hotdog"); }
            //}

        }

        // Default constructor
        public App()
        {
            try
            {
                ReadSubscriptionKey();
                ComputerVisionClient client = Authenticate(SubscriptionKey[0], SubscriptionKey[1]);
                AnalyzeImage(client).Wait();
            }
            // Catch any unexpected errors
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            new App();
        }
    }
}
