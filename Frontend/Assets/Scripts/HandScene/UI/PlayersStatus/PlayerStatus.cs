using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] TMP_Text m_nameText;
    [SerializeField] RawImage m_avatarImage;
    [SerializeField] RawImage m_actionImage;
    [SerializeField] Texture m_tookActionTexture;
    [SerializeField] Texture m_didentTookActionTexture;
    internal void Init(PlayerData playerData)
    {
        m_nameText.text = playerData.Name;
        m_nameText.color = playerData.Color;
        m_avatarImage.texture = playerData.Avatar;
        m_actionImage.texture = m_didentTookActionTexture;
    }

    internal void InitStatus()
    {
        m_actionImage.texture = m_didentTookActionTexture;
    }

    internal void PlayerTookAction()
    {
        m_actionImage.texture = m_tookActionTexture;
    }
}
