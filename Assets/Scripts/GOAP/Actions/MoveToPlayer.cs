using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The Move To Player action, which allows for the Melee action to take place
public class MoveToPlayer : GOAPAction
{
    private SceneHandler sm;

    private void Start()
    {
        sm = GameObject.Find("Scene Manager").GetComponent<SceneHandler>();
    }

    public override bool PrePerform()
    {
        target = sm.GetPlayer().transform.position;
        return true;
    }

    // Checks to see whether player is still withing melee range (3)
    public override bool PostPerform()
    {
        RaycastHit info;
        Vector3 dir = (sm.GetPlayer().transform.position - transform.position).normalized;
        if (Physics.Raycast(transform.position, dir, out info, 3))
        {
            if (info.transform.gameObject.layer == 7)
            {
                return true;
            }
        }
        return false;
    }
}
