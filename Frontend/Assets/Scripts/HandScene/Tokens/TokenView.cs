using System;
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
    [SerializeField] RawImage m_bgImage;
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
        m_rectTransform.anchoredPosition = m_initPosition;
        LeanTween.scale(m_rectTransform, Vector3.one, m_animDuration)
            .setEase(LeanTweenType.easeOutBack)
            .setDelay(m_animDelay * m_index);
    }

    internal void PlaceToken(Vector2 spotScreenPosition, Vector2 hitRelativePosition)
    {
        Vector2 targetPos = spotScreenPosition + hitRelativePosition;
        //print(spotScreenPosition);

        LeanTween.move
            (m_rectTransform, targetPos, m_animDuration)
            .setDelay(m_animDelay * m_index)
            .setEase(LeanTweenType.easeOutCubic);
        //HighlightRightGuess();
    }

    internal void HighlightRightGuess()
    {
        // Color initColor = m_tokenImage.color;
        // LeanTween.value(m_bgImage.gameObject, UpdateColor, initColor, Color.black, m_animDuration / 2)
        //     .setLoopPingPong(5);

    }

    // private void UpdateColor(Color color)
    // {
    //     m_bgImage.color = color;
    // }


}
