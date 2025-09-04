namespace Chickensoft.GoDotTest;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using JetBrains.Annotations;

[AttributeUsage(AttributeTargets.All, AllowMultiple = false), MeansImplicitUse]

/// <summary>Base class for test method attributes.</summary>
public abstract class TestRunnerMethodAttribute : Attribute {
  /// <summary>
  /// Line the attribute was defined on.
  /// </summary>
  public readonly int Line;
  /// <summary>
  /// Creates a new MethodAttribute with the specified line number.
  /// </summary>
  /// <param name="line">Line number.</param>
  protected TestRunnerMethodAttribute([CallerLineNumber] int line = 0) {
    Line = line;
  }
}

/// <summary>
/// Attribute used to mark a method as a test.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class TestAttribute : TestRunnerMethodAttribute {
  /// <summary>
  /// Creates a new TestAttribute with the specified line number.
  /// </summary>
  /// <param name="line">Line number.</param>
  public TestAttribute([CallerLineNumber] int line = 0) : base(line) { }
}

// Many things in this draft of ideas were copied verbatim from xUnit and plopped right into GoDotTest,
// but we should probably remove anything unused and resolve style issues prior to adding a new feature.
// An advantage of using slimmed down xUnit implementations is
// that it would be easier for new parameterized test features to be added over time that are already in xUnit. We could
// start with just the most requested features, and then add less used ones over time until there is feature parity with
// xUnit parameterized tests.
//
// My team is interested in contributing the following features (but probably not more than that):
// 1. InlineDataAttribute
// 2. MemberDataAttribute with Static Methods that return IEnumerable<object?[]>
// 3. MemberData with Static Methods that return TheoryData (strongly typed)

/// <summary>
/// Attribute used to mark a method as a theory.
/// This attribute is roughly designed after <see href="https://github.com/xunit/xunit/blob/main/src/xunit.v3.core/TheoryAttribute.cs"/> but is not at feature parity.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class TheoryAttribute : TestRunnerMethodAttribute {
  /// <summary>
  /// Creates a new TheoryAttribute with the specified line number.
  /// </summary>
  /// <param name="line">Line number.</param>
  public TheoryAttribute([CallerLineNumber] int line = 0) : base(line) { }
}

/// <summary>
/// Base interface that all data attributes (that is, data providers for theories) are
/// expected to implement. Data attributes are valid on methods only.
/// </summary>
public interface IDataAttribute {
  /// <summary>
  /// Returns the data to be used to test the theory.
  /// </summary>
  /// <param name="testMethod">The test method the data attribute is attached to</param>
  ValueTask<IReadOnlyCollection<ITheoryDataRow>> GetData(MethodInfo testMethod);
}

/// <summary>
/// Attribute used to mark a method as a theory.
/// This attribute is roughly designed after <see href="https://github.com/xunit/xunit/blob/main/src/xunit.v3.core/Attributes/DataAttribute.cs"/> but is not at feature parity.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public abstract class DataAttribute : Attribute, IDataAttribute {
  /// <inheritdoc/>
  public abstract ValueTask<IReadOnlyCollection<ITheoryDataRow>> GetData(MethodInfo testMethod);
}

/// <summary>
/// Attribute used to add inline test case data for a theory.
/// This attribute is roughly designed after <see href="https://github.com/xunit/xunit/blob/main/src/xunit.v3.core/InlineDataAttribute.cs"/> but is not at feature parity.
/// </summary>
/// <param name="data">The data values to pass to the theory.</param>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class InlineDataAttribute(params object?[]? data) : DataAttribute {
  /// <summary>
  /// Gets the data to be passed to the test.
  /// </summary>
  // If the user passes null to the constructor, we assume what they meant was a
  // single null value to be passed to the test.
  public object?[] Data { get; } = data ?? [null];

  public override ValueTask<IReadOnlyCollection<ITheoryDataRow>> GetData(MethodInfo testMethod) => throw new NotImplementedException();
}

