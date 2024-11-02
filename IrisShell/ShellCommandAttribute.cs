using System;
using JetBrains.Annotations;

namespace IrisShell
{
    [MeansImplicitUse]
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ShellCommandAttribute : Attribute
    {
        public readonly string? Name;

        public ShellCommandAttribute()
        {
        }

        public ShellCommandAttribute(string name)
        {
            Name = name;
        }
    }
}