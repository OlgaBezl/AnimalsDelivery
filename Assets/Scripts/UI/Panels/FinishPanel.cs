using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class FinishPanel : UIPanel
{
    //[SerializeField] private TryAgainButton _tryAgainButton;
    [SerializeField] private StartButton _nextLevelButton;
    [SerializeField] private AnimatedAnimal _animatedAnimal;
    [SerializeField] private RestartButton _restartButton;
    [SerializeField] private UIImagePainter _backgroundImage;

    private Star[] _stars;
    private int _fillStarsCount;

    protected override void Awake()
    {
        base.Awake();
        _stars = GetComponentsInChildren<Star>();
    }

    private void OnValidate()
    {
        if (_restartButton == null)
            throw new System.ArgumentException(nameof(_restartButton));

        if (_animatedAnimal == null)
            throw new System.ArgumentException(nameof(_animatedAnimal));
    }

    public override void Open()
    {
        Open(_fillStarsCount);
    }

    public void Open(int fillStarsCount)
    {
        _fillStarsCount = fillStarsCount;
        _backgroundImage.SetActualTone(_fillStarsCount > 1);
        _animatedAnimal.Activate();

        IsOpened += Load;
        base.Open();
    }

    public void Load()
    {
        IsOpened -= Load;

        for (int i = 0; i < _fillStarsCount; i++) 
        {
            _stars[i].Fill();
        }

        _animatedAnimal.Show(_fillStarsCount > 1);
    }

    public override void Hide()
    {
        IsHidden += FinalHide;
        base.Hide();
    }

    private void FinalHide()
    {
        IsHidden -= FinalHide;

        _animatedAnimal.Hide();

        foreach (Star star in _stars)
        {
            star.Clear();
        }
    }
}
