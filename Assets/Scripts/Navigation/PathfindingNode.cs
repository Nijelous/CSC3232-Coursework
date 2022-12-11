using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The base node that makes up the rest of the navigation mesh, all cubes, slightly modified from the practical material
public class PathfindingNode : MonoBehaviour
{

    public List<PathfindingNode> allNeighbours = new List<PathfindingNode>();

    public float traversalCost;

    public float g;
    public float h;
    public PathfindingNode parent;
    public int initialType;
    private static readonly Vector3[] directions =
    {
        new Vector3(-1, 0, 0),
        new Vector3(1, 0, 0),
        new Vector3(0, 0, -1),
        new Vector3(0, 0, 1)
    };

    enum NodeType
    {
        Passable,
        Taken,
        Blocked,
        MAX
    }

    NodeType nodeType = NodeType.Passable;

    public bool IsBlocked()
    {
        return nodeType == NodeType.Blocked;
    }

    public float GetFScore()
    {
        return g + h;
    }

    private void FindNeighbours()
    {
        allNeighbours.Clear();
        foreach(Vector3 dir in directions)
        {
            // Adds a short range to this, in order for the different meshes to remain seperate
            if (Physics.Raycast(transform.position, dir, out RaycastHit info, 5))
            {
                PathfindingNode node = info.transform.gameObject.GetComponent<PathfindingNode>();
                if (node)
                {
                    allNeighbours.Add(node);
                }
            }
        }
    }

    private void SetupNode()
    {
        switch (nodeType)
        {
            case NodeType.Passable:
                traversalCost = 1;
                break;
            case NodeType.Blocked:
                traversalCost = -1;
                break;
            case NodeType.Taken:
                traversalCost = 5;
                break;
        }
    }

    public void ToggleNodeType(int type)
    {
        switch (type)
        {
            case 0:
                nodeType = NodeType.Passable;
                break;
            case 1:
                nodeType = NodeType.Taken;
                break;
            case 2:
                nodeType = NodeType.Blocked;
                break;
        }
        SetupNode();
    }

    // Start is called before the first frame update
    void Start()
    {
        FindNeighbours();
        ToggleNodeType(initialType);
    }
}
