using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Application.UI
{
    public class MenuScreen : UiScreen
    {
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _infoButton;
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _privacyPolicyButton;

        public event Action<int> LevelButtonPressEvent;
        public event Action SettingsButtonPressEvent;
        public event Action InfoButtonPressEvent;
        public event Action PlayButtonPressEvent;
        public event Action PrivacyPolicyButtonPressEvent;

        private void OnDestroy()
        {
            _settingsButton.onClick.RemoveAllListeners();
            _infoButton.onClick.RemoveAllListeners();
            _playButton.onClick.RemoveAllListeners();
            _privacyPolicyButton.onClick.RemoveAllListeners();
        }

        public void Initialize()
        {
            _settingsButton.onClick.AddListener(OnSettingsButtonPress);
            _infoButton.onClick.AddListener(OnInfoButtonPress);
            _playButton.onClick.AddListener(OnPlayButtonPress);
            _privacyPolicyButton.onClick.AddListener(OnPrivacyPolicyButtonPress);
        }

        private void OnSettingsButtonPress()
        {
            SettingsButtonPressEvent?.Invoke();
        }
        private void OnInfoButtonPress()
        {
            InfoButtonPressEvent?.Invoke();
        }
        private void OnPlayButtonPress()
        {
            PlayButtonPressEvent?.Invoke();
        }
        private void OnPrivacyPolicyButtonPress()
        {
            PrivacyPolicyButtonPressEvent?.Invoke();
        }
    }
}