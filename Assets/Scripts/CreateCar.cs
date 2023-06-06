using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCar : MonoBehaviour
{
    public GameObject car;

    private void Start()
    {
        InvokeRepeating("CreatingCar", 2f, 2f);
    }

    private void CreatingCar()
    {
        Instantiate(car, transform.position, transform.rotation * Quaternion.Euler(0, 0, 0));
    }
}
