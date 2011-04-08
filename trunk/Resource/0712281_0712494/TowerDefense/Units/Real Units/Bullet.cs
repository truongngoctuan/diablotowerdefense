using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TowerDefense.Units.Real_Units
{
    public class Bullet
    {
        public bool bHit = false;
        Tower _tower;
        ParticleSystem _particleSystem;
        Vector2 _vt2Position;
        Creep _target;
        float _fSpeed = 5;
        float _fAccelerate = 0;
        Vector2 _vt2Direction;
        const float TimeBetweenParticleEffects = 0.05f;
        float timeTillParticleEffect = 0.0f;

        public Bullet(Tower tower, Vector2 vt2Position, Creep target, ParticleSystem particleSystem)
        {
            _tower = tower;
            _vt2Position = vt2Position;
            _target = target;
            _particleSystem = particleSystem;
        }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            float radians = (float)Math.Atan2(_vt2Position.Y - _target.Position.Y, _target.Position.X - _vt2Position.X);
            _vt2Direction.X = (float)Math.Cos(radians);
            _vt2Direction.Y = -(float)Math.Sin(radians);

            _fSpeed += (int)(_fAccelerate);
            _vt2Position += _vt2Direction * _fSpeed;
            if (_target.CheckHit(_vt2Position))
            {
                bHit = true;
                if (_target.State == State.Moving || _target.State == State.Attacked)
                    _tower.Hit(_target);
            }

            UpdateParticleSystem(dt);
        }

        private void UpdateParticleSystem(float dt)
        {
            timeTillParticleEffect -= dt;
            while (timeTillParticleEffect < 0)
            {
                _particleSystem.AddParticleEffect(_vt2Position);
                timeTillParticleEffect += TimeBetweenParticleEffects;
            }
            //_particleSystem.Update(dt);
        }

        public void LoadResource()
        {
            _particleSystem.LoadContent();
        }
    }
}
