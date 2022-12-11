using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// When activated, rotates the rooms and, on first activation, activates the map for that room
// Both activate and deactivate do the same thing, so the sphere material is kept the same
public class RoomSwitch : Switch
{
    [SerializeField]
    private ShadowRoom[] adjRooms = new ShadowRoom[4];

    protected override void Activate()
    {
        transform.parent.GetChild(5).gameObject.SetActive(true);
        transform.parent.GetComponent<ShadowRoom>().Rotate();
        for (int i = 0; i < 4; i++)
        {
            if (adjRooms[i])
            {
                int colour = transform.parent.GetComponent<ShadowRoom>().GetWallColour(i);
                switch (colour)
                {
                    case 0:
                        if (adjRooms[i].GetWallColour((i + 2) % 4) == 0) UpdateDoors(2, i);
                        else if (adjRooms[i].GetWallColour((i + 2) % 4) == 1) UpdateDoors(1, i);
                        else UpdateDoors(0, i);
                        break;
                    case 1:
                        if (adjRooms[i].GetWallColour((i + 2) % 4) == 1) UpdateDoors(2, i);
                        else if (adjRooms[i].GetWallColour((i + 2) % 4) == 0) UpdateDoors(1, i);
                        else UpdateDoors(0, i);
                        break;
                    case 2:
                        if (adjRooms[i].GetWallColour((i + 2) % 4) == 2) UpdateDoors(2, i);
                        else if (adjRooms[i].GetWallColour((i + 2) % 4) == 3) UpdateDoors(1, i);
                        else UpdateDoors(0, i);
                        break;
                    case 3:
                        if (adjRooms[i].GetWallColour((i + 2) % 4) == 3) UpdateDoors(2, i);
                        else if (adjRooms[i].GetWallColour((i + 2) % 4) == 2) UpdateDoors(1, i);
                        else UpdateDoors(0, i);
                        break;
                }
            }
            else
            {
                if (transform.parent.GetComponent<ShadowRoom>().GetWallColour(i) == 1) transform.parent.GetComponent<ShadowRoom>().ChangeDoor(2, i);
                else transform.parent.GetComponent<ShadowRoom>().ChangeDoor(0, i);
            }
        }
    }

    protected override void Deactivate()
    {
        sphere.material = Resources.Load("Materials/Activated", typeof(Material)) as Material;
        transform.parent.GetComponent<ShadowRoom>().Rotate();
        for (int i = 0; i < 4; i++)
        {
            if (adjRooms[i])
            {
                int colour = transform.parent.GetComponent<ShadowRoom>().GetWallColour(i);
                switch (colour)
                {
                    case 0:
                        if (adjRooms[i].GetWallColour((i + 2) % 4) == 0) UpdateDoors(2, i);
                        else if (adjRooms[i].GetWallColour((i + 2) % 4) == 1) UpdateDoors(1, i);
                        else UpdateDoors(0, i);
                        break;
                    case 1:
                        if (adjRooms[i].GetWallColour((i + 2) % 4) == 1) UpdateDoors(2, i);
                        else if (adjRooms[i].GetWallColour((i + 2) % 4) == 0) UpdateDoors(1, i);
                        else UpdateDoors(0, i);
                        break;
                    case 2:
                        if (adjRooms[i].GetWallColour((i + 2) % 4) == 2) UpdateDoors(2, i);
                        else if (adjRooms[i].GetWallColour((i + 2) % 4) == 3) UpdateDoors(1, i);
                        else UpdateDoors(0, i);
                        break;
                    case 3:
                        if (adjRooms[i].GetWallColour((i + 2) % 4) == 3) UpdateDoors(2, i);
                        else if (adjRooms[i].GetWallColour((i + 2) % 4) == 2) UpdateDoors(1, i);
                        else UpdateDoors(0, i);
                        break;
                }
            }
            else
            {
                if (transform.parent.GetComponent<ShadowRoom>().GetWallColour(i) == 1) transform.parent.GetComponent<ShadowRoom>().ChangeDoor(2, i);
                else transform.parent.GetComponent<ShadowRoom>().ChangeDoor(0, i);
            }
        }
    }

    private void UpdateDoors(int state, int i)
    {
        transform.parent.GetComponent<ShadowRoom>().ChangeDoor(state, i);
        adjRooms[i].ChangeDoor(state, (i + 2) % 4);
    }

    public ShadowRoom[] GetAdjacent()
    {
        return adjRooms;
    }
}
