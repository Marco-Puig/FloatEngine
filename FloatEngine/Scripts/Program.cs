using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FloatEngine
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            using (Game game = new Game())
            {
                //Just calling xna to run the game.
                game.Run();
            }
        }
    }
}