using System.Collections.Generic;
using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts.Util
{
    public class AStar {

        private int _width;
        private int _height;

        private Node[,] _grid;
        private Node _endNode;
        private Node _startNode;

        private SearchParameters _searchParameters;

        public AStar(SearchParameters searchParameters)
        {
            _searchParameters = searchParameters;
            InitializeGrid(searchParameters.Map);
            _startNode = _grid[(int) searchParameters.StartLocation.x, (int) searchParameters.StartLocation.y];
            _startNode.State = Node.NodeState.Open;
            _endNode = _grid[(int) searchParameters.EndLocation.x, (int) searchParameters.EndLocation.y];

        }

        public void InitializeGrid(bool[,] map)
        {
            _width = map.GetLength(0);
            _height = map.GetLength(1);
            _grid = new Node[_width, _height];

            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    _grid[x, y] = new Node(new Vector2(x, y), map[x, y], _searchParameters.EndLocation);
                }
            }
        }

        public bool Search(Node currentNode)
        {
            currentNode.State = Node.NodeState.Closed;
            List<Node> nextNodes = GetAdjacentWalkableNodes(currentNode);
            nextNodes.Sort((node1, node2) => node1.F.CompareTo(node2.F));
            foreach (var nextNode in nextNodes)
            {
                if (nextNode.Location == this._endNode.Location)
                {
                    return true;
                }
                else
                {
                    if (Search(nextNode)) // Note: Recurses back into Search(Node)
                        return true;
                }
            }
            return false;
        }

        public List<Vector2> FindPath()
        {
            List<Vector2> path = new List<Vector2>();
            bool success = Search(_startNode);
            if (success)
            {
                Node node = this._endNode;
                while (node.ParentNode != null)
                {
                    path.Add(node.Location);
                    node = node.ParentNode;
                }
                path.Reverse();
            }
            return path;
        }

        private List<Node> GetAdjacentWalkableNodes(Node fromNode)
        {
            List<Node> walkableNodes = new List<Node>();
            IEnumerable<Vector2> nextLocations = GetAdjacentLocations(fromNode.Location);

            foreach (var location in nextLocations)
            {
                int x = (int) location.x;
                int y = (int) location.y;

                // Stay within the _grid's boundaries
                if (x < 0 || x >= _width || y < 0 || y >= _height)
                    continue;

                Node node = _grid[x, y];

                // Ignore non-walkable nodes
                if (!node.IsWalkable)
                    continue;

                // Ignore already-closed nodes
                if (node.State == Node.NodeState.Closed)
                    continue;

                // Already-open nodes are only added to the list if their G-value is lower going via this route.
                if (node.State == Node.NodeState.Open)
                {
                    float traversalCost = Node.GetTraversalCost(node.Location, node.ParentNode.Location);
                    float gTemp = fromNode.G + traversalCost;
                    if (gTemp < node.G)
                    {
                        node.ParentNode = fromNode;
                        walkableNodes.Add(node);
                    }
                }
                else
                {
                    // If it's untested, set the parent and flag it as 'Open' for consideration
                    node.ParentNode = fromNode;
                    node.State = Node.NodeState.Open;
                    walkableNodes.Add(node);
                }
            }

            return walkableNodes;
        }

        private static IEnumerable<Vector2> GetAdjacentLocations(Vector2 fromLocation)
        {
            return new Vector2[]
            {
                new Vector2(fromLocation.x-1, fromLocation.y  ),
                new Vector2(fromLocation.x,   fromLocation.y+1),
                new Vector2(fromLocation.x+1, fromLocation.y  ),
                new Vector2(fromLocation.x,   fromLocation.y-1)

            //Used for diagonal searches
                //new Vector2(fromLocation.x-1, fromLocation.y-1),
                //new Vector2(fromLocation.x-1, fromLocation.y+1),
                //new Vector2(fromLocation.x+1, fromLocation.y+1),
                //new Vector2(fromLocation.x+1, fromLocation.y-1),
            };
        }
    }
}
