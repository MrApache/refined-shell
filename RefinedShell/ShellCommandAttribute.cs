using System;
using JetBrains.Annotations;

namespace RefinedShell
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