using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public int currentHp;
    public int maxHp;

    public PlayerShooting playerShooting;

    public UnityEvent onDamage;
    public UnityEvent onDeath;

    private void Start()
    {
        currentHp = maxHp;
    }


    public void Damage()
    {
        onDamage.Invoke();
        if (currentHp <= 0) Death();
    }

    public void Death()
    {
        //onDeath.Invoke();
        //if (destroyOnDeath) Destroy(gameObject);
    }
}
