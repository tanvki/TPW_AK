
using Data;
using Logic;

namespace Logic
{
    public abstract class LogicAbstractAPI
    {
        public abstract void AddBall();

        public abstract List<Ball> GetBalls();

        public abstract void MoveBalls();

        public abstract void RemoveAllBalls();
        public static LogicAbstractAPI CreateAPI()
        {
            return new LogicAPI();
        }
    }
    internal class LogicAPI : LogicAbstractAPI
    {
        private List<Ball> balls;

        public int size { get; set; } = 500;

        private Random random = new Random();

        private DataAbstractAPI dataAPI;

        public LogicAPI()
        {
            balls = new List<Ball>();
            dataAPI = DataAbstractAPI.CreateAPI();
        }

        public override void MoveBalls()
        {
            foreach (var ball in balls)
            {
                ball.X += ball.XSpeed;
                ball.Y += ball.YSpeed;

                if (ball.X < 0 || ball.X + ball.Diameter > size)
                {
                    ball.XSpeed *= -1;
                }
                if (ball.Y < 0 || ball.Y + ball.Diameter > size)
                {
                    ball.YSpeed *= -1;
                }
            }
        }


        public override void AddBall()
        {
            balls.Add(new Ball(random.NextDouble() * (size - 30) + 10, random.NextDouble() * (size - 30) + 10, random.NextDouble() * 2 + 1, random.NextDouble() * 2 + 1));
        }

        public override List<Ball> GetBalls()
        {
            return balls;
        }

        public override void RemoveAllBalls()
        {
            balls.Clear();
        }

    }
}