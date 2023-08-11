using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Move")]
    Vector3 inputMovement;
    float moveSpeed = 10;

    [Header("REFERENCE")]
    [Tooltip("Reference to the PlayerInput Action Mapping")]
    private PlayerInput playerInput;


    private void Awake()
    {
        playerInput = new PlayerInput();
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
    }

    private void Movement()
    {
        inputMovement = new Vector3(playerInput.Player.Movement.ReadValue<Vector2>().x, 0, playerInput.Player.Movement.ReadValue<Vector2>().y);
        transform.Translate(inputMovement * Time.deltaTime * moveSpeed, Space.World);
    }
}
