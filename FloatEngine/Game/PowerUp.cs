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
    public class PowerUp : AnimatedObject //use as an example for GameObjects
    {
        public PowerUp()
        {
            //use empty contructors since the xml file looks for one to save to.
        }

        public PowerUp(Vector2 inputPosition)
        {
            position = startPosition;
        }

        public override void Initialize()
        {
            base.Initialize();
            collidable = false;
        }

        public override void Load(ContentManager content)
        {
            //Load our image/sprite sheet:
            image = TextureLoader.Load("Art//orbsheet", content);

            //Load any animation stuff if this object animates:
            LoadAnimation("PowerUp.anm", content);
            ChangeAnimation(Animations.IdleLeft); //Set our default animation.

            //Load stuff from our parent class:
            base.Load(content);

            //Customize the size of our bounding box for collisions:
            boundingBoxOffset.X = 0;
            boundingBoxOffset.Y = 0;
            boundingBoxWidth = animationSet.width; //or use image.Width if it's not animated
            boundingBoxHeight = animationSet.height; //or use image.Height if it's not animated
        }

        public override void Update(List<GameObject> objects, Map map)
        {
            if (active == false)
                return;

            CheckPlayerCollison(objects, map);

            base.Update(objects, map);
        }

        public void CheckPlayerCollison(List<GameObject> objects, Map map)
        {
            if (AnimationIsNot(Animations.PickUp) && objects[0].CheckCollision(BoundingBox) == true)
            {
                Player.score++;
                ChangeAnimation(Animations.PickUp);
            }
        }

        protected override void UpdateAnimations()
        {
            if (currentAnimation == null)
                return; //Animation isn't loaded, so return.

            base.UpdateAnimations();

            if (AnimationComplete() == true && GetAnimationName(Animations.PickUp) == currentAnimation.name)
                active = false;
        }
    }
}
