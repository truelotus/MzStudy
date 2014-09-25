using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudyConsoleProject
{
    static class YounExtention
    {
        public static bool IsNullOrEmpty(this String str)
        {
            if (str == null || str == "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsNullList<T>(this IEnumerable<T> list)
        {
            if (list == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static IEnumerable<TSource> YounWhere<TSource>(this IEnumerable<TSource> source, Func<TSource, string> predicate)
        {
            var list = new YounList<TSource>();

            foreach (var item in source)
            {
                if (item.Equals(predicate(item)))
                {
                    list.Add(item);
                }
            }

            return list;
        }

        public static IEnumerable<TSource> YounWhere<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            var list = new YounList<TSource>();

            foreach (var item in source)
            {
                if (predicate(item))
                {
                    list.Add(item);
                }
            }

            return list;
        }

        public static void YounForeach<T>(this IEnumerable<T> source, Action<T> younFunc)
        {
            foreach (var item in source)
            {
                younFunc(item);
            }
        }

        public static IEnumerable<TResult> YounSelect<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector) 
        {
            var list = new YounList<TResult>();
            foreach (var item in source)
            {
                list.Add(selector(item));
            }
            return list;
        }

    }
}
