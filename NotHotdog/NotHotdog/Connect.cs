﻿using System.IO;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;

namespace NotHotdog
{
    public class Connect
    {
        public static string[] SubscriptionKey { get; set; } = new string[2];

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
        public static ComputerVisionClient AuthenticateSession()
        {
            ReadSubscriptionKey();
            ComputerVisionClient client = new ComputerVisionClient
                (new ApiKeyServiceClientCredentials(SubscriptionKey[0]))
            { Endpoint = SubscriptionKey[1] };
            return client;
        }

    }

}
