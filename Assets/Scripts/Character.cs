using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 5;
    [SerializeField] private LayerMask mapLayer;
    [SerializeField] private AudioClip jumpSound; 
    [SerializeField] private AudioClip doubleJumpSound; 
    [SerializeField] private AudioClip deadSound;
    
    private Rigidbody2D _rb;
    private float _movement;
    private bool _facingRight = true;
    private Animator _animator;
    private BoxCollider2D _boxCollider2D;
    private Boolean wannaJump = false;
    private Scene _actualScene;
    public int jumpCount = 0;
    
    private bool _jumpinFromWall;
    void Awake()
    {
        _actualScene = SceneManager.GetActiveScene();
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        _movement = Input.GetAxis("Horizontal");
        CheckCharacterFacing();
        CheckCharacterRunning();
        CharacterJump();
        CheckIsGrounded();
        CheckCharacterFalling();
        _animator.SetBool("OnWall", CheckCharacterIsOnWall());
    }

    private void FixedUpdate()
    { 
        PlayerMovement();
        PlayerStates();
    }

    private void CheckIsCharacterIsJumpingFromWall()
    {
        _jumpinFromWall = false;
    }

    private void CheckCharacterRunning()
    {
        _animator.SetBool("IsRunning", Math.Abs(_movement) > 0);
    }
    
    private bool CheckCharacterIsOnWall()
    {
        return _facingRight ?  IsOnRightWall() : IsOnLeftWall();
    }

    private void CheckCharacterFalling()
    {
        _animator.SetBool("IsFalling", _rb.velocity.y < 0);
    }

    private void CheckCharacterFacing()
    {
        if (_rb.velocity.x < -0.001f && _facingRight)
        {
            _facingRight = false;
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if(_rb.velocity.x > 0.001f && !_facingRight)
        {
            _facingRight = true;
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    private void CheckIsGrounded()
    {
        _animator.SetBool("IsGrounded", IsGrounded());
    }

    private void CharacterJump()
    {
        if (Input.GetKeyDown("space"))
        {
            wannaJump = true;
        }
    }

    private bool IsGrounded()
    {
        var boxCastHit = Physics2D.BoxCast(_boxCollider2D.bounds.center, _boxCollider2D.bounds.size, 0, Vector2.down, 0.05f,
            mapLayer);
        return boxCastHit.collider != null;
    }
    
    private bool IsOnLeftWall()
    {
        var boxCastHit = Physics2D.BoxCast(_boxCollider2D.bounds.center, _boxCollider2D.bounds.size, 0, Vector2.left, 0.05f,
            mapLayer);
        return boxCastHit.collider != null;
    }
    
    private bool IsOnRightWall()
    {
        var boxCastHit = Physics2D.BoxCast(_boxCollider2D.bounds.center, _boxCollider2D.bounds.size, 0, Vector2.right, 0.05f,
            mapLayer);
        return boxCastHit.collider != null;
    }

    private void ResetJumpCount()
    {
        jumpCount = 0;
    }
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            AudioManager.Instance.PlaySound(deadSound);
            SceneManager.LoadScene(_actualScene.buildIndex);
        }
    }

    private void PlayerMovement()
    {
        if (!_jumpinFromWall || _movement != 0f) _rb.velocity = new Vector2(_movement * speed, _rb.velocity.y);
    }

    private void PlayerStates()
    {
        if (wannaJump && IsGrounded())
        {
            AudioManager.Instance.PlaySound(jumpSound);
            ++jumpCount;
            _animator.SetTrigger("Jump");
            _rb.velocity = new Vector2(0, jumpForce);
        }
        else if (wannaJump && CheckCharacterIsOnWall())
        {
            _jumpinFromWall = true;
            ++jumpCount;
            _animator.SetTrigger("Jump");
            AudioManager.Instance.PlaySound(jumpSound);
            if (!_facingRight)
            {
                _rb.AddForce(new Vector2(5, 0), ForceMode2D.Impulse);
                _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
            }
            else
            {
                _rb.AddForce(new Vector2(-5, 0), ForceMode2D.Impulse);
                _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
            }
            Invoke("CheckIsCharacterIsJumpingFromWall", 1f);
        }
        else if (wannaJump && jumpCount < 2)
        {
            AudioManager.Instance.PlaySound(doubleJumpSound);
            ++jumpCount;
            _rb.velocity = new Vector2(0, jumpForce);
            _animator.SetTrigger("DoubleJump");
        }
        else if (IsGrounded())
        {
            _jumpinFromWall = false;
            ResetJumpCount();
        }
        wannaJump = false;
    }
}
