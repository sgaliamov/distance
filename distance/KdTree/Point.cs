using System;

namespace Distance.KdTree
{
    public struct Point : IEquatable<Point>
    {
        public readonly int Id;
        public readonly double[] Coordinates;

        public Point(int id, params double[] coordinates)
        {
            Id = id;
            Coordinates = coordinates;
        }

        public bool Equals(Point other)
        {
            return Id == other.Id
                   && Equals(Coordinates, other.Coordinates);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is Point other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Id * 397) ^ (Coordinates != null ? Coordinates.GetHashCode() : 0);
            }
        }
    }
}
