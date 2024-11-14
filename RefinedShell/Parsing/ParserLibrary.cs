using System;
using System.Collections.Generic;

namespace RefinedShell.Parsing
{
    /// <summary>
    /// Represents a library of parsers for various data types.
    /// </summary>
    public sealed class ParserLibrary
    {
        public static readonly ParserLibrary Default;

        static ParserLibrary()
        {
            Default = new ParserLibrary
            {
                _parsers =
                {
                    { typeof(sbyte),  new SByteParser()  },
                    { typeof(byte),   new ByteParser()   },
                    { typeof(short),  new ShortParser()  },
                    { typeof(ushort), new UShortParser() },
                    { typeof(int),    new IntParser()    },
                    { typeof(uint),   new UIntParser()   },
                    { typeof(long),   new LongParser()   },
                    { typeof(ulong),  new ULongParser()  },
                    { typeof(string), new StringParser() },
                    { typeof(float),  new FloatParser()  },
                    { typeof(double), new DoubleParser() },
                    { typeof(bool),   new BoolParser()   }
                }
            };
        }

        private readonly Dictionary<Type, ITypeParser> _parsers;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParserLibrary"/> class.
        /// </summary>
        public ParserLibrary()
        {
            _parsers = new Dictionary<Type, ITypeParser>();
        }

        /// <summary>
        /// Determines whether the <see cref="ParserLibrary"/> contains the specified type.
        /// </summary>
        public bool Contains(Type type)
        {
            return _parsers.ContainsKey(type);
        }

        internal ITypeParser GetParser(Type type)
        {
            return _parsers[type];
        }

        /// <summary>
        /// Removes the parser with the specified type from the <see cref="ParserLibrary"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Remove<T>()
        {
            _parsers.Remove(typeof(T));
        }

        /// <summary>
        /// Adds the specified type parser to the <see cref="ParserLibrary"/>
        /// </summary>
        public void AddParser<T>(ITypeParser parser)
        {
            if(parser == null)
                throw new ArgumentNullException(nameof(parser));

            if(_parsers.ContainsKey(typeof(T)))
                throw new ArgumentException($"Type '{typeof(T)}' already registered.");

            _parsers.Add(typeof(T), parser);
        }
    }
}