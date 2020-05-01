using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector;

    //todo remove from insperctor
    [SerializeField] [Range(0,1)]float movementFactor; //0 not move 1 to full move
    //Start: 17.19 End: -4.3 Range:12.89

    Vector3 startingPos;
    
    
    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
    }
}
