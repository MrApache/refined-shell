using System.Reflection;
using BenchmarkDotNet.Attributes;

namespace RefinedShell.Tests.Benchmark;

public class MethodInvokation
{
    private readonly Class _class = new Class();

    [Benchmark]
    public void InvokeDirect()
    {
        _class.Method();
    }

    [Benchmark]
    public void InvokeReflection()
    {
        MethodInfo method = typeof(Class).GetMethod("Method", BindingFlags.Instance | BindingFlags.Public | BindingFlags.Default)!;
        method.Invoke(_class, null);
    }

    private sealed partial class Class
    {
        private int _i;

        public void Method()
        {
            _i++;
        }

        private void PrivateMethod(float x, float y, float z)
        {

        }
    }

    private sealed partial class Class
    {
        public void Start(object x, object y, object z)
        {
            float xPos = (float)x;
            float yPos = (float)y;
            float zPos = (float)z;
            PrivateMethod(xPos, yPos, zPos);
        }
    }
}