namespace Chickensoft.GoDotTest;

using System;
using System.Collections;
using System.Collections.Generic;

public abstract class TestDataBase<TTestDataRow, TRawTestDataRow> : IReadOnlyCollection<TTestDataRow>
  where TTestDataRow : class, ITestDataRow {
  private readonly List<TTestDataRow> _data = [];

  public int Count => _data.Count;

  public void Add(TRawTestDataRow row) => _data.Add(Convert(row));

  public virtual void Add(TTestDataRow row) {
    ArgumentNullException.ThrowIfNull(row);
    _data.Add(row);
  }

  public void AddRange(IEnumerable<TRawTestDataRow> rows) {
    foreach (var row in rows) {
      ArgumentNullException.ThrowIfNull(row);
      Add(row);
    }
  }

  public void AddRange(params TRawTestDataRow[] rows) {
    foreach (var row in rows) {
      ArgumentNullException.ThrowIfNull(row);
      Add(row);
    }
  }

  public void AddRange(IEnumerable<TTestDataRow> rows) {
    foreach (var row in rows) {
      ArgumentNullException.ThrowIfNull(row);
      Add(row);
    }
  }

  public void AddRange(params TTestDataRow[] rows) {
    foreach (var row in rows) {
      ArgumentNullException.ThrowIfNull(row);
      Add(row);
    }
  }

  protected abstract TTestDataRow Convert(TRawTestDataRow row);

  public IEnumerator<TTestDataRow> GetEnumerator() => _data.GetEnumerator();

  IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public abstract class TestData : TestDataBase<TestDataRow, object?[]> {
  protected override TestDataRow Convert(object?[] row) => new(row);
}

public class TestData<T> : TestDataBase<TestDataRow<T>, T> {
  public TestData() { }

  public override void Add(TestDataRow<T> row) => base.Add(row ?? Convert(default!));

  public TestData(IEnumerable<T> rows) {
    AddRange(rows);
  }

  public TestData(params T[] rows) {
    AddRange(rows);
  }

  public TestData(IEnumerable<TestDataRow<T>> rows) {
    AddRange(rows);
  }

  public TestData(params TestDataRow<T>[] rows) {
    AddRange(rows);
  }

  protected override TestDataRow<T> Convert(T row) => new(row);
}

public class TestData<T1, T2> : TestDataBase<TestDataRow<T1, T2>, (T1, T2)> {
  public TestData() { }

  public void Add(T1 p1, T2 p2) => Add(new TestDataRow<T1, T2>(p1, p2));

  public TestData(IEnumerable<(T1, T2)> rows) {
    AddRange(rows);
  }

  public TestData(params (T1, T2)[] rows) {
    AddRange(rows);
  }

  public TestData(IEnumerable<TestDataRow<T1, T2>> rows) {
    AddRange(rows);
  }

  public TestData(params TestDataRow<T1, T2>[] rows) {
    AddRange(rows);
  }

  protected override TestDataRow<T1, T2> Convert((T1, T2) row) => new(row.Item1, row.Item2);
}

public class TestData<T1, T2, T3> : TestDataBase<TestDataRow<T1, T2, T3>, (T1, T2, T3)> {
  public TestData() { }

  public void Add(T1 p1, T2 p2, T3 p3) => Add(new TestDataRow<T1, T2, T3>(p1, p2, p3));

  public TestData(IEnumerable<(T1, T2, T3)> rows) {
    AddRange(rows);
  }

  public TestData(params (T1, T2, T3)[] rows) {
    AddRange(rows);
  }

  public TestData(IEnumerable<TestDataRow<T1, T2, T3>> rows) {
    AddRange(rows);
  }

  public TestData(params TestDataRow<T1, T2, T3>[] rows) {
    AddRange(rows);
  }

  protected override TestDataRow<T1, T2, T3> Convert((T1, T2, T3) row) => new(row.Item1, row.Item2, row.Item3);
}

// We can add support for as many strongly typed parameters as desired.
