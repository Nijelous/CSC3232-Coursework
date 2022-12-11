using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** The base interface for actions in GOAP
 * actionName: the name of this action
 * cost: how much the AI wants to do this action, the lower the better
 * target: where the AI needs to move to for the action to take place
 * targetTag: in case there is no specific destination, checks for the tags
 * duration: how long the action takes to complete
 * preConditions: the world state required for the action to take place
 * afterEffects: the effects on the world this action will have
 * agent: the navigation agent
 * agentBeliefs: the beliefs that this specifc agent holds
 * running: whether the action is in use currently
 */
public abstract class GOAPAction : MonoBehaviour
{
    public string actionName = "Action";
    public float cost = 1.0f;
    public Vector3 target;
    public string targetTag;
    public float duration = 0;
    public WorldState[] preConditions;
    public WorldState[] afterEffects;
    public AStarAgent agent;

    public Dictionary<string, int> preconditions;
    public Dictionary<string, int> effects;

    public WorldStates agentBeliefs;

    public bool running = false;

    public GOAPAction()
    {
        preconditions = new Dictionary<string, int>();
        effects = new Dictionary<string, int>();
    }

    public void Awake()
    {
        agent = gameObject.GetComponent<AStarAgent>();

        if(preConditions != null)
        {
            foreach(WorldState w in preConditions)
            {
                preconditions.Add(w.key, w.value);
            }
        }
        if (afterEffects != null)
        {
            foreach (WorldState w in afterEffects)
            {
                effects.Add(w.key, w.value);
            }
        }
    }

    public bool IsAchievable()
    {
        return true;
    }

    public bool IsAchieveableGiven(Dictionary<string, int> conditions)
    {
        foreach(KeyValuePair<string, int> p in preconditions)
        {
            if (!conditions.ContainsKey(p.Key)) return false;
        }
        return true;
    }

    public abstract bool PrePerform();
    public abstract bool PostPerform();
}
