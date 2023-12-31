using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSpotView : MonoBehaviour
{
    [SerializeField] Vector2 m_initPosition;
    [SerializeField] float m_initRotation;
    [SerializeField] Vector2 m_cardsSelectionPosition;
    [SerializeField] Vector2 m_cardsChoosenPosition;
    float m_switchStateDuration;
    RectTransform m_rectTransform;
    Vector2 m_deckPosition;

    public void Init(float switchStateDuration, Vector2 deckPosition)
    {
        m_switchStateDuration = switchStateDuration;
        m_deckPosition = deckPosition;

        m_rectTransform = GetComponent<RectTransform>();
        m_rectTransform.anchoredPosition = m_initPosition;
        m_rectTransform.eulerAngles = new Vector3(0f, 0f, m_initRotation);

    }

    internal void DrawNewCard()
    {
        m_rectTransform.localScale = Vector3.one;
        m_rectTransform.eulerAngles = Vector3.zero;
        m_rectTransform.anchoredPosition = m_deckPosition;
    }

    internal void ApplyIdleView()
    {
        LeanTween.rotateZ
            (m_rectTransform.gameObject, m_initRotation, m_switchStateDuration);

        LeanTween.move
            (m_rectTransform, m_initPosition, m_switchStateDuration);

        LeanTween.scale
            (m_rectTransform, Vector3.one, m_switchStateDuration);
    }
    internal void ApplyCardsSelectionView()
    {
        LeanTween.rotateZ
            (m_rectTransform.gameObject, 0, m_switchStateDuration);

        LeanTween.move
            (m_rectTransform, m_cardsSelectionPosition, m_switchStateDuration);
        LeanTween.scale
            (m_rectTransform, Vector3.one, m_switchStateDuration);

    }

    internal void ApplyCardChoosenView(bool isChoosen)
    {
        if (isChoosen)
        {
            LeanTween.move
                (m_rectTransform, m_cardsChoosenPosition, m_switchStateDuration);
            LeanTween.scale
                (m_rectTransform, Vector3.one * 2, m_switchStateDuration);
        }
        else
        {
            LeanTween.move
                (m_rectTransform, new Vector3(0, -1500, 0), m_switchStateDuration);
        }
    }
}
