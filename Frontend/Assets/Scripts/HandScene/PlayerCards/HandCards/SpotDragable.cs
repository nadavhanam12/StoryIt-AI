

using UnityEngine;
using UnityEngine.EventSystems;

public class SpotDragable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    float m_dragPlayLimit = 400;
    [SerializeField]
    float m_maxDeltaY = 1000;
    HandSpot m_handSpot;
    Vector2 m_touchStartPos;
    Vector2 m_spotStartPos;
    Vector2 m_spotCurPos;
    RectTransform m_rectTransform;

    public void Init(HandSpot handSpot)
    {
        m_handSpot = handSpot;
        m_rectTransform = GetComponent<RectTransform>();
        TurnOff();
    }
    public void TurnOn()
    {
        this.enabled = true;
    }
    public void TurnOff()
    {
        this.enabled = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_touchStartPos = eventData.position;
        if (m_spotStartPos == Vector2.zero)
            m_spotStartPos = m_rectTransform.position;
        m_spotCurPos = m_spotStartPos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (m_touchStartPos == Vector2.zero)
            return;
        m_spotCurPos.y = eventData.position.y;
        //print(m_spotCurPos);
        //print(m_spotStartPos);

        UpdateSpotLocation();
    }

    void UpdateSpotLocation()
    {
        //print(m_spotCurPos.y);
        if (m_spotCurPos.y > m_spotStartPos.y + m_maxDeltaY)
            return;

        if (m_spotCurPos.y > m_spotStartPos.y)
        {
            m_rectTransform.position = m_spotCurPos;
        }
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        if (IsPassPlayDragLimit())
        {
            m_handSpot.SpotClicked();
            TurnOff();
        }
        else
        {
            //print("init drag");
            m_rectTransform.position = m_spotStartPos;
            m_spotStartPos = Vector2.zero;
            m_touchStartPos = Vector2.zero;
        }
    }
    bool IsPassPlayDragLimit()
    {
        return (m_spotCurPos.y - m_spotStartPos.y
                 >= m_dragPlayLimit);

    }


}
