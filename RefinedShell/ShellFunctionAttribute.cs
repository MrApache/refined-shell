//TODO
//Fix naming
using System;
using JetBrains.Annotations;

namespace RefinedShell
{
    /// <summary>
    /// An attribute that marks a method as a shell command.
    /// </summary>
    [MeansImplicitUse]
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class ShellFunctionAttribute : ShellAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShellFunctionAttribute"/> class.
        /// </summary>
        public ShellFunctionAttribute() : base(null) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellFunctionAttribute"/> class with a specified command name.
        /// </summary>
        /// <param name="name">The name of the command.</param>
        public ShellFunctionAttribute(string name) : base(name) {}
    }
}
