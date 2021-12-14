using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace FloatEngine
{
    public class Splash
    {
        Texture2D splash;
        int timer = 120;

        public void Load(ContentManager content)
        {
            splash = TextureLoader.Load("Art/splash", content);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (timer >= 0)
            {
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Resolution.getTransformationMatrix()); //Use when as new spritebatch.begin.
                spriteBatch.Draw(splash, Vector2.Zero, Color.White);
                spriteBatch.End();
            }
            timer -= 1;
        }
    }
}
