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
    public class Projectile : GameObject
    {
        const float speed = 12f; //speed of the projectile
        Character owner;

        int destroyTimer;
        const int maxTimer = 180;

        public Projectile()
        {
            active = false;
        }
        public override void Load(ContentManager content)
        {
            image = TextureLoader.Load("Art//bullet", content);
            base.Load(content);
        }

        public override void Update(List<GameObject> objects, Map map)
        {
            if (active == false)
                return;

            //Update movement:
            position += direction * speed;

            CheckCollisions(objects, map);

            //Update destroy timer.
            destroyTimer--;
            if (destroyTimer <= 0 && active == true)
                Destroy();

            base.Update(objects, map);
        }

        private void CheckCollisions(List<GameObject> objects, Map map)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i].active == true && objects[i] != owner && objects[i].CheckCollision(BoundingBox) == true)
                {
                    Destroy();
                    objects[i].ProjectileResponse();
                    return;
                }
            }

            if (map.CheckCollision(BoundingBox) != Rectangle.Empty)
            {
                Destroy();
            }
        }

        public void Fire(Character inputOwner, Vector2 inputPosition, Vector2 inputDirection)
        {
            owner = inputOwner;
            position = inputPosition;
            direction = inputDirection;
            active = true;
            destroyTimer = maxTimer;
        }

    }
}
