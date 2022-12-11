using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// The base switch class, to be inherited from for all the switches due to them all having different effects
public class Switch : MonoBehaviour
{
    [SerializeField]
    protected int requiredKills;

    protected SceneHandler sm;

    [SerializeField]
    protected Renderer sphere;

    protected bool activated;

    [SerializeField]
    protected TextMeshPro tmp;

    // Start is called before the first frame update
    protected void Start()
    {
        sm = GameObject.Find("Scene Manager").GetComponent<SceneHandler>();
    }

    // Update is called once per frame
    protected void Update()
    {
        // Shows text when the player is in range with a useful prompt which rotates towards the player
        if ((sm.GetPlayer().transform.position - gameObject.transform.position).magnitude < 3f)
        {
            tmp.gameObject.SetActive(true);
            Quaternion lookRotation = Quaternion.LookRotation(GameObject.Find("Main Camera").transform.forward);
            tmp.transform.rotation = Quaternion.Slerp(tmp.transform.rotation, lookRotation, Time.deltaTime * 5.0f);
            if (sm.GetEnemiesKilled() < requiredKills)
            {
                tmp.text = "Shadows to Vanquish: " + (requiredKills - sm.GetEnemiesKilled());
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                ToggleActive();
            }
        }
        else
        {
            tmp.gameObject.SetActive(false);
        }
    }

    public void CheckKillPass()
    {
        if(sm.GetEnemiesKilled() >= requiredKills && !activated)
        {
            sphere.material = Resources.Load("Materials/Deactivated", typeof(Material)) as Material;
            tmp.text = "Press R to activate";
        }
    }

    public void ToggleActive()
    {
        activated = !activated;
        if (activated)
        {
            sphere.material = Resources.Load("Materials/Activated", typeof(Material)) as Material;
            tmp.text = "Press R to deactivate";
            Activate();
        }
        else
        {
            sphere.material = Resources.Load("Materials/Deactivated", typeof(Material)) as Material;
            tmp.text = "Press R to activate";
            Deactivate();
        }
    }

    protected virtual void Activate()
    {
        Debug.Log("Activated");
    }

    protected virtual void Deactivate()
    {
        Debug.Log("Deactivated");
    }
}
