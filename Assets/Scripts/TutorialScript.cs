using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    public GameObject[] tutorialText;

    private void Start()
    {
        DisableAllText();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("bld1"))
        {
            DisableAllText();
            tutorialText[0].SetActive(true);
            tutorialText[1].SetActive(true);
        }

        if (collision.collider.CompareTag("bld2"))
        {
            DisableAllText();
            tutorialText[2].SetActive(true);
        }

        if (collision.collider.CompareTag("bld3"))
        {
            DisableAllText();
            tutorialText[3].SetActive(true);
        }

        if (collision.collider.CompareTag("bld4"))
        {
            DisableAllText();
            tutorialText[4].SetActive(true);
        }

    }

    public void DisableAllText()
    {
        tutorialText[0].SetActive(false);
        tutorialText[1].SetActive(false);
        tutorialText[2].SetActive(false);
        tutorialText[3].SetActive(false);
        tutorialText[4].SetActive(false);
    }
}
