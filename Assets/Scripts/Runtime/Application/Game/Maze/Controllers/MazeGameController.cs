using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MazeGameController : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private List<Enemy> _enemies;
    [SerializeField] private Button _pauseButton;
    [SerializeField] private int _keys;

    public int Keys => _keys;

    public event Action PlayerFinishedEvent;
    public event Action PlayerDeadEvent;
    public event Action PauseGameEvent;
    public event Action UnpauseGameEvent;
    public event Action KeyPickedUpEvent;

    private void OnEnable()
    {
        _player.gameObject.SetActive(true);
        foreach (var enemy in _enemies)
        {
            enemy.gameObject.SetActive(true);
        }

        _player.PlayerFinishedEvent += OnPlayerFinished;
        _player.PlayerDeadEvent += OnPlayerDead;
        _player.PlayerPickedUpKeyEvent += OnKeyPickedUp;
        _pauseButton.onClick.AddListener(OnPauseGame);

        _keys = 0;
    }

    private void OnDisable()
    {
        _player.PlayerFinishedEvent -= OnPlayerFinished;
        _player.PlayerDeadEvent -= OnPlayerDead;
        _pauseButton.onClick.RemoveAllListeners();
    }

    private void OnDestroy()
    {
        _player.PlayerFinishedEvent -= OnPlayerFinished;
        _player.PlayerDeadEvent -= OnPlayerDead;
        _pauseButton.onClick.RemoveAllListeners();
    }

    private void OnPlayerFinished()
    {
        StopMoveAll();
        PlayerFinishedEvent?.Invoke();
    }

    private void OnPlayerDead()
    {
        StopMoveAll();
        PlayerDeadEvent?.Invoke();
    }

    private void OnKeyPickedUp()
    {
        KeyPickedUpEvent?.Invoke();
        _keys++;
    }

    public void OnPauseGame()
    {
        StopMoveAll();
        PauseGameEvent?.Invoke();
    }

    public void OnUnpauseGame()
    {
        ContinuesMoveAll();
        UnpauseGameEvent?.Invoke();
    }

    private void StopMoveAll()
    {
        _player.StopMove();

        foreach (var enemy in _enemies)
        {
            enemy.StopMove();
        }
    }

    private void ContinuesMoveAll()
    {
        _player.ContinueMove();

        foreach (var enemy in _enemies)
        {
            enemy.ContinueMove();
        }
    }

    public void Destroy()
    {
        _player.PlayerFinishedEvent -= OnPlayerFinished;
        _player.PlayerDeadEvent -= OnPlayerDead;
        Destroy(gameObject);
    }
}