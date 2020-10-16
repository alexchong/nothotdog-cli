using System;
using System.IO;
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
            {
                try
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
                catch (IOException e)
                {
                    Console.WriteLine(e.Message);
                }
                // TODO: Implement more specific Exceptions for read file errors
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
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
                (new ApiKeyServiceClientCredentials(key)) { Endpoint = endpoint };
            return client;
        }

        public NotHotdog()
        {
            try
            {
                ReadSubscriptionKey();
                ComputerVisionClient client = Authenticate(SubscriptionKey[0], SubscriptionKey[1]);
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

        }
    }
}
