using System;
using NUnit.Framework;
using AIMLbot;
using System.Xml;

namespace Tests.AIMLTagHandlers
{
    [TestFixture]
    public class josaTagTests
    {
        private AIMLbot.Bot mockBot;
        private AIMLbot.User mockUser;
        private AIMLbot.Request mockRequest;
        private AIMLbot.Result mockResult;
        private AIMLbot.Utils.SubQuery mockQuery;
        private AIMLbot.AIMLTagHandlers.lowercase mockBotTagHandler;

        [TestFixtureSetUp]
        public void setupMockObjects()
        {
            this.mockBot = new Bot();
            this.mockBot.loadSettings();
            this.mockUser = new User("1", this.mockBot);
            this.mockRequest = new Request("This is a test", this.mockUser, this.mockBot);
            this.mockQuery = new AIMLbot.Utils.SubQuery("This is a test <that> * <topic> *");
            this.mockResult = new Result(this.mockUser, this.mockBot, this.mockRequest);
        }

    }
}