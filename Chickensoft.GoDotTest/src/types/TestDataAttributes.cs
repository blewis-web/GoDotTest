namespace Chickensoft.GoDotTest;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

/// <summary>
/// Base Attribute used to add data for a test.
/// </summary>
public abstract class DataAttribute : Attribute {
  /// <summary>
  /// Gets the untyped data to be passed to the test.
  /// </summary>
  /// <param name="testClassType">The type of the test class.</param>
  public abstract object?[] GetRawData(Type testClassType);
}

/// <summary>
/// Attribute used to add inline data for a single test case.
/// See <see href="https://github.com/xunit/xunit/blob/main/src/xunit.v3.core/InlineDataAttribute.cs"/> and
/// <see href="https://github.com/MikeSchulze/gdUnit4Net/blob/master/Api/src/core/attributes/TestCaseAttribute.cs"/>
/// for reference, but customize the solution to keep GoDotTest as light as possible.
/// </summary>
/// <param name="data">The data values to pass to the theory.</param>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class InlineDataAttribute(params object?[]? data) : DataAttribute {
  /// <inheritdoc/>
  // If the user passes null to the constructor, we assume what they meant was a
  // single null value to be passed to the test.
  public override object?[] GetRawData(Type testClassType) => data ?? [null];
}

/// <summary>
/// Attribute used to provide test data for one or more test cases via a public static method.
/// See <see href="https://github.com/xunit/xunit/blob/main/src/xunit.v3.core/MemberDataAttribute.cs"/> and
/// <see href="https://github.com/MikeSchulze/gdUnit4Net/blob/master/Api/src/core/attributes/DataPointAttribute.cs"/>
/// for reference, but customize the solution to keep GoDotTest as light as possible.
/// </summary>
/// <param name="memberName">
/// The name of the public static member on the test class that will provide the test data.
/// It is recommended to use the <c>nameof</c> operator to ensure compile-time safety, e.g., <c>nameof(SomeMemberName)</c>.
/// </param>
/// <param name="arguments">Arguments needed by the static method to generate test cases.</param>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class MemberDataAttribute(string memberName, params object?[] arguments) : DataAttribute {
  /// <summary>
  /// Gets the member name.
  /// </summary>
  public string MemberName { get; } = memberName;

  public object?[] Arguments { get; } = arguments;

  public override object?[] GetRawData(Type testClassType) {
    var accessor = GetMethodAccessor(testClassType);
    if (accessor is null) {
      return [];
    }

    return accessor();
  }

  private Func<object?[]>? GetMethodAccessor(Type testClassType) {
    MethodInfo? methodInfo = null;

    var argumentTypes = Arguments is null ? [] : Arguments.Select(arg => arg?.GetType()).ToArray();

    foreach (var t in GetTestClassTypes(testClassType, includeInterfaces: true)) {
      var methodInfoArray = t.GetRuntimeMethods()
        .Where(m => m.IsStatic
          && m.Name == MemberName
          && ParameterTypesCompatible(m.GetParameters(), argumentTypes))
        .ToArray();
      if (methodInfoArray.Length == 0) {
        continue;
      }
      if (methodInfoArray.Length == 1) {
        methodInfo = methodInfoArray[0];
        break;
      }

      methodInfo = methodInfoArray
        .FirstOrDefault(m => m.GetParameters().Length == argumentTypes.Length);
      if (methodInfo is not null) {
        break;
      }

      throw new ArgumentException($"The call to method '{testClassType.Name}.{MemberName}'" +
        $" is ambiguous between {methodInfoArray.Length} different options for the given arguments.");
    }

    if (methodInfo is null || !methodInfo.IsStatic) {
      return null;
    }

    return () => (object?[]?)methodInfo.Invoke(null, Arguments ?? []) ?? [];
  }

  private static bool ParameterTypesCompatible(ParameterInfo[] parameters, Type?[] argumentTypes)
    => throw new NotImplementedException();

  private static IEnumerable<Type> GetTestClassTypes(Type testClassType, bool includeInterfaces) {
    HashSet<Type> interfaces = [];

    for (var t = testClassType; t is not null; t = t.BaseType) {
      yield return t;

      if (includeInterfaces) {
        foreach (var @interface in t.GetInterfaces()) {
          interfaces.Add(@interface);
        }
      }
    }

    foreach (var @interface in interfaces) {
      yield return @interface;
    }
  }
}
