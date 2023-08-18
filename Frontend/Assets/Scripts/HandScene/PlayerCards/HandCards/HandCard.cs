using System;
using UnityEngine;
using UnityEngine.UI;

public class HandCard : MonoBehaviour
{
    [SerializeField] RawImage m_pictureBackground;
    [SerializeField] RawImage m_picture;
    [SerializeField] float m_pictureAnimDuration;
    public CardData Card { get; private set; }
    Color m_initColor;
    LTDescr m_curTween;
    public void SetCard(CardData cardData)
    {
        //m_picture.texture = cardData.Picture;
        m_initColor = m_pictureBackground.color;
        Card = cardData;
        SetTexture(cardData.Picture);
    }

    void SetTexture(Texture newPicture)
    {
        // Fade out the current texture
        Color color = Color.white;
        color.a = 0;
        m_picture.color = color;
        m_curTween = LeanTween.alpha(m_picture.rectTransform, 0.0f, 0)
            .setOnComplete(() =>
        {
            // Set the next texture on the RawImage
            m_picture.texture = newPicture;
            // Fade in the next texture
            m_curTween = LeanTween.alpha(m_picture.rectTransform, 1.0f, m_pictureAnimDuration)
            .setOnComplete(() =>
                { m_curTween = null; });
        });
    }

    internal void HideCard()
    {
        gameObject.SetActive(false);
        if (m_curTween != null)
        {
            m_curTween = null;
            LeanTween.cancel(gameObject);
            m_pictureBackground.color = m_initColor;
        }

    }

    internal void SetFlippedCard()
    {
        gameObject.SetActive(true);
        if (m_curTween == null)
            m_picture.color = Color.black;
    }

    internal void HighlightCard()
    {
        //print("add right spot VFX");

        m_curTween = LeanTween.value(m_pictureBackground.gameObject, UpdateColor, m_initColor,
                        Color.black, m_pictureAnimDuration / 2)
           .setLoopPingPong(10);

    }

    private void UpdateColor(Color color)
    {
        m_pictureBackground.color = color;
    }
}
