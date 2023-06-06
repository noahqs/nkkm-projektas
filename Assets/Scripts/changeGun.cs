using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGun : MonoBehaviour
{
    public GameObject[] guns;
    public FirstPersonMovement player;
    public GameObject[] gunsOnGround;
    public bool[] equipAllow;
    public PlayerSliding playerSliding;
    public bool allowSlide;

    private void Start()
    {
        DisableAllGuns();
        equipAllow[0] = false;
        equipAllow[1] = false;
        allowSlide = true;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.V))
        {
            DisableAllGuns();
            DefaultSpeed();
            allowSlide = true;
        }

        if (Input.GetKey(KeyCode.Alpha1) && equipAllow[0])
        {
            DisableAllGuns();
            DefaultSpeed();
            guns[0].gameObject.SetActive(true);
            allowSlide = false;
        }
        if (Input.GetKey(KeyCode.Alpha2) && equipAllow[1])
        {
            DisableAllGuns();
            guns[1].gameObject.SetActive(true);
            player.speed = 5;
            player.runSpeed = 10;
            allowSlide = false;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pistol"))
        {
            Destroy(gunsOnGround[0]);
            equipAllow[0] = true;
        }
        if (collision.gameObject.CompareTag("Uzi"))
        {
            Destroy(gunsOnGround[1]);
            equipAllow[1] = true;
        }
        if (collision.gameObject.CompareTag("Plane"))
        {
            equipAllow[1] = true;
            equipAllow[0] = true;    
        }
    }
}
