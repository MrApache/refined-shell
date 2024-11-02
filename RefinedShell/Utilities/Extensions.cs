using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RefinedShell.Utilities
{
    internal static class Extensions
    {
        public static IEnumerable<MethodInfo> WithAttribute<T>(this MethodInfo[] methodCollection) where T : Attribute
        {
            return methodCollection.Where(m => m.GetCustomAttribute<T>() != null);
        }

        public static ReadOnlyMemory<char> Split(this ReadOnlyMemory<char> memory, char separator, ref int position)
        {
            for (int i = position; i < memory.Length; i++)
            {
                if (i + 1 != memory.Length)
                {
                    char character = memory.Span[i];
                    if (character != separator && character != '\0')
                        continue;
                }
                else
                {
                    i++;
                }

                ReadOnlyMemory<char> result = memory.Slice(position, i - position);
                position = i + 1;
                return result;
            }

            return memory;
        }
    }
}