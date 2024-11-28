using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private DamageType Type;

    [SerializeField, Tooltip("Higher time = Slower rate")] private int onStayDamageTime;
    
    private float timer = 0f;

    public enum DamageType
    {
        OnEnter,
        OnStay
    }

    private void OnTriggerEnter(Collider other)
    {
        //Checks if collides with player. Then, reduces health by damage set ONCE.
        if ( other.gameObject.CompareTag("Player"))
        {
            if (Type == DamageType.OnEnter)
            {
                other.gameObject.GetComponent<PlayerHealth>().Damage(damage);

                if (damage < 0)
                { Debug.Log("Player has recovered " + -damage + " health"); }
                else
                { Debug.Log("Player has taken " + damage + " damage"); }

                other.gameObject.GetComponent<PlayerHealth>().Healthbar();
            }

            if (Type == DamageType.OnStay)
            {
                { Debug.Log("Player is in poison!"); }
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        //Checks if collides with player. Then, reduces health by damage set PER FIXED UPDATE.
        if (other.gameObject.CompareTag("Player") && Type == DamageType.OnStay)
        {
            timer++;

            if( timer >= Mathf.Abs(onStayDamageTime))
            {
                other.gameObject.GetComponent<PlayerHealth>().Damage(damage);

                timer = 0;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && Type == DamageType.OnStay)
        {
            { Debug.Log("Player exit poison range"); }
        }
    }
}
