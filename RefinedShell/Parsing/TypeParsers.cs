using System;
using System.Collections.Generic;

namespace RefinedShell.Parsing
{
    public static class TypeParsers
    {
        private static readonly Dictionary<Type, ITypeParser> _parsers;

        static TypeParsers()
        {
            _parsers = new Dictionary<Type, ITypeParser>
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
            };
        }

        internal static bool Contains(Type type)
        {
            return _parsers.ContainsKey(type);
        }

        internal static ITypeParser GetParser(Type type)
        {
            return _parsers[type];
        }

        internal static void Remove<T>()
        {
            _parsers.Remove(typeof(T));
        }

        public static void AddParser<T>(ITypeParser parser)
        {
            if(parser == null)
                throw new ArgumentNullException(nameof(parser));

            if(_parsers.ContainsKey(typeof(T)))
                throw new ArgumentException($"Type '{typeof(T)}' already registered.");

            _parsers.Add(typeof(T), parser);
        }
    }
}