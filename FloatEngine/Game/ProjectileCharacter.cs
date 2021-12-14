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
    public class ProjectileCharacter : Character
    {
        List<Projectile> projectiles = new List<Projectile>();

        const int numOfProjs = 20;

        public ProjectileCharacter() 
        {

        }

        public override void Initialize()
        {
            if (projectiles.Count == 0)
            {
                for (int i = 0; i < numOfProjs; i++)
                    projectiles.Add(new Projectile());
            }
            base.Initialize();
        }

        public override void Load(ContentManager content)
        {
            for (int i = 0; i < numOfProjs; i++)
                projectiles[i].Load(content);

            base.Load(content);
        }

        public override void Update(List<GameObject> objects, Map map)
        {
            for (int i = 0; i < numOfProjs; i++)
                projectiles[i].Update(objects, map);

            base.Update(objects, map);
        }
        public void Fire()
        {
            for (int i = 0; i < numOfProjs; i++)
            {
                if(projectiles[i].active == false)
                {                  
                    projectiles[i].Fire(this, position, direction);
                    break;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < numOfProjs; i++)
                projectiles[i].Draw(spriteBatch);

            base.Draw(spriteBatch);
        }
    }
}
