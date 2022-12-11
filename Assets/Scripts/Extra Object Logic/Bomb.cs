using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The logic for the bomb, used to blow up certain obstacles
public class Bomb : MonoBehaviour
{
    [SerializeField]
    private GameObject sphere;

    private SceneHandler sm;

    // Start is called before the first frame update
    void Start()
    {
        sm = GameObject.Find("Scene Manager").GetComponent<SceneHandler>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 0)
        {
            StartCoroutine(Explode());
        }
        else if (collision.gameObject.tag == "Explodable")
        {
            StartCoroutine(Explode());
            Destroy(collision.gameObject);
        }
    }

    // Expands a smaller sphere inside the bomb to look like an explosion
    IEnumerator Explode()
    {
        float counter = 0.25f;
        while(counter > 0)
        {
            counter -= Time.deltaTime;
            sphere.transform.localScale *= 1.02f;
            yield return null;
        }
        sm.ToggleBomb();
        Destroy(gameObject);
    }
}
