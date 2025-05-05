using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonFireManager : MonoBehaviour
{
    public HealthManager health;
    public PlayerPullController player;
    // Start is called before the first frame update

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            health.isHealing = true;
            health.Heal(100f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        health.isHealing = false;
    }

}
