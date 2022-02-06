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
    public class Enemy: Character
    {
        int respawnTimer;
        const int maxRespawnTimer = 60;

        Random random = new Random();

        SoundEffect explosion;

        public Enemy()
        {

        }

        public Enemy(Vector2 inputPosition)
        {
            position = inputPosition;
        }

        public override void Initialize()
        {
            active = true;
            collidable = true;
            position.X = random.Next(0, 1100);

            base.Initialize();
        }

        public override void Load(ContentManager content)
        {
            image = TextureLoader.Load("Art//enemy", content);
            explosion = content.Load<SoundEffect>("Audio//explosion");
            base.Load(content);
            boundingBoxHeight = image.Height - 10;
        }

        public override void Update(List<GameObject> objects, Map map)
        {
            if (respawnTimer > 0)
            {
                respawnTimer--;
                if (respawnTimer <= 0)
                    Initialize();
            }

            base.Update(objects, map);
        }

        public override void ProjectileResponse()
        {
            active = false;
            respawnTimer = maxRespawnTimer;
            Player.score++;

            explosion.Play(); //(float volume, float pitch, float pan); ex: (2f, 3f, 5f);

            base.ProjectileResponse();
        }
    }
}
