using RefinedShell.Interpreter;

namespace RefinedShell.Tests;

internal sealed class TestNode : Node
{
    public override bool Equals(Node? other)
    {
        if (other is TestNode)
            return true;
        return false;
    }
}