using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    //TODO RESTE LEVEL Loader
    Rigidbody rigidBody;
    [Header("Main Variables")]
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 10f;
    [SerializeField] float sceneLoadTime = 1f;

    [Header("Particle System")]
    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem thrustersParticlesPort;
    [SerializeField] ParticleSystem thrustersParticlesStarboard;
    [SerializeField] ParticleSystem sucessParticles;
    [SerializeField] ParticleSystem deathParticles;

    [Header("Audio")]
    AudioSource[] audioSource;//Removed Serializefield tag and array still workes
    [SerializeField] AudioClip[] audioClips;

    enum State {Alive, Dying, Transending };
    State state = State.Alive;


    //Testing///
    bool doCollision = true;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponents<AudioSource>();

        const float tau = Mathf.PI * 2f;
        print("tsu: " + Mathf.Sin(tau/4));
       
        
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            RespondToThrust();
            RespondToRotate();
        }

        if (Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }
    }

    private void RespondToDebugKeys()
    {
        //Setup Debug Keys
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            LoadPrevLevel();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            doCollision = !doCollision;
            if (doCollision) { Debug.Log("Collisions ON"); }
            else{ Debug.Log("Collisions OFF"); }
         
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
            mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust(float thrustThisFrame)
    {
        if (!audioSource[0].isPlaying)
        {
            audioSource[0].PlayOneShot(audioClips[0]);
        }
        mainEngineParticles.Play();
        rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);
        //Debug.Log("Thrusting");
    }

    private void RespondToRotate()
    {
        rigidBody.freezeRotation = true;       
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))///When Pressing A//////
        {                    
            if (!audioSource[1].isPlaying)
            {                
                audioSource[1].Play();
                thrustersParticlesPort.Play();
                //thrustersParticles.transform.rotation.y = 90; 
            }
            transform.Rotate(Vector3.forward * rotationThisFrame);
            //Debug.Log("Rotating Left");
        }

        else if (Input.GetKey(KeyCode.D))///When Pressing D//////
        {
            if (!audioSource[1].isPlaying)
            {
                audioSource[1].Play();
                thrustersParticlesStarboard.Play(); 
            }
            transform.Rotate(-Vector3.forward * rotationThisFrame);
            //Debug.Log("Rotating Right");
        }

        rigidBody.freezeRotation = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive || !doCollision) { return; }

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
        deathParticles.Play();
        Invoke("LoadFirstLevel", sceneLoadTime);
    }

    private void StartSucessSequence()
    {
        state = State.Transending;
        audioSource[0].Stop();
        audioSource[0].PlayOneShot(audioClips[1]);
        sucessParticles.Play();
        Invoke("LoadNextLevel", sceneLoadTime);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex == 5) { Debug.Log("No more levels");  return; }
        SceneManager.LoadScene(currentSceneIndex + 1);//TODO Alllow more than 2 levels
    }

    private void LoadPrevLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex == 0) { Debug.Log("No more levels");  return; }
        SceneManager.LoadScene(currentSceneIndex - 1);//TODO Alllow more than 2 levels
    }




}
