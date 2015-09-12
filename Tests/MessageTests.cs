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
            var originalMessage = e.Message;
            AssertionImprover.AddAssertionSourceIfAvailable(e);
            Assert.That(e.Message, Is.EqualTo($"In:{failedAssertionDescription}\r\n" + originalMessage));
        }
    }
}
