using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class playerShooting : MonoBehaviour
{
    public GameObject hitPrefab;
    public float maxDistance = 100;

    private AudioSource source;
    public AudioClip shootSound;

    public ParticleSystem muzzleFlash;

    public UnityEvent onShoot;

    public int maxAmmo = 12;
    public int ammo;

    public float recoilAngle = 1;
    public int shotsPerAmmo = 5;

    //public int damage = 10;

    private void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }
    }

    //void TryShoot()
    //{
    //    if (ammo <= 0) return;
    //    ammo--;
    //    onShoot.Invoke();

    //    for (int i = 0; i < shotsPerAmmo; i++)
    //    {
    //        Shoot();
    //    }
    //}


    void Shoot()
    {

        var cam = Camera.main;
        var dir = cam.transform.forward;

        var offsetX = Random.Range(-recoilAngle, recoilAngle);
        var offsetY = Random.Range(-recoilAngle, recoilAngle);
        dir = Quaternion.Euler(offsetX, offsetY, 0) * dir;

        var ray = new Ray(cam.transform.position, dir);

        muzzleFlash.Play();
        print("shot");
        source.PlayOneShot(shootSound);


        if (Physics.Raycast(ray, out var hit, maxDistance))
        {

            if (!hit.transform.CompareTag("Enemy"))
            {
                var hitObj = Instantiate(hitPrefab, hit.point, Quaternion.Euler(0, 0, 0), hit.transform);
                Destroy(hitObj);
                print("hit");
                hitObj.transform.forward = hit.normal;
                hitObj.transform.position += hit.normal * 0.02f;
            }
        }
    }
}
