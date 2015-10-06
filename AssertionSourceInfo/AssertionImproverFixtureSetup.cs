using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ExceptionServices;

namespace Gtc.AssertionSourceInfo
{
    public static class AssertionImproverFixtureSetup
    {
        /// <summary>
        /// Call this from an assembly fixture setup to improve your addin
        /// </summary>
        public static void Install()
        {
            AppDomain.CurrentDomain.FirstChanceException += AddAssertionInfo;
        }

        public static void Uninstall()
        {
            AppDomain.CurrentDomain.FirstChanceException -= AddAssertionInfo;
        }

        private static void AddAssertionInfo(object sender, FirstChanceExceptionEventArgs args)
        {
            AssertionImprover.AddAssertionSourceIfAvailable(args.Exception, new StackTrace(true).GetFrames().Skip(1));
        }
    }
}