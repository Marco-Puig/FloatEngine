using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FloatEngine
{
  public class SnowEmitter : Emitter
  {
    public SnowEmitter(Sprite particle) : base(particle)
    {

    }

    protected override void ApplyGlobalVelocity()
    {
      var xSway = (float)Game.Random.Next(-2, 2);
      foreach (var particle in _particles)
        particle.Velocity.X = (xSway * particle.Scale) / 50;
    }

    protected override Sprite GenerateParticle()
    {
      var sprite = _particlePrefab.Clone() as Sprite;

      var xPosition = Game.Random.Next(0, Game.ScreenWidth);
      var ySpeed = Game.Random.Next(10, 100) / 100f;

      sprite.Position = new Vector2(xPosition, -sprite.Rectangle.Height);
      sprite.Opacity = (float)Game.Random.NextDouble();
      sprite.Rotation = MathHelper.ToRadians(Game.Random.Next(0, 360));
      sprite.Scale = (float)Game.Random.NextDouble() + Game.Random.Next(0, 3);
      sprite.Velocity = new Vector2(0, ySpeed);

      return sprite;
    }
  }
}
