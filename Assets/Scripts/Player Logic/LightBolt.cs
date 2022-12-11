using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// The logic for the light player shooting bullets, works almost identically to ShadowBolt, except with a different prefab and less ammo, as well as bomb logic
public class LightBolt : MonoBehaviour
{
    [SerializeField]
    private Transform bullet;

    [SerializeField]
    private Transform spawnPos;

    private SceneHandler sm;

    [SerializeField]
    private Text text;

    [SerializeField]
    private GameObject[] ammoList;

    private int ammo = 5;

    // Start is called before the first frame update
    void Start()
    {
        sm = GameObject.Find("Scene Manager").GetComponent<SceneHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && !sm.ShadowActive() && ammo > 0)
            {
                // If the player has the bomb, throws it from where it is on their head and releases all the constraints
                if (!sm.HasBomb())
                {
                    Vector3 aimDir = (gameObject.GetComponent<ThirdPersonMovement>().mouseWorldPosition - spawnPos.position).normalized;
                    Instantiate(bullet, spawnPos.position, Quaternion.LookRotation(aimDir, Vector3.up));
                    ammo--;
                    ammoList[ammo].SetActive(false);
                    if(ammo == 0)
                    {
                        StartCoroutine(Reload());
                    }
                }
                else
                {
                    Vector3 aimDir = transform.forward;
                    GameObject bomb = GameObject.Find("Player").transform.Find("Bomb(Clone)").gameObject;
                    bomb.transform.parent = null;
                    bomb.GetComponent<Rigidbody>().useGravity = true;
                    bomb.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    bomb.GetComponent<Rigidbody>().velocity = transform.forward * 20f;
                }
            }
        }
    }

    IEnumerator Reload()
    {
        text.text = "Reloading";
        yield return new WaitForSeconds(2f);
        foreach(GameObject go in ammoList)
        {
            go.SetActive(true);
        }
        text.text = "Light Ammo:";
        ammo = 5;
    }
}
