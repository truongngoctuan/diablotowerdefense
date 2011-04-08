using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TowerDefense
{
    public class Particle
    {
        private Vector2 position;
        private Vector2 velocity;
        private Vector2 acceleration;
        private float lifetime;
        private float age;
        private float scale;
        private float orientation;
        private float angularVelocity;
        
        
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }      
        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        public Vector2 Acceleration
        {
            get { return acceleration; }
            set { acceleration = value; }
        }
        public float Lifetime
        {
            get { return lifetime; }
            set { lifetime = value; }
        }
        public float Age
        {
            get { return age; }
            set { age = value; }
        }      
        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }
        public float Orientation
        {
            get { return orientation; }
            set { orientation = value; }
        }
        public float AngularVelocity
        {
            get { return angularVelocity; }
            set { angularVelocity = value; }
        }

        public bool Active
        {
            get { return Age < Lifetime; }
        }

        public void Initialize(Vector2 position, Vector2 velocity, Vector2 acceleration,
                        float lifetime, float scale, float rotationSpeed, float orientation)
        {
            // set the values to the requested values
            this.Position = position;
            this.Velocity = velocity;
            this.Acceleration = acceleration;
            this.Lifetime = lifetime;
            this.Scale = scale;
            this.AngularVelocity = rotationSpeed;
            this.Age = 0.0f;
            this.Orientation = orientation;
        }

        /// <summary>
        /// Update for the particle.  Does an Euler step.
        /// </summary>
        /// <param name="delta">Time step</param>
        public void Update(float delta)
        {
            // Update velocity
            Velocity += Acceleration * delta;

            // Update position
            Position += Velocity * delta;

            // Update orientation
            Orientation += AngularVelocity * delta;

            // Update age
            Age += delta;
        }


    }
}
