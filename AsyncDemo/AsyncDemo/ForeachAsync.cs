using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AsyncDemo
{
    public static class ForEachAsync
    {
        public static async Task ForEach1<T>(this IEnumerable<T> list, Func<T, Task> func)
        {
            foreach (T value in list)
            {
                await func(value);
            }
        }
    }
}
