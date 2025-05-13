using System.Threading;
using Core.UI;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Application.UI
{
    public class MessagePopup : BasePopup
    {
        [SerializeField] private Button _okButton;
        [SerializeField] private TextMeshProUGUI _message;

        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            _okButton.gameObject.SetActive(true);
            _okButton.onClick.AddListener(Hide);

            return base.Show(data, cancellationToken);
        }
    }
}