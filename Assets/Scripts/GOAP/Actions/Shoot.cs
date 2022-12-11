using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The Shoot action, which satisfies the hitShadow goal, though it is more inconsistent and therefore higher cost
public class Shoot : GOAPAction
{
    [SerializeField]
    private Transform bullet;

    [SerializeField]
    private Transform spawnPos;

    private SceneHandler sm;

    // How often this one happens is based on the difficulty, giving a small buffer between shots
    private void Start()
    {
        sm = GameObject.Find("Scene Manager").GetComponent<SceneHandler>();
        switch (GameObject.Find("State Manager").GetComponent<StateManager>().difficulty)
        {
            case StateManager.Difficulty.Easy:
                duration = 2;
                break;
            case StateManager.Difficulty.Medium:
                duration = 1;
                break;
            case StateManager.Difficulty.Hard:
                duration = 0;
                break;
        }
    }

    public override bool PrePerform()
    {
        target = transform.position;
        return true;
    }

    public override bool PostPerform()
    {
        Vector3 aimDir = (sm.GetPlayer().transform.position - spawnPos.position).normalized;
        transform.forward = aimDir;
        Instantiate(bullet, spawnPos.position, Quaternion.LookRotation(aimDir, Vector3.up));
        return true;
    }
}
