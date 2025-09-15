namespace Chickensoft.GoDotTest;

using System.Collections.Generic;

/// <summary>
/// Represents a single test case in a Test (a method with the TestAttribute
/// which can have one or more test cases).
/// </summary>
/// <param name="ExecutionSequence">
/// Sequence of test methods to be executed to run a single test case.
/// </param>
/// <param name="RawDataRow">
/// The untyped data to be used in this test case for a parameterized method.
/// </param>
public record TestCase(IEnumerable<ITestMethod> ExecutionSequence, object?[] RawDataRow);
