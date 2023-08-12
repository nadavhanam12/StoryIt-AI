using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using System;

public class SpotDetailsView : MonoBehaviour
{
    [SerializeField] RawImage m_avatarImage;
    [SerializeField] TMP_Text m_indexText;
    float m_switchStateDuration;
    public void Init(int index, float switchStateDuration)
    {
        ToggleAvatar(false);
        m_indexText.gameObject.SetActive(false);

        m_indexText.text = (1 + index).ToString();
        m_switchStateDuration = switchStateDuration;
    }
    internal void SetAvater(Texture avatarTexture)
    {
        m_avatarImage.texture = avatarTexture;
        Color curColor = Color.white;
        curColor.a = 0;
        m_avatarImage.color = curColor;
    }
    public void ToggleAvatar(bool showDetails)
    {
        m_avatarImage.gameObject.SetActive(showDetails);
        float tagetAlpha = 0.0f;
        if (showDetails)
            tagetAlpha = 1.0f;
        LeanTween.alpha(m_avatarImage.rectTransform, tagetAlpha, m_switchStateDuration);
    }
    public async void ShowIndex()
    {
        await Task.Delay((int)(m_switchStateDuration * 1000));
        m_indexText.gameObject.SetActive(true);
    }

}
