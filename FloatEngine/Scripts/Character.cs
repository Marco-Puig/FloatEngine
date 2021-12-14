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
    public class Character : AnimatedObject
    {
        public Vector2 velocity;

        //customize the feel for the movement:
        protected float decel = 1.2f; //lower it is, the slower you slow down.
        protected float accel = .78f; //lower it is, the slower you take off. 
        protected float maxSpeed = 5f;

        const float gravity = 1f;
        const float jumpVelocity = 16f; //How much we jump.
        const float maxFallVelocity = 32;

        protected bool jumping;
        public static bool applyGravity = true; //use gravity for a platformer, do not use for a top down game.

        public override void Initialize()
        {
            velocity = Vector2.Zero;
            jumping = false;
            base.Initialize();
        }

        public override void Update(List<GameObject> objects, Map map)
        {
            UpdateMovement(objects, map);
            base.Update(objects, map);
        }

        private void UpdateMovement(List<GameObject> objects, Map map)
        {
            if (velocity.X != 0 && CheckCollisions(map, objects, true) == true)
                velocity.X = 0;

            position.X += velocity.X;

            if (velocity.Y != 0 && CheckCollisions(map, objects, false) == true)
                velocity.Y = 0;

            position.Y += velocity.Y;

            if(applyGravity == true) 
                ApplyGravity(map);

            velocity.X = TendToZero(velocity.X, decel);
            if (applyGravity == false)
                velocity.Y = TendToZero(velocity.Y, decel);
        }

        private void ApplyGravity(Map map)
        {
            if (jumping == true || OnGround(map) == Rectangle.Empty)
                velocity.Y += gravity;

            if (velocity.Y > maxFallVelocity)
                velocity.Y = maxFallVelocity;
        }

        protected void MoveRight()
        {
            velocity.X += accel + decel;

            if (velocity.X > maxSpeed)
                velocity.X = maxSpeed;

            direction.X = 1;
        }

        protected void MoveLeft()
        {
            velocity.X -= accel + decel;

            if (velocity.X < -maxSpeed)
                velocity.X = -maxSpeed;

            direction.X = -1;
        }

        protected void MoveDown()
        {
            velocity.Y += accel + decel;

            if (velocity.Y > maxSpeed)
                velocity.Y = maxSpeed;

            direction.Y = 1;
        }

        protected void MoveUp()
        {
            velocity.Y -= accel + decel;

            if (velocity.Y < -maxSpeed)
                velocity.Y = -maxSpeed;

            direction.Y = -1;
        }
        protected bool Jump(Map map)
        {
            if (jumping == true)
                return false;

            if(velocity.Y == 0 && OnGround(map) != Rectangle.Empty)
            {
                velocity.Y -= jumpVelocity;
                jumping = true;
                return true;
            }

            return false;
        }
        protected virtual bool CheckCollisions(Map Map, List<GameObject> objects, bool xAxis)
        {
            Rectangle futureBoundingBox = BoundingBox;

            int maxX = (int)maxSpeed;
            int maxY = (int)maxSpeed;

            if (applyGravity == true)
                maxY = (int)jumpVelocity;

            if(xAxis == true && velocity.X != 0)
            {
                if (velocity.X > 0)
                    futureBoundingBox.X += maxX;
                else
                    futureBoundingBox.X -= maxX;
            }
            else if (applyGravity == false && xAxis == false && velocity.Y != 0)
            {
                if (velocity.Y > 0)
                    futureBoundingBox.Y += maxY;
                else
                    futureBoundingBox.Y -= maxY;
            }
            else if (applyGravity == true && xAxis == false && velocity.Y != gravity)
            {
                if (velocity.Y > 0)
                    futureBoundingBox.Y += maxY;
                else
                    futureBoundingBox.Y -= maxY;
            }


            Rectangle wallCollision = Map.CheckCollision(futureBoundingBox);

            if (wallCollision != Rectangle.Empty)
            {
                if (applyGravity == true && velocity.Y >= gravity && (futureBoundingBox.Bottom > wallCollision.Top - maxSpeed) && (futureBoundingBox.Bottom <= wallCollision.Top + velocity.Y))
                {
                    LandResponse(wallCollision);
                    return true;
                }
                else
                    return true;
            }

            //Check for object collisions:
            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i] != this && objects[i].active == true && objects[i].collidable == true && objects[i].CheckCollision(futureBoundingBox))
                    return true;
            }

            return false;
        }

        public void LandResponse(Rectangle wallCollision)
        {
            position.Y = wallCollision.Top - (boundingBoxHeight + boundingBoxOffset.Y);
            velocity.Y = 0;
            jumping = false;
        }

        protected float TendToZero(float val, float amount)
        {
            if (val > 0f && (val -= amount) < 0f) return 0f;
            if (val < 0f && (val += amount) > 0f) return 0f;
            return val;
        }

        protected Rectangle OnGround(Map map)
        {
            Rectangle futureBoundingBox = new Rectangle((int)(position.X + boundingBoxOffset.X), (int)(position.Y + boundingBoxOffset.Y + (velocity.Y + gravity)), boundingBoxWidth, boundingBoxHeight);

            return map.CheckCollision(futureBoundingBox);
        }

    }
}
