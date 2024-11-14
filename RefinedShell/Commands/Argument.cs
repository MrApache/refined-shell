using System;
using System.Reflection;

namespace RefinedShell.Commands
{
    /// <summary>
    /// Represents a command argument.
    /// </summary>
    public sealed class Argument
    {
        /// <summary>
        /// Gets the type of the argument.
        /// </summary>
        public readonly Type Type;

        /// <summary>
        /// Gets the name of the argument.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Determines whether an argument is optional.
        /// </summary>
        public readonly bool IsOptional;

        /// <summary>
        /// Initializes a new instance of the <see cref="Argument"/>.
        /// </summary>
        public Argument(Type type, string name, bool isOptional)
        {
            Type = type;
            Name = name;
            IsOptional = isOptional;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Argument"/>.
        /// </summary>
        public Argument(ParameterInfo parameterInfo)
        {
            Type = parameterInfo.ParameterType;
            Name = parameterInfo.Name;
            IsOptional = parameterInfo.IsOptional;
        }
    }
}