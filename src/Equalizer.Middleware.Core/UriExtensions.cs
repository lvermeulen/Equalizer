using System;
using System.Collections.Generic;
using System.Linq;

namespace Equalizer.Middleware.Core
{
    public static class UriExtensions
    {
        private static string TrimAfter(this string s, string trim)
        {
            int index = s.IndexOf(trim, StringComparison.Ordinal);
            if (index > 0)
            {
                s = s.Substring(0, index);
            }

            return s;
        }

        internal static bool StartsWith<T>(this IEnumerable<T> left, IEnumerable<T> right, IEqualityComparer<T> comparer = null)
        {
            if (left == null)
            {
                throw new ArgumentNullException(nameof(left));
            }
            if (right == null)
            {
                throw new ArgumentNullException(nameof(right));
            }
            if (comparer == null)
            {
                comparer = EqualityComparer<T>.Default;
            }

            using (var leftEnumerator = left.GetEnumerator())
            using (var rightEnumerator = right.GetEnumerator())
            {
                for (;;)
                {
                    if (!rightEnumerator.MoveNext())
                    {
                        return true;
                    }
                    if (!leftEnumerator.MoveNext())
                    {
                        return false;
                    }
                    if (!comparer.Equals(leftEnumerator.Current, rightEnumerator.Current))
                    {
                        return false;
                    }
                }
            }
        }

        public static bool StartsWithSegments(this Uri uri, string path)
        {
            // support root path
            bool isRootPath = string.IsNullOrEmpty(path) || path == "/";
            if (isRootPath)
            {
                return uri.Segments.Length == 1;
            }

            // trim query string & separators
            path = path.TrimAfter("?").Trim('/');
            var pathSegments = path.Split('/');

            // skip starting path & trim separators
            var segments = uri.Segments.Skip(1).Select(x => x.Trim('/'));

            return segments.StartsWith(pathSegments);
        }
    }
}
