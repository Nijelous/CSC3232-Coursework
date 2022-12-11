using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// The parent class of all agents, setting their base behaviour

// The goals of the agent, which it will search for in the leaf nodes of the planner
public class SubGoal {
    public Dictionary<string, int> sgoals;
    public bool remove;

    public SubGoal(string s, int i, bool r)
    {
        sgoals = new Dictionary<string, int>();
        sgoals.Add(s, i);
        remove = r;
    }
}

/**
 * actions: all the actions that the agent can carry out
 * goals: all the goals, and therefore sub goals, ordered by importance (highest to lowest)
 * planner: the class that calculates the best action to take given the world state and beliefs
 * actionQueue: the current queue of actions being carried out
 * currentGoal: the goal being worked towards
 * beliefs: the world states specific to this agent
 * finishingAction: if the agent has activated the coroutine to finish the action
 */
public class GOAPAgent : MonoBehaviour
{
    public List<GOAPAction> actions = new List<GOAPAction>();
    public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();

    GOAPPlanner planner;
    public Queue<GOAPAction> actionQueue;
    public GOAPAction currentAction;
    SubGoal currentGoal;
    public WorldStates beliefs = new WorldStates();

    private bool finishingAction = false;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        GOAPAction[] acts = GetComponents<GOAPAction>();
        foreach (GOAPAction a in acts) actions.Add(a);
    }

    // Cancels the queue if PostPerform cannot be carried out, as the next action won't be possible
    private IEnumerator CompleteAction(float duration)
    {
        yield return new WaitForSeconds(duration);
        if (!currentAction.PostPerform())
        {
            actionQueue = null;
        }
        currentAction.running = false;
        finishingAction = false;
    }

    void LateUpdate()
    {
        if(currentAction != null && currentAction.running) // If the action is running, then check whether it is ready to be finished, or stuck
        {
            if(currentAction.agent.IsMoving() && currentAction.agent.RemainingDistance() < 1f)
            {
                if (!finishingAction)
                {
                    StartCoroutine(CompleteAction(currentAction.duration));
                    finishingAction = true;
                }
            }
            else if(!currentAction.agent.IsMoving() && currentAction.agent.RemainingDistance() >= 1f)
            {
                currentAction = null;
                actionQueue = null;
            }
            return;
        }
        // Replanning, going through all goals depending on priority, and passing in the actions available, the goals, and the beliefs of the agent
        if(planner == null || actionQueue == null) // If the queue is set to null, this was a forced replanning, planners are null initially
        {
            planner = new GOAPPlanner();

            var sortedGoals = from entry in goals orderby entry.Value descending select entry;

            foreach(KeyValuePair<SubGoal, int> sg in sortedGoals)
            {
                actionQueue = planner.Plan(actions, sg.Key.sgoals, beliefs);
                if(actionQueue != null)
                {
                    currentGoal = sg.Key;
                    break;
                }
            }
        }

        if(actionQueue != null && actionQueue.Count == 0) // If the queue is empty, this has run it's course and must replan
        {
            if (currentGoal.remove)
            {
                goals.Remove(currentGoal);
            }
            planner = null;
        }

        if(actionQueue != null && actionQueue.Count > 0) // If the action isn't running, but there are more actions to run, then prepares the next action
        {
            currentAction = actionQueue.Dequeue();
            if (currentAction.PrePerform()) // If the preperform fails, then the whole thing is aborted, otherwise starts up the next action
            {
                if (currentAction.target == null && currentAction.targetTag != "") currentAction.target = GameObject.FindGameObjectWithTag(currentAction.targetTag).transform.position;
                if(currentAction.target != null)
                {
                    currentAction.running = true;
                    currentAction.agent.MoveTo(currentAction.target);
                }
            }
            else
            {
                actionQueue = null;
            }
        }
    }
}
