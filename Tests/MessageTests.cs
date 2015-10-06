using Gtc.AssertionSourceInfo;
using NUnit.Framework;
using System;

namespace Tests
{
    /// <summary>
    /// Do not reformat this class, the line spacing is part of the test
    /// </summary>
    [TestFixture]
    public class MessageMutationTests
    {
        [TestFixtureSetUp]
        public void Install()
        {
            AssertionImproverFixtureSetup.Install();
        }

        [TestFixtureTearDown]
        public void Uninstall()
        {
            AssertionImproverFixtureSetup.Uninstall();
        }

        [Test]
        public void SingleLine()
        {
            try
            {
                Assert.That(1, Is.EqualTo(2));
            }
            catch (Exception e)
            {
                TestMessageMutated(e, " Assert.That(1, Is.EqualTo(2));");
            }
        }

        [Test]
        public void MultiLine()
        {
            try
            {
                Assert.That(1,
                    Is.EqualTo(2));
            }
            catch (Exception e)
            {
                TestMessageMutated(e, @"
 Assert.That(1,
     Is.EqualTo(2));");
            }
        }

        private static void TestMessageMutated(Exception e, string failedAssertionDescription)
        {
            Assert.That(e.Message, Is.StringStarting($"In:{failedAssertionDescription}\r\n"), "Should have added a prefix");
            Assert.That(e.Message, Is.StringContaining($"Expected:"), "Normal message should still be present");
        }
    }
}
