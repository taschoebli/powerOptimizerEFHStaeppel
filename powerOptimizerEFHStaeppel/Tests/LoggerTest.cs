using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace powerOptimizerEFHStaeppel.Tests
{
    [TestFixture]
    public class LoggerTest
    {
        #region Properties

        public LoggerTestData TestData { get; } = new LoggerTestData();

        #endregion

        #region Setup

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
        }

        [SetUp]
        public void Setup()
        {
        }

        #endregion

        #region TearDown

        [OneTimeTearDown]
        public void CleanUp()
        {
        }

        [TearDown]
        public void TearDown()
        {
            TestData.Reset();
        }

        #endregion

        #region Tests

        [Test]
        public void AddMessageLine_ShouldAddMessageToMessageLineItems()
        {
            //Arrange
            var logger = TestData.Logger;

            //Act
            logger?.AddMessageLine(TestData.MessagePVPower);

            //Assert
            Assert.AreEqual(1, logger?.MessageLineItems.Count);
        }

        #endregion
    }
}
