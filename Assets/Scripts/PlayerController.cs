using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private CharacterController controller;
    private Vector3 direction;
    public float forwardSpeed;


    private int desiredLane = 1;
    public float laneDistance = 4; //odlegloœæ pomiedzy 2 liniami 

    public float jumpForce;

    public float Gravity = -20;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
        direction.z = forwardSpeed;



        direction.y += Gravity*Time.deltaTime;

        if(controller.isGrounded){
            if(Input.GetKeyDown(KeyCode.UpArrow)){
                    Jump();
            }

        }
        else {
            direction.y += Gravity *Time.deltaTime;
        }


        
        if(Input.GetKeyDown(KeyCode.RightArrow)){
            desiredLane++;
            if(desiredLane==3)
                desiredLane = 2;
        }

         if(Input.GetKeyDown(KeyCode.LeftArrow)){
            desiredLane--;
            if(desiredLane==-1) 
                desiredLane = 0;
        }

        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
        if(desiredLane == 0){
            targetPosition += Vector3.left * laneDistance;
        }
        else if (desiredLane == 2){
            targetPosition += Vector3.right * laneDistance;
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition,60*Time.fixedDeltaTime);
    }

    private void FixedUpdate(){
        controller.Move(direction * Time.fixedDeltaTime);
    }

    private void Jump(){
        direction.y = jumpForce;
    }
}
