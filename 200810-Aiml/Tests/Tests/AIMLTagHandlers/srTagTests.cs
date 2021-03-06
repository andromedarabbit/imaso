using System;
using NUnit.Framework;
using AIMLbot;
using System.Xml;

namespace Tests.AIMLTagHandlers
{
    [TestFixture]
    public class srTagTests
    {
        private AIMLbot.Bot mockBot;
        private AIMLbot.User mockUser;
        private AIMLbot.Request mockRequest;
        private AIMLbot.Result mockResult;
        private AIMLbot.Utils.SubQuery mockQuery;
        private AIMLbot.AIMLTagHandlers.sr mockBotTagHandler;

        [SetUp]
        public void setupMockObjects()
        {
            this.mockBot = new Bot();
            this.mockBot.loadSettings();
            this.mockBot.loadAIMLFromFiles();
            this.mockUser = new User("1", this.mockBot);
            this.mockRequest = new Request("This is a test", this.mockUser, this.mockBot);
            this.mockQuery = new AIMLbot.Utils.SubQuery("This is a test <that> * <topic> *");
            this.mockQuery.InputStar.Insert(0, "first star");
            this.mockQuery.InputStar.Insert(0, "second star");
            this.mockResult = new Result(this.mockUser, this.mockBot, this.mockRequest);
        }

        [Test]
        public void testSRAIWithValidInput()
        {
            XmlNode testNode = StaticHelpers.getNode("<sr/>");
            this.mockQuery.InputStar.Insert(0,"sraisucceeded");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.sr(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("Test passed.", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testSRAIRecursion()
        {
            XmlNode testNode = StaticHelpers.getNode("<sr/>");
            this.mockQuery.InputStar.Insert(0,"srainested");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.sr(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("Test passed.", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testSRAIEmpty()
        {
            XmlNode testNode = StaticHelpers.getNode("<sr/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.sr(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testSRAIBad()
        {
            XmlNode testNode = StaticHelpers.getNode("<se/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.sr(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("", this.mockBotTagHandler.Transform());
        }
    }
}
