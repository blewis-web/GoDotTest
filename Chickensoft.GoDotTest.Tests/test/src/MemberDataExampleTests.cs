namespace Chickensoft.GoDotTest.Tests;

using System.Collections.Generic;
using System.Linq;
using Chickensoft.GoDotTest;
using Godot;
using Shouldly;

public class MemberDataExampleTests : TestClass {
  public MemberDataExampleTests(Node testScene) : base(testScene) { }

  // Getting this to work would be the second PR/step.

  public static IEnumerable<object[]> GetUntypedTestData() => [
    [1, 2, 3],
    [-4, -6, -10],
    [-2, 2, 0]
  ];

  [Test]
  [MemberData(nameof(GetUntypedTestData))]
  public void CanAddExampleWithUntypedTestData(int a, int b, int expected)
    => (a + b).ShouldBe(expected);


  // Getting this to work would be the third PR/step.

  public static TestData<int, int, int> GetStronglyTypedTestData() => new() {
    { 1, 2, 3 },
    { -4, -6, -10 },
    { -2, 2, 0 },
  };

  [Test]
  [MemberData(nameof(GetStronglyTypedTestData))]
  public void CanAddExampleWithStronglyTypedTestData(int a, int b, int expected)
    => (a + b).ShouldBe(expected);


  // Getting these to work would be the fourth PR/step.

  public static IEnumerable<object[]> GetUntypedTestDataGeneratedWithArgument(int factor)
    => GetUntypedTestData().ToList()
      .ConvertAll(row => row.Select(x => (object)((int)x * factor)).ToArray());

  public static TestData<int, int, int> GetStronglyTypedTestDataGeneratedWithArgument(int factor)
    => [.. GetStronglyTypedTestData().Select(row
      => (row.Data.Item1 * factor, row.Data.Item2 * factor, row.Data.Item3 * factor))];

  [Test]
  [MemberData(nameof(GetUntypedTestDataGeneratedWithArgument), 0)]
  [MemberData(nameof(GetUntypedTestDataGeneratedWithArgument), 1)]
  [MemberData(nameof(GetUntypedTestDataGeneratedWithArgument), 2)]
  [MemberData(nameof(GetStronglyTypedTestDataGeneratedWithArgument), 0)]
  [MemberData(nameof(GetStronglyTypedTestDataGeneratedWithArgument), 1)]
  [MemberData(nameof(GetStronglyTypedTestDataGeneratedWithArgument), 2)]
  public void CanAddExampleWithArguments(int a, int b, int expected)
    => (a + b).ShouldBe(expected);
}
