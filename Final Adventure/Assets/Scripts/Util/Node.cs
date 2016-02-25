using System;
using UnityEngine;
using System.Collections;

public class Node
{

    public Vector2 Location { get; private set; }

    public bool IsWalkable { get; set; }

    public float F
    {
        get { return this.G + this.H; }
    }

    public float G { get; private set; }

    public float H { get; private set; }

    public NodeState State { get; set; }

    private Node _parentNode;

    public Node ParentNode
    {
        get { return _parentNode; }
        set
        {
            _parentNode = value;
            this.G = _parentNode.G + GetTraversalCost(this.Location, _parentNode.Location);
        }
    }

    public enum NodeState { Untested, Open, Closed }

    public Node(Vector2 location, bool isWalkable, Vector2 endLocation)
    {
        Location = location;
        State = NodeState.Untested;
        IsWalkable = isWalkable;
        H = GetTraversalCost(Location, endLocation);
        G = 0;
    }

    public static float GetTraversalCost(Vector2 location, Vector2 otherLocation)
    {
        float deltaX = otherLocation.x - location.x;
        float deltaY = otherLocation.y - location.y;
        return (float) Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
    }

}
