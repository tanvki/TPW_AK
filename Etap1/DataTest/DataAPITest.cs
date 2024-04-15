using Data;
using System.Diagnostics;

namespace LogicTest
{
    [TestClass]
    public class DataAPITest
    {
        [TestMethod]
        public void CreateAPITest()
        {
            DataAbstractAPI api = DataAbstractAPI.CreateAPI();
            Assert.IsNotNull(api);
        }
    }
}