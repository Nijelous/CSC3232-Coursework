using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The logic for the shadow being able to move under certain objects, attached to all Obstacle objects
public class ShadowContraction : MonoBehaviour
{
    // Set so that there are not multiple triggers of this at once, since only one is necessary
    private bool colliding = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7 && !colliding)
        {
            colliding = true;
            // If the player is in the middle of the object on either the x or z axis, shadow is flat on the floor
            if ((other.gameObject.transform.position.x < gameObject.transform.position.x + gameObject.transform.parent.localScale.x / 2 &&
                other.gameObject.transform.position.x > gameObject.transform.position.x - gameObject.transform.parent.localScale.x / 2) ||
                (other.gameObject.transform.position.z < gameObject.transform.position.z + gameObject.transform.parent.localScale.z / 2 &&
                other.gameObject.transform.position.z > gameObject.transform.position.z - gameObject.transform.parent.localScale.z / 2))
            {
                other.gameObject.transform.localScale = new Vector3(other.gameObject.transform.localScale.x, 0.01f, other.gameObject.transform.localScale.z);
                other.gameObject.GetComponent<CharacterController>().center = new Vector3(0, 79, 0);
                other.gameObject.GetComponent<CapsuleCollider>().height = 0.01f;
                other.gameObject.gameObject.GetComponent<CharacterController>().enabled = false;
                other.gameObject.transform.position = new Vector3(other.gameObject.transform.position.x, 0.01f, other.gameObject.transform.position.z);
                other.gameObject.GetComponent<CharacterController>().enabled = true;
                Physics.IgnoreLayerCollision(7, 8);
            }
            // If the player is on the very edge of the object on the x axis, shadow becomes flat along the y and z axis
            else if ((other.gameObject.transform.position.x >= gameObject.transform.position.x + gameObject.transform.parent.localScale.x / 2 &&
                other.gameObject.transform.position.x - other.gameObject.transform.localScale.x / 2 < gameObject.transform.position.x + gameObject.transform.parent.localScale.x / 2) ||
                (other.gameObject.transform.position.x <= gameObject.transform.position.x - gameObject.transform.parent.localScale.x / 2 &&
                other.gameObject.transform.position.x + other.gameObject.transform.localScale.x / 2 > gameObject.transform.position.x - gameObject.transform.parent.localScale.x / 2))
            {
                other.gameObject.GetComponent<Rigidbody>();
                Physics.IgnoreLayerCollision(7, 8);
                other.gameObject.gameObject.GetComponent<CharacterController>().enabled = false;
                other.gameObject.transform.position = new Vector3(other.gameObject.transform.position.x - (other.gameObject.transform.position.x > gameObject.transform.position.x ? 0.5f : -0.5f), 
                    other.gameObject.transform.position.y, other.gameObject.transform.position.z - (other.gameObject.transform.position.z > gameObject.transform.position.z ? 0.5f : -0.5f));
                other.gameObject.GetComponent<CharacterController>().enabled = true;
                other.gameObject.GetComponent<CharacterController>().radius = 0.01f;
                other.gameObject.GetComponent<CapsuleCollider>().radius = 0.01f;
                other.gameObject.transform.localScale = new Vector3(0.01f, other.gameObject.transform.localScale.y, other.gameObject.transform.localScale.z);
            }
            // If the player is on the very edge of the object on the z axis, shadow becomes flat along the y and z axis
            else if ((other.gameObject.transform.position.z >= gameObject.transform.position.z + gameObject.transform.parent.localScale.z / 2 &&
                other.gameObject.transform.position.z - other.gameObject.transform.localScale.z / 2 < gameObject.transform.position.z + gameObject.transform.parent.localScale.z / 2) ||
                (other.gameObject.transform.position.z <= gameObject.transform.position.z - gameObject.transform.parent.localScale.z / 2 &&
                other.gameObject.transform.position.z + other.gameObject.transform.localScale.z / 2 > gameObject.transform.position.z - gameObject.transform.parent.localScale.z / 2))
            {
                Physics.IgnoreLayerCollision(7, 8);
                other.gameObject.gameObject.GetComponent<CharacterController>().enabled = false;
                other.gameObject.transform.position = new Vector3(other.gameObject.transform.position.x - (other.gameObject.transform.position.x > gameObject.transform.position.x ? 0.5f : -0.5f),
                    other.gameObject.transform.position.y, other.gameObject.transform.position.z - (other.gameObject.transform.position.z > gameObject.transform.position.z ? 0.5f : -0.5f));
                other.gameObject.GetComponent<CharacterController>().enabled = true;
                other.gameObject.GetComponent<CharacterController>().radius = 0.01f;
                other.gameObject.GetComponent<CapsuleCollider>().radius = 0.01f;
                other.gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, 0.01f);
            }
        }
    }

    // On exit, restores the shadow to its original proportions
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            other.gameObject.transform.localScale = new Vector3(1.2f, 1.8f, 1.2f);
            other.gameObject.GetComponent<CharacterController>().center = new Vector3(0, 0, 0);
            other.gameObject.GetComponent<CharacterController>().radius = 0.6f;
            other.gameObject.GetComponent<CapsuleCollider>().height = 1.8f;
            other.gameObject.GetComponent<CapsuleCollider>().radius = 0.6f;
            Physics.IgnoreLayerCollision(7, 8, false);
            colliding = false;
        }
    }
}
