using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;

namespace RefinedShell.Tests.Benchmark;

[MemoryDiagnoser]
public class ObjectCopy
{
    private ArgumentStruct[] _structs;
    private ArgumentClass[] _classes;

    private ArgumentStruct _str;
    private ArgumentClass _class;

    [GlobalSetup]
    public void Setup()
    {
        _structs = new ArgumentStruct[1000];
        _classes = new ArgumentClass[1000];

        _str = new ArgumentStruct();
        _class = new ArgumentClass();
    }

    [Benchmark]
    public void InitializeArray_Struct()
    {
        PassStruct(_str);
    }

    [Benchmark]
    public void InitializeArray_Class()
    {
        PassClass(_class);
    }

    //4 us
    private void PassStruct(ArgumentStruct @struct)
    {
        for (int i = 0; i < _structs.Length; i++)
        {
            _structs[i] = @struct;
        }
    }

    //2us
    private void PassClass(ArgumentClass @class)
    {
        for (int i = 0; i < _classes.Length; i++)
        {
            _classes[i] = @class;
        }
    }

    private readonly struct ArgumentStruct
    {
        public readonly Type Type;
        public readonly string Name;
    }

    private sealed class ArgumentClass
    {
        public readonly Type Type;
        public readonly string Name;
        public readonly bool IsOptional;
    }
}