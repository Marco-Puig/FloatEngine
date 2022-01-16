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
    public class RainObject : GameObject
    {
        public RainObject()
        {
           
        }

        public RainObject(Vector2 inputPosition)
        {
            position = startPosition;
        }

        public override void Initialize()
        {
            base.Initialize();
            collidable = false;
            Game.ParticleLoaded = 2;
        }

    }
}