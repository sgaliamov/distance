namespace Distance.KdTree
{
    public sealed class NodeDistance
    {
        public NodeDistance(Node node, double distance)
        {
            Node = node;
            Distance = distance;
        }

        public double Distance { get; }
        public Node Node { get; }
    }
}
