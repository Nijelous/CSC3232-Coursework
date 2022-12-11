using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Switch to open and close a linked door
public class DoorSwitch : Switch
{
    [SerializeField]
    GameObject door;

    protected override void Activate()
    {
        door.SetActive(false);
    }

    protected override void Deactivate()
    {
        door.SetActive(true);
    }
}
