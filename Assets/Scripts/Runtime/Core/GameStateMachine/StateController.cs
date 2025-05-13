using Cysharp.Threading.Tasks;
using System.Threading;

namespace Core.StateMachine
{
    public abstract class StateController
    {
        private StateMachine _stateMachine;

        protected readonly ILogger Logger;

        protected StateController(ILogger logger)
        {
            Logger = logger;
        }

        public void Initialize(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public abstract UniTask Enter(CancellationToken cancellationToken);

        public virtual UniTask Exit()
        {
            return UniTask.CompletedTask;
        }

        protected void GoTo<T>() where T : StateController
        {
            _stateMachine.GoTo<T>();
        }
    }
}