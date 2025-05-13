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
    public class GameOverLosePopup : BasePopup
    {
        [SerializeField] private Button _restartLevelButton;

        public event Action RestartLevelPressEvent;

        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            AudioService.PlaySound(ConstAudio.LoseSound);

            return base.Show(data, cancellationToken);
        }

        public void Initialize()
        {
            _restartLevelButton.onClick.AddListener(OnRestartLevelButtonPress);
        }

        public override void DestroyPopup()
        {
            AudioService.PlaySound(ConstAudio.PressButtonSound);
            _restartLevelButton.onClick.RemoveAllListeners();
            base.DestroyPopup();
        }

        public void OnRestartLevelButtonPress()
        {
            RestartLevelPressEvent?.Invoke();
        }
    }
}