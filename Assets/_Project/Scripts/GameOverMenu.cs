using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;

/// <summary>
/// Menu that appears when the player loses. Has an example use of DOTween!
/// </summary>
[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasGroup))]
public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private RectTransform panel;
    
    private Canvas _canvas;
    private CanvasGroup _group;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _group = GetComponent<CanvasGroup>();
    }

    public void Show()
    {
        _canvas.enabled = true;
        _group.alpha = 0f;
        _group.DOKill();
        _group.DOFade(1f, 0.5f);
        panel.DOKill();
        panel.DOLocalMoveY(-300f, 0.5f).From().SetEase(Ease.OutBack);
    }

    [UsedImplicitly]
    public void OnRestartButtonClicked()
    {
        GameManager.Instance.RestartGame();
    }
}
