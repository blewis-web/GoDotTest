namespace Chickensoft.GoDotTest;

using System;

public interface ITestDataRow {
  object?[] GetData();
}

public abstract class TestDataRowBase : ITestDataRow {
  protected abstract object?[] GetData();

  object?[] ITestDataRow.GetData() => GetData();
}

public class TestDataRow(params object?[] data) : TestDataRowBase {
  public object?[] Data => data;

  protected override object?[] GetData() => data;
}

public sealed class TestDataRow<T1>(T1 p1) : TestDataRowBase {
  public T1 Data => p1;

  protected override object?[] GetData() => [p1];

  public static implicit operator TestDataRow<T1>(T1 p1) => new(p1);

  public static implicit operator T1(TestDataRow<T1> p1) {
    ArgumentNullException.ThrowIfNull(p1);

    return p1.Data;
  }
}

public sealed class TestDataRow<T1, T2>(T1 p1, T2 p2) : TestDataRowBase {
  public (T1, T2) Data => (p1, p2);

  protected override object?[] GetData() => [p1, p2];

  public static implicit operator TestDataRow<T1, T2>((T1, T2) row) => new(row.Item1, row.Item2);
}

public sealed class TestDataRow<T1, T2, T3>(T1 p1, T2 p2, T3 p3) : TestDataRowBase {
  public (T1, T2, T3) Data => (p1, p2, p3);

  protected override object?[] GetData() => [p1, p2, p3];

  public static implicit operator TestDataRow<T1, T2, T3>((T1, T2, T3) row) => new(row.Item1, row.Item2, row.Item3);
}

// We can add support for as many strongly typed parameters as desired.
