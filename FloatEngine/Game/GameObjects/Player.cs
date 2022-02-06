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
    public class Player : ProjectileCharacter
    {
        public static int score;

        SoundEffect song; //move to seperate audio class eventually.
        SoundEffectInstance songInstance;

        public Player()
        {

        }
        public Player(Vector2 inputPosition)
        {
            position = inputPosition;
            applyGravity = true;
        }

        public override void Initialize()
        {
            score = 0;
            base.Initialize();
        }
        public override void Load(ContentManager content)
        {
            image = TextureLoader.Load("Art//sprite2", content);

            //Load our animation stuff:
            LoadAnimation("GirlPlayer.anm", content);
            ChangeAnimation(Animations.IdleLeft);

            base.Load(content);

            boundingBoxOffset.X = 0; 
            boundingBoxOffset.Y = 0;
            boundingBoxWidth = animationSet.width; 
            boundingBoxHeight = animationSet.height - 18; //adjust for character height

            //Load Song:
            song = content.Load<SoundEffect>("Audio//song");

            if (songInstance == null)
                songInstance = song.CreateInstance();
        }
        public override void Update(List<GameObject> objects, Map map)
        {
            CheckInput(objects, map);
            //UpdateMusic();
            base.Update(objects, map);
        }

        private void UpdateMusic()
        {
            if (songInstance.State != SoundState.Playing)
            {
                songInstance.IsLooped = true;
                songInstance.Play();
            }
                
        }
        private void CheckInput(List<GameObject> objects, Map map)
        {
            //Calling Input in /Utilities/Input.cs.
            if (Character.applyGravity == false)
            {
                if (Input.IsKeyDown(Keys.D) == true)
                    MoveRight();
                else if (Input.IsKeyDown(Keys.A) == true)
                    MoveLeft();
                if (Input.IsKeyDown(Keys.W) == true)
                    MoveUp();
                else if (Input.IsKeyDown(Keys.S) == true)
                    MoveDown();
            }
            else
            {
                if (Input.IsKeyDown(Keys.D) == true)
                    MoveRight();
                else if (Input.IsKeyDown(Keys.A) == true)
                    MoveLeft();

                if (Input.IsKeyDown(Keys.Space) == true)
                    Jump(map);
            }

            if (Input.KeyPressed(Keys.E))
                Fire();
        }

        protected override void UpdateAnimations()
        {
            if (currentAnimation == null)
                return;

            base.UpdateAnimations();

            if(velocity != Vector2.Zero || jumping == true)
            {
                if (direction.X < 0 && AnimationIsNot(Animations.RunLeft))
                    ChangeAnimation(Animations.RunLeft);
                else if (direction.X > 0 && AnimationIsNot(Animations.RunRight))
                    ChangeAnimation(Animations.RunRight);
            }
            else if (velocity == Vector2.Zero && jumping == false)
            {
                if (direction.X < 0 && AnimationIsNot(Animations.IdleLeft))
                    ChangeAnimation(Animations.IdleLeft);
                else if (direction.X > 0 && AnimationIsNot(Animations.IdleRight))
                    ChangeAnimation(Animations.IdleRight);
            }
        }
    }
}
