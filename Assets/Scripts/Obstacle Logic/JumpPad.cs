using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The jump pad, which increases the players jump height on it, only working for a full player
public class JumpPad : MonoBehaviour
{
    [SerializeField]
    float jumpMultiplier;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            Debug.Log("Gravity On");
            other.gameObject.GetComponent<ThirdPersonMovement>().SetGravity(-14f/jumpMultiplier, 2f*jumpMultiplier);
        }
    }

    // Upon leaving, gives the player a few seconds before resetting their gravity and jump height
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            Debug.Log("Gravity Coming Off");
            StartCoroutine(EndGravity(other.gameObject));
        }
    }

    IEnumerator EndGravity(GameObject player)
    {
        yield return new WaitForSeconds(1);
        player.GetComponent<ThirdPersonMovement>().SetGravity(-14, 2);
    }
}
