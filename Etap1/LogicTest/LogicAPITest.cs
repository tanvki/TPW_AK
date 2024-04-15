using Logic;

namespace LogicTest
{
    [TestClass]
    public class LogicAPITest
    {
        [TestMethod]
        public void CreateAPITest()
        {
            LogicAbstractAPI api = LogicAbstractAPI.CreateAPI();
            Assert.IsNotNull(api);
        }

        [TestMethod]
        public void AddAndGetBallsTest()
        {
            LogicAbstractAPI api = LogicAbstractAPI.CreateAPI();
            Assert.AreEqual(api.GetBalls().Count, 0);
            api.AddBall();
            api.AddBall();
            Assert.AreEqual(api.GetBalls().Count, 2);
        }

        [TestMethod]
        public void MoveBallsTest()
        {
            LogicAbstractAPI api = LogicAbstractAPI.CreateAPI();
            api.AddBall();
            Assert.AreEqual(api.GetBalls().Count, 1);
            double[] coordinates = new double[2];
            coordinates[0] = api.GetBalls()[0].X;
            coordinates[1] = api.GetBalls()[0].Y;
            api.MoveBalls();
            Assert.AreNotEqual(coordinates[0], api.GetBalls()[0].X);
            Assert.AreNotEqual(coordinates[0], api.GetBalls()[0].Y);
        }
    }
}