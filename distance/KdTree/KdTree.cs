using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Distance.Models;

namespace Distance.KdTree
{
    public sealed class KdTree
    {
#if DEBUG
        public static int Counter;
#endif
        private Node _root;

        public void Build(IEnumerable<Location> locations)
        {
            var copy = locations.ToArray();

            _root = Build(copy, 0, copy.Length, 0);
        }

        public LocationDistance[] Nearest(Coordinates target, double radius)
        {
#if DEBUG
            Counter = 0;
#endif
            var result = new List<LocationDistance>();

            Nearest(_root, target, radius, result);

            return result.ToArray();
        }

        private static void Nearest(
            Node current,
            Coordinates target,
            double radius,
            ICollection<LocationDistance> result)
        {
#if DEBUG
            Interlocked.Increment(ref Counter);
#endif

            var distance = target.Distance(current.Location.Coordinates);
            if (distance <= radius)
            {
                result.Add(new LocationDistance(current.Location.Address, current.Location.Coordinates, distance));
            }

            var value = target.Values[current.Axis];
            var median = current.Location.Coordinates.Values[current.Axis];
            var distanceToMedian = current.Distance(target);

            var goRight = value - median > 0;
            if (goRight)
            {
                if (current.Right != null)
                {
                    Nearest(current.Right, target, radius, result);
                }

                if (current.Left != null && distanceToMedian <= radius)
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

                if (current.Right != null && distanceToMedian <= radius)
                {
                    Nearest(current.Right, target, radius, result);
                }
            }
        }

        private static Node Build(Location[] locations, int start, int length, int depth)
        {
            if (length <= 0)
            {
                return null;
            }

            var axis = depth % 2;
            Array.Sort(locations, start, length, CoordinatesComparer.Get(axis));

            var half = start + length / 2;
            var median = locations[half];

            var leftStart = start;
            var leftLength = half - start;

            var rightStart = half + 1;
            var rightLength = length - length / 2 - 1;

            var left = Build(locations, leftStart, leftLength, depth + 1);
            var right = Build(locations, rightStart, rightLength, depth + 1);

            return new Node(median, left, right, axis);
        }

        private sealed class Node
        {
            public readonly int Axis;
            public readonly Node Left;
            public readonly Location Location;
            public readonly Node Right;

            public Node(Location location, Node left, Node right, int axis)
            {
                Location = location;
                Left = left;
                Right = right;
                Axis = axis;
            }

            public double Distance(Coordinates target)
            {
                return Axis == 0
                           ? new Coordinates(Location.Coordinates.Latitude, target.Longitude).Distance(target)
                           : new Coordinates(target.Latitude, Location.Coordinates.Longitude).Distance(target);
            }
        }
    }
}
