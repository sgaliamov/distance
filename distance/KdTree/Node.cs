using System;
using System.Collections.Generic;

namespace Distance.KdTree
{
    public sealed class Node
    {
        public readonly int Axis;
        public readonly Node Left;
        public readonly Point Position;
        public readonly Node Right;

        public Node(Point position, Node left, Node right, int axis)
        {
            Position = position;
            Left = left;
            Right = right;
            Axis = axis;
        }

        public NodeDistance[] Nearest(
            Node current,
            Point target,
            double radius)
        {
            var result = new List<NodeDistance>();

            Nearest(this, target, radius, result);

            return result.ToArray();
        }

        private static void Nearest(
            Node current,
            Point target,
            double radius,
            ICollection<NodeDistance> result)
        {
            var d = Distance(target, current.Position);
            if (d <= radius)
            {
                result.Add(new NodeDistance(current, d));
            }

            var value = target.Coordinates[current.Axis];
            var median = current.Position.Coordinates[current.Axis];
            var u = value - median;

            if (u > 0)
            {
                if (current.Right != null)
                {
                    Nearest(current.Right, target, radius, result);
                }

                if (current.Left != null && Math.Abs(u) <= radius)
                {
                    Nearest(current.Left, target, radius, result);
                }
            }
            else
            {
                if (current.Left != null)
                {
                    Nearest(current.Left, target, radius, result);
                }

                if (current.Right != null && Math.Abs(u) <= radius)
                {
                    Nearest(current.Right, target, radius, result);
                }
            }
        }

        private static double Distance(Point x, Point y)
        {
            var xCoordinates = x.Coordinates;
            var yCoordinates = y.Coordinates;

            double result = 0;
            for (var i = 0; i < xCoordinates.Length; i++)
            {
                var a = xCoordinates[i] - yCoordinates[i];

                result += a * a;
            }

            return result;
        }
    }
}
