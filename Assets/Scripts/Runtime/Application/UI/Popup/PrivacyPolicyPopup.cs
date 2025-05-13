using Core.UI;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class PrivacyPolicyPopup : BasePopup
{
    [SerializeField] private Button _backButton;

    public event Action BackButtonPressEvent;

    public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
    {
        _backButton.onClick.RemoveAllListeners();

        _backButton.onClick.AddListener(DestroyPopup);

        return base.Show(data, cancellationToken);
    }
}