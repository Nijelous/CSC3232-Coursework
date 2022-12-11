using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

/** Deals with the swapping to and from shadow form, including camera movement
 * shadow: the shadow prefab
 * player: the player in the scene
 * cinemachine: the free look camera
 * aimCamera: the camera for aiming down sight
 * crosshair: the crosshair image on the canvas
 * sm: the SceneManager class in the scene
 * shadowAmmo: the prefab of the ammo display
 */
public class ShadowController : MonoBehaviour
{
    [SerializeField]
    private GameObject shadow;

    private GameObject player;

    private CinemachineFreeLook cinemachine;

    private CinemachineVirtualCamera aimCamera;

    private Image crosshair;

    private SceneHandler sm;

    [SerializeField]
    private GameObject shadowAmmo;

    // Start is called before the first frame update
    void Start()
    {
        sm = GameObject.Find("Scene Manager").GetComponent<SceneHandler>();
        player = GameObject.FindGameObjectWithTag("Player");
        cinemachine = GameObject.Find("Third Person Camera").GetComponent<CinemachineFreeLook>();
        aimCamera = GameObject.Find("Third Person Camera Aim").GetComponent<CinemachineVirtualCamera>();
        crosshair = GameObject.Find("Crosshair").GetComponent<Image>();
        if (!shadowAmmo) shadowAmmo = GameObject.Find("Shadow Ammo");
    }

    // Update is called once per frame
    void Update()
    {
        // Switching from player to shadow
        if (Input.GetKeyDown(KeyCode.E) && gameObject.name == "Player" && !GameObject.Find("Shadow(Clone)"))
        {
            // Drops the bomb should the player have it
            if (sm.HasBomb())
            {
                Transform bomb = transform.Find("Bomb(Clone)");
                bomb.parent = null;
                bomb.GetComponent<Rigidbody>().useGravity = true;
                bomb.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            }
            // Sets up the camera and Scene Manager's shadow tracking, as well as instantiating the shadow behind the player
            shadowAmmo.SetActive(true);
            GameObject newShadow = Instantiate(shadow, new Vector3(transform.position.x, transform.position.y, transform.position.z - 2f), Quaternion.identity);
            sm.SetShadow(newShadow);
            cinemachine.LookAt = newShadow.transform;
            cinemachine.Follow = newShadow.transform;
            aimCamera.Follow = newShadow.transform;
        }
        else if(Input.GetKeyDown(KeyCode.E) && gameObject.name == "Shadow(Clone)") // Switching back to the player
        {
            RemoveShadow();
        }

        // If aiming the cinemachine cameras are switched
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            aimCamera.Priority = 11;
            crosshair.enabled = true;
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            aimCamera.Priority = 9;
            crosshair.enabled = false;
        }
    }

    public void RemoveShadow()
    {
        sm.ToggleShadows();
        shadowAmmo.SetActive(false);
        cinemachine.LookAt = player.transform;
        cinemachine.Follow = player.transform;
        aimCamera.Follow = player.transform;
        shadowAmmo.transform.GetChild(0).GetComponent<Text>().text = "Shadow Ammo: ";
        for(int i = 1; i < shadowAmmo.transform.childCount; i++)
        {
            shadow.transform.GetChild(i).gameObject.SetActive(true);
        }
        Destroy(gameObject);

    }
}
