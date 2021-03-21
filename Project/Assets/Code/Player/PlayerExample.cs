using System;
using UnityEngine;

public class PlayerExample : Singleton<PlayerExample>, IEventSource
{
    public float rotationSpeed = 100;
    public float movementSpeed = 5;
    public float sprintSpeed = 10;
    public float gravity = 100;
    CharacterController characterController;
    Animator animatorController;



    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animatorController = GetComponent<Animator>();

    }

    private void Update()
    {
        HandleMovement();
        HandleAttack();
    }

    private void HandleAttack()
    {
        if (Input.GetButton("Attack"))
        {
            animatorController.SetTrigger("Attack");

        }
        else animatorController.ResetTrigger("Attack");
    }

    void HandleMovement()
    {
        //Cheap movement
        float forwardMovement = Input.GetAxis("VerticalLS");
        float speed = Input.GetButton("Sprint") ? sprintSpeed : movementSpeed;


        if (forwardMovement == 0)
        {
            animatorController.SetInteger("State", 0);
        }
        else if (speed == sprintSpeed)
        {
            animatorController.SetInteger("State", 2);
        }
        else animatorController.SetInteger("State", 1);

      
        Vector3 moveDirection = (-transform.forward * forwardMovement) + (-transform.up * gravity * Time.deltaTime);
        characterController.Move(moveDirection * speed * Time.deltaTime);

        float desiredRotation = Input.GetAxis("HorizontalLS");
        transform.Rotate(Vector3.up, desiredRotation * rotationSpeed * Time.deltaTime);
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
