using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class playerShooting : MonoBehaviour
{
    //Gun stats
    public int damage;
    public float timeBetweenShooting, spread, range, reload, timeBetweenShots;
    public int magazineSize, bulletsPerClick;
    public bool allowHold;
    private int bulletsLeft, bulletsShot;

    bool shooting, readyToShoot, reloading;

    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;

    public GameObject bulletHole;
    public GameObject woundHole;
    public ParticleSystem muzzleFlash;
    public ParticleSystem cartridgeEffect;
    public TextMeshProUGUI text;

    public AudioSource audioSourceShoot;
    public AudioSource audioSourceReload;
    public AudioSource audioSourceEmpty;
    public AudioClip shootSound;
    public AudioClip reloadSound;
    public AudioClip emptySound;

    private void Start()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;

        audioSourceShoot = GetComponent<AudioSource>();
        audioSourceReload = GetComponent<AudioSource>();
        audioSourceEmpty = GetComponent<AudioSource>();
    }

    private void Update()
    {
        MyInput();

        text.SetText(bulletsLeft + " / " + magazineSize);
    }

    private void MyInput()
    {
        if (allowHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();
        if (Input.GetKeyDown(KeyCode.Mouse0) && bulletsLeft <= 0 && !reloading) audioSourceEmpty.PlayOneShot(emptySound, 0.7f);

        //shoot

        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerClick;
            Shoot();
        }
    }

    public void Shoot()
    {
        readyToShoot = false;

        //Spread
        float x = UnityEngine.Random.Range(-spread, spread);
        float y = UnityEngine.Random.Range(-spread, spread);

        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

        //Raycast
        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, whatIsEnemy))
        {
            //Debug.Log(rayHit.collider.name);

            if (rayHit.collider.CompareTag("Enemy"))
            {
                rayHit.collider.GetComponent<Enemy>().TakeDamage(damage);
                var wound = Instantiate(woundHole, rayHit.point, transform.rotation * Quaternion.Euler(0f, 180f, 0f));
                Destroy(wound, 2);
            }
            else
            {
                var bullet = Instantiate(bulletHole, rayHit.point, transform.rotation * Quaternion.Euler(0f, 180f, 0f));
                Destroy(bullet, 5);
            }
        }

        muzzleFlash.Play();
        cartridgeEffect.Play();

        audioSourceShoot.PlayOneShot(shootSound, 0.7f);

        bulletsLeft--;
        bulletsShot--;

        Invoke("ResetShot", timeBetweenShooting);

        if (bulletsShot > 0 && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);

    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    private void Reload()
    {
        reloading = true;
        audioSourceReload.PlayOneShot(reloadSound, 0.7f);
        Invoke("ReloadFinished", reload);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
