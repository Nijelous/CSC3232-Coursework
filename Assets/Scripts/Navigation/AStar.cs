using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The AStar algorithm, slightly tweaked from the practical
public class AStar : MonoBehaviour
{
    private GameObject destination;

    [SerializeField]
    private LayerMask nodeMask;

    private List<PathfindingNode> path = new List<PathfindingNode>();

    PathfindingNode GetNodeForPos(Vector3 start)
    {
        if (Physics.Raycast(start, Vector3.down, out RaycastHit info, 999f, nodeMask))
        {
            return info.transform.GetComponent<PathfindingNode>();
        }
        return null;
    }

    float CalculateHeuristic(PathfindingNode n)
    {
        return (n.transform.position - destination.transform.position).magnitude;
    }

    public bool FindPath(Vector3 start, Vector3 dest)
    {
        path.Clear();
        PathfindingNode startNode = GetNodeForPos(start);
        PathfindingNode destNode = GetNodeForPos(dest);
        if(!startNode || !destNode)
        {
            return false;
        }
        destination = destNode.gameObject;
        startNode.parent = null;
        startNode.g = 0.0f;
        startNode.h = CalculateHeuristic(startNode);

        List<PathfindingNode> openList = new List<PathfindingNode>();
        List<PathfindingNode> closedList = new List<PathfindingNode>();

        openList.Add(startNode);
        while(openList.Count > 0)
        {
            PathfindingNode node = openList[0];
            for(int i = 1; i < openList.Count; ++i)
            {
                if(openList[i].GetFScore() < node.GetFScore())
                {
                    node = openList[i];
                }
            }
            if(node == destNode)
            {
                while (node)
                {
                    path.Add(node);
                    node = node.parent;
                }
                return true;
            }
            foreach(PathfindingNode n in node.allNeighbours)
            {
                if (closedList.Contains(n))
                {
                    continue;
                }
                if (n.IsBlocked())
                {
                    closedList.Add(node);
                    continue;
                }
                float newH = CalculateHeuristic(n);
                float newG = node.GetFScore() + n.traversalCost;
                float newF = newG = newH;
                bool inList = openList.Contains(n);

                if(newF < node.GetFScore() || !inList)
                {
                    if (!inList)
                    {
                        n.h = newH;
                        openList.Add(n);
                    }
                    n.g = newG;
                    n.h = newH;
                    n.parent = node;
                }
            }
            openList.Remove(node);
            closedList.Add(node);
        }
        return false;
    }

    // returns the path in order for the agent to utilise it
    public List<PathfindingNode> GetPath()
    {
        return path;
    }
}
