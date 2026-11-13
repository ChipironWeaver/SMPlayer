using System;
using System.Collections.Generic;
using NaughtyAttributes;
using States;
using TMPro;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D),
    typeof(SpriteRenderer),
    typeof(InputController))]
public class PlayerController : MonoBehaviour 
{

    [SerializeField] private float _speed;
    [SerializeField] private float _airControl;
    [SerializeField] private float _jumpForce;
    [SerializeField,Expandable] private List<ComboAction> _comboActions;
    [SerializeField] private PolygonCollider2D _hurtBox;
    [Header("Ground Check")]
    [SerializeField] private float _groundCheckDistance;

    [SerializeField] private Vector2 _groundCheckSize;
    [SerializeField] private LayerMask _groundLayer;
    [Header("Debug")]
    [SerializeField] private TextMeshProUGUI _debugStateText;
    [SerializeField] private TextMeshProUGUI _debugGroundedText;
    [SerializeField] private TextMeshProUGUI _debugJumpInputText;
    
    private Rigidbody2D _rigidbody2D;
    private PlayerHealthController _healthController;
    private SpriteRenderer _spriteRenderer;
    private State _currentState;
    private InputController _inputController;
    private PlayerDamageSource _playerDamageSource;
    private Sprite _lastSprite;
    private readonly List<Vector2> _points = new();
    private float _lastMoveX;
    private RuntimeAnimatorController _baseAnimatorController;
    private int _currentComboIndex;
    private PlayerHealthController _playerHealthController;
    
    public Rigidbody2D Rigidbody2D => _rigidbody2D;
    public SpriteRenderer SpriteRenderer => _spriteRenderer;
    public PlayerDamageSource PlayerDamageSource => _playerDamageSource;
    public RuntimeAnimatorController BaseAnimatorController => _baseAnimatorController;
    public PlayerHealthController PlayerHealthController => _playerHealthController;
    public float Speed => _speed;
    public float JumpForce => _jumpForce;
    public float AirControl => _airControl;
    public bool IsGrounded 
    {
        get
        {
            RaycastHit2D hit =
                Physics2D.BoxCast(transform.position, _groundCheckSize, 0, Vector2.down, _groundCheckDistance, _groundLayer);
            return hit.collider;
        }
    }

    private void Awake() 
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _inputController = GetComponent<InputController>();
        _healthController = GetComponent<PlayerHealthController>();
        _playerDamageSource = _hurtBox.gameObject.GetComponent<PlayerDamageSource>();
        _baseAnimatorController = GetComponent<Animator>().runtimeAnimatorController ;
        _playerHealthController = GetComponent<PlayerHealthController>();
        
        _healthController.OnDamage.AddListener(OnDamage);
        _healthController.OnDeath.AddListener(OnDeath);
        _playerDamageSource.OnBlockedHit.AddListener(OnBlockHit);
        
        SwitchState(new IdleState(gameObject));
        
    }

    public void OnDamage(DamageInfo damageInfo)
    {
        Debug.Log($"first : {damageInfo.Knockback} boom {damageInfo.Knockback.normalized}");
        _rigidbody2D.AddForce(damageInfo.Knockback.normalized * damageInfo.KnockbackStrength, ForceMode2D.Impulse);
        SwitchState(new StunState(gameObject,Color.red));
    }

    public void OnDeath()
    {
        SwitchState(new KoState(gameObject));
        Debug.Log($"maaaaannnnn im dead asf, said {name}");
    }

    public void OnBlockHit(float direction)
    {
        SwitchState(new StunState(gameObject,Color.cornflowerBlue));
        _rigidbody2D.AddForce(new Vector2(3 * direction * -1 , 3), ForceMode2D.Impulse);
    }
    
    private void Update() 
    {
        _currentState.OnUpdate();
        
        DoStateLogic();
        
        
        //DEBUG
        if(_debugStateText) _debugStateText.text = $"State: {_currentState.GetType().Name}";
        if(_debugGroundedText) _debugGroundedText.text = $"Is Grounded: {IsGrounded}";
        if(_debugJumpInputText) _debugJumpInputText.text = $"Is Jumping: {_inputController.IsJumping}";
    }
    

    private void DoStateLogic()
    {
        if (_currentState is StunState && _currentState.IsFinished)
        {
            SwitchState(new IdleState(gameObject));
        }
        if (_currentState is BlockState && _currentState.IsFinished)
        {
            SwitchState(new IdleState(gameObject));
        }
        if (_currentState is AttackState && _currentState.IsFinished)  // ATK -> base
        {
            SwitchState(new RecoveryState(gameObject));
        }
        if (_currentState is RecoveryState && _currentState.IsFinished)  // ATK -> base
        {
            if (_inputController.IsAttacking.Value)
            {
                _currentComboIndex ++;
                if (_currentComboIndex >= _comboActions.Count) _currentComboIndex = 0;
                SwitchState(new AttackState(gameObject,_comboActions[_currentComboIndex]));
            }
            else SwitchState(new MoveState(gameObject));
        }
        if (_currentState is LandingState && _currentState.IsFinished)  // Land -> base
        {
            SwitchState(new MoveState(gameObject));
        }
        if (_currentState is MoveState && _inputController.Move.x == 0)
        {
            SwitchState(new IdleState(gameObject));
        }
        if (_currentState is IdleState && _inputController.Move.x != 0)
        {
            SwitchState(new MoveState(gameObject));
        }
        
        if (_currentState is MoveState or IdleState && _inputController.IsJumping.Value && IsGrounded) 
        {
            _inputController.IsJumping.Stop();
            SwitchState(new JumpState(gameObject));
        }

        if (_currentState is JumpState && !IsGrounded)
        {
            SwitchState(new AirbornState(gameObject));
        }
        
        if (_currentState is AirbornState && IsGrounded)
        {
            SwitchState(new LandingState(gameObject));
        }
        
        if (_currentState is MoveState or IdleState && _inputController.IsAttacking.Value) 
        {
            SwitchState(new AttackState(gameObject,_comboActions[0]));
            _currentComboIndex = 0;
        }
        if (_currentState is MoveState or IdleState && _inputController.IsBlocking.Value) 
        {
            SwitchState(new BlockState(gameObject));
        }
        
        if (_currentState is MoveState or IdleState && !IsGrounded) 
        {
            SwitchState(new AirbornState(gameObject, 0.5f));
        }
    }
    
    private void SwitchState(State state)
    {
        if (state == _currentState) return;
        //Debug.Log($"Switching to {state.GetType().Name}");
        _currentState?.OnExit();
        _currentState = state;
        _currentState?.OnEnter();
    }

    private bool _hurtBoxActive;

    public void OnAttackActive() 
    {
        _hurtBoxActive = true;
        _lastSprite = null;
        _hurtBox.gameObject.SetActive(true);
    }

    public void OnAttackFinished() 
    {
        _hurtBoxActive = false;
        _hurtBox.gameObject.SetActive(false);
    }

    private void LateUpdate() 
    {
        if (!_hurtBoxActive) return;
        
        Sprite sprite = _spriteRenderer.sprite;
        if (sprite == _lastSprite) return;
        _lastSprite = sprite;

        int shapeCount = sprite ? sprite.GetPhysicsShapeCount() : 0;
        _hurtBox.pathCount = shapeCount;

        for (int i = 0; i < shapeCount; i++) 
        {
            sprite.GetPhysicsShape(i, _points);

            if (_spriteRenderer.flipX)
                for (int p = 0; p < _points.Count; p++)
                    _points[p] = new Vector2(-_points[p].x, _points[p].y);

            _hurtBox.SetPath(i, _points);
        }
    }
}