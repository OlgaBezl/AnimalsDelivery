using DG.Tweening;
using System;
using UnityEngine;

public class UIPanel : MonoBehaviour
{
    [SerializeField] private GameObject _background;
    [SerializeField] private float _jumpHeight = 200f;
    [SerializeField] private float _jumpDuration = 0.25f;
    [SerializeField] private float _verticalOffset = 800f; 
    [SerializeField] private float _verticalMoveDuration = 0.75f;
    [SerializeField] private ChangePanelButtom _closeButtom;

    private RectTransform _rectTransform;

    public event Action IsHidden;
    public event Action IsOpened;

    protected virtual void Awake()
    {
        _rectTransform = (RectTransform)transform;
    }

    public void SetReturnButton(UIPanel returnPanel)
    {
        if (returnPanel == null || _closeButtom == null)
            return;

        _closeButtom.ChangeSecondPanel(returnPanel);
    }

    public virtual void Hide()
    {
        Debug.Log("Hide OnClick ");

        DOTween.Sequence().
            Append(_rectTransform.DOAnchorPosY(_jumpHeight, _jumpDuration)).
            Append(_rectTransform.DOAnchorPosY(-_verticalOffset, _verticalMoveDuration)).SetUpdate(true).
            onComplete += () =>
            {
                Debug.Log("Hide onComplete ");
                _background.gameObject.SetActive(false);
                gameObject.SetActive(false);
                IsHidden?.Invoke();
                Debug.Log("IsHidden null " + IsHidden == null);
            };
    }

    public virtual void Open()
    {
        _background.gameObject.SetActive(true);
        gameObject.SetActive(true);
        
        DOTween.Sequence().
            Append(_rectTransform.DOAnchorPosY(-_verticalOffset, 0f)).
            Append(_rectTransform.DOAnchorPosY(_jumpHeight, _verticalMoveDuration)).
            Append(_rectTransform.DOAnchorPosY(0f, _jumpDuration)).SetUpdate(true).
            onComplete += () => IsOpened?.Invoke();
    }
}
