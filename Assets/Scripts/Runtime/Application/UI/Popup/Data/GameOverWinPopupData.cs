using Core.UI;

namespace Application.UI
{
    public class GameOverWinPopupData : BasePopupData
    {
        private int _keysCount;

        public int KeysCount => _keysCount;

        public GameOverWinPopupData(int keysCount)
        {
            _keysCount = keysCount;
        }
    }
}