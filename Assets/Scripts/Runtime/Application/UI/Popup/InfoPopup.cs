using System;
using System.Threading;
using Core.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Application.UI
{
    public class InfoPopup : BasePopup
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _settingButton;

        public event Action SettingsButtonPressEvent;

        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            _backButton.onClick.RemoveAllListeners();
            _settingButton.onClick.RemoveAllListeners();

            _backButton.onClick.AddListener(Hide);
            _settingButton.onClick.AddListener(OnSettingsButtonPress);

            return base.Show(data, cancellationToken);
        }

        public void OnSettingsButtonPress()
        {
            SettingsButtonPressEvent?.Invoke();
        }
    }
}