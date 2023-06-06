using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int currentHp;
    public int maxHp;
    public TextMeshProUGUI hpText;

    public GameObject start;

    public Enemy enemy;

    public AudioClip deathSound;
    public AudioSource audioSourceDeath;

    private void Start()
    {
        currentHp = maxHp;
        //hpText = GetComponent<TextMeshProUGUI>();
        audioSourceDeath = GetComponent<AudioSource>();
    }

    private void Update()
    {
        hpText.SetText(currentHp.ToString());
    }

    public void TakeDamage()
    {
        if (currentHp > 0) currentHp -= enemy.damage;
        print(currentHp);
        if (currentHp <= 0) Death();
    }

    public void Death()
    {
        transform.position = start.transform.position;
        currentHp = maxHp;
        audioSourceDeath.PlayOneShot(deathSound);

    }
}
