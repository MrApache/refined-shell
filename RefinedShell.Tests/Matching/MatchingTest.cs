using System.Collections.Generic;
using NUnit.Framework;
using RefinedShell.Matching;

namespace RefinedShell.Tests.Matching;

internal sealed class MatchingTest
{
    //[Test]
    public void PatternParsing()
    {
        // 0 = 48
        // 9 = 57
        // A = 65 // Z = 90
        // _ = 95
        // a = 97
        // z = 122
        List<Token> tokens = PatternParser.Parse("^[^-|+]?[0-9]+([.][0-9]*)?");
        Assert.That(tokens.Count, Is.EqualTo(3));
    }

    [Test]
    public void DFABuilderTest()
    {
        //const string pattern = @"[a\-z]";
        //const string pattern = @"[\w]";
        //const string pattern = "^\"{1}[^\"]*\"{1}";
        //const string pattern = "^[-|+]?[0-9]+([.][0-9]*)?";
        //const string pattern = "world$";
        //const string pattern = "(a|b|c)";
        //const string pattern = "(a|b|c)+";
        //const string pattern = ".";
        //const string pattern = @"\w+\d+";
        //const string pattern = @"[a-z]+\d+";
        //const string pattern = "[a-d][e-h]";
        //const string pattern = @"[a-zA-Z]+\d*\W";
        const string pattern = @"[\w\s]+"; //FIXME
        //const string pattern = @"^\s+";
        //const string pattern = "^[a-zA-Z_][a-zA-Z0-9_]*";
        //const string pattern = "^\"[^\"]*\"";
        //const string pattern = "^[-|+]?[0-9]+([.][0-9]*)?";
        //const string pattern = "^[-|+]?[0-9]+";

        List<Token> tokens = PatternParser.Parse(pattern);
        Rule[] states = DFABuilder.Build(tokens);
        MatchContext context = new MatchContext("$(command arg1 arg2)");
        Matcher matcher = new Matcher(pattern);
        Match match = matcher.Match(context.Text);
    }

    //[Test]
    public void MatcherTest()
    {
        Matcher matcher = new Matcher("^[a-zA-Z_][a-zA-Z0-9_]*");
        //const string input = "_getValue2 arg1 arg2";
        const string input = "1";
        bool resultX = matcher.IsMatch(input);
        Match match = matcher.Match(input);
        string result = input.Substring(match.Start, match.Length);
    }
}
