using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The Strafe, or Reload, action, which allows for a shot to be taken
public class Strafe : GOAPAction
{
    private bool right = true;

    private float distance = 10;

    // How far the AI moves is based on the difficulty, giving a small buffer between shots
    private void Start()
    {
        switch(GameObject.Find("State Manager").GetComponent<StateManager>().difficulty)
        {
            case StateManager.Difficulty.Easy:
                distance = 15;
                break;
            case StateManager.Difficulty.Medium:
                distance = 10;
                break;
            case StateManager.Difficulty.Hard:
                distance = 5;
                break;
        }
    }

    // Switches up the strafe direction, if it can, each time this runs
    public override bool PrePerform()
    {
        RaycastHit info;
        if (right)
        {
            right = false;
            if (!Physics.Raycast(transform.position, transform.right, out info, distance, 1 << LayerMask.NameToLayer("Default")))
            {
                target = transform.position + (transform.right * distance);
                return true;
            }
            else if (!Physics.Raycast(transform.position, -transform.right, out info, distance, 1 << LayerMask.NameToLayer("Default")))
            {
                target = transform.position - (transform.right * distance);
                return true;
            }
            
        }
        else
        {
            right = true;
            if (!Physics.Raycast(transform.position, -transform.right, out info, distance, 1 << LayerMask.NameToLayer("Default")))
            {
                target = transform.position - (transform.right * distance);
                return true;
            }
            else if (!Physics.Raycast(transform.position, transform.right, out info, distance, 1 << LayerMask.NameToLayer("Default")))
            {
                target = transform.position + (transform.right * distance);
                return true;
            }
        }
        return false;
    }

    public override bool PostPerform()
    {
        return true;
    }
}
