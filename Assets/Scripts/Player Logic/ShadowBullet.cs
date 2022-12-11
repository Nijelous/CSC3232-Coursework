using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The bullet shot by all shadow entities
public class ShadowBullet : MonoBehaviour
{
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Starts it off moving in a forward direction
    private void Start()
    {
        float speed = 20f;
        rb.velocity = transform.forward * speed;
    }

    // When the bullet collides, checks to see whether what it hit was the player, as the monsters have their own collision detection and rules
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 7 && !GameObject.Find("Player").GetComponent<PlayerStats>().hitImmune)
        {
            collision.gameObject.GetComponent<ShadowController>().RemoveShadow();
            GameObject.Find("Player").GetComponent<PlayerStats>().Hit();
        }
        else if(collision.gameObject.layer == 6) {
            GameObject.Find("Player").GetComponent<PlayerStats>().LightHit();
        }
        Destroy(gameObject);
    }
}
