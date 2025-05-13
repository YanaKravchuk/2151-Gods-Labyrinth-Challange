using Core.StateMachine;
using Cysharp.Threading.Tasks;
using System.Threading;
using Application.UI;
using Application.Game;
using Core.Factory;
using Application.Game.Maze;
using UnityEngine;
using System.Threading.Tasks;
using System;
using Core;

public class MazeGameState : StateController
{
    private readonly IUiService _uiService;
    private ISettingProvider _settingProvider;
    private string _screen;
    private CancellationToken _cancellationTokenSource;
    private GameObjectFactory _gameObjectFactory;
    private GameOverWinPopup _gameOverWinPopup;
    private GameOverLosePopup _gameOverLosePopup;
    private PausePopup _pausePopup;
    private MazeGameController _mazeGameController;
    private LevelsModel _levelsModel;

    public MazeGameState(Core.ILogger logger,
        IUiService uiService,
        GameObjectFactory gameObjectFactory,
        LevelsModel levelsModel,
        ISettingProvider settingProvider) : base(logger)
    {
        _uiService = uiService;
        _gameObjectFactory = gameObjectFactory;
        _levelsModel = levelsModel;
        _settingProvider = settingProvider;
    }

    public override async UniTask Enter(CancellationToken cancellationToken)
    {
        await LevelLoad();
        await UniTask.CompletedTask;
    }

    public override UniTask Exit()
    {
        CleanupGameOverWinPopup();
        CleanupGameOverLosePopup();
        CleanupPausePopup();
        CleanupMazeGameController();

        return base.Exit();
    }

    private void CleanupGameOverWinPopup()
    {
        if (_gameOverWinPopup != null)
        {
            _gameOverWinPopup.RestartLevelPressEvent -= RestartLevel;
            _gameOverWinPopup.GoToHomeButtonPressEvent -= GoToHome;
            _gameOverWinPopup.GoToNextLevelButtonPressEvent -= GoToNextLevelAsync;
            _gameOverWinPopup.DestroyPopup();
            _gameOverWinPopup = null;
        }
    }

    private void CleanupGameOverLosePopup()
    {
        if (_gameOverLosePopup != null)
        {
            _gameOverLosePopup.RestartLevelPressEvent -= RestartLevel;
            _gameOverLosePopup.DestroyPopup();
            _gameOverLosePopup = null;
        }
    }

    private void CleanupPausePopup()
    {
        if (_pausePopup != null)
        {
            _pausePopup.RestartLevelPressEvent -= RestartLevel;
            _pausePopup.GoToHomeButtonPressEvent -= GoToHome;
            _pausePopup.PauseButtonPressEvent -= _mazeGameController.OnPauseGame;
            _pausePopup.UnpauseButtonPressEvent -= _mazeGameController.OnUnpauseGame;
            _pausePopup.DestroyPopup();
            _pausePopup = null;
        }
    }

    private void CleanupMazeGameController()
    {
        if (_mazeGameController != null)
        {
            _mazeGameController.PlayerFinishedEvent -= ShowGameOverWinPopup;
            _mazeGameController.PlayerDeadEvent -= ShowGameOverLosePopup;
            _mazeGameController.PauseGameEvent -= ShowGamePausePopup;
            _mazeGameController.Destroy();
            _mazeGameController = null;
        }
    }

    protected async void ShowGameOverWinPopup()
    {
        if (_gameOverWinPopup != null)
            return;
        _gameOverWinPopup = _uiService.GetPopup<GameOverWinPopup>(ConstPopups.GameOverWinPopup);
        _gameOverWinPopup.Initialize();
        _gameOverWinPopup.RestartLevelPressEvent += RestartLevel;
        _gameOverWinPopup.GoToHomeButtonPressEvent += GoToHome;
        _gameOverWinPopup.GoToNextLevelButtonPressEvent += GoToNextLevelAsync;
        await _gameOverWinPopup.Show(new GameOverWinPopupData(_mazeGameController.Keys));
    }

    protected async void ShowGameOverLosePopup()
    {
        if (_gameOverLosePopup != null)
        {
            return;
        }

        _gameOverLosePopup = _uiService.GetPopup<GameOverLosePopup>(ConstPopups.GameOverLosePopup);
        _gameOverLosePopup.Initialize();
        _gameOverLosePopup.RestartLevelPressEvent += RestartLevel;

        await _gameOverLosePopup.Show(new GameOverLosePopupData());
    }

    protected async void ShowGamePausePopup()
    {

        if (_pausePopup != null)
        {
            return;
        }
        _pausePopup = _uiService.GetPopup<PausePopup>(ConstPopups.PausePopup);
        _pausePopup.Initialize();
        _pausePopup.RestartLevelPressEvent += RestartLevel;
        _pausePopup.GoToHomeButtonPressEvent += GoToHome;
        _pausePopup.PauseButtonPressEvent += _mazeGameController.OnPauseGame;
        _pausePopup.UnpauseButtonPressEvent += _mazeGameController.OnUnpauseGame;
        await _pausePopup.Show(new PausePopupData());
    }

    protected void GoToHome()
    {
        Exit();
        GoTo<MenuStateController>();
    }

    protected async void GoToNextLevelAsync()
    {
        await Exit();
        _levelsModel.CurrentLevel += 1;
        GoTo<MazeGameState>();
    }

    protected async void RestartLevel()
    {
        await Exit();
        GoTo<MazeGameState>();
    }

    protected async UniTask LevelLoad()
    {
        var gameConfig = _settingProvider.Get<GameConfig>();
        var allowLevelsID = gameConfig.openLevels;

        if (_levelsModel.CurrentLevel <= allowLevelsID)
        {
            var gameObject = await _gameObjectFactory.Create(ConstMazeGame.Level_ + _levelsModel.CurrentLevel);
            _mazeGameController = gameObject.GetComponent<MazeGameController>();

            if (_mazeGameController != null && _mazeGameController.isActiveAndEnabled)
            {
                _mazeGameController.PlayerFinishedEvent += ShowGameOverWinPopup;
                _mazeGameController.PlayerDeadEvent += ShowGameOverLosePopup;
                _mazeGameController.PauseGameEvent += ShowGamePausePopup;
            }
        }
        else
        {
            var messagePopup = _uiService.GetPopup<MessagePopup>(ConstPopups.MessagePopup);
            messagePopup.DestroyPopupEvent += GoToHome;
            await messagePopup.Show(new MessagePopupData());
        }

        await UniTask.CompletedTask;
    }
}