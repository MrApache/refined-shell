using System;
using System.Collections.Generic;
using System.Linq;

namespace RefinedShell.Matching
{
    internal static class DFABuilder
    {
        private static List<Token> _tokens = null!;
        private static int _position;
        private static Stack<Rule> _rules = null!;

        private static void MoveRight()
        {
            _position++;
        }

        private static void TryMoveRight()
        {
            if(_position + 1 < _tokens.Count)
                MoveRight();
        }
        
        private static void MoveLeft()
        {
            _position--;
        }

        private static Token CurrentSymbol()
        {
            return _tokens[_position];
        }

        public static Rule[] Build(List<Token> tokens)
        {
            _tokens = tokens;
            _rules = new Stack<Rule>(tokens.Count / 2);

            while (_position < tokens.Count) {
                Rule rule = GetRule();
                _rules.Push(rule);
                MoveRight();
            }

            _position = 0;
            _tokens = null!;
            return _rules.Reverse().ToArray();
        }

        private static Rule GetRule()
        {
            Rule? rule;
            switch (CurrentSymbol().Value) {
                case "^" when CurrentSymbol().Modifier == Modifier.None:
                {
                    MoveRight();
                    return new StartOfLine(GetRule());
                }
                case "$" when CurrentSymbol().Modifier == Modifier.None:
                {
                    Rule previousRule = _rules.Pop();
                    return new EndOfLine(previousRule);
                }
                case "[" when CurrentSymbol().Modifier == Modifier.None:
                {
                    MoveRight();
                    rule = GetClass();
                    MoveRight();
                    MoveRight();
                    break;
                }
                case "(" when CurrentSymbol().Modifier == Modifier.None:
                {
                    MoveRight();
                    rule = BuildGroup();
                    MoveRight();
                    break;
                }
                case "." when CurrentSymbol().Modifier == Modifier.None:
                {
                    rule = new AnySingle(null);
                    MoveRight();
                    break;
                }
                case "|" when CurrentSymbol().Modifier == Modifier.None:
                {
                    Rule previousRule = _rules.Pop();
                    MoveRight();
                    return new Or(previousRule, GetRule());
                }
                default:
                {
                    rule = new SymbolMatching(CurrentSymbol().Value);
                    MoveRight();
                    break;
                }
            }

            if (rule == null) {
                throw new Exception($"Unknown token: {CurrentSymbol().Value}");
            }

            if(_position < _tokens.Count)
                return TryAppendCondition(rule);
            return rule;
        }

        private static Rule TryAppendCondition(Rule current)
        {
            switch(CurrentSymbol().Value) {
                case "?": return new ZeroOrOne(current);
                case "+": return new OneOrMore(current);
                case "*": return new ZeroOrMore(current);
                case "{":
                {
                    MoveRight();
                    current = BuildCountCondition(current);
                    TryMoveRight();
                    current = TryAppendCondition(current);
                    break;
                }
                default: MoveLeft(); break;
            }

            return current;
        }

        private static Rule BuildCountCondition(Rule child)
        {
            int left = -1;
            int right = -1;
            bool isBetween = false;

            while(CurrentSymbol().Value != "}") {
                Token token = CurrentSymbol();
                if(token.Value == ",") {
                    isBetween = true;
                }
                else if(isBetween){
                    right = int.Parse(token.Value);
                }
                else {
                    left = int.Parse(token.Value);
                }
                MoveRight();
            }

            if(isBetween) {
                return new Between(left, right, child);
            }
            return new ExactlyOf(left, child);
        }

        private static Rule BuildGroup()
        {
            Stack<Rule> previous = _rules;
            Stack<Rule> temp = new Stack<Rule>(16);
            _rules = temp;
            while(CurrentSymbol().Value != ")") {
                Rule rule = GetRule();
                _rules.Push(rule);
                MoveRight();
            }
            _rules = previous;
            return new Group(temp.Reverse().ToList());
        }

        private static Rule GetClass(Rule? child = null)
        {
            Token token = CurrentSymbol();
            if (token.Value == "|" && token.Modifier == Modifier.None) {
                MoveRight();
                return new Or(child!, CreateClass());
            }

            Rule group;
            if (token.Value == "^" && token.Modifier == Modifier.None) {
                MoveRight();
                group = new InvertedClass(CurrentSymbol().Value);
            }
            else if (token.Value == "[" && token.Modifier != Modifier.Symbol) {
                MoveRight();
                Rule rule = GetClass(child);
                group = CreateClass(rule);
            }
            else {
                group = CreateClass();
            }

            if(IsNextSymbolIsOr()) {
                MoveRight();
                Rule rule = GetClass(group);
                return rule;
            }

            return group;
        }

        private static bool IsNextSymbolIsOr()
        {
            MoveRight();
            bool result = CurrentSymbol().Value == "|";
            MoveLeft();
            return result;
        }

        private static Class CreateClass(Rule? rule = null)
        {
            return new Class(CurrentSymbol().Value, rule);
        }
    }

}
