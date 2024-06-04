

using Data;
using System.Collections.Concurrent;
using System.Numerics;

namespace Logic
{
    public abstract class LogicAbstractAPI
    {
        public abstract int Width { get; set; }
        public abstract int Height { get; set; }
        public abstract int GetBallDiameter(int id);
        public abstract Vector2 GetBallPosition(int id);
        public abstract event EventHandler<(int Id, float X, float Y, int Diameter)>? LogicLayerEvent;

        public abstract void AddBalls(int count);

        public abstract int GetBallsCount();


        public abstract void RemoveAllBalls();
        public static LogicAbstractAPI CreateAPI(int width, int height, DataAbstractAPI? data = null)
        {
            return new LogicAPI(width, height, data);


        }

        internal class LogicAPI : LogicAbstractAPI
        {
            private readonly object _collisionLock = new();
            public override int Width { get; set; }
            public override int Height { get; set; }
            public override event EventHandler<(int Id, float X, float Y, int Diameter)>? LogicLayerEvent;
            private ConcurrentDictionary<(int, int), bool> _collisionFlags = new ConcurrentDictionary<(int, int), bool>();
            private DataAbstractAPI _dataAPI;

            public LogicAPI(int width, int height, DataAbstractAPI? data)
            {
                _dataAPI = data != null ? data : DataAbstractAPI.CreateAPI(width, height);
                Width = width;
                Height = height;
            }



            public override void AddBalls(int count)
            {
                _dataAPI.CreateBalls(count);
                for (int i = 0; i < count; i++)
                {
                    _dataAPI.GetBall(i).BallChanged += PositionChanged;

                }

            }


            public override Vector2 GetBallPosition(int id)
            {
                return _dataAPI.GetBall(id).Position;
            }

            public override int GetBallDiameter(int id)
            {
                return _dataAPI.GetBall(id).Diameter;
            }

            public override void RemoveAllBalls()
            {
                for (int i = 0; i < _dataAPI.GetBallCount(); i++)
                {
                    _dataAPI.GetBall(i).BallChanged -= PositionChanged;

                }
                _dataAPI.RemoveBalls();
            }

            private void PositionChanged(object? sender, EventArgs e)
            {
                if (sender == null)
                {
                    return;
                }
                IBall ball = (IBall)sender;
                lock (_collisionLock)
                {
                    DetectBallCollision(ball);
                }
                DetectWallCollision(ball);
                LogicLayerEvent?.Invoke(this, (ball.Id, ball.Position.X, ball.Position.Y, ball.Diameter));
            }


            private void DetectBallCollision(IBall firstBall)
            {



                for (int i = 0; i < _dataAPI.GetBallCount(); i++)
                {

                    IBall secondBall = _dataAPI.GetBall(i);
                    if (firstBall == secondBall)
                    {
                        continue;
                    }

                    if (!HasCollisionBeenChecked(secondBall, firstBall) && IsCollision(firstBall, secondBall))
                    {
                        MarkCollisionAsChecked(firstBall, secondBall);


                        Vector2 newFirstBallVel = NewVelocity(firstBall, secondBall);
                        Vector2 newSecondBallVel = NewVelocity(secondBall, firstBall);
                        if (Vector2.Distance(firstBall.Position, secondBall.Position) > Vector2.Distance(
                            firstBall.Position + newFirstBallVel, secondBall.Position + newSecondBallVel))
                        {
                            return;
                        }
                        firstBall.Velocity = newFirstBallVel;
                        secondBall.Velocity = newSecondBallVel;



                    }
                    else
                    {
                        RemoveCollisionFromChecked(secondBall, firstBall);
                    }

                }


            }

            private void MarkCollisionAsChecked(IBall firstBall, IBall secondBall)
            {
                int id1 = firstBall.Id;
                int id2 = secondBall.Id;
                var key = (id1, id2);
                _collisionFlags.TryAdd(key, true);
            }

            private void RemoveCollisionFromChecked(IBall firstBall, IBall secondBall)
            {
                int id1 = firstBall.Id;
                int id2 = secondBall.Id;
                var key = (id1, id2);
                _collisionFlags.Remove(key, out _);
            }

            private bool HasCollisionBeenChecked(IBall firstBall, IBall secondBall)
            {
                int id1 = firstBall.Id;
                int id2 = secondBall.Id;
                var key = (id1, id2);
                return _collisionFlags.ContainsKey(key);
            }



            private Vector2 NewVelocity(IBall firstBall, IBall secondBall)
            {
                var ball1Vel = firstBall.Velocity;
                var ball2Vel = secondBall.Velocity;
                var distance = firstBall.Position - secondBall.Position;
                return firstBall.Velocity -
                       2.0f * secondBall.Mass / (firstBall.Mass + secondBall.Mass)
                       * (Vector2.Dot(ball1Vel - ball2Vel, distance) * distance) /
                       (float)Math.Pow(distance.Length(), 2);
            }

            private bool IsCollision(IBall firstBall, IBall secondBall)
            {
                if (firstBall == null || secondBall == null)
                {
                    return false;
                }
                float distance = Vector2.Distance(firstBall.Position, secondBall.Position);
                return distance <= (firstBall.Diameter + secondBall.Diameter) / 2;
            }

            private void DetectWallCollision(IBall ball)
            {

                Vector2 newVel = ball.Velocity;
                int Radius = ball.Diameter / 2;
                if (ball.Position.X - Radius <= 0)
                {
                    newVel.X = Math.Abs(ball.Velocity.X);

                }
                else if (ball.Position.X + Radius >= Width)
                {
                    newVel.X = -Math.Abs(ball.Velocity.X);

                }
                if (ball.Position.Y - Radius <= 0)
                {
                    newVel.Y = Math.Abs(ball.Velocity.Y);
                }
                else if (ball.Position.Y + Radius >= Height)
                {
                    newVel.Y = -Math.Abs(ball.Velocity.Y);

                }
                ball.Velocity = newVel;
            }

            public override int GetBallsCount()
            {
                return _dataAPI.GetBallCount();
            }

        }

    }

}