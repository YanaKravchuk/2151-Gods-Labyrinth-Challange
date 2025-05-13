using Application.Game.Maze;
using Core;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private EnemyConfig _enemyConfig;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Transform[] _patrolPoints;
    [SerializeField] private ISettingProvider _settingProvider;
    [SerializeField] private List<Vector3> _patrolPointsPosition;
    private int _currentPatrolIndex = 0;
    private bool _isChasing = false;

    [Inject]
    public void Construct(ISettingProvider settingProvider)
    {
        _settingProvider = settingProvider;
    }

    private void Start()
    {
        Initialize();
        GoToNextPatrolPoint();
    }

    private void Update()
    {
        if (_isChasing)
        {
            if (GetDistanceToPlayer() <= _enemyConfig.detectionRadius)
            {
                _isChasing = true;
                SetTarget(_targetTransform.position);
            }
            else
            {
                _isChasing = false;
                SetTarget(_patrolPointsPosition[_currentPatrolIndex]);
                return;
            }
        }
        else
        {
            if (GetDistanceToPlayer() <= _enemyConfig.detectionRadius)
            {
                _isChasing = true;
            }

            if (!_agent.pathPending && _agent.remainingDistance <= 0.5f)
            {
                GoToNextPatrolPoint();
            }

        }
        RotateModel();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(ConstMazeGame.PlayerTag))
        {
            Player player;
            collision.TryGetComponent<Player>(out player);
            player?.PlayerCaughtEvent?.Invoke(5);
        }
    }

    private void Initialize()
    {
        _enemyConfig = _settingProvider.Get<EnemyConfig>();

        foreach (var partolPoint in _patrolPoints)
        {
            _patrolPointsPosition.Add(partolPoint.position);
        }

        if (TryGetComponent<NavMeshAgent>(out _agent))
        {
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;
        }

        _enemyConfig = _settingProvider.Get<EnemyConfig>();

        UpdateEnemyConfig();
    }

    //todo : del (add to SRDebugger)
    private void UpdateEnemyConfig()
    {
        _agent.speed = _enemyConfig.speed;
    }

    private void SetTarget(Vector3 moveTarget)
    {
        _agent.SetDestination(moveTarget);
    }

    private void RotateModel()
    {
        if (_agent.velocity.sqrMagnitude > 0.01f)
        {
            Vector2 direction = _agent.velocity.normalized;

            float targetAngleZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            Quaternion currentRotation = transform.rotation;

            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngleZ);

            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, _enemyConfig.rotationSpeed * Time.deltaTime);
        }
    }

    private float GetDistanceToPlayer()
    {
        return Vector3.Distance(_targetTransform.position, transform.position);
    }

    private void GoToNextPatrolPoint()
    {
        if (_patrolPointsPosition.Count == 0)
            return;

        _currentPatrolIndex = (_currentPatrolIndex + 1) % _patrolPoints.Length;
        SetTarget(_patrolPointsPosition[_currentPatrolIndex]);
    }

    public void StopMove()
    {
        _agent.isStopped = true;
        _agent.speed = 0;
    }

    public void ContinueMove()
    {
        _agent.isStopped = false;
        _agent.speed = _enemyConfig.speed;
    }
}