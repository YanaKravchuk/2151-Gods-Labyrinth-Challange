using UnityEngine;
using UnityEngine.AI;
using JoystickPack;
using Core;
using Zenject;
using System;
using Application.Game.Maze;
using Application.UI;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    [SerializeField] private VariableJoystick _joystick;
    [SerializeField] private PlayerConfig _playerConfig;
    [SerializeField] private List<KeyType> keys;

    private ISettingProvider _settingProvider;
    private IUiService _uiService;
    private NavMeshAgent _agent;
    private Vector3 _direction;
    private bool _isStoped;

    public Action<int> PlayerCaughtEvent;
    public event Action PlayerFinishedEvent;
    public event Action PlayerDeadEvent;
    public event Action PlayerPickedUpKeyEvent;

    [Inject]
    public void Construct(ISettingProvider settingProvider, IUiService uiService)
    {
        _settingProvider = settingProvider;
        _uiService = uiService;
    }

    private void OnEnable()
    {
        Initialize();
        PlayerCaughtEvent += TakeDamage;
    }

    private void Update()
    {
        if (_isStoped)
            return;
        MoveWithJoystick();
    }

    private void OnDestroy()
    {
        PlayerFinishedEvent -= StopMove;
        PlayerDeadEvent -= StopMove;
        PlayerCaughtEvent -= TakeDamage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(ConstMazeGame.FinishTag))
        {
            PlayerFinishedEvent?.Invoke();
        }
        if (collision.CompareTag(ConstMazeGame.EnemyTag))
        {
            PlayerDeadEvent?.Invoke();
        }
        if (collision.CompareTag(ConstMazeGame.KeyTag))
        {
            PlayerPickedUpKeyEvent?.Invoke();
            var key = collision.GetComponent<Key>();
            keys.Add(key.KeyType);
            collision.gameObject.SetActive(false);
        }
        if (collision.CompareTag(ConstMazeGame.DoorTag))
        {
            var door = collision.GetComponent<Door>();
            door.TryOpenDoor(keys);
        }
    }

    private void Initialize()
    {
        _playerConfig = _settingProvider.Get<PlayerConfig>();

        if (TryGetComponent<NavMeshAgent>(out _agent) == false)
        {
            _agent = gameObject.AddComponent<NavMeshAgent>();
        }

        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _agent.speed = _playerConfig.speed;

        PlayerFinishedEvent += StopMove;
        PlayerDeadEvent += StopMove;

        keys = new List<KeyType>();
    }

    //todo : add SRDebugger button
    private void UpdatePlayerConfig()
    {
        _agent.speed = _playerConfig.speed;
    }

    private void MoveWithJoystick()
    {

        _direction = new Vector3(_joystick.Horizontal, _joystick.Vertical, 0);

        if (_direction.magnitude >= 0.1f)
        {
            Vector3 moveDirection = _direction.normalized * _playerConfig.moveRange * Time.deltaTime;

            Vector3 targetPosition = transform.position + moveDirection * _agent.radius;
            transform.position += _direction.normalized * _playerConfig.speed * Time.deltaTime;

            float angleZ = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angleZ);
        }
    }

    private void TakeDamage(int damage)
    {
        Debug.Log($"player Take Damage = {damage}");
    }

    public void StopMove()
    {
        _joystick.gameObject.SetActive(false);
        _agent.isStopped = true;
        _isStoped = true;
    }

    public void ContinueMove()
    {
        _joystick.gameObject.SetActive(true);
        _agent.isStopped = false;
        _isStoped = false;
    }
}