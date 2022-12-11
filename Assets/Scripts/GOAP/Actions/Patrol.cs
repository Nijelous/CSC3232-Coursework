using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The Patrol action, which satisfies the explore goal
public class Patrol : GOAPAction
{
    private Vector3[] directions =
    {
        new Vector3(0, 0, 1),
        new Vector3(1, 0, 0),
        new Vector3(0, 0, -1),
        new Vector3(-1, 0, 0)
    };

    // Tries to either go in the same direction, or a new one which switches up each runthrough of this action
    public override bool PrePerform()
    {
        float r = Random.Range(10, 50);
        RaycastHit info;
        if(!Physics.Raycast(transform.position, transform.forward, out info, r, 1 << LayerMask.NameToLayer("Default")))
        {
            target = transform.position + (transform.forward * r);
            return true;
        }
        foreach (Vector3 dir in directions)
        {
            if (!Physics.Raycast(transform.position, dir, out info, r, 1 << LayerMask.NameToLayer("Default")))
            {
                target = transform.position + (dir * r);
                return true;
            }
        }
        Vector3 temp = directions[0];
        for(int i = 0; i < 3; i++)
        {
            directions[i] = directions[i + 1];
        }
        directions[3] = temp;
        return false;
    }

    public override bool PostPerform()
    {
        return true;
    }
}
