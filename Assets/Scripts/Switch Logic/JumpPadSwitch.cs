using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Turns on the linked JumpPad by activating the pad
public class JumpPadSwitch : Switch
{
    [SerializeField]
    GameObject jumpPad;

    protected override void Activate()
    {
        jumpPad.transform.GetChild(1).gameObject.SetActive(true);
    }

    protected override void Deactivate()
    {
        jumpPad.transform.GetChild(1).gameObject.SetActive(false);
    }
}
