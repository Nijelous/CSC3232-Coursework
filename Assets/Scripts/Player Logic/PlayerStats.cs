using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The stats of the player, and the logic around that
public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private GameObject[] healthBar;

    private int hp;

    private int lightHit;

    private int maxLightHit;

    public bool hitImmune;

    private void Start()
    {
        // Sets the health of the player depending on the difficulty they chose
        switch (GameObject.Find("State Manager").GetComponent<StateManager>().difficulty)
        {
            case StateManager.Difficulty.Easy:
                hp = 5;
                maxLightHit = 10;
                break;
            case StateManager.Difficulty.Medium:
                hp = 4;
                maxLightHit = 7;
                healthBar[4].SetActive(false);
                break;
            case StateManager.Difficulty.Hard:
                hp = 3;
                maxLightHit = 5;
                healthBar[4].SetActive(false);
                healthBar[3].SetActive(false);
                break;
        }
    }

    // Upon being hit, the player gets a few seconds of hit immunity, as sometimes the player was taking 2 hearts of damage in one hit
    public void Hit()
    {
        hitImmune = true;
        hp--;
        healthBar[hp].SetActive(false);
        if(hp == 0)
        {
            GameObject.Find("State Manager").GetComponent<StateManager>().LoseStart();
        }
        StartCoroutine(HitImmune());
    }

    public void LightHit()
    {
        lightHit++;
        if(lightHit >= maxLightHit)
        {
            lightHit = 0;
            Hit();
        }
    }

    IEnumerator HitImmune()
    {
        yield return new WaitForSeconds(3);
        hitImmune = false;
    }
}
