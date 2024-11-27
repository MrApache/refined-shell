using System;
using JetBrains.Annotations;

namespace RefinedShell
{
    /// <summary>
    /// An attribute that marks a method as a pure shell command.
    /// </summary>
    [MeansImplicitUse]
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class PureShellFunctionAttribute : ShellAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PureShellFunctionAttribute"/> class.
        /// </summary>
        public PureShellFunctionAttribute() : base(null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PureShellFunctionAttribute"/> class with a specified command name.
        /// </summary>
        /// <param name="name">The name of the command.</param>
        public PureShellFunctionAttribute(string name) : base(name)
        { }
    }
}