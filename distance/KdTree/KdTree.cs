using System;
using System.Linq;

namespace Distance.KdTree
{
    public static class KdTree
    {
        public static Node Build(Point[] points)
        {
            var copy = points.ToArray();

            return Build(copy, 0, points.Length, 0);
        }

        private static Node Build(Point[] points, int start, int length, int depth)
        {
            if (length <= 0)
            {
                return null;
            }

            var axis = depth % 3;
            Array.Sort(points, start, length, PointAxisComparer.Get(axis));

            var half = start + length / 2;
            var median = points[half];

            var leftStart = start;
            var leftLength = half - start;

            var rightStart = half + 1;
            var rightLength = length - length / 2 - 1;

            var left = Build(points, leftStart, leftLength, depth + 1);
            var right = Build(points, rightStart, rightLength, depth + 1);

            return new Node(median, left, right, axis);
        }
    }
}
