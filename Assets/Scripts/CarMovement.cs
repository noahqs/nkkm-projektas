using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public int speed = 1;
    public Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(speed, 0f, 0f);
        rb.AddForce(movement);
    }
}
