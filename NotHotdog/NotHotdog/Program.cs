using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Drawing;
using Console = Colorful.Console;
using Colorful;

namespace NotHotdog
{
    class App
    {
        public enum Hotdog
        {
            Hotdog,
            NotHotdog
        }

        Hotdog hotdog = Hotdog.Hotdog;
        Hotdog notHotdog = Hotdog.NotHotdog;

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
        }


        public async Task AnalyzeImage(ComputerVisionClient client)
        {

            string url;
            string input;

            // Assign GET response list items if food image URL is finally validated
            while (true)
            {
                Console.Write("Enter/paste food image full url (e.g. https://...): ");
                input = Console.ReadLine();

                if (IsImageUrl(input))
                {
                    url = input;
                    break;
                }

            }

            // Initialize List for GET response items
            List<VisualFeatureTypes?> features = new List<VisualFeatureTypes?>()
            {
                VisualFeatureTypes.Description, VisualFeatureTypes.Categories
            };

            // Assign expression to invoke remote image analysis
            ImageAnalysis results = await client.AnalyzeImageAsync(url, features);

            // Assign image description caption
            var imageDescription = results.Description.Captions[0].Text;

            Console.WriteLine("\nAnalyzing image...\n");
            Thread.Sleep(3000);

            // Recursively call on AnalyzeImage if entered image url is not food
            foreach (var category in results.Categories)
            {
                if (!category.Name.Contains("food"))
                {
                    Console.WriteLine("Subject of image is not food.");
                    Thread.Sleep(3000);
                    //Console.WriteLine($"You entered an image URL of {imageDescription}.");
                    Console.WriteLine($"You entered an image URL of {imageDescription}.", Color.LightGoldenrodYellow);
                    Thread.Sleep(3000);
                    Console.WriteLine("\n( u_u) Please enter a image URL of food.\n");
                    Thread.Sleep(1500);
                    AnalyzeImage(client).Wait();
                }
            }

            // Iterate through description captions in case more than one is stored
            foreach (var caption in results.Description.Captions)
            {
                //Console.WriteLine($"{caption.Text} with confidence {caption.Confidence}");
                
                if ((caption.Text.Contains("hotdog") || caption.Text.Contains("hot dog")) && caption.Confidence > 0.45)
                {
                    Console.WriteLine(hotdog, Color.HotPink);
                }
                else { Console.WriteLine(notHotdog); }
            }

            while (true)
            {
                Console.Write("\nContinue? [y/n]: ");

                input = Console.ReadLine().ToLower();

                switch (input)
                {
                    case "y":
                        AnalyzeImage(client).Wait();
                        break;
                    case "n":
                        return;
                    default:
                        Console.WriteLine("Enter 'y' or 'n'");
                        break;
                }
            }



        }

        public void PrintHotdog()
        {
            Console.WriteAscii("hotdog cli", Color.FromArgb(244,212,255));
        }

        // Default constructor
        public App()
        {
            try
            {
                PrintHotdog();
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
