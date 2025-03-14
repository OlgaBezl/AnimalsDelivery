using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class UIImagePainter : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private BiomPainter _biomPainter;

    private void OnValidate()
    {
        if (_image == null)
            throw new NullReferenceException(nameof(_image));
    }

    public void SetActualTone(bool nextLevel)
    {
        LevelInfo lastLevel = YandexGame.savesData.Levels.FirstOrDefault(lvl => lvl.LevelNumber + (nextLevel ? 1 : 0) == YandexGame.savesData.CurrentLevel.LevelNumber);
        Color tone = _biomPainter.GetTone(lastLevel.Biom);
        tone.a = 1f;
        _image.color = tone;
    }
}
