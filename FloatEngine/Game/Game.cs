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
    public class Game : Microsoft.Xna.Framework.Game
    {
        //Rendering managers
        GraphicsDeviceManager graphics; //access to graphics/display device
        SpriteBatch spriteBatch; //renders graphics using sprite batch
        Matrix view, projection; //render graphics via FNA3D graphics api
        Matrix[] bonetransformations; //transform


        //Screen
        public static int ScreenHeight = 720;
        public static int ScreenWidth = 1280;

        //List
        public List<GameObject> objects = new List<GameObject>();
        public Map map = new Map();
        public Splash splash = new Splash();

        //UI
        GameHUD gameHUD = new GameHUD();
        EditorWindow editor;

        //Game States
        private State _currentState;
        private State _nextState;
        public bool _contentLoaded;

        //Particle System
        public static Random Random;
        private SnowEmitter _snowEmitter;
        public static int ParticleLoaded = 0;

        //Levels
        public int countLevel = 1;

        //Models - (For Testing)
        Model model;

        public void ChangeState(State state)
        {
            _nextState = state;
        }
        public Game() //This is the constructor, this function is called whenever the game class is created.
        {
            //Set graphics
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //Set resolution
            Resolution.Init(ref graphics);
            Resolution.SetVirtualResolution(1280, 720); //Resoultion our assests are based in.  

#if DEBUG
            Resolution.SetResolution(1280, 720, false); //Main resolution - Debug.

#else
            Resolution.SetResolution(1920, 1080, true); //Main resolution - Release.
#endif
        }

        /// <summary>
        /// This function is automatically called when the game launches to initialize any non-graphic variables.
        /// </summary>
        protected override void Initialize()
        {
#if DEBUG //pre processor directives.
            editor = new EditorWindow(this);
            editor.Show();
#endif
            Random = new Random();
            base.Initialize(); //intialize non graphic variables

            Camera.Initialize();
            Global.Initialize(this);
        }

        /// <summary>
        /// Automatically called when your game launches to load any game assets (graphics, audio etc.)
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            _currentState = new MenuState(this, graphics.GraphicsDevice, Content);
            _contentLoaded = false;
            _snowEmitter = new SnowEmitter(new Sprite(Content.Load<Texture2D>("Particles//Snow")));

            //Load model:
            model = Content.Load<Model>("Models//3080-model");
            view = Matrix.CreateLookAt(new Vector3(80, 0, 0), Vector3.Zero, Vector3.Up);
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), graphics.GraphicsDevice.Viewport.AspectRatio, .1f, 1000f);


#if DEBUG //pre-processor directives.
            editor.LoadTextures(Content);
#endif
            map.Load(Content);
            gameHUD.Load(Content);
            splash.Load(Content);
            
            //selecting level...
            if (countLevel == 1)
                LoadLevel("Level1.float");
            if (countLevel == 2)
                LoadLevel("Level2.float");
        }
        /// <summary>
        /// Called each frame to update the game. Games usually runs 60 frames per second.
        /// Each frame the Update function will run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            if (_nextState != null)
            {
                _currentState = _nextState;
                _contentLoaded = true;

                _nextState = null;
            }

            _currentState.Update(gameTime);
            _currentState.PostUpdate(gameTime);

            Input.Update();
            UpdateObjects();
            map.Update(objects);
            UpdateCamera();
            _snowEmitter.Update(gameTime);
            //Camera.Zoom, Camera.Rotation

#if DEBUG   //pre-processor directives.
            editor.Update(objects, map);
#endif          
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game is ready to draw to the screen, it's also called each frame.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            //This will clear what's on the screen each frame, if we don't clear the screen will look like a mess:
            GraphicsDevice.Clear(Color.CornflowerBlue); //Select default color.

            Resolution.BeginDraw();

            _currentState.Draw(gameTime, spriteBatch);
            //BackBG.Draw(spriteBatch);

            //Draw sprites.
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Camera.GetTransformMatrix()); //layer depth sorting, ex: layer = 0 drawn up front
#if DEBUG   //pre processor directives.
            editor.Draw(spriteBatch);
#endif
            if (_contentLoaded == true)
            {
                DrawObjects();
                map.DrawWalls(spriteBatch);

                if (ParticleLoaded == 1)
                    _snowEmitter.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();

            if (_contentLoaded == true)
                gameHUD.Draw(spriteBatch);
            else
                splash.Draw(spriteBatch);

            //Not using sprite batch for models
            if (_contentLoaded == true)
            {
                bonetransformations = new Matrix[model.Bones.Count];
                model.CopyAbsoluteBoneTransformsTo(bonetransformations);

                foreach (ModelMesh mesh in model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.World = bonetransformations[mesh.ParentBone.Index];
                        effect.View = view;
                        effect.Projection = projection;
                        effect.EnableDefaultLighting();
                    }
                    mesh.Draw();
                }
            }

            //Calling the draw function via FNA
            base.Draw(gameTime);
        }

        public void LoadLevel(string fileName)
        {
            //Set FileName:
            Global.levelName = fileName;

            //Load Level Data:
            LevelData levelData = XmlHelper.Load("Editor/EditorFiles/" + fileName);

            map.walls = levelData.walls;
            map.decor = levelData.decor;
            objects = levelData.objects;

            map.LoadMap(Content);

            LoadObjects();
        }
        public void LoadObjects()
        {
            for (int i = 0; i < objects.Count; i++)
            {
                objects[i].Initialize();
                objects[i].Load(Content);
            }
        }

        public void UpdateObjects()
        {
            for (int i = 0; i < objects.Count; i++)
            {
                objects[i].Update(objects, map);
            }
        }

        public void DrawObjects()
        {
            for (int i = 0; i < objects.Count; i++)
            {
                objects[i].Draw(spriteBatch);
            }
            for (int i = 0; i < map.decor.Count; i++)
            {
                map.decor[i].Draw(spriteBatch);
            }
        }
        private void UpdateCamera()
        {
            if (objects.Count == 0)
                return;

            Camera.Update(objects[0].position);
        }
    }
}