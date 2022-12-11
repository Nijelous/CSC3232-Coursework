using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// The shooting from the player Shadow
public class ShadowBolt : MonoBehaviour
{
    [SerializeField]
    Transform bullet;

    [SerializeField]
    Transform spawnPos;

    private Text text;

    private GameObject[] ammoList; // The images of the ammo

    private int ammo = 10;

    // Start is called before the first frame update
    void Start()
    {
        text = GameObject.Find("Shadow Ammo").transform.GetChild(0).GetComponent<Text>();

        ammoList = new GameObject[ammo];
        for(int i = 0; i < ammo; i++)
        {
            ammoList[i] = GameObject.Find("Shadow Ammo").transform.GetChild(i + 1).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Shooting logic, instantiates the bullet with the forward being the vector between the place in the world the mouse has hit and the spawner;
        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && ammo > 0)
            {
                Vector3 aimDir = (gameObject.GetComponent<ThirdPersonMovement>().mouseWorldPosition - spawnPos.position).normalized;
                Instantiate(bullet, spawnPos.position, Quaternion.LookRotation(aimDir, Vector3.up));
                ammo--;
                ammoList[ammo].SetActive(false);
                if (ammo == 0)
                {
                    StartCoroutine(Reload());
                }
            }
        }
    }

    IEnumerator Reload()
    {
        text.text = "Reloading";
        yield return new WaitForSeconds(2f);
        foreach (GameObject go in ammoList)
        {
            go.SetActive(true);
        }
        text.text = "Shadow Ammo:";
        ammo = 10;
    }

}
