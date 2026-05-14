using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    [Header("Core References")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [Header("Movement Parameters")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpForce = 12f;
    [SerializeField] private float _knockbackForce = 7f;

    [Header("Ground Check (Physics)")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundCheckRadius = 0.15f;
    [SerializeField] private LayerMask _groundLayer;

    [Header("Animation Clips")]
    [SerializeField] private PlayerAnimationClips _animationClips;

    private PlayerMovement _movement;
    private PlayerHealth _health;
    private PlayerPlayableAnimator _playerAnimator;

    private Coroutine _immunityCoroutine;
    private WaitForSeconds _blinkDelay;

    public PlayerHealth Health => _health;

    private void Awake()
    {
        _movement = new PlayerMovement(_rb, _moveSpeed, _jumpForce, _knockbackForce, _groundCheck, _groundCheckRadius, _groundLayer);
        _health = new PlayerHealth(3);
        _playerAnimator = new PlayerPlayableAnimator(_animator, _animationClips);

        _blinkDelay = new WaitForSeconds(0.1f);

        _health.OnTakeDamage += HandleDamage;
        _health.OnDie += HandleDeath;
    }

    private void Update()
    {
        if (_health.IsDead) return;

        float inputX = Input.GetAxisRaw("Horizontal");
        bool jumpInput = Input.GetButtonDown("Jump");
        if (inputX != 0f)
        {
            _spriteRenderer.flipX = inputX < 0f;
        }
        _movement.ProcessInput(inputX);
        if (jumpInput) _movement.Jump();

        UpdateAnimations();
    }

    private void FixedUpdate()
    {
        if (_health.IsDead || _health.IsKnockedBack) return;
        _movement.ApplyMovement();
    }

    public void TakeDamage(int amount, Vector2 sourcePos)
    {
        if (_health.IsInvincible || _health.IsDead) return;

        _movement.ApplyKnockback(sourcePos);
        _health.TakeDamage(amount);
    }

    private void HandleDamage()
    {
        if (_immunityCoroutine != null) StopCoroutine(_immunityCoroutine);
        _immunityCoroutine = StartCoroutine(ImmunityRoutine());
    }

    private void HandleDeath()
    {
        _playerAnimator.SetState(PlayerPlayableAnimator.State.Die);
        _movement.Stop();
    }

    private void UpdateAnimations()
    {
        if (_health.IsDead) return;

        if (_health.IsKnockedBack)
        {
            _playerAnimator.SetState(PlayerPlayableAnimator.State.Hit);
            return;
        }

        Vector2 vel = _rb.velocity;
        bool isGrounded = _movement.IsGrounded();

        if (vel.y > 0.1f && !isGrounded)
            _playerAnimator.SetState(PlayerPlayableAnimator.State.Jump);
        else if (vel.y < -0.1f && !isGrounded)
            _playerAnimator.SetState(PlayerPlayableAnimator.State.Fall);
        else if (Mathf.Abs(vel.x) > 0.1f)
            _playerAnimator.SetState(PlayerPlayableAnimator.State.Run);
        else
            _playerAnimator.SetState(PlayerPlayableAnimator.State.Idle);
    }

    private IEnumerator ImmunityRoutine()
    {
        _health.SetInvincible(true);
        float elapsed = 0f;

        while (elapsed < 1f)
        {
            _spriteRenderer.color = Color.white;
            yield return _blinkDelay;
            _spriteRenderer.color = Color.clear;
            yield return _blinkDelay;
            elapsed += 0.2f;
        }

        _spriteRenderer.color = Color.white;
        _health.SetInvincible(false);
        _health.IsKnockedBack = false;
    }

    private void OnDestroy()
    {
        _health.OnTakeDamage -= HandleDamage;
        _health.OnDie -= HandleDeath;

        _playerAnimator?.Dispose();
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (_groundCheck == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
    }
#endif
}