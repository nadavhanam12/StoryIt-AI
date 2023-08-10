using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TokenView : MonoBehaviour
{
    [SerializeField] RectTransform m_rectTransform;
    [SerializeField] TMP_Text m_nameText;
    [SerializeField] RawImage m_tokenImage;
    [SerializeField] float m_animDelay;
    float m_animDuration;
    Vector3 m_initPosition;
    int m_index;
    internal void Init(int index, PlayerData playerData, float animDuration)
    {
        m_index = index;
        m_animDuration = animDuration;
        m_initPosition = m_rectTransform.anchoredPosition;
        m_nameText.text = playerData.Name;
        m_nameText.color = playerData.Color;
        m_tokenImage.texture = playerData.Avatar;

        Color color = m_tokenImage.color;
        color.a = 0.5f;
        m_tokenImage.color = color;
    }

    internal void ToggleTokenPlayed(bool isPlayed)
    {
        float targetAlpha = isPlayed ? 1f : 0.5f;
        LeanTween.alpha(m_tokenImage.rectTransform, targetAlpha, m_animDuration);
        LeanTween.alpha(m_nameText.rectTransform, targetAlpha, m_animDuration);
    }

    internal void TurnOff()
    {
        LeanTween.scale(m_rectTransform, Vector3.zero, m_animDuration);
    }
    internal void TurnOn()
    {
        LeanTween.scale(m_rectTransform, Vector3.one, m_animDuration)
            .setEase(LeanTweenType.easeOutBack)
            .setDelay(m_animDelay * m_index);
    }
}
