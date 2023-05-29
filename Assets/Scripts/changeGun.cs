using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeGun : MonoBehaviour
{
    public GameObject[] guns;
    public FirstPersonMovement player;

    private void Start()
    {
        DisableAllGuns();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.V))
        {
            DisableAllGuns();
            DefaultSpeed();
        }
        
        if (Input.GetKey(KeyCode.Alpha1))
        {
            DisableAllGuns();
            DefaultSpeed();
            guns[0].gameObject.SetActive(true);
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            DisableAllGuns();
            guns[1].gameObject.SetActive(true);
            player.speed = 5;
            player.runSpeed = 10;
        }
    }


    public void DisableAllGuns()
    {
        guns[0].gameObject.SetActive(false);
        guns[1].gameObject.SetActive(false);
    }

    public void DefaultSpeed()
    {
        player.speed = 6;
        player.runSpeed = 12;
    }
}
