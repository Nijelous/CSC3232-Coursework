using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The Melee action, which satsfies the hitShadow goal with a guaranteed hit, hence a lower cost
public class Melee : GOAPAction
{
    private SceneHandler sm;

    private void Start()
    {
        sm = GameObject.Find("Scene Manager").GetComponent<SceneHandler>();
    }

    public override bool PrePerform()
    {
        target = transform.position;
        return true;
    }

    public override bool PostPerform()
    {
        transform.forward = (sm.GetPlayer().transform.position - transform.position).normalized;
        if (sm.GetShadow())
        {
            sm.GetShadow().GetComponent<ShadowController>().RemoveShadow();
            GameObject.Find("Player").GetComponent<PlayerStats>().Hit();
            return true;
        }
        return false;
    }
}
