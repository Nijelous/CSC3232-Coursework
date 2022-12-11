using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The world singleton that holds the overall world states that affects all AI
public sealed class GOAPWorld
{
    private static readonly GOAPWorld instance = new GOAPWorld();
    private static WorldStates world;

    static GOAPWorld()
    {
        world = new WorldStates();
    }

    private GOAPWorld()
    {
    }

    public static GOAPWorld Instance
    {
        get { return instance; }
    }

    public WorldStates GetWorld()
    {
        return world;
    }
}
