using Application.Services.Audio;
using Application.UI;
using Core.UI;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class PausePopup : BasePopup
{
    [SerializeField] private Button _restartLevelButton;
    [SerializeField] private Button _goToHomeButton;
    [SerializeField] private Button _unpauseButton;

    public event Action RestartLevelPressEvent;
    public event Action GoToHomeButtonPressEvent;
    public event Action UnpauseButtonPressEvent;
    public event Action PauseButtonPressEvent;

    public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
    {
        GameOverWinPopupData gameOverWinPopupData = data as GameOverWinPopupData;

        return base.Show(data, cancellationToken);
    }

    public void Initialize()
    {
        _restartLevelButton.onClick.AddListener(OnRestartLevelButtonPress);
        _goToHomeButton.onClick.AddListener(OnGoToHomeButtonPress);
        _unpauseButton.onClick.AddListener(OnUnpauseButtonPress);
    }

    public override void DestroyPopup()
    {
        AudioService.PlaySound(ConstAudio.PressButtonSound);
        _restartLevelButton.onClick.RemoveAllListeners();
        _goToHomeButton.onClick.RemoveAllListeners();
        _unpauseButton.onClick.RemoveAllListeners();
        Destroy(gameObject);
    }

    public void OnRestartLevelButtonPress()
    {
        RestartLevelPressEvent?.Invoke();
    }

    public void OnGoToHomeButtonPress()
    {
        GoToHomeButtonPressEvent?.Invoke();
    }

    public void OnUnpauseButtonPress()
    {
        UnpauseButtonPressEvent?.Invoke();
        Hide();
    }

    public void OnPauseButtonPress()
    {
        PauseButtonPressEvent?.Invoke();
    }
}