using System;
using System.IO;

namespace NotHotdog
{
    class NotHotdog
    {
        public string[] SubscriptionKey { get; private set; } = new string[2];

        // Read in text file with Computer Vision API key/endpoint
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
                // TODO: Implement more specific Exceptions for read file errors
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        // Initialize app components
        public void Init()
        {
            ReadSubscriptionKey();
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            // NOTE: Test for if `SubscriptionKey`  works
            NotHotdog app = new NotHotdog();
            app.Init();
            string[] foo = new string[2];
            foreach (string line in app.SubscriptionKey)
            {
                Console.WriteLine(line);
            }
        }
    }
}
