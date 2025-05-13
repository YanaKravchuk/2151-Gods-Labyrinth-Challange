using System;
using System.Collections.Generic;
using Core;
using Application.Services.Audio;
using Cysharp.Threading.Tasks;
using Core.Services.ScreenOrientation;

namespace Application.Services
{
    public class SettingsProvider : ISettingProvider
    {
        private readonly IAssetProvider _assetProvider;

        private Dictionary<Type, BaseSettings> _settings = new Dictionary<Type, BaseSettings>();

        public SettingsProvider(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public async UniTask Initialize()
        {
            var audioConfig = await _assetProvider.Load<AudioConfig>(ConstConfigs.AudioConfig);
            var enemyConfig = await _assetProvider.Load<EnemyConfig>(ConstConfigs.EnemyConfig);
            var playerConfig = await _assetProvider.Load<PlayerConfig>(ConstConfigs.PlayerConfig);
            var gameConfig = await _assetProvider.Load<GameConfig>(ConstConfigs.GameConfig);
            var screenOrientationConfig = await _assetProvider.Load<ScreenOrientationConfig>(ConstConfigs.ScreenOrientationConfig);

            Set(audioConfig);
            Set(enemyConfig);
            Set(playerConfig);
            Set(gameConfig);
            Set(screenOrientationConfig);
        }

        public T Get<T>() where T : BaseSettings
        {
            if (_settings.ContainsKey(typeof(T)))
            {
                var setting = _settings[typeof(T)];
                return setting as T;
            }

            throw new Exception("No setting found");
        }

        public void Set(BaseSettings config)
        {
            if (_settings.ContainsKey(config.GetType()))
                return;

            _settings.Add(config.GetType(), config);
        }
    }
}