using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputHandler : MonoBehaviour
{

    Vector2 moveInputVector = Vector2.zero;
    Vector2 viewInputVector = Vector2.zero;

    //Other Components
    CharacterMovementHandler characterMovementHandler;

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    void Update()
    {
        //View Input
        //viewInputVector.x = Input.GetAxis("Mouse X");
        //viewInputVector.y = Input.GetAxis("Mouse Y") * -1; //Invert the mouse look

        //characterMovementHandler.SetViewInputVector(viewInputVector);

        //Move Input
        moveInputVector.x = Input.GetAxis("Horizontal");
        moveInputVector.y = Input.GetAxis("Vertical");
    }

    public NetworkInputData GetNetworkInput()
    {
        NetworkInputData networkInputData = new NetworkInputData();

        //View Data
        //networkInputData.rotationInput = viewInputVector.x;

        //Move Data
        networkInputData.movementInput = moveInputVector;

        return networkInputData;
    }
}
