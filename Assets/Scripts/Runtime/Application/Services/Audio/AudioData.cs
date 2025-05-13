using System;
using UnityEngine;

namespace Application.Services.Audio
{
    [Serializable]
    public class AudioData
    {
        public string Id;
        public AudioType AudioType;
        public AudioClip Clip;
    }

    public enum AudioType
    {
        Music,
        Sound
    }
}