using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    [SerializeField] AudioSource[] audioSource;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 10f;
    



    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponents<AudioSource>();
       
        
    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();  
    }

    private void Thrust()
    {
        float thrustThisFrame = mainThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            if (!audioSource[0].isPlaying)
            {
                audioSource[0].Play();
            }

            rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);
            Debug.Log("Thrusting");
        }
        else
        {
            audioSource[0].Stop();
        }
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true;       
        float rotationThisFrame = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            if (!audioSource[1].isPlaying)
            {
                audioSource[1].Play();
            }
            transform.Rotate(Vector3.forward * rotationThisFrame);
            Debug.Log("Rotating Left");
        }

        else if (Input.GetKey(KeyCode.D))
        {
            if (!audioSource[1].isPlaying)
            {
                audioSource[1].Play();
            }
            transform.Rotate(-Vector3.forward * rotationThisFrame);
            Debug.Log("Rotating Right");
        }

        rigidBody.freezeRotation = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("OK");
                break;

            default:
                Debug.Log("Dead");
                break;
        }
    }


}
