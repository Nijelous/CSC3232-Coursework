using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The agent class for the ShadowMonsters, with 3 goals
public class ShadowMonster : GOAPAgent
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        SubGoal s1 = new SubGoal("hideFromLight", 1, false);
        SubGoal s2 = new SubGoal("hitShadow", 1, false);
        SubGoal s3 = new SubGoal("explore", 1, false);
        goals.Add(s1, 3);
        goals.Add(s2, 2);
        goals.Add(s3, 1);
        beliefs.AddState("isVisible", 1);
    }
}
