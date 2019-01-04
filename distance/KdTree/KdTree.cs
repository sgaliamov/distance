using System;

namespace Distance.KdTree
{
    public sealed class KdTree
    {
        public Node Build(Point[] points)
        {
            return Build(points, 0);
        }

        private static Node Build(Point[] points, int depth)
        {
            if (points.Length == 0)
            {
                return null;
            }

            var axis = depth % 3;

            Array.Sort(points, PointAxisComparer.Get(axis));

            var median = points.Length / 2;

            var left = new Point[median];
            Buffer.BlockCopy(points, 0, left, 0, median);

            var right = new Point[points.Length - 1];
            Buffer.BlockCopy(points, median, right, 0, right.Length);

            return new Node(
                points[median],
                Build(left, depth + 1),
                Build(right, depth + 1));
        }
    }
}
