using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource[] audioSource;//Removed Serializefield tag and array still workes
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 10f;
    [SerializeField] float sceneLoadTime = 1f;
    [SerializeField] AudioClip[] audioClips;

    enum State {Alive, Dying, Transending };
    State state = State.Alive;


    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponents<AudioSource>();
       
        
    }

    // Update is called once per frame
    void Update()
    {

        if (state == State.Alive)
        {
            RespondToThrust();
            RespondToRotate();
        }
    }

    private void RespondToThrust()
    {
        float thrustThisFrame = mainThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust(thrustThisFrame);
        }
        else
        {
            audioSource[0].Stop();
        }
    }

    private void ApplyThrust(float thrustThisFrame)
    {
        if (!audioSource[0].isPlaying)
        {
            audioSource[0].PlayOneShot(audioClips[0]);
        }

        rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);
        Debug.Log("Thrusting");
    }

    private void RespondToRotate()
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
        if (state != State.Alive) { return; }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("OK");
                break;
            case "Finish":
                StartSucessSequence();
                break;

            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        audioSource[0].Stop();
        audioSource[0].PlayOneShot(audioClips[2]);
        Invoke("LoadFirstLevel", sceneLoadTime);
    }

    private void StartSucessSequence()
    {
        state = State.Transending;
        audioSource[0].Stop();
        audioSource[0].PlayOneShot(audioClips[1]);
        Invoke("LoadNextLevel", sceneLoadTime);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1);//TODO Alllow more than 2 levels

    }
}
