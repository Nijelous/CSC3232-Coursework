using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// The collection point for bombs
public class BombBox : MonoBehaviour
{
    private SceneHandler sm;

    private GameObject player;

    [SerializeField]
    private TextMeshPro tmp;

    [SerializeField]
    private GameObject bomb;

    private void Start()
    {
        sm = GameObject.Find("Scene Manager").GetComponent<SceneHandler>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // If the player is close enough, activates the text like a switch, and if they don't already have a bomb when they press the button, spawns a new one
        // Above their head, and alerts the handler that a bomb is in play
        if ((player.transform.position - gameObject.transform.position).magnitude < 3f)
        {
            tmp.gameObject.SetActive(true);
            Quaternion lookRotation = Quaternion.LookRotation(GameObject.Find("Main Camera").transform.forward);
            tmp.transform.rotation = Quaternion.Slerp(tmp.transform.rotation, lookRotation, Time.deltaTime * 5.0f);
            if (Input.GetKeyDown(KeyCode.R) && !sm.ShadowActive() && !sm.HasBomb())
            {
                GameObject b = Instantiate(bomb, Vector3.zero, Quaternion.identity, player.transform);
                b.transform.localPosition = new Vector3(0, 2f, 0);
                b.transform.localScale = new Vector3(1, 0.7f, 1);
                sm.ToggleBomb();
            }
        }
        else
        {
            tmp.gameObject.SetActive(false);
        }
    }
}
