using Data;
using System.Numerics;
using Logic;


namespace LogicTest
{
    [TestClass]
    public class LogicAPITest
    {

        internal class BallTest : IBall
        {
            private Task task;

            private bool _move = true;

            private int _diameter;

            public BallTest(int id)
            {
                Id = id;
                _position = new Vector2(200 - id * 50, 200);
                _velocity = new Vector2((id * 2) + 1, 0);
                _diameter = 30;
                Mass = 60;
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
                while (_move)
                {
                    Position += _velocity;

                    BallChanged?.Invoke(this, EventArgs.Empty);
                    float delay = 20 / _velocity.Length();
                    await Task.Delay((int)delay);
                }

            }

            public void Dispose()
            {
                _move = false;
                task.Dispose();
            }
        }

        internal class DataAPITest : DataAbstractAPI
        {
            public DataAPITest(int width, int height)
            {
                Width = width;
                Height = height;
                _balls = new List<IBall>();
            }
            private List<IBall> _balls;
            public override int Width { get; }
            public override int Height { get; }


            private readonly Random _random = new Random();

            public override int GetBallCount()
            {
                return _balls.Count();
            }

            public override IBall GetBall(int index)
            {
                return _balls[index];
            }

            public override void CreateBalls(int count)
            {
                for (int i = 0; i < count; i++)
                {
                    BallTest ball = new BallTest(i);
                    _balls.Add(ball);
                }
            }

            public override void RemoveBalls()
            {
                foreach (IBall ball in _balls)
                {
                    ball.Dispose();
                }
                _balls.Clear();
            }

        }



        [TestMethod]
        public void CreateAPITest()
        {
            LogicAbstractAPI api = LogicAbstractAPI.CreateAPI(500, 500, new DataAPITest(500, 500));
            Assert.IsNotNull(api);
        }

        [TestMethod]
        public void AddBallsTest()
        {
            LogicAbstractAPI api = LogicAbstractAPI.CreateAPI(500, 500, new DataAPITest(500, 500));
            api.AddBalls(3);
            Assert.IsTrue(api.GetBallsCount() == 3);
        }

        [TestMethod]
        public void WallCollisionsTest()
        {
            LogicAbstractAPI api = LogicAbstractAPI.CreateAPI(500, 500, new DataAPITest(500, 500));
            api.AddBalls(1);
            float lastX = api.GetBallPosition(0).X;
            api.LogicLayerEvent += (sender, args) =>
            {

                if (api.GetBallPosition(0).X < lastX)
                {
                    Assert.IsTrue(api.GetBallPosition(0).X < 500);
                }
                else
                {
                    lastX = api.GetBallPosition(0).X;
                }

            };

        }

        [TestMethod]
        public void BallsCollisionsTest()
        {
            LogicAbstractAPI api = LogicAbstractAPI.CreateAPI(500, 500, new DataAPITest(500, 500));
            api.AddBalls(2);
            float lastX1 = api.GetBallPosition(0).X;
            float lastX2 = api.GetBallPosition(1).X;
            float lastDistance = Vector2.Distance(api.GetBallPosition(0), api.GetBallPosition(1));
            api.LogicLayerEvent += (sender, args) =>
            {
                if (api.GetBallPosition(0).X > lastX1 && api.GetBallPosition(1).X < lastX2
                && lastDistance < Vector2.Distance(api.GetBallPosition(0), api.GetBallPosition(1)))
                {
                    Assert.IsTrue(api.GetBallPosition(0).X > api.GetBallPosition(1).X);
                    Assert.IsTrue(lastDistance < Vector2.Distance(api.GetBallPosition(0), api.GetBallPosition(1)));
                }
                else
                {
                    lastX1 = api.GetBallPosition(0).X;
                    lastX2 = api.GetBallPosition(1).X;
                    lastDistance = Vector2.Distance(api.GetBallPosition(0), api.GetBallPosition(1));
                }
            };
        }

        [TestMethod]
        public void ParametersTest()
        {
            LogicAbstractAPI api = LogicAbstractAPI.CreateAPI(500, 500, new DataAPITest(500, 500));
            Assert.AreEqual(api.Width, 500);
            Assert.AreEqual(api.Height, 500);
            api.Width = 600;
            api.Height = 600;
            Assert.AreEqual(api.Width, 600);
            Assert.AreEqual(api.Height, 600);
        }
    }
}