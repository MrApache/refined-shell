using System;
using JetBrains.Annotations;

namespace RefinedShell
{
    /// <summary>
    /// Abstract base class for all custom attributes.
    /// </summary>
    [MeansImplicitUse]
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field)]
    public abstract class ShellAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the name of the command.
        /// If <c>null</c>, the name of the method will be used as the command name.
        /// </summary>
        public readonly string? Name;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellAttribute"/> class with a specified command name.
        /// </summary>
        /// <param name="name">The name of the command.</param>
        protected ShellAttribute(string? name)
        {
            Name = name;
        }
    }
}