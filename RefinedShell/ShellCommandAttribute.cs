using System;
using JetBrains.Annotations;

namespace RefinedShell
{
    /// <summary>
    /// An attribute that marks a method as a shell command.
    /// </summary>
    [MeansImplicitUse]
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ShellCommandAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the name of the command.
        /// If <c>null</c>, the name of the method will be used as the command name.
        /// </summary>
        public readonly string? Name;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellCommandAttribute"/> class.
        /// </summary>
        public ShellCommandAttribute() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellCommandAttribute"/> class with a specified command name.
        /// </summary>
        /// <param name="name">The name of the command.</param>
        public ShellCommandAttribute(string name)
        {
            Name = name;
        }
    }
}