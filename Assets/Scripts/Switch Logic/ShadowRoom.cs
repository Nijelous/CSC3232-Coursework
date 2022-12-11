using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The handler class for a room in the House of Shadows
public class ShadowRoom : MonoBehaviour
{
    [SerializeField]
    private int[] roomConfig;

    [SerializeField]
    LayerMask nodeMask;

    private bool initialUpdate = true;

    private void Start()
    {
        // If a roomConfig has not already been asigned, makes a random one following these rules so the puzzle is actually completable:
        // 1) Must have at least one red/blue and one green/yellow door
        // 2) Room 1 must have its 2nd door be blue
        // 3) Room 6, 8 and 9 must all have one blue door, but it cannot be the outward facing door for 6 and 8
        if (roomConfig.Length != 4)
        {
            bool flag = true;
            roomConfig = new int[4];
            int[] counter = new int[] { 0, 0, 0, 0 };
            while (flag)
            {
                for (int i = 0; i < 4; i++)
                {
                    roomConfig[i] = Random.Range(0, 4);
                    counter[roomConfig[i]]++;
                }
                if((counter[0] != 0 || counter[1] != 0) && (counter[2] != 0 || counter[3] != 0)) // Rule 1
                {
                    flag = false;
                    if(gameObject.name == "Room 1" && roomConfig[1] != 1) // Rule 2
                    {
                        if(counter[2] == 0 && counter[3] == 0)
                        {
                            roomConfig[2] = 2;
                        }
                        else if((counter[2] == 1 && counter[3] == 0 && roomConfig[1] == 2) || (counter[3] == 1 && counter[2] == 0 && roomConfig[1] == 3))
                        {
                            roomConfig[2] = roomConfig[1];
                        }
                        roomConfig[1] = 1;
                    }
                    if ((gameObject.name == "Room 6" || gameObject.name == "Room 8" || gameObject.name == "Room 9") && counter[1] == 0) // Rule 3
                    {
                        flag = true;
                    }
                    else if(gameObject.name == "Room 6" && roomConfig[0] == 1)
                    {
                        if(counter[1] == 1)
                        {
                            roomConfig[0] = roomConfig[2];
                            roomConfig[2] = 1;
                        }
                        else
                        {
                            roomConfig[0] = 0;
                        }
                    }
                    else if(gameObject.name == "Room 8" && roomConfig[3] == 1)
                    {
                        if (counter[1] == 1)
                        {
                            roomConfig[3] = roomConfig[1];
                            roomConfig[1] = 1;
                        }
                        else
                        {
                            roomConfig[3] = 0;
                        }
                    }
                }
            }
        }
        // Updates all the doors to have the correct materials
        for (int i = 0; i < roomConfig.Length; i++)
        {
            switch (roomConfig[i])
            {
                case 0:
                    transform.GetChild(i).GetChild(3).GetChild(1).GetComponent<Renderer>().material = Resources.Load("Materials/Red", typeof(Material)) as Material;
                    transform.GetChild(i).GetChild(3).GetChild(2).GetComponent<Renderer>().material = Resources.Load("Materials/Red", typeof(Material)) as Material;
                    transform.GetChild(i).GetChild(3).GetChild(3).GetComponent<Renderer>().material = Resources.Load("Materials/Red", typeof(Material)) as Material;
                    break;
                case 1:
                    transform.GetChild(i).GetChild(3).GetChild(1).GetComponent<Renderer>().material = Resources.Load("Materials/Blue", typeof(Material)) as Material;
                    transform.GetChild(i).GetChild(3).GetChild(2).GetComponent<Renderer>().material = Resources.Load("Materials/Blue", typeof(Material)) as Material;
                    transform.GetChild(i).GetChild(3).GetChild(3).GetComponent<Renderer>().material = Resources.Load("Materials/Blue", typeof(Material)) as Material;
                    break;
                case 2:
                    transform.GetChild(i).GetChild(3).GetChild(1).GetComponent<Renderer>().material = Resources.Load("Materials/Green", typeof(Material)) as Material;
                    transform.GetChild(i).GetChild(3).GetChild(2).GetComponent<Renderer>().material = Resources.Load("Materials/Green", typeof(Material)) as Material;
                    transform.GetChild(i).GetChild(3).GetChild(3).GetComponent<Renderer>().material = Resources.Load("Materials/Green", typeof(Material)) as Material;
                    break;
                case 3:
                    transform.GetChild(i).GetChild(3).GetChild(1).GetComponent<Renderer>().material = Resources.Load("Materials/Yellow", typeof(Material)) as Material;
                    transform.GetChild(i).GetChild(3).GetChild(2).GetComponent<Renderer>().material = Resources.Load("Materials/Yellow", typeof(Material)) as Material;
                    transform.GetChild(i).GetChild(3).GetChild(3).GetComponent<Renderer>().material = Resources.Load("Materials/Yellow", typeof(Material)) as Material;
                    break;
            }
        }
    }

