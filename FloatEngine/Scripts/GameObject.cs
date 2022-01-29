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
    public class GameObject
    {
        protected Texture2D image;
        public Vector2 position;
        public Color drawColor = Color.White;
        public float scale = 1f, rotation = 0f;
        public float layerDepth = 0.5f;
        public bool active = true;
        protected Vector2 center;

        public bool collidable = true;
        protected int boundingBoxWidth, boundingBoxHeight;
        protected Vector2 boundingBoxOffset;
        Texture2D boundingBoxImage;
        public Color drawBoxColor = new Color(120, 120, 120, 120);
        const bool drawBoundingBoxes = true; //const means can true statement can never be changed
        protected Vector2 direction = new Vector2(1, 0);

        public Vector2 startPosition = new Vector2(-1, -1);

        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)(position.X + boundingBoxOffset.X), (int)(position.Y + boundingBoxOffset.Y), boundingBoxWidth, boundingBoxHeight);
            }
        }

        public GameObject()
        {

        }

        public virtual void Initialize()
        {
            if (startPosition == new Vector2(-1, -1))
                startPosition = position;
        }

        public virtual void SetToDefaultPosition()
        {
            position = startPosition;
        }

        public virtual void Load(ContentManager content)
        {
            boundingBoxImage = TextureLoader.Load("Art//pixel", content);

            CalculateCenter();

            if(image != null)
            {
                boundingBoxWidth = image.Width;
                boundingBoxHeight = image.Height - 10;
            }
        }
        public virtual void Update(List<GameObject> objects, Map map)
        {

        }

        public virtual bool CheckCollision(Rectangle input)
        {
            return BoundingBox.Intersects(input);
        }

        public virtual void Draw(Matrix projection, SpriteBatch spriteBatch)
        {
            if(boundingBoxImage != null && drawBoundingBoxes == true && active == true)
                spriteBatch.Draw(boundingBoxImage, position, BoundingBox, drawBoxColor, rotation, Vector2.Zero, 1f, SpriteEffects.None, .1f);

            if (image != null && active == true)
                spriteBatch.Draw(image, position, null, drawColor, rotation, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
        }

        public virtual void ProjectileResponse()
        {

        }

        private void CalculateCenter()
        {
            if (image == null)
                return;

            center.X = image.Width / 2;
            center.Y = image.Height / 2;
        }
        public void Destroy() //Helper function for any GameObject to use.
        {
            active = false;
        }
    }
}
