﻿using System;
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
	public class ExampleModel : GameObject
	{
		Matrix view, projection; //render graphics via FNA3D graphics api
		Matrix[] bonetransformations; //transform
		Model model;


		public override void Initialize()
		{ 
			base.Initialize();
		}
		public override void Load(ContentManager content)
		{
			model = content.Load<Model>("Models//3080-model");
		}
		public void Update(GraphicsDevice graphics)
		{
			view = Matrix.CreateLookAt(new Vector3(80, 0, 0), Vector3.Zero, Vector3.Up);
			projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), graphics.Viewport.AspectRatio, .1f, 1000f);
		}	
        public override void Draw3D(GameTime gameTime, SpriteBatch spriteBatch)
        {
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
        }
    }
}
