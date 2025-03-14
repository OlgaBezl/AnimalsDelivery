using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPainter : MonoBehaviour
{
    [SerializeField] private List<MeshRenderer> _meshRenderers;

    public const float HorizontalColorTextureOffset = 0.1666f;

    [field: SerializeField] public bool IsMainMaterial { get; private set; }
    public int ColorIndex { get; private set; }

    private void OnValidate()
    {
        if (_meshRenderers == null || _meshRenderers.Count == 0)
            throw new NullReferenceException(nameof(_meshRenderers));
    }

    public void RandomPaint(Texture biomTexture, int? previousIndex)
    {
        foreach (MeshRenderer mesh in _meshRenderers)
        {
            mesh.material.mainTexture = biomTexture;
        }

        ColorIndex = IsMainMaterial ? ColorPallet.GetRandomColorIndex() : GetRandomPaleColorIndex(previousIndex);
        MoveTextureByIndex(ColorIndex, ColorPallet.GrayIndex);
    }

    public void PaintByIndex(int colorIndex, int currentColorIndex)
    {
        foreach (MeshRenderer mesh in _meshRenderers)
        {
            if (mesh.material == null)
                mesh.material = new Material(mesh.material);
        }

        MoveTextureByIndex(colorIndex, currentColorIndex);
    }

    public void SetShaderMaterialTexture(Texture texture)
    {
        foreach (MeshRenderer mesh in _meshRenderers)
        {
            mesh.sharedMaterial.mainTexture = texture;
        }
    }

    public void SetTone(Color tone)
    {
        foreach (MeshRenderer renderer in _meshRenderers)
        {
            renderer.material.color = tone;
        }
    }

    public void MoveTextureByOffset( float horizontalTextureOffset)
    {
        foreach (MeshRenderer mesh in _meshRenderers)
        {
            if (mesh.material == null)
                mesh.material = new Material(mesh.material);

            mesh.material.mainTextureOffset = new Vector2(horizontalTextureOffset, 0);
        }
    }

    public void HighlightOn()
    {
        float duration = 0.75f;
        float minLumin = 0.5f;
        float maxLumin = 1f;
        string propertyName = "_MinLight";

        foreach (MeshRenderer mesh in _meshRenderers)
        {
            if (mesh.material == null)
                mesh.material = new Material(mesh.material);

            DOTween.Sequence().
            Append(mesh.material.DOFloat(maxLumin, propertyName, duration)).
            Append(mesh.material.DOFloat(minLumin, propertyName, duration)).
            Append(mesh.material.DOFloat(maxLumin, propertyName, duration)).
            Append(mesh.material.DOFloat(minLumin, propertyName, duration)).
            Append(mesh.material.DOFloat(maxLumin, propertyName, duration)).
            Append(mesh.material.DOFloat(minLumin, propertyName, duration)).
            //SetEase(Ease.Linear).
            onComplete += () =>
            {
                Debug.Log("duration " + duration);
            };

        }
    }

    private void MoveTextureByIndex(int colorIndex, int currentColorIndex)
    {
        if (currentColorIndex != colorIndex)
        {
            currentColorIndex = colorIndex;

            foreach (MeshRenderer mesh in _meshRenderers)
            {
                mesh.material.mainTextureOffset = new Vector2(HorizontalColorTextureOffset * colorIndex, 0);
            }
        }
    }

    private int GetRandomPaleColorIndex(int? previousIndex)
    {
        int index = UnityEngine.Random.Range(0, ColorPallet.ColorsCountWithoutGray + 1);

        if (previousIndex.HasValue)
        {
            while (index == previousIndex.Value)
                index = UnityEngine.Random.Range(0, ColorPallet.ColorsCountWithoutGray + 1);
        }

        return index;
    }
}