/// <summary>
/// Attribute used to add test case data to a test from a public static method (with parameters).
/// This attribute is roughly designed after <see href="https://github.com/xunit/xunit/blob/main/src/xunit.v3.core/MemberDataAttribute.cs"/> but is not at feature parity.
/// </summary>
/// <param name="memberName">
/// The name of the public static member on the test class that will provide the test data
/// It is recommended to use the <c>nameof</c> operator to ensure compile-time safety, e.g., <c>nameof(SomeMemberName)</c>.
/// </param>
/// <param name="arguments">The arguments to be passed to the member (only supported for methods; ignored for everything else)</param>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class MemberDataAttribute(string memberName, params object?[] arguments) : DataAttribute {
  /// <summary>
  /// Gets the member name.
  /// </summary>
  public string MemberName { get; } = memberName;

  /// <summary>
  /// Gets or sets the arguments passed to the member. Only supported for static methods.
  /// </summary>
  public object?[] Arguments { get; } = arguments;

  public override ValueTask<IReadOnlyCollection<ITheoryDataRow>> GetData(MethodInfo testMethod) => throw new NotImplementedException();
}

/// <summary>
/// Attribute used to mark a setup method to be called once before all the
/// tests run.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class SetupAllAttribute : TestRunnerMethodAttribute {
  /// <summary>
  /// Creates a new SetupAllAttribute with the specified line number.
  /// </summary>
  /// <param name="line">Line number.</param>
  public SetupAllAttribute([CallerLineNumber] int line = 0) : base(line) { }
}

/// <summary>
/// Attribute used to mark a cleanup method to be called once after all the
/// tests run.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class CleanupAllAttribute : TestRunnerMethodAttribute {
  /// <summary>
  /// Creates a new CleanupAllAttribute with the specified line number.
  /// </summary>
  /// <param name="line">Line number.</param>
  public CleanupAllAttribute([CallerLineNumber] int line = 0) : base(line) { }
}

/// <summary>
/// Attribute used to mark a setup method to be called before each test runs.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class SetupAttribute : TestRunnerMethodAttribute {
  /// <summary>
  /// Creates a new SetupAttribute with the specified line number.
  /// </summary>
  /// <param name="line">Line number.</param>
  public SetupAttribute([CallerLineNumber] int line = 0) : base(line) { }
}

/// <summary>
/// Attribute used to mark a cleanup method to be called after each test runs.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class CleanupAttribute : TestRunnerMethodAttribute {
  /// <summary>
  /// Creates a new CleanupAttribute with the specified line number.
  /// </summary>
  /// <param name="line">Line number.</param>
  public CleanupAttribute([CallerLineNumber] int line = 0) : base(line) { }
}

/// <summary>
/// Attribute used to mark a method to be called when a test suite encounters a
/// failure.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class FailureAttribute : TestRunnerMethodAttribute {
  /// <summary>
  /// Creates a new FailureAttribute with the specified line number.
  /// </summary>
  /// <param name="line">Line number.</param>
  public FailureAttribute([CallerLineNumber] int line = 0) : base(line) { }
}

/// <summary>
/// Test method attribute used to customize the timeout duration of a test
/// method. This overrides any global timeout settings.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class TimeoutAttribute : Attribute {
  /// <summary>Test method timeout, in milliseconds.</summary>
  public int TimeoutMilliseconds { get; }
  /// <summary>
  /// Create a new timeout attribute with the specified timeout.
  /// </summary>
  /// <param name="timeoutMilliseconds">Number of milliseconds to wait
  /// before timing out.</param>
  public TimeoutAttribute(int timeoutMilliseconds) {
    TimeoutMilliseconds = timeoutMilliseconds;
  }
}

/// <summary>
/// Test suite class attribute which indicates the test methods in the class
/// depend on the success of the previous method. If this attribute is
/// applied, all of the test and utility methods in the test suite must
/// succeed. If a method fails, the remaining methods in the suite will not
/// be run.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class SequentialAttribute : Attribute {
  /// <summary>Create a new sequential attribute.</summary>
  public SequentialAttribute() { }
}
