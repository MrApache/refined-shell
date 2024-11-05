using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RefinedShell.Utilities
{
    public static class Extensions
    {
        internal static IEnumerable<MethodInfo> WithAttribute<T>(this MethodInfo[] methodCollection) where T : Attribute
        {
            return methodCollection.Where(m => m.GetCustomAttribute<T>() != null);
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

        public static string Substring(this string source, ProblemSegment segment)
        {
            return source.Substring(segment.Start, segment.Length);
        }
    }
}