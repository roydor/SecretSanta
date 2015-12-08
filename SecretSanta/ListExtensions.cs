using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta
{
    public static class ListExtensions
    {
        private static Random _rand = new Random();

        public static void Shuffle<T>(this List<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int k = _rand.Next(i, list.Count);
                T value = list[k];
                list[k] = list[i];
                list[i] = value;
            }
        }
    }
}
