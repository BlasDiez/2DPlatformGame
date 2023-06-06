using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Pollo : MonoBehaviour
{
    [SerializeField] private float velocity;
    [SerializeField] private LayerMask enemyLayer;

    private Rigidbody2D _rb;
    private Animator _animator;
    private BoxCollider2D _boxCollider2D;
    public bool facingLeft = true;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
        transform.localScale = new Vector3(1f, 1f, 1f);
        _rb.velocity = new Vector2(-velocity, 0);
    }
    void Update()
    {
        CheckPolloIsRunning();
    }

    private void CheckPolloIsRunning()
    {
        _animator.SetBool("IsRunning", Math.Abs(velocity) > 0);
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.otherCollider.GetType() == typeof(CapsuleCollider2D))
        {
            if (facingLeft)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                _rb.velocity = new Vector2(velocity, 0);
                facingLeft = false;
            }
            else
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
                _rb.velocity = new Vector2(-velocity, 0);
                facingLeft = true;
            }
        }
    }
}
