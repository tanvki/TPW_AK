using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Data
{

    internal class Ball : IBall
    {
        private Task task;

        private bool _move = true;

        private int _diameter;

        private Stopwatch _stopwatch;

        public Ball(float x, float y, int mass, Vector2 velocity, int diameter, int id)
        {
            _stopwatch = new Stopwatch();
            Id = id;
            _position = new Vector2(x, y);
            _velocity = velocity;
            _diameter = diameter;
            Mass = mass;
            task = Task.Run(Move);
        }

        public event EventHandler? BallChanged;

        private Vector2 _position;

        public Vector2 Position
        {
            get => _position;

            private set
            {
                _position = value;
            }
        }

        private Vector2 _velocity;

        public Vector2 Velocity
        {
            get => _velocity;
            set
            {

                _velocity = value;

            }
        }

        public int Diameter
        {
            get => _diameter;
        }

        public float X => _position.X;

        public float Y => _position.Y;

        public int Mass { get; }


        public int Id { get; }

        private async void Move()
        {
            int delay = 15;

            while (_move)
            {
                _stopwatch.Restart();
                _stopwatch.Start();

                Update(delay);

                _stopwatch.Stop();
                await Task.Delay(delay - (int)_stopwatch.ElapsedMilliseconds < 0 ? 0 : delay - (int)_stopwatch.ElapsedMilliseconds);
            }

        }

        private void Update(long time)
        {
            Position += _velocity * time;
            BallChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            _move = false;
            task.Dispose();
        }
    }
}
