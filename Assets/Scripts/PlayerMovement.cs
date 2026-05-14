using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement
{
    private readonly Rigidbody2D _rb;
    private readonly float _speed;
    private readonly float _jumpForce;
    private readonly float _knockbackForce;

    private readonly Transform _groundCheck;
    private readonly float _groundCheckRadius;
    private readonly LayerMask _groundLayer;

    // Кэшированный массив размером 1 для Zero Allocation Physics Check
    private readonly Collider2D[] _groundHitCache = new Collider2D[1];

    private float _inputX;

    public PlayerMovement(Rigidbody2D rb, float speed, float jumpForce, float knockbackForce,
                          Transform groundCheck, float groundCheckRadius, LayerMask groundLayer)
    {
        _rb = rb;
        _speed = speed;
        _jumpForce = jumpForce;
        _knockbackForce = knockbackForce;

        _groundCheck = groundCheck;
        _groundCheckRadius = groundCheckRadius;
        _groundLayer = groundLayer;
    }

    public void ProcessInput(float x) => _inputX = x;

    public void ApplyMovement()
    {
        _rb.velocity = new Vector2(_inputX * _speed, _rb.velocity.y);
    }

    public bool IsGrounded()
    {
        int hits = Physics2D.OverlapCircleNonAlloc(_groundCheck.position, _groundCheckRadius, _groundHitCache, _groundLayer);
        return hits > 0;
    }

    public void Jump()
    {
        if (IsGrounded())
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
        }
    }

    public void ApplyKnockback(Vector2 sourcePosition)
    {
        Vector2 dir = ((Vector2)_rb.transform.position - sourcePosition).normalized;
        dir.y = 0.5f;
        _rb.velocity = Vector2.zero;
        _rb.AddForce(dir.normalized * _knockbackForce, ForceMode2D.Impulse);
    }

    public void Stop() => _rb.velocity = Vector2.zero;
}