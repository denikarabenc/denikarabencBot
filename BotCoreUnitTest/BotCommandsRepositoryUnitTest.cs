using Microsoft.VisualStudio.TestTools.UnitTesting;
using BotCore.BotCommands;

namespace BotCoreUnitTest
{
    [TestClass]
    public class BotCommandsRepositoryUnitTest
    {
        [TestInitialize]
        public void Initialize()
        {
            BotCommandsRepository repository = new BotCommandsRepository("somePath");
        }

        [TestMethod]
        public void TestMethod1()
        {
            Assert.IsTrue(true);
        }
    }
}