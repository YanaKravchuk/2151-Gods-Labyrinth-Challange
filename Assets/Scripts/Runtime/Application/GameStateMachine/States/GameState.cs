using Cysharp.Threading.Tasks;
using Application.Game;
using Core.StateMachine;
using ILogger = Core.ILogger;
using System.Threading;

namespace Application.GameStateMachine
{
    public class GameState : StateController
    {
        private readonly StateMachine _stateMachine;

        private readonly MenuStateController _menuStateController;
        private readonly MazeGameState MazeGameState;
        private readonly UserDataStateChangeController _userDataStateChangeController;

        public GameState(ILogger logger,
            MenuStateController menuStateController,
            StateMachine stateMachine,
            MazeGameState firstLevelMazeStateController,
            UserDataStateChangeController userDataStateChangeController) : base(logger)
        {
            _stateMachine = stateMachine;
            _menuStateController = menuStateController;
            MazeGameState = firstLevelMazeStateController;
            _userDataStateChangeController = userDataStateChangeController;
        }

        public override async UniTask Enter(CancellationToken cancellationToken)
        {
            await _userDataStateChangeController.Run(default);

            _stateMachine.Initialize(_menuStateController,MazeGameState);
            _stateMachine.GoTo<MenuStateController>();
        }
    }
}