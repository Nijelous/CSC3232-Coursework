using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The individual "pixel" objects on the enemy, handles the damage taken and is destroyed when hit
public class MonsterCollisionPixel : MonoBehaviour
{
    public int number;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 11) // If hit by a shadow bolt, damage is calculated depending on whether they hit the eye, or based on difficulty
        {
            int damage = 0;
            if (gameObject.GetComponent<MeshRenderer>().material.ToString() == "Shadow Eye (Instance) (UnityEngine.Material)")
            {
                damage = 50;
            }
            else
            {
                switch (GameObject.Find("State Manager").GetComponent<StateManager>().difficulty)
                {
                    case StateManager.Difficulty.Easy:
                        damage = 8;
                        break;
                    case StateManager.Difficulty.Medium:
                        damage = 5;
                        break;
                    case StateManager.Difficulty.Hard:
                        damage = 2;
                        break;
                }
            }
            GetComponentInParent<MonsterCollisionLayer>().Damage(damage);
            GetComponentInParent<MonsterCollisionLayer>().RemoveChild(GetComponent<BoxCollider>(), number);
            Destroy(gameObject);
        }
        else if(collision.gameObject.layer == 10) // If hit by a light bolt, die
        {
            int damage = 100;
            GetComponentInParent<MonsterCollisionLayer>().Damage(damage);
            Destroy(gameObject);
        }
    }
}
