using System;

namespace RefinedShell.Interpreter
{
    internal abstract class Node : IEquatable<Node>
    {
        public abstract bool Equals(Node? other);
    }
}