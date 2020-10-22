using System;
using System.Drawing;
using System.Threading;
using Console = Colorful.Console;

namespace NotHotdog
{
    class App
    {
        public enum Hotdog
        {
            Hotdog,
            NotHotdog
        }

        /// <summary>
        /// Print some cute hero banner ASCII art
        /// </summary>
        public void PrintHotdog()
        {
            Console.WriteAscii("hotdog cli", Color.FromArgb(244, 212, 255));
        }

        /// <summary>
        /// Return user input as string
        /// </summary>
        /// <returns></returns>
        public string InputImageURL()
        {
            System.Console.Write("Enter food image full url (e.g. https://www.*.jpg): ");
            var input = Console.ReadLine();
            return input;
        }

        // Main view for NotHotdog app
        public void HotdogPrompt()
        {
            if (!ComputerVision.IsRemoteImageDescription("hotdog") &&
                    !ComputerVision.IsRemoteImageCategory())
            {
                Console.WriteLine("Subject of image is not food.");
                Thread.Sleep(3000);
                Console.WriteLine($"You entered an image URL of {ComputerVision.ImageDescription}.", Color.LightGoldenrodYellow);
                Thread.Sleep(3000);
                Console.WriteLine("\n( u_u) Please enter an image URL of food.\n");
                Thread.Sleep(1500);
                Console.WriteLine(Hotdog.NotHotdog, Color.CornflowerBlue);
            }
            else if (ComputerVision.IsRemoteImageDescription("hotdog"))
            {
                Console.WriteLine(Hotdog.Hotdog, Color.HotPink);
            }
            else
            {
                Console.WriteLine(Hotdog.NotHotdog, Color.CornflowerBlue);
            }
        }

        /// <summary>
        /// Exit prompt for program
        /// </summary>
        public bool IsExit()
        {
            while (true)
            {
                System.Console.WriteLine("Continue? [y/n]");
                var input = Console.ReadLine().ToLower();

                switch (input)
                {
                    case "y":
                        return true;
                    case "n":
                        System.Environment.Exit(0);
                        break;
                    default:
                        System.Console.WriteLine("Please enter 'y' or 'n'");
                        continue;
                }

            }

        }

        // Default constructor
        public App()
        {
            PrintHotdog();
            while (true)
            {
                try
                {
                    ComputerVision.AnalyzeRemoteImageHelper(InputImageURL());
                    HotdogPrompt();
                    IsExit();
                }
                catch (NullReferenceException)
                {
                    System.Console.WriteLine("The URL pasted from clipboard is not a valid direct link to an image.");
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                }
            }

        }

        public App(string url)
        {
            PrintHotdog();
            try
            {
                ComputerVision.AnalyzeRemoteImageHelper(url);
                HotdogPrompt();
                return;
            }
            catch (NullReferenceException)
            {
                System.Console.WriteLine("The URL argument used is not a valid direct link to an image.");
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            new App();
            //new App("https://chadhyams.files.wordpress.com/2015/02/michael-jordan.jpg");
        }
    }
}
