namespace Chickensoft.GoDotTest.Tests;

using Chickensoft.GoDotTest;
using Godot;
using Shouldly;

// I couldn't find examples of automated testing for either xUnit or gdUnit on
// the attributes themselves, so I'm not sure how that could be done, but we
// can try to dive into that in implementation. It is possible that I just
// missed it. In the worst case, maybe we can include some non-exhaustive
// example tests that run?
public class InlineDataExampleTests : TestClass {
  public InlineDataExampleTests(Node testScene) : base(testScene) { }

  // Getting this to work would be the first PR/step.
  [Test]
  [InlineData(1, 2, 3)]
  [InlineData(-4, -6, -10)]
  [InlineData(-2, 2, 0)]
  public void CanAddExample(int a, int b, int expected)
    => (a + b).ShouldBe(expected);
}
