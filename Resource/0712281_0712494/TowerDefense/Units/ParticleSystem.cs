using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace TowerDefense
{
    public class ParticleEffect
    {
        public Texture2D texture;
        // The list of live particles 
        public LinkedList<Particle> liveParticles;

        // A list of available particles
        public LinkedList<Particle> availableParticles;

        public ParticleEffect()
        {
            liveParticles = new LinkedList<Particle>();
            availableParticles = new LinkedList<Particle>();
        }
    }

    public abstract class ParticleSystem
    {
        protected List<ParticleEffect> particleEffects;
      
        protected int maxNumParticles;
        protected float maxAcceleration;
        protected float maxInitialSpeed;
        protected float maxLifetime;
        protected float maxRotationSpeed;
        protected float maxScale;

        protected int minNumParticles;
        protected float minAcceleration;
        protected float minInitialSpeed;
        protected float minLifetime;
        protected float minRotationSpeed;
        protected float minScale;

        int iBaseSprite;

        protected Vector2 origin;
        //protected SpriteBatch spriteBatch;
        protected SpriteBlendMode spriteBlendMode;

        protected int nNumSprites;
        protected string strResourceFolder;
        public Texture2D Texture
        {
            get { return ResourceManager._rsTowerSprites[(int)RandomHelper.RandomBetween(iBaseSprite, iBaseSprite + nNumSprites - 0.01f)]; }
        }


        protected abstract void InitializeConstants();

        protected virtual void InitializeParticle(Particle p, Vector2 where)
        {
            // Determine the initial particle direction
            Vector2 direction = PickParticleDirection();

            // pick some random values for our particle
            float velocity = RandomHelper.RandomBetween(minInitialSpeed, maxInitialSpeed);
            float acceleration = RandomHelper.RandomBetween(minAcceleration, maxAcceleration);
            float lifetime = RandomHelper.RandomBetween(minLifetime, maxLifetime);
            float scale = RandomHelper.RandomBetween(minScale, maxScale);
            float rotationSpeed = RandomHelper.RandomBetween(minRotationSpeed, maxRotationSpeed);
            float orientation = RandomHelper.RandomBetween(0, (float)Math.PI * 2);

            // then initialize it with those random values. initialize will save those,
            // and make sure it is marked as active.
            p.Initialize(where, velocity * direction, acceleration * direction,
                          lifetime, scale, rotationSpeed, orientation);
        }

        public ParticleSystem(int baseSprite, int numSprite, string resourceFolder)
        {
            iBaseSprite = baseSprite;
            nNumSprites = numSprite;
            strResourceFolder = resourceFolder;
            particleEffects = new List<ParticleEffect>();
            Initialize();
        }

        protected void Initialize()
        {
            InitializeConstants();
        }

        public virtual void AddParticleEffect(Vector2 where)
        {
            ParticleEffect particleEffect = new ParticleEffect();
            particleEffect.availableParticles = new LinkedList<Particle>();
            for (int i = 0; i < maxNumParticles; i++)
            {
                particleEffect.availableParticles.AddLast(new Particle());
            }
            
            particleEffect.texture = Texture;
            for (int i = 0; i < maxNumParticles && particleEffect.availableParticles.Count > 0; i++)
            {
                // Remove the node from the list of available particles
                LinkedListNode<Particle> node = particleEffect.availableParticles.First;
                particleEffect.availableParticles.Remove(node);

                // Initialize the particle
                Particle p = node.Value;
                InitializeParticle(p, where);

                // Add to the list of live particles
                particleEffect.liveParticles.AddLast(node);
            }

            particleEffects.Add(particleEffect);
        }

        public virtual void Update(double deltaTime)
        {
            float delta = (float)deltaTime;

            foreach(ParticleEffect particleEffect in particleEffects)
            {
                for (LinkedListNode<Particle> node = particleEffect.liveParticles.First; node != null; )
                {
                    LinkedListNode<Particle> nextNode = node.Next;
                    node.Value.Update(delta);
                    if (!node.Value.Active)
                    {
                        particleEffect.liveParticles.Remove(node);
                        particleEffect.availableParticles.AddLast(node);
                    }

                    node = nextNode;
                }
            }

            for (int i = 0; i < particleEffects.Count;)
            {
                if (particleEffects[i].liveParticles.Count == 0)
                {
                    particleEffects.Remove(particleEffects[i]);
                }
                else
                    i++;
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
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

                    float alpha = 4 * normalizedAge * (1 - normalizedAge);
                    Color color = new Color(new Vector4(1, 1, 1, alpha));

                    // make particles grow as they age. they'll start at 75% of their size,
                    // and increase to 100% once they're finished.
                    float scale = p.Scale * (.75f + .25f * normalizedAge);

                    spriteBatch.Draw(particleEffect.texture, p.Position - GlobalVar.glRootCoordinate, null, color,
                        p.Orientation, origin, scale, SpriteEffects.None, 0.0f);
                }
            }

            spriteBatch.End();
        }

        protected virtual Vector2 PickParticleDirection()
        {
            float angle = RandomHelper.RandomBetween(0, MathHelper.TwoPi);
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }

        public virtual void LoadContent()
        {
            int iCurrentSprite = iBaseSprite;
            string[] strSprites = System.IO.Directory.GetFiles(strResourceFolder);
            for (int i = 0; i < nNumSprites; i++)
            {
                string strPath = strSprites[i].Substring(8, strSprites[0].IndexOf('.') - 8);
                ResourceManager._rsTowerSprites[iCurrentSprite++] = GlobalVar.glContentManager.Load<Texture2D>(strPath);
            }

            // ... and calculate the center. this'll be used in the draw call, we
            // always want to rotate and scale around this point.
            origin.X = ResourceManager._rsTowerSprites[iBaseSprite].Width / 2;
            origin.Y = ResourceManager._rsTowerSprites[iBaseSprite].Height / 2;
        }
    }

    public static class RandomHelper
    {
        // a random number generator that the whole system can share.
        private static Random random = new Random();
        public static Random Random
        {
            get { return random; }
        }


        //  a handy little function that gives a random float between two
        // values. This will be used in several places in the sample, in particular in
        // ParticleSystem.InitializeParticle.
        public static float RandomBetween(float min, float max)
        {
            return min + (float)random.NextDouble() * (max - min);
        }
    }
}
