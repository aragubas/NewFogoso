using System;
using Microsoft.Xna.Framework;

namespace Fogoso
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        { 
            Console.WriteLine("NewFogoso DotNet Core - 'May 22' 2021\n" + 
                              "========================================");

            Console.WriteLine($"Current platform: {Global.OSName}");

            // Create Game Instance
            Game gameInstance = new Main();
            gameInstance.Run();
            
        }
    }
}
