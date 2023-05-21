using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotorRotation : MonoBehaviour
{
    public int rotationSpeed = 5;
    public AudioSource rotorSound;

    void Update()
    {
        transform.Rotate(new Vector3(0, rotationSpeed, 0));
        //rotorSound.Play();
    }
}
