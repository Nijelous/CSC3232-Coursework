using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Simple bullet of light for the player
public class LightBullet : MonoBehaviour
{
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        float speed = 20f;
        rb.velocity = transform.forward * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
