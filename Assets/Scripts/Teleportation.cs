using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleportation : MonoBehaviour
{
    public bool objectiveDone;

    private void Start()
    {
        objectiveDone = false;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Objective"))
        {
            Destroy(other.gameObject);
            objectiveDone = true;
        }
        if (other.gameObject.CompareTag("EndObj"))
        {
            SceneManager.LoadScene("End");  
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Teleporter"))
        {
            SceneManager.LoadScene("Level1");
        }
        if (collision.gameObject.CompareTag("Teleporter2") && objectiveDone)
        {
            SceneManager.LoadScene("Level2");
        }

    }
}
