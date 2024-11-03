using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using RefinedShell.Utilities;

namespace RefinedShell
{
    internal sealed class CommandCollection : ICollection<ICommand>, IReadOnlyCollection<ICommand>
    {
        private readonly Dictionary<StringToken, ICommand> _commands;

        public int Count => _commands.Count;
        public bool IsReadOnly => false;

        public ICommand this[StringToken name]
        {
            get => _commands[name];
            set => _commands[name] = value;
        }

        public CommandCollection()
        {
            _commands = new Dictionary<StringToken, ICommand>();
        }

        public void Add(ICommand item)
        {
            _commands.Add(item.Name, item);
        }

        public void Clear()
        {
            _commands.Clear();
        }

        public bool Contains(StringToken name)
        {
            return _commands.ContainsKey(name);
        }

        public bool TryGetCommand(StringToken name, [NotNullWhen(returnValue: true)] out ICommand? command)
        {
            command = null;
            if(Contains(name))
            {
                command = _commands[name];
                return true;
            }
            return false;
        }
        
        public void Remove(StringToken name)
        {
            _commands.Remove(name);
        }

        public bool Remove(ICommand item)
        {
            return _commands.Remove(item.Name);
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(_commands.GetEnumerator());
        }

        IEnumerator<ICommand> IEnumerable<ICommand>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        bool ICollection<ICommand>.Contains(ICommand item)
        {
            throw new InvalidOperationException();
        }

        void ICollection<ICommand>.CopyTo(ICommand[] array, int arrayIndex)
        {
            throw new InvalidOperationException();
        }

        public struct Enumerator : IEnumerator<ICommand>
        {
            private Dictionary<StringToken, ICommand>.Enumerator _enumerator;
            public ICommand Current => _enumerator.Current.Value;
            object IEnumerator.Current => Current;

            public Enumerator(Dictionary<StringToken, ICommand>.Enumerator enumerator)
            {
                _enumerator = enumerator;
            }

            public bool MoveNext()
            {
                return _enumerator.MoveNext();
            }

            public void Reset()
            {
                ((IEnumerator)_enumerator).Reset();
            }

            public void Dispose()
            {
                _enumerator.Dispose();
            }
        }
    }
}