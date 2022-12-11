using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** The agent navigating the mesh of nodes
 * nav: the AStar algorithm attached to the object doing the pathfidning
 * moving: whether the AI has a path and is using it
 * sm: the SceneManager class in the scene 
 * path: the path the agent is currently following
 * destination: where the agent is going
 */
public class AStarAgent : MonoBehaviour
{
    private AStar nav;

    private bool moving = false;

    private SceneHandler sm;

    private List<PathfindingNode> path;

    private Vector3 destination;

    // Start is called before the first frame update
    void Start()
    {
        path = new List<PathfindingNode>();

        nav = GetComponent<AStar>();

        sm = GameObject.Find("Scene Manager").GetComponent<SceneHandler>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (sm.GetTerrainChanged()) // If the terrain has updated, checks to see if there's still a valid path to the destination, and if not then it stops moving
        {
            if (nav.FindPath(transform.position, destination))
            {
                path = nav.GetPath();
            }
            else
            {
                moving = false;
            }
            sm.ToggleTerrainChanged(false);
        }
        else if(moving && path.Count > 0) // Otherwise, if the path exists and the player is moving, constantly check for the next node
        {
            if((path[path.Count-1].transform.position - transform.position).magnitude < 6.1f) // If the current node is within range, then get the next node if it exists
            {
                path.RemoveAt(path.Count-1);
                if(path.Count == 0) // If it doesn't, stop moving
                {
                    moving = false;
                }
                else
                {
                    transform.forward = new Vector3(path[path.Count-1].transform.position.x - transform.position.x, 0f, path[path.Count - 1].transform.position.z - transform.position.z).normalized;
                }
            }
            else // If it's not, then move towards that node
            {
                transform.position += transform.forward * Time.deltaTime * GetComponent<MonsterStats>().GetSpeed();
            }
        }
    }

    public bool IsMoving()
    {
        return moving;
    }

    public void MoveTo(Vector3 dest)
    {
        destination = dest;
        if(nav.FindPath(transform.position, destination))
        {
            path = nav.GetPath();
            moving = true;
        }
    }

    public float RemainingDistance()
    {
        if (path.Count == 0) return 0f;
        else return (path[0].transform.position - transform.position).magnitude - 6;
    }
}
