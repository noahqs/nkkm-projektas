using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleportation : MonoBehaviour
{
    public Animator fadeAnimator;
    public float fadeDuration = 1.0f;
    public string targetLevelName;

    private bool isTeleporting = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTeleporting)
        {
            StartCoroutine(Teleport());
        }
    }

    IEnumerator Teleport()
    {
        isTeleporting = true;
        fadeAnimator.SetTrigger("FadeOut");

        yield return new WaitForSeconds(fadeDuration);

        SceneManager.LoadScene(targetLevelName);

        yield return new WaitForEndOfFrame();

        fadeAnimator.SetTrigger("FadeIn");

        yield return new WaitForSeconds(fadeDuration);

        isTeleporting = false;
    }
}
