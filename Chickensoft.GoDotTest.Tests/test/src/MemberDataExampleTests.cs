namespace Chickensoft.GoDotTest.Tests;

using System.Collections.Generic;
using Chickensoft.GoDotTest;
using Godot;
using Shouldly;

public class MemberDataExampleTests : TestClass {
  public MemberDataExampleTests(Node testScene) : base(testScene) { }

  public static IEnumerable<object[]> GetUntypedTestData() => [
    [1, 2, 3],
    [-4, -6, -10],
    [-2, 2, 0]
  ];

  public static TheoryData<int, int, int> GetStronglyTypedTestData() => new() {
    { 1, 2, 3 },
    { -4, -6, -10 },
    { -2, 2, 0 },
  };

  [Theory]
  [MemberData(nameof(GetUntypedTestData))]
  [MemberData(nameof(GetStronglyTypedTestData))]
  public void CanAddExample(int a, int b, int expected)
    => (a + b).ShouldBe(expected);
}
