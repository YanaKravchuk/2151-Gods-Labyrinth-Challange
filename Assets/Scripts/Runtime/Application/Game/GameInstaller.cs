using UnityEngine;
using Zenject;
using JoystickPack;

namespace Application.Game
{
    [CreateAssetMenu(fileName = "GameInstaller", menuName = "Installers/GameInstaller")]
    public class GameInstaller : ScriptableObjectInstaller<GameInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<MenuStateController>().AsSingle();
            Container.Bind<MazeGameState>().AsSingle();
            Container.Bind<Joystick>().AsSingle();
        }
    }
}