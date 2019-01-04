using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Distance.KdTree
{
    public sealed class PointAxisComparer : IComparer<Point>
    {
        private static readonly ConcurrentDictionary<int, IComparer<Point>> Comparers 
            = new ConcurrentDictionary<int, IComparer<Point>>();

        private readonly int _axis;

        private PointAxisComparer(int axis)
        {
            _axis = axis;
        }

        public int Compare(Point x, Point y)
        {
            return x.Coordinates[_axis].CompareTo(y.Coordinates[_axis]);
        }

        public static IComparer<Point> Get(int axis)
        {
            return Comparers.GetOrAdd(axis, key => new PointAxisComparer(key));
        }
    }
}
