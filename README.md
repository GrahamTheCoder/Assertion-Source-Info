# Assertion-Source-Info
Adds the statement which caused the assertion failure to the assertion error message.

## Example
```
Assert.That("baboon".IndexOf("boo"), Is.EqualTo(3));
```
The normal NUnit message would be of the form:

Expected 3 but was 2

With this test helper it's of the form:

In: Assert.That("baboon".IndexOf("boo"), Is.EqualTo(3)); Expected 3 but was 2


## How to use with NUnit
```csharp
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
