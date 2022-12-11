using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// The class for planning out next actions of an AI
/** Node
 * parent: the node that came before this one
 * cost: the amount it takes to carry out to this node, the lower the better
 * states: the current states taken into account by the node, and enacted by afterEffects of the actions
 * action: the action tied to the node
 */
public class Node {
    public Node parent;
    public float cost;
    public Dictionary<string, int> states;
    public GOAPAction action;

    public Node(Node parent, float cost, Dictionary<string, int> allStates, GOAPAction action)
    {
        this.parent = parent;
        this.cost = cost;
        states = new Dictionary<string, int>(allStates);
        this.action = action;
    }
}

public class GOAPPlanner
{
    public Queue<GOAPAction> Plan(List<GOAPAction> actions, Dictionary<string, int> goal, WorldStates states)
    {
        // Checks if the actions are doable
        List<GOAPAction> usableActions = new List<GOAPAction>();
        foreach(GOAPAction a in actions)
        {
            if (a.IsAchievable()) usableActions.Add(a);
        }

        // Collects all the initial states, combining the beliefs of the agent and the world states
        List<Node> leaves = new List<Node>();
        Node start = new Node(null, 0, GOAPWorld.Instance.GetWorld().GetStates(), null);
        foreach(KeyValuePair<string, int> s in states.states)
        {
            start.states.Add(s.Key, s.Value);
        }

        bool success = BuildGraph(start, leaves, usableActions, goal);

        // If the graph didn't get anything, just returns null as there is no plan
        if (!success)
        {
            // Debug.Log("No Plan");
            return null;
        }

        // Finds the cheapest path constructed using the leaf nodes
        Node cheapest = null;
        foreach(Node leaf in leaves)
        {
            if (cheapest == null) cheapest = leaf;
            else
            {
                if (leaf.cost < cheapest.cost) cheapest = leaf;
            }
        }

        // Gets the list of actions and queues them up to pass back
        List<GOAPAction> result = new List<GOAPAction>();
        Node n = cheapest;
        while(n != null)
        {
            if (n.action != null)
            {
                result.Insert(0, n.action);
            }
            n = n.parent;
        }

        Queue<GOAPAction> queue = new Queue<GOAPAction>();
        foreach (GOAPAction a in result)
        {
            queue.Enqueue(a);
        }

        return queue;
    }

    // Gets the graph of actions recursively
    private bool BuildGraph(Node parent, List<Node> leaves, List<GOAPAction> usableActions, Dictionary<string, int> goal)
    {
        // Tries every single action
        bool foundPath = false;
        foreach(GOAPAction action in usableActions)
        {
            // Checks whether the action is doable at this current point in time
            if (action.IsAchieveableGiven(parent.states))
            {
                // If it is, creates a new node and continues down this path, removing that action from the set of actions
                Dictionary<string, int> currentState = new Dictionary<string, int>(parent.states);
                foreach(KeyValuePair<string, int> effects in action.effects)
                {
                    if (!currentState.ContainsKey(effects.Key)) currentState.Add(effects.Key, effects.Value);
                }
                Node node = new Node(parent, parent.cost + action.cost, currentState, action);

                // If the node causes the goal to be added to the states, then it is complete
                if(GoalAchieved(goal, currentState))
                {
                    leaves.Add(node);
                    foundPath = true;
                }
                // Otherwise, recurse
                else
                {
                    List<GOAPAction> subset = ActionSubset(usableActions, action);
                    bool found = BuildGraph(node, leaves, subset, goal);
                    if (found) foundPath = true;
                }
            }
        }
        return foundPath;
    }

    private bool GoalAchieved(Dictionary<string, int> goal, Dictionary<string, int> state)
    {
        foreach(KeyValuePair<string, int> g in goal)
        {
            if (!state.ContainsKey(g.Key)) return false;
        }
        return true;
    }

    private List<GOAPAction> ActionSubset(List<GOAPAction> actions, GOAPAction remove)
    {
        List<GOAPAction> subset = new List<GOAPAction>();
        foreach(GOAPAction a in actions)
        {
            if (!a.Equals(remove)) subset.Add(a);
        }
        return subset;
    }
}
