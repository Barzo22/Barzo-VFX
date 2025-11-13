using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TP2 - Ariadna Delpiano

public class Control
{
    private Movement _movement;
    private Camera _camera;

    public Control(Movement movement, Camera camera)
    {
        _movement = movement;
        _camera = camera;
    }


    public void ArtificialUpdate()
    {
        var v = Input.GetAxis("Vertical");
        var h = Input.GetAxis("Horizontal");

        // Obtener la direccion hacia adelante de la camara
        var cameraForward = _camera.transform.forward;
        cameraForward.y = 0f;
        cameraForward = cameraForward.normalized;

        // Obtener la direccion hacia los lados de la camara
        var cameraRight = _camera.transform.right;
        cameraRight.y = 0f;
        cameraRight = cameraRight.normalized;

        // Calcular la direccion del movimiento combinando la direccion de la camara y las entradas del jugador
        var moveDirection = cameraForward * v + cameraRight * h;

        _movement.Move(moveDirection.z, moveDirection.x);

        if (Input.GetKeyDown(KeyCode.Space) && !_movement.IsJumping)
        {
            _movement.Jump();
        }
    }

}
