using System;
using System.Reflection;

namespace RefinedShell.Commands
{
    public sealed class Argument
    {
        public readonly Type Type;
        public readonly string Name;
        public readonly bool IsOptional;

        public Argument(Type type, string name, bool isOptional)
        {
            Type = type;
            Name = name;
            IsOptional = isOptional;
        }

        public Argument(ParameterInfo parameterInfo)
        {
            Type = parameterInfo.ParameterType;
            Name = parameterInfo.Name;
            IsOptional = parameterInfo.IsOptional;
        }
    }
}