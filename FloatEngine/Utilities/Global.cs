using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace FloatEngine
{
    static class Global
    {
        public static Game game;
        public static Random random = new Random();
        public static string levelName;

        public static void Initialize(Game inputGame)
        {
            game = inputGame;
        }
    }
}
