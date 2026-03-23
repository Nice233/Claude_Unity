using System;
using System.Diagnostics;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Unity.Profiling;
using UnityEngine.Profiling;

[Serializable]
public class ArrayTypePerformanceTests
{
    private const int ArraySize = 10000;
    private const int Iterations = 1000;

    #region 测试用数据类型

    private struct TestItemStruct
    {
        public int Id;
        public string Name;
    }

    private class TestItemClass
    {
        public int Id;
        public string Name;
    }

    #endregion

    #region 值类型测试 (struct)

    [Test]
    public void TestStructArray_Add()
    {
        using var profiler = new ProfilerMarker("StructArray_Add").Auto();
        var array = new TestItemStruct[ArraySize];
        for (int i = 0; i < ArraySize; i++)
            array[i] = new TestItemStruct { Id = i, Name = "Item" + i };

        for (int iter = 0; iter < Iterations; iter++)
        {
            var value = new TestItemStruct { Id = iter, Name = "New" + iter };
            if (array.Length > 0)
                array[array.Length - 1] = value;
        }
        UnityEngine.Debug.Log($"Struct Add completed");
    }

    [Test]
    public void TestStructArray_Search()
    {
        using var profiler = new ProfilerMarker("StructArray_Search").Auto();
        var array = new TestItemStruct[ArraySize];
        for (int i = 0; i < ArraySize; i++)
            array[i] = new TestItemStruct { Id = i, Name = "Item" + i };

        for (int iter = 0; iter < Iterations; iter++)
        {
            int target = iter % ArraySize;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Id == target) break;
            }
        }
        UnityEngine.Debug.Log($"Struct Search completed");
    }

    [Test]
    public void TestStructArray_Modify()
    {
        using var profiler = new ProfilerMarker("StructArray_Modify").Auto();
        var array = new TestItemStruct[ArraySize];
        for (int i = 0; i < ArraySize; i++)
            array[i] = new TestItemStruct { Id = i, Name = "Item" + i };

        for (int iter = 0; iter < Iterations; iter++)
        {
            int idx = iter % ArraySize;
            array[idx] = new TestItemStruct { Id = iter + 1000, Name = "Modified" + iter };
        }
        UnityEngine.Debug.Log($"Struct Modify completed");
    }

    [Test]
    public void TestStructArray_Remove()
    {
        using var profiler = new ProfilerMarker("StructArray_Remove").Auto();
        var array = new TestItemStruct[ArraySize];
        for (int i = 0; i < ArraySize; i++)
            array[i] = new TestItemStruct { Id = i, Name = "Item" + i };

        for (int iter = 0; iter < Iterations && ArraySize > 1; iter++)
        {
            int removeIdx = iter % (ArraySize - 1);
            for (int i = removeIdx; i < ArraySize - 1; i++)
            {
                array[i] = array[i + 1];
            }
        }
        UnityEngine.Debug.Log($"Struct Remove completed");
    }

    #endregion

    #region 引用类型测试 (class)

    [Test]
    public void TestClassArray_Add()
    {
        using var profiler = new ProfilerMarker("ClassArray_Add").Auto();
        var array = new TestItemClass[ArraySize];
        for (int i = 0; i < ArraySize; i++)
            array[i] = new TestItemClass { Id = i, Name = "Item" + i };

        for (int iter = 0; iter < Iterations; iter++)
        {
            var item = new TestItemClass { Id = iter, Name = "New" + iter };
            if (array.Length > 0)
                array[array.Length - 1] = item;
        }
        UnityEngine.Debug.Log($"Class Add completed");
    }

    [Test]
    public void TestClassArray_Search()
    {
        using var profiler = new ProfilerMarker("ClassArray_Search").Auto();
        var array = new TestItemClass[ArraySize];
        for (int i = 0; i < ArraySize; i++)
            array[i] = new TestItemClass { Id = i, Name = "Item" + i };

        for (int iter = 0; iter < Iterations; iter++)
        {
            int target = iter % ArraySize;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] != null && array[i].Id == target) break;
            }
        }
        UnityEngine.Debug.Log($"Class Search completed");
    }

    [Test]
    public void TestClassArray_Modify()
    {
        using var profiler = new ProfilerMarker("ClassArray_Modify").Auto();
        var array = new TestItemClass[ArraySize];
        for (int i = 0; i < ArraySize; i++)
            array[i] = new TestItemClass { Id = i, Name = "Item" + i };

        for (int iter = 0; iter < Iterations; iter++)
        {
            int idx = iter % ArraySize;
            if (array[idx] != null)
            {
                array[idx].Id = iter + 1000;
                array[idx].Name = "Modified" + iter;
            }
        }
        UnityEngine.Debug.Log($"Class Modify completed");
    }

    [Test]
    public void TestClassArray_Remove()
    {
        using var profiler = new ProfilerMarker("ClassArray_Remove").Auto();
        var array = new TestItemClass[ArraySize];
        for (int i = 0; i < ArraySize; i++)
            array[i] = new TestItemClass { Id = i, Name = "Item" + i };

        for (int iter = 0; iter < Iterations && ArraySize > 1; iter++)
        {
            int removeIdx = iter % (ArraySize - 1);
            for (int i = removeIdx; i < ArraySize - 1; i++)
            {
                array[i] = array[i + 1];
            }
        }
        UnityEngine.Debug.Log($"Class Remove completed");
    }

    #endregion

    #region 完整性能对比测试

    [Test]
    public void TestPerformanceComparison()
    {
        using var profiler = new ProfilerMarker("PerformanceComparison").Auto();

        var sb = new System.Text.StringBuilder();
        sb.AppendLine("=== 数组类型性能测试 ===");
        sb.AppendLine($"数组规模: {ArraySize}, 操作迭代: {Iterations}");
        sb.AppendLine();

        // 值类型测试
        var structAdd = MeasureStructAdd();
        var structSearch = MeasureStructSearch();
        var structModify = MeasureStructModify();
        var structRemove = MeasureStructRemove();

        sb.AppendLine("--- 值类型 (struct[]) ---");
        sb.AppendLine($"添加: {structAdd}ms");
        sb.AppendLine($"查询: {structSearch}ms");
        sb.AppendLine($"修改: {structModify}ms");
        sb.AppendLine($"删除: {structRemove}ms");

        // 引用类型测试
        var classAdd = MeasureClassAdd();
        var classSearch = MeasureClassSearch();
        var classModify = MeasureClassModify();
        var classRemove = MeasureClassRemove();

        sb.AppendLine();
        sb.AppendLine("--- 引用类型 (class[]) ---");
        sb.AppendLine($"添加: {classAdd}ms");
        sb.AppendLine($"查询: {classSearch}ms");
        sb.AppendLine($"修改: {classModify}ms");
        sb.AppendLine($"删除: {classRemove}ms");

        // 性能对比
        sb.AppendLine();
        sb.AppendLine("--- 性能对比 (class/struct) ---");
        sb.AppendLine($"添加: {(classAdd / (float)structAdd):F2}x");
        sb.AppendLine($"查询: {(classSearch / (float)structSearch):F2}x");
        sb.AppendLine($"修改: {(classModify / (float)structModify):F2}x");
        sb.AppendLine($"删除: {(classRemove / (float)structRemove):F2}x");

        UnityEngine.Debug.Log(sb.ToString());
        UnityEngine.Debug.Log("=== 测试完成，请在 Profiler 面板中查看结果 ===");

        Assert.Pass("性能测试完成");
        Profiler.enabled = false;
    }

    private long MeasureStructAdd()
    {
        using var profiler = new ProfilerMarker("MeasureStructAdd").Auto();
        var array = new TestItemStruct[ArraySize];
        for (int i = 0; i < ArraySize; i++)
            array[i] = new TestItemStruct { Id = i, Name = "Item" + i };

        var sw = Stopwatch.StartNew();
        for (int iter = 0; iter < Iterations; iter++)
        {
            var value = new TestItemStruct { Id = iter, Name = "New" + iter };
            if (array.Length > 0)
                array[array.Length - 1] = value;
        }
        sw.Stop();
        return sw.ElapsedMilliseconds;
    }

    private long MeasureStructSearch()
    {
        using var profiler = new ProfilerMarker("MeasureStructSearch").Auto();
        var array = new TestItemStruct[ArraySize];
        for (int i = 0; i < ArraySize; i++)
            array[i] = new TestItemStruct { Id = i, Name = "Item" + i };

        var sw = Stopwatch.StartNew();
        for (int iter = 0; iter < Iterations; iter++)
        {
            int target = iter % ArraySize;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Id == target) break;
            }
        }
        sw.Stop();
        return sw.ElapsedMilliseconds;
    }

    private long MeasureStructModify()
    {
        using var profiler = new ProfilerMarker("MeasureStructModify").Auto();
        var array = new TestItemStruct[ArraySize];
        for (int i = 0; i < ArraySize; i++)
            array[i] = new TestItemStruct { Id = i, Name = "Item" + i };

        var sw = Stopwatch.StartNew();
        for (int iter = 0; iter < Iterations; iter++)
        {
            int idx = iter % ArraySize;
            array[idx] = new TestItemStruct { Id = iter + 1000, Name = "Modified" + iter };
        }
        sw.Stop();
        return sw.ElapsedMilliseconds;
    }

    private long MeasureStructRemove()
    {
        using var profiler = new ProfilerMarker("MeasureStructRemove").Auto();
        var array = new TestItemStruct[ArraySize];
        for (int i = 0; i < ArraySize; i++)
            array[i] = new TestItemStruct { Id = i, Name = "Item" + i };

        var sw = Stopwatch.StartNew();
        for (int iter = 0; iter < Iterations && ArraySize > 1; iter++)
        {
            int removeIdx = iter % (ArraySize - 1);
            for (int i = removeIdx; i < ArraySize - 1; i++)
            {
                array[i] = array[i + 1];
            }
        }
        sw.Stop();
        return sw.ElapsedMilliseconds;
    }

    private long MeasureClassAdd()
    {
        using var profiler = new ProfilerMarker("MeasureClassAdd").Auto();
        var array = new TestItemClass[ArraySize];
        for (int i = 0; i < ArraySize; i++)
            array[i] = new TestItemClass { Id = i, Name = "Item" + i };

        var sw = Stopwatch.StartNew();
        for (int iter = 0; iter < Iterations; iter++)
        {
            var item = new TestItemClass { Id = iter, Name = "New" + iter };
            if (array.Length > 0)
                array[array.Length - 1] = item;
        }
        sw.Stop();
        return sw.ElapsedMilliseconds;
    }

    private long MeasureClassSearch()
    {
        using var profiler = new ProfilerMarker("MeasureClassSearch").Auto();
        var array = new TestItemClass[ArraySize];
        for (int i = 0; i < ArraySize; i++)
            array[i] = new TestItemClass { Id = i, Name = "Item" + i };

        var sw = Stopwatch.StartNew();
        for (int iter = 0; iter < Iterations; iter++)
        {
            int target = iter % ArraySize;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] != null && array[i].Id == target) break;
            }
        }
        sw.Stop();
        return sw.ElapsedMilliseconds;
    }

    private long MeasureClassModify()
    {
        using var profiler = new ProfilerMarker("MeasureClassModify").Auto();
        var array = new TestItemClass[ArraySize];
        for (int i = 0; i < ArraySize; i++)
            array[i] = new TestItemClass { Id = i, Name = "Item" + i };

        var sw = Stopwatch.StartNew();
        for (int iter = 0; iter < Iterations; iter++)
        {
            int idx = iter % ArraySize;
            if (array[idx] != null)
            {
                array[idx].Id = iter + 1000;
                array[idx].Name = "Modified" + iter;
            }
        }
        sw.Stop();
        return sw.ElapsedMilliseconds;
    }

    private long MeasureClassRemove()
    {
        using var profiler = new ProfilerMarker("MeasureClassRemove").Auto();
        var array = new TestItemClass[ArraySize];
        for (int i = 0; i < ArraySize; i++)
            array[i] = new TestItemClass { Id = i, Name = "Item" + i };

        var sw = Stopwatch.StartNew();
        for (int iter = 0; iter < Iterations && ArraySize > 1; iter++)
        {
            int removeIdx = iter % (ArraySize - 1);
            for (int i = removeIdx; i < ArraySize - 1; i++)
            {
                array[i] = array[i + 1];
            }
        }
        sw.Stop();
        return sw.ElapsedMilliseconds;
    }

    #endregion
}