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

  [Test]
  [MemberData(nameof(GetUntypedTestData))]
  public void CanAddExampleWithUntypedTestData(int a, int b, int expected)
    => (a + b).ShouldBe(expected);


  // public static TheoryData<int, int, int> GetStronglyTypedTestData() => new() {
  //   { 1, 2, 3 },
  //   { -4, -6, -10 },
  //   { -2, 2, 0 },
  // };

  // [Test]
  // [MemberData(nameof(GetStronglyTypedTestData))]
  // public void CanAddExampleWithStronglyTypedTestData(int a, int b, int expected)
  // => (a + b).ShouldBe(expected);


  // public static IEnumerable<object[]> GetUntypedTestDataWithArguments(int factor)
  //   => GetUntypedTestData().ToList()
  //     .ConvertAll(dataRow => dataRow.Select(x => (object)((int)x * factor)).ToArray());

  // Arguments for the static methods that provide data should probably be a
  // separate feature / PR (maybe feature 4 in the parameterized tests series now)?
  // [Test]
  // [MemberData(nameof(GetUntypedTestDataWithArguments), 0)]
  // [MemberData(nameof(GetUntypedTestDataWithArguments), 1)]
  // [MemberData(nameof(GetUntypedTestDataWithArguments), 2)]
  // public void CanAddExampleWithArguments(int a, int b, int expected)
  //   => (a + b).ShouldBe(expected);
}
