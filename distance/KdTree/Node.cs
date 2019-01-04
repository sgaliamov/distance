using System;
using System.Collections.Generic;
using System.Linq;

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

        public NodeDistance[] Nearest(Point target, double radius)
        {
            var result = new LinkedList<NodeDistance>();

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

            if (u > 0) // todo: use stack
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
            var lat1 = x.Coordinates[0];
            var long1 = x.Coordinates[1];
            var lat2 = y.Coordinates[0];
            var long2 = y.Coordinates[1];

            var rLat1 = Math.PI * lat1 / 180;
            var rLat2 = Math.PI * lat2 / 180;
            var rTheta = Math.PI * (long2 - long1) / 180;
            var dist = Math.Sin(rLat1) * Math.Sin(rLat2) + Math.Cos(rLat1) * Math.Cos(rLat2) * Math.Cos(rTheta);

            return Math.Acos(dist) * 180 * 60 * 1.1515 * 1609.344 / Math.PI;
        }
    }
}
