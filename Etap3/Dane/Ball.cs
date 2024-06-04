using System.Diagnostics;
using System.Numerics;

namespace Data
{

    internal class Ball : IBall
        {
            private Task task;

            private bool _move = true;

            private int _diameter;

            private Stopwatch _stopwatch;

            private int _mass;

            public Ball(float x, float y, int mass, Vector2 velocity, int diameter, int id)
            {
                _stopwatch = new Stopwatch();
                Id = id;
                _position = new Vector2(x, y);
                _velocity = velocity;
                _diameter = diameter;
                _mass = mass;
                task = Task.Run(Move);
            }

            public Ball(Ball other)
            {
                _stopwatch = new Stopwatch();
                Id = other.Id;
                _position = new Vector2(other.Position.X, other.Position.Y);
                _velocity = other.Velocity;
                _diameter = other.Diameter;
                _mass = other.Mass;
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

            public int Mass { get => _mass;
                private set { _mass = value; }
                }


            public int Id { get; }

            private async void Move()
            {
                float time;

                while (_move)
                {
                    _stopwatch.Restart();
                    _stopwatch.Start();
                    time = (2 / _velocity.Length());
                    Update(time);
                                       
                    _stopwatch.Stop();
                    await Task.Delay(time - _stopwatch.ElapsedMilliseconds < 0 ? 0 : (int)(time - _stopwatch.ElapsedMilliseconds));
                }

            }

            private void Update(float time)
            {
                Position += _velocity * time;              
                BallChanged?.Invoke(this, EventArgs.Empty);
            }

            public void Dispose()
            {
                _move = false;
                task.Wait();
                task.Dispose();     
            }

       
    }
   
}
