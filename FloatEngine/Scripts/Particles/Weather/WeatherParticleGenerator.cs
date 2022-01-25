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
    class WeatherParticleGenerator
    {
        Texture2D texture;
        float spawnWidth;
        float density;

        List<WeatherParticle> weatherParticle = new List<WeatherParticle>();

        float timer;

        Random rand1, rand2;

        public WeatherParticleGenerator(Texture2D newTexture, float newSpawnWidth, float newDensity)
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

            for (int i = 0; i < weatherParticle.Count; i++)
            {
                weatherParticle[i].Update();

                if(weatherParticle[i].Position.Y > graphics.Viewport.Height)
                {
                    weatherParticle.RemoveAt(i);
                    i--;
                }
            }
        }           
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (WeatherParticle snow in weatherParticle)
                snow.Draw(spriteBatch);
        }
        public void createParticle()
        {
            weatherParticle.Add(new WeatherParticle(texture, new Vector2(
                -50 + (float)rand1.NextDouble() * spawnWidth, 0), 
                new Vector2 (1, rand2.Next(5, 8))));
        }
    }
}
