using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The stats of the monsters, and the logic around that, including some manupulation of the GOAP
public class MonsterStats : MonoBehaviour
{
    [SerializeField]
    private int hp;

    [SerializeField]
    private float mass;

    private int blocks;

    private int massLoss;

    private SceneHandler sm;

    private GOAPAgent agent;

    private bool notEntered = true;

    // Start is called before the first frame update
    void Start()
    {
        sm = GameObject.Find("Scene Manager").GetComponent<SceneHandler>();
        for (int i = 0; i < transform.childCount; i++)
        {
            blocks += transform.GetChild(i).childCount;
        }
        massLoss = 1 / blocks;
        agent = GetComponent<GOAPAgent>();
        agent.beliefs.AddState("cannotSeePlayer", 1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Constantly checks for whether the player is within range, and if they are then change the GOAP beliefs and set it to replan
        Vector3 dir = (sm.GetPlayer().transform.position - transform.position).normalized;
        if (Physics.Raycast(transform.position, dir, out RaycastHit info, 80))
        {
            if (info.transform.gameObject.layer == 6 || info.transform.gameObject.layer == 7)
            {
                if((sm.GetPlayer().transform.position - transform.position).magnitude < 5.5f*GetSpeed())
                {
                    agent.beliefs.AddState("playerInRange", 1);
                }
                else
                {
                    agent.beliefs.RemoveState("playerInRange");
                }
                agent.beliefs.RemoveState("cannotSeePlayer");
                agent.beliefs.AddState("seePlayer", 1);
                if (notEntered)
                {
                    notEntered = false;
                    agent.actionQueue = null;
                    agent.currentAction.running = false;
                    agent.currentAction = null;
                    notEntered = false;
                }
            }
            else
            {
                agent.beliefs.RemoveState("seePlayer");
                agent.beliefs.AddState("cannotSeePlayer", 1);
                notEntered = true;
            }
        }
    }

    public void Die()
    {
        sm.EnemyKilled();
        Destroy(gameObject);
    }

    public void Damage(int damage)
    {
        hp -= damage;
        mass -= massLoss;
        if(hp <= 0)
        {
            Die();
        }
    }

    // As the player loses mass, the faster it becomes
    public float GetSpeed()
    {
        return 3.5f * Mathf.Pow(2f - mass, 2);
    }
}
