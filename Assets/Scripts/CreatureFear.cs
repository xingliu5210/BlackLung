using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatureFear : MonoBehaviour
{
    [SerializeField] private float fearBar;
    [SerializeField] private float maxFear;

    public void TakeDamage(int damage)
    {
        if (damage <= 0) return;
        if (fearBar <= 0) return;

        fearBar -= damage;

        //Restrict Fear to minimum 0
        if (fearBar < 0)
        { fearBar = 0; }

        Debug.Log(gameObject.name + " has " + fearBar + "/" + maxFear + " fearPoints remaining");

        if (fearBar <= 0) Escape();
    }

    private void Escape()
    {
        Debug.Log("Creature has been scared off!");
        fearBar = 100; //For testing purposes. Remove line for full build!
    }
}
