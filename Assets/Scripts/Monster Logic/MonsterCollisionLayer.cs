using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The layers in which the pixels reside in the hierarchical collision detection system
public class MonsterCollisionLayer : MonoBehaviour
{
    [SerializeField]
    MonsterCollision parent;

    [SerializeField]
    List<BoxCollider> allColliders;

    private List<int> activatedColliders = new List<int>();

    private List<Collider> otherColliders = new List<Collider>();

    private void Start()
    {
        allColliders = new List<BoxCollider>();
        for(int i = 0; i < transform.childCount; i++)
        {
            allColliders.Add(transform.GetChild(i).GetComponent<BoxCollider>());
            transform.GetChild(i).GetComponent<MonsterCollisionPixel>().number = i;
        }
    }

    // When activated, activates the 8 closest pixels to where the collision object is
    private void OnTriggerEnter(Collider other)
    {
        otherColliders.Add(other);
        List<int> closest = new List<int>();
        List<float> distance = new List<float>();
        for (int i = 0; i < allColliders.Count; i++)
        {
            float currentDist = Mathf.Sqrt(Mathf.Pow(allColliders[i].transform.position.x - other.transform.position.x, 2f) +
                Mathf.Pow(allColliders[i].transform.position.y - other.transform.position.y, 2f) +
                Mathf.Pow(allColliders[i].transform.position.z - other.transform.position.z, 2f));
            if (closest.Count < 8)
            {
                closest.Add(i);
                distance.Add(i);
            }
            else
            {
                float largest = Mathf.Max(distance.ToArray());
                if (currentDist < largest)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (distance[j] == largest)
                        {
                            closest[j] = i;
                            distance[j] = currentDist;
                        }
                    }
                }
            }
        }
        foreach (int i in closest)
        {
            if (!activatedColliders.Contains(i))
            {
                //Debug.Log("Activated: " + i);
                activatedColliders.Add(i);
                allColliders[i].enabled = true;
            }
        }
    }

    // On leaving, checks to see if all objects have left, and if they have then the colliders are set to inactive again
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Should Remove");
        for(int i = 0; i < otherColliders.Count; ++i)
        {
            try
            {
                Debug.Log(otherColliders[i].name);
                if (otherColliders[i].name == other.name)
                {
                    Debug.Log("Gottem");
                    otherColliders.Remove(otherColliders[i]);
                    break;
                }
            }
            catch(MissingReferenceException ex)
            {
                Debug.Log("Missing Reference Exception: " + ex);
                otherColliders.Remove(otherColliders[i]);
                i--;
            }
        }
        //otherColliders.Remove(other);
        parent.ExitChild(other);
        if (otherColliders.Count == 0)
        {
            Debug.Log("Removing");
            foreach (int i in activatedColliders)
            {
                allColliders[i].enabled = false;
            }
            activatedColliders.Clear();
            parent.EmptyChild(GetComponent<BoxCollider>());
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    public void Damage(int damage)
    {
        GetComponentInParent<MonsterCollision>().Damage(damage);
    }

    // Updates the held colliders to shrink the list upon one being destroyed
    public void RemoveChild(BoxCollider c, int i)
    {
        allColliders.Remove(c);
        activatedColliders.Remove(i);
        for(int j = 0; j < activatedColliders.Count; j++)
        {
            if (activatedColliders[j] > i) activatedColliders[j] -= 1;
        }
    }
}