    private void Update()
    {
        // Updates the doors attributes depending on the opposing door
        // Two of the same colour = open, Red/Blue or Green/Yellow = shadow only, anything else = closed
        // Done in a single update at the start as it needs ALL other rooms to have done their start method to run
        if (initialUpdate)
        {
            ShadowRoom[] rooms = transform.GetChild(4).GetComponent<RoomSwitch>().GetAdjacent();
            for (int i = 0; i < 4; i++)
            {
                if (rooms[i])
                {
                    switch (roomConfig[i])
                    {
                        case 0:
                            if (rooms[i].GetWallColour((i + 2) % 4) == 0) UpdateDoors(2, i, rooms[i]);
                            else if (rooms[i].GetWallColour((i + 2) % 4) == 1) UpdateDoors(1, i, rooms[i]);
                            else UpdateDoors(0, i, rooms[i]);
                            break;
                        case 1:
                            if (rooms[i].GetWallColour((i + 2) % 4) == 1) UpdateDoors(2, i, rooms[i]);
                            else if (rooms[i].GetWallColour((i + 2) % 4) == 0) UpdateDoors(1, i, rooms[i]);
                            else UpdateDoors(0, i, rooms[i]);
                            break;
                        case 2:
                            if (rooms[i].GetWallColour((i + 2) % 4) == 2) UpdateDoors(2, i, rooms[i]);
                            else if (rooms[i].GetWallColour((i + 2) % 4) == 3) UpdateDoors(1, i, rooms[i]);
                            else UpdateDoors(0, i, rooms[i]);
                            break;
                        case 3:
                            if (rooms[i].GetWallColour((i + 2) % 4) == 3) UpdateDoors(2, i, rooms[i]);
                            else if (rooms[i].GetWallColour((i + 2) % 4) == 2) UpdateDoors(1, i, rooms[i]);
                            else UpdateDoors(0, i, rooms[i]);
                            break;
                    }
                }
                else
                {
                    if (GetWallColour(i) == 1) ChangeDoor(2, i);
                    else ChangeDoor(0, i);
                }
            }
            initialUpdate = false;
        }
    }

    // Rotates the room by one clockwise, updating the materials to the appropriate colours
    public void Rotate()
    {
        int store = roomConfig[0];
        for(int i = 0; i < roomConfig.Length-1; i++)
        {
            roomConfig[i] = roomConfig[i + 1];
        }
        roomConfig[roomConfig.Length-1] = store;
        for(int i = 0; i < roomConfig.Length; i++)
        {
            switch (roomConfig[i])
            {
                case 0:
                    transform.GetChild(i).GetChild(3).GetChild(1).GetComponent<Renderer>().material = Resources.Load("Materials/Red", typeof(Material)) as Material;
                    transform.GetChild(i).GetChild(3).GetChild(2).GetComponent<Renderer>().material = Resources.Load("Materials/Red", typeof(Material)) as Material;
                    transform.GetChild(i).GetChild(3).GetChild(3).GetComponent<Renderer>().material = Resources.Load("Materials/Red", typeof(Material)) as Material;
                    break;
                case 1:
                    transform.GetChild(i).GetChild(3).GetChild(1).GetComponent<Renderer>().material = Resources.Load("Materials/Blue", typeof(Material)) as Material;
                    transform.GetChild(i).GetChild(3).GetChild(2).GetComponent<Renderer>().material = Resources.Load("Materials/Blue", typeof(Material)) as Material;
                    transform.GetChild(i).GetChild(3).GetChild(3).GetComponent<Renderer>().material = Resources.Load("Materials/Blue", typeof(Material)) as Material;
                    break;
                case 2:
                    transform.GetChild(i).GetChild(3).GetChild(1).GetComponent<Renderer>().material = Resources.Load("Materials/Green", typeof(Material)) as Material;
                    transform.GetChild(i).GetChild(3).GetChild(2).GetComponent<Renderer>().material = Resources.Load("Materials/Green", typeof(Material)) as Material;
                    transform.GetChild(i).GetChild(3).GetChild(3).GetComponent<Renderer>().material = Resources.Load("Materials/Green", typeof(Material)) as Material;
                    break;
                case 3:
                    transform.GetChild(i).GetChild(3).GetChild(1).GetComponent<Renderer>().material = Resources.Load("Materials/Yellow", typeof(Material)) as Material;
                    transform.GetChild(i).GetChild(3).GetChild(2).GetComponent<Renderer>().material = Resources.Load("Materials/Yellow", typeof(Material)) as Material;
                    transform.GetChild(i).GetChild(3).GetChild(3).GetComponent<Renderer>().material = Resources.Load("Materials/Yellow", typeof(Material)) as Material;
                    break;
            }
        }
    }

