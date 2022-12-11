using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Basic proximity floating text, for helper markers to give useful hints or instructions
public class ProximityActivation : MonoBehaviour
{
    [SerializeField]
    private float range;

    [SerializeField]
    private TextMeshPro tmp;

    private SceneHandler sm;

    // Start is called before the first frame update
    void Start()
    {
        sm = GameObject.Find("Scene Manager").GetComponent<SceneHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        // Text smoothly rotates to the position of the player
        if ((sm.GetPlayer().transform.position - gameObject.transform.position).magnitude < range)
        {
            tmp.enabled = true;
            Quaternion lookRotation = Quaternion.LookRotation(GameObject.Find("Main Camera").transform.forward);
            tmp.transform.rotation = Quaternion.Slerp(tmp.transform.rotation, lookRotation, Time.deltaTime * 5.0f);
        }
        else
        {
            tmp.enabled = false;
        }
    }
}
