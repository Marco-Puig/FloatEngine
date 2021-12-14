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
    public class Map
    {
        public List<Decor> decor = new List<Decor>();

        public List<Wall> walls = new List<Wall>();
        Texture2D wallImage;

        public int mapWidth = 15;
        public int mapHeight = 9;
        public int tileSize = 128;

        public void LoadMap(ContentManager content)
        {
            for (int i = 0; i < decor.Count; i++)
                decor[i].Load(content, decor[i].imagePath);
        }

        public void Load(ContentManager content)
        {
            wallImage = TextureLoader.Load("Art//pixel", content);
        }

        public Rectangle CheckCollision(Rectangle input)
        {
            for (int i = 0; i < walls.Count; i++)
            {
                if (walls[i] != null && walls[i].wall.Intersects(input) == true)
                    return walls[i].wall;
            }

            return Rectangle.Empty;
        }

        public void Update(List<GameObject> objects)
        {
            for (int i = 0; i < decor.Count; i++)
                decor[i].Update(objects, this);
        }

        public void DrawWalls(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < walls.Count; i++)
            {
                if (walls[i] != null && walls[i].active == true)
                    spriteBatch.Draw(wallImage, new Vector2(walls[i].wall.X, walls[i].wall.Y), walls[i].wall, Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, .7f);
            }

        }

        public Point GetTileIndex(Vector2 inputPosition)
        {
            if (inputPosition == new Vector2(-1, -1))
                return new Point(-1, -1);

            return new Point((int)inputPosition.X / tileSize, (int)inputPosition.Y / tileSize);
        }
    }

    public class Wall
    {
        public Rectangle wall;
        public bool active = true;

        public Wall()
        {

        }

        public Wall(Rectangle inputRectangle)
        {
            wall = inputRectangle;
        }
    }

    public class Decor : GameObject
    {
        public string imagePath;
        public Rectangle sourceRect;

        public string Name { get { return imagePath; } }

        public Decor()
        {
            collidable = false;
        }

        public Decor(Vector2 inputPosition, string inputImagePath, float inputDepth)
        {
            position = inputPosition;
            imagePath = inputImagePath;
            layerDepth = inputDepth;
            active = true;
            collidable = true;
        }
        public virtual void Load(ContentManager content, string asset)
        {
            image = TextureLoader.Load(asset, content);
            image.Name = asset;

            boundingBoxHeight = image.Height;
            boundingBoxWidth = image.Width;

            if (sourceRect == Rectangle.Empty)
                sourceRect = new Rectangle(0, 0, image.Width, image.Height);
        }

        public void SetImage(Texture2D input, string newPath)
        {
            image = input;
            imagePath = newPath;
            boundingBoxWidth = sourceRect.Width = image.Width;
            boundingBoxHeight = sourceRect.Height = image.Height;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (image != null && active == true)       
                spriteBatch.Draw(image, position, sourceRect, drawColor, rotation, Vector2.Zero, scale, SpriteEffects.None, layerDepth);          
        }
    }
}

