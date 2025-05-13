using Application.UI;
using Core.UI;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionPopup : BasePopup
{
    [SerializeField] private Button _backButton;
    [SerializeField] private List<SimpleButton> _levelButtons;

    public event Action<int> LevelButtonPressEvent;

    public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
    {
        _backButton.onClick.RemoveAllListeners();
        _backButton.onClick.AddListener(DestroyPopup);

        foreach (var levelButton in _levelButtons)
        {
            levelButton.Button.onClick.AddListener(() => OnLevelButtonPress(levelButton.Level_id));
        }

        return base.Show(data, cancellationToken);
    }

    private void OnLevelButtonPress(int level_id)
    {
        LevelButtonPressEvent?.Invoke(level_id);
        DestroyPopup();
    }
}