# Assertion-Source-Info
Adds the statement which caused the assertion failure to the assertion error message.

# How to use with NUnit
```
[SetUpFixture]
public class AssemblySetUp
{
	[SetUp]
	public void SetUp()
	{
		Gtc.AssertionSourceInfo.AssertionImproverFixtureSetup.Install();
	}
	
	[TearDown]
	public void TearDown()
	{
		Gtc.AssertionSourceInfo.AssertionImproverFixtureSetup.Uninstall();
	}
}
```
