using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Switch-esque method that ends the game when activated. Larger range than a typical switch.
public class GameEnd : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro tmp;

    private SceneHandler sm;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        sm = GameObject.Find("Scene Manager").GetComponent<SceneHandler>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if ((player.transform.position - gameObject.transform.position).magnitude < 6f)
        {
            tmp.gameObject.SetActive(true);
            Quaternion lookRotation = Quaternion.LookRotation(GameObject.Find("Main Camera").transform.forward);
            tmp.transform.rotation = Quaternion.Slerp(tmp.transform.rotation, lookRotation, Time.deltaTime * 5.0f);
            //tmp.gameObject.transform.LookAt(GameObject.Find("Main Camera").transform);
            if (Input.GetKeyDown(KeyCode.R))
            {
                GameObject.Find("State Manager").GetComponent<StateManager>().WinStart();
            }
        }
        else
        {
            tmp.gameObject.SetActive(false);
        }
    }
}
