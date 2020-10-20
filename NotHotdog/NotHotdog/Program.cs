//using System;
using System.Drawing;
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
            Console.WriteAscii("hotdog cli", Color.FromArgb(244,212,255));
        }

        /// <summary>
        /// Exit prompt for program
        /// </summary>
        public void DisplayExitPrompt()
        {
            while (true)
            {
                System.Console.WriteLine("Continue? [y/n]");
                var input = Console.ReadLine().ToLower();

                switch (input)
                {
                    case "y":
                        return;
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
                ComputerVision.AnalyzeRemoteImageHelper();

                while (ComputerVision.IsRemoteImageCategory())
                {
                    if (ComputerVision.IsRemoteImageDescription("hotdog"))
                    {
                        Console.WriteLine(Hotdog.Hotdog, Color.HotPink);
                    }
                    else
                    {
                        Console.WriteLine(Hotdog.NotHotdog, Color.CornflowerBlue);
                    }
                    break;
                }

                DisplayExitPrompt();
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
