using System;
using System.Threading;
using Application.Services.Audio;
using Application.Services.UserData;
using Core.UI;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Application.UI
{
    public class SettingsPopup : BasePopup
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _difficultyModeButton;
        [SerializeField] private TextMeshProUGUI _difficultyModeText;
        [SerializeField] private Toggle _soundVolumeToggle;
        [SerializeField] private Toggle _musicVolumeToggle;


        private GameDifficultyMode _gameDifficultyMode;

        public event Action<bool> SoundVolumeChangeEvent;
        public event Action<bool> MusicVolumeChangeEvent;
        public event Action<GameDifficultyMode> GameDifficultyModeChangeEvent;

        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            SettingsPopupData settingsPopupData = data as SettingsPopupData;

            var isSoundVolume = settingsPopupData.IsSoundVolume;
            var isMusicVolume = settingsPopupData.IsMusicVolume;

            _soundVolumeToggle.onValueChanged.AddListener(OnSoundVolumeValueChanged);
            _musicVolumeToggle.onValueChanged.AddListener(OnMusicVolumeToggleValueChanged);

            _soundVolumeToggle.isOn = isSoundVolume;
            _musicVolumeToggle.isOn = isMusicVolume;

            _backButton.onClick.AddListener(DestroyPopup);

            AudioService.PlaySound(ConstAudio.OpenPopupSound);

            return base.Show(data, cancellationToken);
        }

        private void OnSoundVolumeValueChanged(bool value)
        {
            AudioService.PlaySound(ConstAudio.PressButtonSound);
            SoundVolumeChangeEvent?.Invoke(value);
        }

        private void OnMusicVolumeToggleValueChanged(bool value)
        {
            AudioService.PlaySound(ConstAudio.PressButtonSound);
            MusicVolumeChangeEvent?.Invoke(value);
        }

        public override void DestroyPopup()
        {
            AudioService.PlaySound(ConstAudio.PressButtonSound);
            Destroy(gameObject);
        }
    }
}