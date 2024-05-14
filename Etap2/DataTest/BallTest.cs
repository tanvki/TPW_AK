using Data;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Numerics;

namespace DataTest
{
    [TestClass]
    public class BallTest
    {
        DataAbstractAPI api = DataAbstractAPI.CreateAPI(500, 500);

        [TestMethod]
        public void BallProperties()
        {
            api.CreateBalls(1);

            Assert.IsNotNull(api.GetBall(0).Diameter);
            Assert.IsNotNull(api.GetBall(0).Mass);
            Assert.IsNotNull(api.GetBall(0).Id);
            Assert.IsNotNull(api.GetBall(0).Position);
            Assert.IsNotNull(api.GetBall(0).X);
            Assert.IsNotNull(api.GetBall(0).Y);

            Vector2 vel = api.GetBall(0).Velocity;
            api.GetBall(0).Velocity = new Vector2(0, 0);
            Assert.AreNotEqual(api.GetBall(0).Velocity, vel);
        }

    }
}