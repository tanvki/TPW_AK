using Etap0;

namespace Test
{
    [TestClass]
    public class CalcTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            {
                Calculator program = new Calculator();

                Assert.AreEqual(program.add(1.5, 1.5), 3);
                Assert.AreEqual(program.subtract(3, 2), 1);
                Assert.AreEqual(program.divide(3, 2), 1.5);
                Assert.AreEqual(program.multiply(3, 3), 9);

            }
        }
    }
}