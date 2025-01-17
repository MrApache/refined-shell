using NUnit.Framework;
using RefinedShell.Tests.Examples.Correct;

namespace RefinedShell.Tests;

[TestFixture]
[TestOf(typeof(ExecutionResult))]
internal sealed class ExecutionResultEquality
{
    [Test]
    public void Equals_ReturnValueIsNull()
    {
        ExecutionResult a = new ExecutionResult(true, null, ProblemSegment.None);
        ExecutionResult b = new ExecutionResult(true, 1, ProblemSegment.None);
        Assert.That(a.Equals(b), Is.False);
    }

    [Test]
    public void Equals_Operator()
    {
        ExecutionResult a = new ExecutionResult(true, null, ProblemSegment.None);
        ExecutionResult b = new ExecutionResult(true, 1, ProblemSegment.None);
        Assert.That(a == b, Is.False);
    }

    [Test]
    public void NotEquals_Operator()
    {
        ExecutionResult a = new ExecutionResult(true, null, ProblemSegment.None);
        ExecutionResult b = new ExecutionResult(true, 1, ProblemSegment.None);
        Assert.That(a != b, Is.True);
    }

    [Test]
    public void GetHashCode_Override()
    {
        ExecutionResult a = new ExecutionResult(true, null, ProblemSegment.None);
        ExecutionResult b = new ExecutionResult(true, null, ProblemSegment.None);
        Assert.That(a.GetHashCode() == b.GetHashCode(), Is.True);
    }

    [Test]
    public void Equals_ValueType()
    {
        ExecutionResult a = new ExecutionResult(true, 10000, ProblemSegment.None);
        ExecutionResult b = new ExecutionResult(true, 10000, ProblemSegment.None);
        Assert.That(a == b, Is.True);
    }
}