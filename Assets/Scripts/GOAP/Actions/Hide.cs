using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The Hide action, which satisfies the hideFromLight goal
public class Hide : GOAPAction
{
    public override bool PrePerform()
    {
        target = transform.position;
        return true;
    }

    public override bool PostPerform()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        GetComponent<GOAPAgent>().beliefs.RemoveState("isVisible");
        return true;
    }
}
