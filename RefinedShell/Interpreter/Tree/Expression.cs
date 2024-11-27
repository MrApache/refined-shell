using System;
using System.Collections.Generic;
using System.Linq;

namespace RefinedShell.Interpreter
{
    internal sealed class Expression : Node
    {
        private readonly List<Node> _nodes;

        public int Count => _nodes.Count;

        public Expression()
        {
            _nodes = new List<Node>();
        }

        public Expression(params Node[] list)
        {
            _nodes = list.ToList();
        }

        public void Add(Node node)
        {
            _nodes.Add(node);
        }

        public List<Node>.Enumerator GetEnumerator()
        {
            return _nodes.GetEnumerator();
        }

        public override bool Equals(Node? other)
        {
            if (!(other is Expression expr))
                return false;
            return _nodes.SequenceEqual(expr._nodes);
        }

        public override bool Equals(object? obj)
        {
            if (obj is Node node)
                return Equals(node);
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_nodes);
        }
    }
}
