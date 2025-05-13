using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Application.Services.Audio
{
    [CreateAssetMenu(fileName = "AudioConfig", menuName = "Config/AudioConfig")]
    public sealed class AudioConfig : BaseSettings
    {
        public List<AudioData> Audio;
    }
}