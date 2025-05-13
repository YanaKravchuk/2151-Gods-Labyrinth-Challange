using Core.StateMachine;
using Application.Services.Audio;
using Application.UI;
using Application.Services.UserData;
using Cysharp.Threading.Tasks;
using ILogger = Core.ILogger;
using Core;
using System.Threading;

namespace Application.Game
{
    public class MenuStateController : StateController
    {
        private readonly IUiService _uiService;
        private readonly IAudioService _audioService;
        private readonly UserDataService _userDataService;

        private ISettingProvider _settingProvider;
        private MenuScreen _menuScreen;
        private LevelsModel _levelsModel;

        public MenuStateController(ILogger logger,
            IUiService uiService,
            UserDataService userDataService,
            IAudioService audioService,
            LevelsModel levelsModel,
             ISettingProvider settingProvider) : base(logger)
        {
            _uiService = uiService;
            _userDataService = userDataService;
            _audioService = audioService;
            _levelsModel = levelsModel;
            _settingProvider = settingProvider;
        }

        public override UniTask Enter(CancellationToken cancellationToken)
        {
            _menuScreen = _uiService.GetScreen<MenuScreen>(ConstScreens.MenuScreen);
            _menuScreen.SettingsButtonPressEvent += ShowSettingsPopup;
            _menuScreen.InfoButtonPressEvent += ShowInfoPopup;
            _menuScreen.PlayButtonPressEvent += ShowLevelSelectionPopup;
            _menuScreen.PrivacyPolicyButtonPressEvent += ShowPrivacyPolicyPopup;
            _menuScreen.Initialize();
            _menuScreen.ShowAsync().Forget();


            return UniTask.CompletedTask;
        }

        public override async UniTask Exit()
        {
            _menuScreen.SettingsButtonPressEvent -= ShowSettingsPopup;
            _menuScreen.InfoButtonPressEvent -= ShowInfoPopup;
            _menuScreen.LevelButtonPressEvent -= GoLevel;
            _menuScreen.PlayButtonPressEvent -= ShowLevelSelectionPopup;
            _menuScreen.PrivacyPolicyButtonPressEvent -= ShowPrivacyPolicyPopup;

            await _uiService.HideScreen(ConstScreens.MenuScreen);
        }

        private void ShowSettingsPopup()
        {
            var settingsPopup = _uiService.GetPopup<SettingsPopup>(ConstPopups.SettingsPopup);

            settingsPopup.SoundVolumeChangeEvent += OnChangeSoundVolume;
            settingsPopup.MusicVolumeChangeEvent += OnChangeMusicVolume;

            var userData = _userDataService.GetUserData();
            var isSoundVolume = userData.SettingsData.IsSoundVolume;
            var isMusicVolume = userData.SettingsData.IsMusicVolume;

            settingsPopup.Show(new SettingsPopupData(isSoundVolume, isMusicVolume));
        }

        private void ShowInfoPopup()
        {
            var infoPopup = _uiService.GetPopup<InfoPopup>(ConstPopups.InfoPopup);

            infoPopup.SettingsButtonPressEvent += ShowSettingsPopup;

            infoPopup.Show(new InfoPopupData());
        }

        private void ShowPrivacyPolicyPopup()
        {
            var popup = _uiService.GetPopup<PrivacyPolicyPopup>(ConstPopups.PrivacyPolicyPopup);

            popup.Show(null);
        }

        private void ShowLevelSelectionPopup()
        {
            var popup = _uiService.GetPopup<LevelSelectionPopup>(ConstPopups.LevelSelectionPopup);
            popup.LevelButtonPressEvent += GoLevel;

            popup.Show(null);
        }

        private void OnChangeSoundVolume(bool state)
        {
            _audioService.SetVolume(AudioType.Sound, state ? 1 : 0);
            var userData = _userDataService.GetUserData();
            userData.SettingsData.IsSoundVolume = state;
        }

        private void OnChangeMusicVolume(bool state)
        {
            _audioService.SetVolume(AudioType.Music, state ? 1 : 0);
            var userData = _userDataService.GetUserData();
            userData.SettingsData.IsMusicVolume = state;
        }

        private async void GoLevel(int level_id)
        {
            var gameConfig = _settingProvider.Get<GameConfig>();
            var allowLevelsID = gameConfig.openLevels;

            if (level_id <= allowLevelsID)
            {
                _levelsModel.CurrentLevel = level_id;
                GoTo<MazeGameState>();
            }
            else
            {
                var messagePopup = _uiService.GetPopup<MessagePopup>(ConstPopups.MessagePopup);
                await messagePopup.Show(new MessagePopupData());
            }
        }
    }
}