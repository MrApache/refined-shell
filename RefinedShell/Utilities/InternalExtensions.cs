using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RefinedShell.Commands;

namespace RefinedShell.Utilities
{
    internal static class InternalExtensions
    {
        internal static bool IsStatic(this MemberInfo member)
        {
            return member switch
            {
                FieldInfo field => field.IsStatic,
                MethodInfo method => method.IsStatic,
                PropertyInfo property => property.IsStaticProperty(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        internal static Argument[] ToArguments(this ParameterInfo[] parameters)
        {
            Argument[] arguments = new Argument[parameters.Length];
            for (int i = 0; i < parameters.Length; i++) {
                ParameterInfo parameter = parameters[i];
                arguments[i] = new Argument(parameter);
            }

            return arguments;
        }

        private static bool IsStaticProperty(this PropertyInfo property)
        {
            return property.GetMethod != null && property.GetMethod.IsStatic
                   || property.SetMethod != null && property.SetMethod.IsStatic;
        }

        internal static string GetName(this MemberInfo member)
        {
            return member.GetCustomAttribute<ShellFunctionAttribute>()!.Name ?? member.Name;
        }

        internal static IEnumerable<U> WithAttribute<U, T>(this U[] memberCollection)
            where T : Attribute where U : MemberInfo
        {
            return memberCollection.Where(m => m.GetCustomAttribute<T>() != null);
        }

        internal static bool SequenceEqual(this ICollection a, ICollection b)
        {
            if (a.Count != b.Count)
                return false;

            IEnumerator e1 = a.GetEnumerator();
            IEnumerator e2 = b.GetEnumerator();
            try
            {

                while (e1.MoveNext()) {
                    if (!(e2.MoveNext() && Equals(e1.Current, e2.Current))) {
                        return false;
                    }
                }

                return !e2.MoveNext();
            }
            finally
            {
                if(e1 is IDisposable disposableE1)
                    disposableE1.Dispose();
                if(e2 is IDisposable disposableE2)
                    disposableE2.Dispose();
            }
        }

        internal static void AddRange<T>(this ICollection<T> source, IEnumerable<T> collection)
        {
            foreach(T item in collection) {
                source.Add(item);
            }
        }

        internal static string AsString(this IReadOnlyCollection<char> chars)
        {
            Span<char> buffer = stackalloc char[chars.Count];
            int i = 0;
            foreach(char c in chars) {
                buffer[i++] = c;
            }
            return buffer.ToString();
        }
    }
}
