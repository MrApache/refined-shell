using System;

namespace IrisShell.Interpreter
{
    internal abstract class Node : IEquatable<Node>
    {
        public abstract bool Equals(Node? other);
    }
}