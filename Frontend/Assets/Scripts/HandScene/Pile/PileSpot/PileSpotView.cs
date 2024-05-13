using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PileSpotView : MonoBehaviour
{
    [SerializeField] Vector2 m_initPosition;
    [SerializeField] float m_initRotation;
    [SerializeField] Vector2 m_cardsSelectionPosition;
    [SerializeField] Vector2 m_cardsChoosenPosition;
    [SerializeField] float m_cardsChoosenScaleUp = 1.75f;
    [SerializeField] LeanTweenType m_tweenType;


    float m_switchStateDuration;
    RectTransform m_rectTransform;

    public void Init(float switchStateDuration)
    {
        m_switchStateDuration = switchStateDuration;
        m_rectTransform = GetComponent<RectTransform>();
        InitPos();
    }

    internal void InitPos()
    {
        m_rectTransform.anchoredPosition = m_initPosition;
        m_rectTransform.eulerAngles = new Vector3(0f, 0f, m_initRotation);
    }
    internal void ApplyIdleView()
    {
        LeanTween.rotateZ
            (m_rectTransform.gameObject, m_initRotation, m_switchStateDuration)
                    .setEase(m_tweenType);

        LeanTween.move
            (m_rectTransform, m_initPosition, m_switchStateDuration)
                    .setEase(m_tweenType);

        LeanTween.scale
            (m_rectTransform, Vector3.one, m_switchStateDuration)
                    .setEase(m_tweenType);
    }

    internal void ApplyCardsSelectionView()
    {
        LeanTween.rotateZ
            (m_rectTransform.gameObject, 0, m_switchStateDuration)
                    .setEase(m_tweenType);

        LeanTween.move
            (m_rectTransform, m_cardsSelectionPosition, m_switchStateDuration)
                    .setEase(m_tweenType);
        LeanTween.scale
            (m_rectTransform, Vector3.one, m_switchStateDuration)
                    .setEase(m_tweenType);

    }

    internal void ApplyCardChoosenView(bool isChoosen)
    {
        if (isChoosen)
        {
            LeanTween.move
                (m_rectTransform, m_cardsChoosenPosition, m_switchStateDuration)
                    .setEase(m_tweenType);
            LeanTween.scale
                (m_rectTransform, Vector3.one * m_cardsChoosenScaleUp, m_switchStateDuration)
                    .setEase(m_tweenType);
        }
        else
        {
            LeanTween.move
                (m_rectTransform, new Vector3(0, -1500, 0), m_switchStateDuration)
                    .setEase(m_tweenType);
        }
    }

    internal void PileCard()
    {
        // pilling the card animation

    }


}
