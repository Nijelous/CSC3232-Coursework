using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages all the aspects of the scene, whether that be for the player or for other game objects, providing a central place to grab important information from
public class SceneHandler : MonoBehaviour
{
    private int enemiesKilled = 0;

    private GameObject shadow;

    private Switch[] switches;

    private GameObject[] shadowWalls;

    private ShadowRoom[] shadowRooms;

    private bool paused;

    private bool hasBomb = false;

    private bool terrainChanged = false;

    private GameObject player;

    public int GetEnemiesKilled()
    {
        return enemiesKilled;
    }

    public void EnemyKilled()
    {
        enemiesKilled++;
        foreach(Switch s in FindObjectsOfType<Switch>())
        {
            s.CheckKillPass();
        }
    }

    public GameObject GetShadow()
    {
        if (!shadow)
        {
            return null;
        }
        else
        {
            return shadow;
        }
    }

    public void SetShadow(GameObject s)
    {
        shadow = s;
        ToggleShadows();
    }

    public void ToggleShadows()
    {
        foreach (GameObject go in shadowWalls)
        {
            go.SetActive(!go.activeSelf);
        }
    }

    public bool ShadowActive()
    {
        if (!shadow)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool HasBomb()
    {
        return hasBomb;
    }

    public void ToggleBomb()
    {
        hasBomb = !hasBomb;
    }

    public bool GetTerrainChanged()
    {
        return terrainChanged;
    }

    public void ToggleTerrainChanged(bool b)
    {
        terrainChanged = b;
    }

    public GameObject GetPlayer()
    {
        if (!player)
        {
            return GameObject.Find("Player");
        }
        return player;
    }

    // Start is called before the first frame update, assigning all the necessary GameObjects
    void Start()
    {
        switches = FindObjectsOfType<Switch>();
        List<GameObject> gameObjects = new List<GameObject>();
        foreach(GameObject go in FindObjectsOfType<GameObject>())
        {
            if(go.layer == 12)
            {
                gameObjects.Add(go);
                go.SetActive(false);
            }
        }
        shadowWalls = gameObjects.ToArray();
        shadowRooms = new ShadowRoom[9];
        GameObject houseOfShadows = GameObject.Find("House of Shadows");
        for(int i = 0; i < 3; i++)
        {
            shadowRooms[i] = houseOfShadows.transform.GetChild(i).GetComponent<ShadowRoom>();
        }
        player = GameObject.Find("Player");
    }

    // Update is called once per frame, here being used to keep track of what the player currently is
    void Update()
    {
        if (GetShadow())
        {
            player = GetShadow();
            GOAPWorld.Instance.GetWorld().AddState("playerIsShadow", 1);
            GOAPWorld.Instance.GetWorld().RemoveState("playerIsLight");
        }
        else
        {
            player = GameObject.Find("Player");
            GOAPWorld.Instance.GetWorld().AddState("playerIsLight", 1);
            GOAPWorld.Instance.GetWorld().RemoveState("playerIsShadow");

        }
    }
}
