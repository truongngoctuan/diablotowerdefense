using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense.Units
{
    public class BulletParticleSystem : ParticleSystem
    {
        public BulletParticleSystem(int baseSprite, int numSprite, string resourceFolder) 
            : base(baseSprite, numSprite, resourceFolder)
        {
        }

        protected override void InitializeConstants()
        {
            // high initial speed with lots of variance.  make the values closer
            // together to have more consistently circular explosions.
            minInitialSpeed = 0;
            maxInitialSpeed = 100;

            // doesn't matter what these values are set to, acceleration is tweaked in
            // the override of InitializeParticle.
            minAcceleration = 0.0f;
            maxAcceleration = 0.0f;

            // explosions should be relatively short lived
            minLifetime = 0.1f;
            maxLifetime = 0.5f;

            minScale = .1f;
            maxScale = 1.0f;

            minNumParticles = 5;
            maxNumParticles = 15;

            minRotationSpeed = -MathHelper.PiOver4;
            maxRotationSpeed = MathHelper.PiOver4;

            // additive blending is very good at creating fiery effects.
            spriteBlendMode = SpriteBlendMode.Additive;

            //DrawOrder = AdditiveDrawOrder;
        }

        protected override void InitializeParticle(Particle p, Vector2 where)
        {
            // Determine the initial particle direction
            Vector2 direction = PickParticleDirection();

            // pick some random values for our particle
            float velocity = RandomHelper.RandomBetween(minInitialSpeed, maxInitialSpeed);
            float acceleration = RandomHelper.RandomBetween(minAcceleration, maxAcceleration);
            float lifetime = RandomHelper.RandomBetween(minLifetime, maxLifetime);
            float scale = (maxInitialSpeed - velocity) / (maxInitialSpeed - minInitialSpeed) * (maxScale - minScale) + minScale;
            float rotationSpeed = RandomHelper.RandomBetween(minRotationSpeed, maxRotationSpeed);
            float orientation = RandomHelper.RandomBetween(0, (float)Math.PI * 2);

            // then initialize it with those random values. initialize will save those,
            // and make sure it is marked as active.
            p.Initialize(where, velocity * direction, acceleration * direction,
                          lifetime, scale, rotationSpeed, orientation);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // tell sprite batch to begin, using the spriteBlendMode specified in
            // initializeConstants
            spriteBatch.Begin(spriteBlendMode);

            foreach (ParticleEffect particleEffect in particleEffects)
            {
                foreach (Particle p in particleEffect.liveParticles)
                {
                    // Life time as a value from 0 to 1
                    float normalizedAge = p.Age / p.Lifetime;

                    //float alpha = 4 * normalizedAge * (1 - normalizedAge);
                    //Color color = new Color(new Vector4(1, 1, 1, alpha));
                    float alpha = normalizedAge;
                    Color color = new Color(new Vector4(1, 1, 1, 1 - alpha));

                    // make particles grow as they age. they'll start at 75% of their size,
                    // and increase to 100% once they're finished.
                    float scale = p.Scale * (.75f + .25f * normalizedAge);

                    spriteBatch.Draw(particleEffect.texture, p.Position - GlobalVar.glRootCoordinate, null, color,
                        p.Orientation, origin, scale, SpriteEffects.None, 0.0f);
                }
            }

            spriteBatch.End();
        }

        protected override Vector2 PickParticleDirection()
        {
            // Point the particles somewhere between 80 and 100 degrees.
            // tweak this to make the smoke have more or less spread.
            float radians = RandomHelper.RandomBetween(
                MathHelper.ToRadians(0), MathHelper.ToRadians(360));

            Vector2 direction = Vector2.Zero;
            // from the unit circle, cosine is the x coordinate and sine is the
            // y coordinate. We're negating y because on the screen increasing y moves
            // down the monitor.
            direction.X = (float)Math.Cos(radians);
            direction.Y = -(float)Math.Sin(radians);
            return direction;
        }
    }
}
