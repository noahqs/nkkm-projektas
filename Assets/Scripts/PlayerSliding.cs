using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSliding : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playerObj;
    private Rigidbody rb;
    private FirstPersonMovement pm;
    public FirstPersonAudio pmAudio;
    public ChangeGun slideAllow;

    [Header("Sliding")]
    public float maxSlideTime;
    public float slideForce;
    private float slideTimer;

    public float slideYScale;
    private float startYScale;

    public float slopeSlideTimer;

    [Header("Input")]
    public KeyCode slideKey = KeyCode.LeftControl;
    private float horizontalInput;
    private float verticalInput;

    public bool sliding;
    public bool allowSlide;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<FirstPersonMovement>();

        startYScale = playerObj.localScale.y;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(slideKey) && (horizontalInput != 0 || verticalInput != 0) && slideAllow.allowSlide) StartSlide();

        if (Input.GetKeyUp(slideKey) && sliding) StopSlide();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Slope"))
        {
            maxSlideTime *= slopeSlideTimer;
        }
    }

    private void FixedUpdate()
    {
        if (sliding)
            SlidingMovement();
    }

    private void StartSlide()
    {
        sliding = true;
        playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        slideTimer = maxSlideTime;
    }

    private void SlidingMovement()
    {
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);

        slideTimer -= Time.deltaTime;

        pmAudio.stepAudio.volume = 0;

        if (slideTimer <= 0) StopSlide();
    }

    private void StopSlide()
    {
        pmAudio.stepAudio.volume = 100;
        sliding = false;
        playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);
    }
}
