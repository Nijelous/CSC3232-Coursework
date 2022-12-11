using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The outer shell of the hierarchical collision detection system
public class MonsterCollision : MonoBehaviour
{
    [SerializeField]
    private BoxCollider[] allColliders;

    [SerializeField]
    private MonsterStats stats;

    private List<int> activatedColliders = new List<int>();

    private List<Collider> otherColliders = new List<Collider>();

    // When entered, gets the closest layer to the object and awakens its collider it if it hasn't already been
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetType() == typeof(SphereCollider))
        {
            for(int i = 0; i < allColliders.Length; i++)
            {
                Vector3 minBounds = allColliders[i].bounds.min;
                Vector3 maxBounds = allColliders[i].bounds.max;

                Vector3 clampedPos = new Vector3(Mathf.Clamp(other.transform.position.x, minBounds.x, maxBounds.x),
                    Mathf.Clamp(other.transform.position.y, minBounds.y, maxBounds.y),
                    Mathf.Clamp(other.transform.position.z, minBounds.z, maxBounds.z));
                if((clampedPos-other.transform.position).magnitude < (other as SphereCollider).radius)
                {
                    if (!activatedColliders.Contains(i))
                    {
                        activatedColliders.Add(i);
                        allColliders[i].enabled = true;
                    }
                }
            }
        }
        else if(other.GetType() == typeof(BoxCollider))
        {
            Vector3 minBoundsOther = other.bounds.min;
            Vector3 maxBoundsOther = other.bounds.max;
            for(int i = 0; i < allColliders.Length; i++)
            {
                Vector3 minBounds = allColliders[i].bounds.min;
                Vector3 maxBounds = allColliders[i].bounds.max;

                if ((minBoundsOther.x <= maxBounds.x && maxBoundsOther.x >= minBounds.x) &&
                    (minBoundsOther.y <= maxBounds.y && maxBoundsOther.y >= minBounds.y) &&
                    (minBoundsOther.z <= maxBounds.z && maxBoundsOther.z >= minBounds.z))
                {
                    if (!activatedColliders.Contains(i))
                    {
                        activatedColliders.Add(i);
                        allColliders[i].enabled = true;
                    }
                    break;
                }
            }
        }
    }

    // Upon an item leaving, checks to see whether it can deactivate the layers, meaning there are no objects inside of its collider
    private void OnTriggerExit(Collider other)
    {
        foreach (Collider c in otherColliders)
        {
            if (c.name == other.name)
            {
                otherColliders.Remove(c);
                break;
            }
        }
        if (otherColliders.Count == 0)
        {
            foreach(int i in activatedColliders)
            {
                allColliders[i].enabled = false;
            }
            activatedColliders.Clear();
        }
    }

    // If a layer is exited, then calculate whether it'll be inside of another layer
    public void ExitChild(Collider other)
    {
        if (otherColliders.Contains(other)){
            int closest = 0;
            float distance = Mathf.Infinity;
            for (int i = 0; i < allColliders.Length; i++)
            {
                float currentDist = Mathf.Sqrt(Mathf.Pow(allColliders[i].transform.position.x - other.transform.position.x, 2f) +
                    Mathf.Pow(allColliders[i].transform.position.y - other.transform.position.y, 2f) +
                    Mathf.Pow(allColliders[i].transform.position.z - other.transform.position.z, 2f));
                if (currentDist < distance)
                {
                    closest = i;
                    distance = currentDist;
                }
            }
            if (!activatedColliders.Contains(closest))
            {
                activatedColliders.Add(closest);
                allColliders[closest].enabled = true;
            }
        }
    }

    public void EmptyChild(Collider layer)
    {
        for(int i = 0; i < allColliders.Length; i++)
        {
            if(allColliders[i] == layer)
            {
                activatedColliders.Remove(i);
                break;
            }
        }
    }

    public void Damage(int damage)
    {
        stats.Damage(damage);
    }
}
