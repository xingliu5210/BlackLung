using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public GameObject ally;

    public float health;
    public float maxHealth;
    [SerializeField] private Image healthBar;

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Take life");
            Damage(25);
        }
    }
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

    public void Damage(float dmg)
    {
        if (health > 0)
        {
            health -= dmg;
            Healthbar();
        }
    }

    public void FullHeal()
    {
        if (health != maxHealth)
        {
            health = maxHealth;
            Healthbar();
            Debug.Log("FEAR BAR REPLENISHED!");
        }
    }

    private void HealthDepleted()
    {
        Debug.Log("FEAR BAR DEPLETED!");
        GetComponent<Checkpoint>().Respawn();
        ally.GetComponent<Checkpoint>().Respawn();
    }
}
