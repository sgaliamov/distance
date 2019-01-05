using System.Collections.Concurrent;
using System.Collections.Generic;
using Distance.Models;

namespace Distance.KdTree
{
    using Compares = ConcurrentDictionary<int, IComparer<Location>>;

    public sealed class CoordinatesComparer : IComparer<Location>
    {
        private static readonly Compares Comparers = new Compares();

        private readonly int _axis;

        private CoordinatesComparer(int axis)
        {
            _axis = axis;
        }

        public int Compare(Location x, Location y)
        {
            return x.Coordinates.Values[_axis].CompareTo(y.Coordinates.Values[_axis]);
        }

        public static IComparer<Location> Get(int axis)
        {
            return Comparers.GetOrAdd(axis, key => new CoordinatesComparer(key));
        }
    }
}
