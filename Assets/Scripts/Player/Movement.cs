using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TP2 - Ariadna Delpiano

public class Movement
{
    private float speed;
    private float jumpForce;
    private Rigidbody _playerBody;
    private bool _isJumping = false;

    public bool IsJumping
    {
        get { return _isJumping; }
    }

    public Movement(Rigidbody playerBody, float speed)
    {
        _playerBody = playerBody;
        this.speed = speed;
        jumpForce = 5f;
    }

    public void Move(float vertical, float horizontal)
    {
        Vector3 movement = new Vector3(horizontal, 0f, vertical) * speed * Time.deltaTime;
        _playerBody.MovePosition(_playerBody.position + movement);
    }

    public void Jump()
    {
        _isJumping = true;
        _playerBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public void ResetJump()
    {
        _isJumping = false;
    }

}