    // Updates the doors open and closed state
    public void ChangeDoor(int state, int i)
    {
        RaycastHit info;
        switch (state)
        {
            case 0: // Close
                transform.GetChild(i).GetChild(3).GetChild(0).gameObject.SetActive(true);
                transform.GetChild(i).GetChild(3).GetChild(0).GetComponent<Renderer>().material = Resources.Load("Materials/Door", typeof(Material)) as Material;
                transform.GetChild(i).GetChild(3).GetChild(0).gameObject.layer = 0;
                transform.GetChild(i).GetChild(3).GetChild(0).GetChild(2).gameObject.SetActive(false);
                if (Physics.Raycast(transform.GetChild(i).GetChild(3).GetChild(0).GetChild(0).position, Vector3.down, out info, 999f, nodeMask))
                {
                    info.transform.GetComponent<PathfindingNode>().ToggleNodeType(2);
                }
                if(Physics.Raycast(transform.GetChild(i).GetChild(3).GetChild(0).GetChild(1).position, Vector3.down, out info, 999f, nodeMask))
                {
                    info.transform.GetComponent<PathfindingNode>().ToggleNodeType(2);
                }
                break;
            case 1: // Shadow Only
                transform.GetChild(i).GetChild(3).GetChild(0).gameObject.SetActive(true);
                transform.GetChild(i).GetChild(3).GetChild(0).GetComponent<Renderer>().material = Resources.Load("Materials/Shadow", typeof(Material)) as Material;
                transform.GetChild(i).GetChild(3).GetChild(0).gameObject.layer = 8;
                transform.GetChild(i).GetChild(3).GetChild(0).GetChild(2).gameObject.SetActive(true);
                if (Physics.Raycast(transform.GetChild(i).GetChild(3).GetChild(0).GetChild(0).position, Vector3.down, out info, 999f, nodeMask))
                {
                    info.transform.GetComponent<PathfindingNode>().ToggleNodeType(0);
                }
                if (Physics.Raycast(transform.GetChild(i).GetChild(3).GetChild(0).GetChild(1).position, Vector3.down, out info, 999f, nodeMask))
                {
                    info.transform.GetComponent<PathfindingNode>().ToggleNodeType(0);
                }
                break;
            case 2: // Open
                transform.GetChild(i).GetChild(3).GetChild(0).gameObject.SetActive(false);
                if (Physics.Raycast(transform.GetChild(i).GetChild(3).GetChild(0).GetChild(0).position, Vector3.down, out info, 999f, nodeMask))
                {
                    info.transform.GetComponent<PathfindingNode>().ToggleNodeType(0);
                }
                if (Physics.Raycast(transform.GetChild(i).GetChild(3).GetChild(0).GetChild(1).position, Vector3.down, out info, 999f, nodeMask))
                {
                    info.transform.GetComponent<PathfindingNode>().ToggleNodeType(0);
                }
                break;
        }
        // Tells the scene manager the terrain has changed, causing any moving AI to recalculate its path
        GameObject.Find("Scene Manager").GetComponent<SceneHandler>().ToggleTerrainChanged(true);
    }

    private void UpdateDoors(int state, int i, ShadowRoom room)
    {
        ChangeDoor(state, i);
        room.ChangeDoor(state, (i + 2) % 4);
    }


    public int GetWallColour(int wall)
    {
        return roomConfig[wall];
    }
}
