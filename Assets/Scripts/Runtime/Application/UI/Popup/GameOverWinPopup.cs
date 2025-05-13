using System;
using System.Threading;
using Application.Services.Audio;
using Core.UI;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Application.UI
{
    public class GameOverWinPopup : BasePopup
    {
        [SerializeField] private TextMeshProUGUI _keysCountText;
        [SerializeField] private Button _restartLevelButton;
        [SerializeField] private Button _goToHomeButton;
        [SerializeField] private Button _goToNextLevelButton;

        public event Action RestartLevelPressEvent;
        public event Action GoToHomeButtonPressEvent;
        public event Action GoToNextLevelButtonPressEvent;

        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            AudioService.PlaySound(ConstAudio.WinSound);

            GameOverWinPopupData gameOverWinPopupData = data as GameOverWinPopupData;
            _keysCountText.text = gameOverWinPopupData.KeysCount.ToString();

            return base.Show(data, cancellationToken);
        }

        public void Initialize()
        {
            _restartLevelButton.onClick.AddListener(OnRestartLevelButtonPress);
            _goToHomeButton.onClick.AddListener(OnGoToHomeButtonPress);
            _goToNextLevelButton.onClick.AddListener(OnGoToNextLevelButtonPress);
        }

        public override void DestroyPopup()
        {
            AudioService.PlaySound(ConstAudio.PressButtonSound);
            _restartLevelButton.onClick.RemoveAllListeners();
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

        public void OnGoToNextLevelButtonPress()
        {
            GoToNextLevelButtonPressEvent?.Invoke();
        }
    }
}