using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The Unhide Action, which allows all of the other actions to be taken again once the shadow player is in play or the player is out of sight
public class Unhide : GOAPAction
{
    public override bool PrePerform()
    {
        target = transform.position;
        return true;
    }

    public override bool PostPerform()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }

        GetComponent<GOAPAgent>().beliefs.AddState("isVisible", 1);
        return true;
    }

    
}
