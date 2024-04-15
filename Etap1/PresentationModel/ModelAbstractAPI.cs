using Logic;
using System.Collections.Generic;
using System.ComponentModel;

namespace PresentationModel
{
    public abstract class ModelAbstractAPI
    {
        public abstract List<BallModel> BallModels { get; }

        public abstract void AddModelBalls();

        public abstract void AddBalls(int number);

        public abstract void Start();

        public abstract void Stop();

        public static ModelAbstractAPI CreateApi()
        {
            return new ModelAPI();
        }
    }

    internal class ModelAPI : ModelAbstractAPI, IDisposable
    {
        private LogicAbstractAPI logicAPI;

        private Timer timer;

        public ModelAPI()
        {

            logicAPI = LogicAbstractAPI.CreateAPI();
        }

        public override List<BallModel> BallModels { get; } = new List<BallModel>();

        public override void AddModelBalls()
        {

            foreach (var ball in logicAPI.GetBalls())
            {
                BallModels.Add(new BallModel(ball.X, ball.Y, ball.Diameter));
            }

        }

        public override void Start()
        {
            timer = new Timer(Move, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(10));
        }

        private void Move(object? state)
        {
            logicAPI.MoveBalls();
            for (int i = 0; i < BallModels.Count; i++)
            {
                BallModels[i].X = logicAPI.GetBalls()[i].X;
                BallModels[i].Y = logicAPI.GetBalls()[i].Y;
            }
        }

        public override void AddBalls(int number)
        {
            for (int i = 0; i < number; i++)
            {
                logicAPI.AddBall();

            }
        }

        public override void Stop()
        {
            this.Dispose();
            BallModels.Clear();
            logicAPI.RemoveAllBalls();

        }

        public void Dispose()
        {
            timer.Dispose();
        }

    }
}