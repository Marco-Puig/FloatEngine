using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace FloatEngine
{
    class SnowParticleGenerator
    {
        Texture2D texture;

        float spawnWidth;
        float density;

        List<Snow> snow = new List<Snow>();

        float timer;

        Random rand1, rand2;

        public SnowParticleGenerator(Texture2D newTexture, float newSpawnWidth, float newDensity)
        {
            texture = newTexture;
            spawnWidth = newSpawnWidth;
            density = newDensity;

            rand1 = new Random();
            rand2 = new Random();
        }
        public void Update(GameTime gameTime, GraphicsDevice graphics)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            while (timer > 0)
            {
                timer -= 1f / density;
                createParticle();
            }

            for (int i = 0; i < snow.Count; i++)
            {
                snow[i].Update();

                if(snow[i].Position.Y > graphics.Viewport.Height)
                {
                    snow.RemoveAt(i);
                    i--;
                }
            }
        }           
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Snow snow in snow)
                snow.Draw(spriteBatch);
        }
        public void createParticle()
        {
            snow.Add(new Snow(texture, new Vector2(
                -50 + (float)rand1.NextDouble() * spawnWidth, 0), 
                new Vector2 (1, rand2.Next(5, 8))));
        }
    }
}
