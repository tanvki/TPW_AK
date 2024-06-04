using Logic;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace PresentationModel
{


    public abstract class ModelAbstractAPI
    {
        public abstract int _width { get; }
        public abstract int _height { get; }

        public abstract void AddBalls(int number);

        public abstract void Stop();
        public ObservableCollection<BallModel> Balls { get; set; }
        public static ModelAbstractAPI CreateApi(int w, int h)
        {
            return new ModelAPI(w, h);
        }

        internal class ModelAPI : ModelAbstractAPI
        {

            public override int _width { get; }
            public override int _height { get; }
            private LogicAbstractAPI _logicAPI;


            public ModelAPI(int w, int h)
            {
                _width = w;
                _height = h;
                _logicAPI = LogicAbstractAPI.CreateAPI(_width, _height);
                Balls = new ObservableCollection<BallModel>();
                _logicAPI.LogicLayerEvent += UpdateBall;
            }



            public override void AddBalls(int number)
            {
                _logicAPI.AddBalls(number);
                for (int i = 0; i < number; i++)
                {
                    BallModel ballModel = new BallModel(_logicAPI.GetBallPosition(i).X, _logicAPI.GetBallPosition(i).Y, _logicAPI.GetBallDiameter(i));
                    Balls.Add(ballModel);
                }
            }

            public override void Stop()
            {

                _logicAPI.RemoveAllBalls();
                Balls.Clear();

            }

            private void UpdateBall(object? sender, (int id, float x, float y, int diameter) args)
            {


                if (args.id >= Balls.Count)
                {
                    return;
                }
                Balls[args.id].Move(args.x - args.diameter / 2, args.y - args.diameter / 2);

            }


        }
    }


}