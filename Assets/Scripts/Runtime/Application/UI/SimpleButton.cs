using Application.Services.Audio;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Application.UI
{
    public class SimpleButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private int _level_id;
        [SerializeField] private Animation _pressAnimation;

        private IAudioService _audioService;

        public int Level_id => _level_id;
        public Button Button => _button;

        [Inject]
        public void Construct(IAudioService audioService)
        {
            _audioService = audioService;
        }

        public void PlayPressAnimation()
        {
            _pressAnimation.Play();
            _audioService.PlaySound(ConstAudio.PressButtonSound);
        }
    }
}