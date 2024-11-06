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
        internal static Argument[] ToArguments(this ParameterInfo[] parameters)
        {
            Argument[] arguments = new Argument[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                ParameterInfo parameter = parameters[i];
                arguments[i] = new Argument(parameter);
            }

            return arguments;
        }

        internal static bool IsStatic(this PropertyInfo property)
        {
            return property.GetMethod != null && property.GetMethod.IsStatic
                   || property.SetMethod != null && property.SetMethod.IsStatic;
        }

        internal static string GetName(this MemberInfo member)
        {
            return member.GetCustomAttribute<ShellCommandAttribute>()!.Name ?? member.Name;
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

                while (e1.MoveNext())
                {
                    if (!(e2.MoveNext() && Equals(e1.Current, e2.Current)))
                    {
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
    }
}