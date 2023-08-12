using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Move")]
    Vector3 inputMovement;
    float moveSpeed = 8;
    public float moveinput;//for animation

    [Header("REFERENCE")]
    [Tooltip("Reference to the PlayerInput Action Mapping")]
    private PlayerInput playerInput;
    private CharacterController controller;


    private void Awake()
    {
        playerInput = new PlayerInput();
        controller = transform.parent.GetComponent<CharacterController>();
    }

    /// <summary>
    /// enable Player input
    /// </summary>
    private void OnEnable()
    {
        playerInput.Enable();
    }

    /// <summary>
    /// disable Player input
    /// </summary>
    private void OnDisable()
    {
        playerInput.Disable();
    }

    void Update()
    {
        Movement();
        FaceMouse();
    }

    private void Movement()
    {
        inputMovement = new Vector3(playerInput.Player.Movement.ReadValue<Vector2>().x, 0, playerInput.Player.Movement.ReadValue<Vector2>().y);
        moveinput = inputMovement.magnitude;

        // Verwende den CharacterController für die Bewegung
        controller.Move(inputMovement * Time.deltaTime * moveSpeed);

        // Setze die Y-Bewegung auf 0, um die vertikale Bewegung zu blockieren
        Vector3 newPosition = controller.transform.position;
        newPosition.y = 0; // Setze die Y-Komponente auf 0
        controller.transform.position = newPosition;
        //inputMovement = new Vector3(playerInput.Player.Movement.ReadValue<Vector2>().x, 0, playerInput.Player.Movement.ReadValue<Vector2>().y);
        //transform.Translate(inputMovement * Time.deltaTime * moveSpeed, Space.World);
    }

    private void FaceMouse()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.red);
            Debug.DrawRay(transform.position, gameObject.transform.forward * 100, Color.blue);

            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }
    }
}