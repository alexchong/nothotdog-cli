using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace NotHotdog
{
    class NotHotdog
    {
        //public enum Hotdog
        //{
        //    Hotdog,
        //    NotHotdog
        //}

        public string[] SubscriptionKey { get; private set; } = new string[2];

        /// <summary>
        /// Read in text file with Computer Vision API key/endpoint
        /// </summary>
        public void ReadSubscriptionKey()
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
        public ComputerVisionClient Authenticate(string key, string endpoint)
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
        public bool IsImageUrl(string url)
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

        public async Task AnalyzeImage(ComputerVisionClient client)
        {
            while (true)
            {

                Console.Write("Enter image url: ");

                // Use regex to test for valid image url (e.g. ...com/hot-dog.jpg)
                var url = Console.ReadLine();

                // TODO: If image is not hotdog, let the user know the image is literally not a hotdog
                List<VisualFeatureTypes?> features = new List<VisualFeatureTypes?>()
                    {
                        VisualFeatureTypes.Description, VisualFeatureTypes.Categories
                    };

                ImageAnalysis results = await client.AnalyzeImageAsync(url, features);

                // NOTE: Testing get methods for image captions
                foreach (var caption in results.Description.Captions)
                {
                    //Console.WriteLine($"{caption.Text} with confidence {caption.Confidence}");
                    if (caption.Text.Contains("hotdog")) { Console.WriteLine("This is a hotdog"); }
                    else { Console.WriteLine("This is not a hotdog"); }
                }


                //foreach (var caption in results.Categories)
                //{
                //    Console.WriteLine($"{caption.Name} with confidence {caption.Score}");
                //}

                return;

            }

        }

        // Default constructor
        public NotHotdog()
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

            NotHotdog app = new NotHotdog();
        }
    }
}
