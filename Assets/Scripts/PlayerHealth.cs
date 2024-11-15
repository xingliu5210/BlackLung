using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float health;
    public float maxHealth;
    [SerializeField] private Image healthBar;

    public void Healthbar()
    {
        //Restrict Health to maximum/minimum
        if (health > maxHealth)
        { health = maxHealth; }

        if (health < 0)
        { health = 0; }

        //Translate health values into health bar
        healthBar.fillAmount = Mathf.Clamp(health / maxHealth, 0, 1);

        Debug.Log("Player has " + health + "/" + maxHealth + "hp");

        if (health <= 0)
        { HealthDepleted(); }
    }

    private void HealthDepleted()
    {
        Debug.Log("HEALTH DEPLETED!");
        GetComponent<Checkpoint>().Respawn();
    }
}
