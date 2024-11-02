using System.Collections.Generic;
using IrisShell.Parsing;
using NUnit.Framework;

namespace IrisShell.Tests.Parsing
{
    internal abstract class TypeParsing<T>
    {
        protected abstract Dictionary<string, (bool result, T value)> TestCases { get; }
        private readonly ITypeParser _parser;

        protected TypeParsing()
        {
            _parser = TypeParsers.GetParser(typeof(T));
        }

        [Test]
        public void Parse()
        {
            foreach ((string input, (bool result, T value)) in TestCases)
            {
                string[] args = input.Split(' ');
                bool canParse = _parser.CanParse(args);
                Assert.That(canParse, Is.EqualTo(result), $"Input {input}");
                if(!canParse)
                    continue;
                T parsedValue = (T)_parser.Parse(args);
                Assert.That(parsedValue, Is.EqualTo(value), $"Input {input}");
            }
        }
    }
}