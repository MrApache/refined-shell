using System;
using System.Collections.Generic;

namespace RefinedShell.Matching
{
    internal sealed class Matcher
    {
        private readonly Rule[] _rules;

        public Matcher(string pattern)
        {
            List<Token> tokens = PatternParser.Parse(pattern);
            _rules = DFABuilder.Build(tokens);
        }

        public bool IsMatch(string input)
        {
            ThrowIfHasErrors(input, 0, input.Length);
            return IsMatch(input.AsSpan());
        }

        public bool IsMatch(string input, int start)
        {
            int length = input.Length - start;
            ThrowIfHasErrors(input, start, length);
            return IsMatch(input.AsSpan(start, length));
        }

        public bool IsMatch(string input, int start, int length)
        {
            ThrowIfHasErrors(input, start, length);
            return IsMatch(input.AsSpan(start, length));
        }

        public bool IsMatch(ReadOnlySpan<char> input, int start)
        {
            int length = input.Length - start;
            ThrowIfHasErrors(input, start, length);
            return IsMatch(input.Slice(start, length));
        }

        public bool IsMatch(ReadOnlySpan<char> input, int start, int length)
        {
            ThrowIfHasErrors(input, start, length);
            return IsMatch(input.Slice(start, length));
        }

        public bool IsMatch(ReadOnlySpan<char> input)
        {
            MatchContext context = new MatchContext(input);

            foreach (Rule rule in _rules)
            {
                FindStatus status = rule.Evaluate(ref context);
                if (status == FindStatus.Found) {
                    continue;
                }

                if (status == FindStatus.NotFound) {
                    return false;
                }
            }

            return true;
        }

        public Match Match(string input)
        {
            ThrowIfHasErrors(input, 0, input.Length);
            return Match(input.AsSpan());
        }

        public Match Match(string input, int start)
        {
            int length = input.Length - start;
            ThrowIfHasErrors(input, start, length);
            return Match(input.AsSpan(start, length));
        }

        public Match Match(string input, int start, int length)
        {
            ThrowIfHasErrors(input, start, length);
            return Match(input.AsSpan(start, length));
        }

        public Match Match(ReadOnlySpan<char> input, int start)
        {
            int length = input.Length - start;
            ThrowIfHasErrors(input, start, length);
            return Match(input.Slice(start, length));
        }

        public Match Match(ReadOnlySpan<char> input, int start, int length)
        {
            ThrowIfHasErrors(input, start, length);
            return Match(input.Slice(start, length));
        }

        public Match Match(ReadOnlySpan<char> input)
        {
            int start = -1; // Начало совпадения. -1 означает, что совпадений пока нет.
            int length = 0; // Полная длина совпадения.

            foreach (Rule rule in _rules) {
                MatchContext context = new MatchContext(input);
                context.Start = start == -1 ? 0 : start + length;
                FindStatus status = rule.Evaluate(ref context);

                if (status == FindStatus.Found) {
                    if (start == -1) {
                        start = context.Start;
                    }

                    // Увеличиваем длину совпадения с учётом новой длины.
                    length = context.Start + context.Length - start;

                    continue;
                }

                // Если хотя бы одно правило возвращает NotFound, совпадение недействительно.
                if (status == FindStatus.NotFound) {
                    return new Match(0, 0, false);
                }
            }

            // Если start остался -1, то совпадений не было.
            if (start == -1) {
                return new Match(0, 0, false);
            }

            return new Match(start, length, true);
        }

        private static void ThrowIfHasErrors(string input, int start, int length)
        {
            if (string.IsNullOrEmpty(input))
                throw new Exception("Empty string");
            if (start < 0)
                throw new ArgumentOutOfRangeException(nameof(start));
            if( length > input.Length || length < 0)
                throw new ArgumentOutOfRangeException(nameof(length));
        }

        private static void ThrowIfHasErrors(ReadOnlySpan<char> input, int start, int length)
        {
            if (input.IsEmpty)
                throw new Exception("String is empty");
            if (start < 0)
                throw new ArgumentOutOfRangeException(nameof(start));
            if( length > input.Length || length < 0)
                throw new ArgumentOutOfRangeException(nameof(length));
        }
    }
}
