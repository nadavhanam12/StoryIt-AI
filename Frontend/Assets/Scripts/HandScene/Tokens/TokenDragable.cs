

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TokenDragable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{

    TokenController m_tokenController;
    Vector2 m_initPos;
    RectTransform m_rectTransform;
    public void Init(TokenController tokenController)
    {
        m_tokenController = tokenController;
        m_rectTransform = GetComponent<RectTransform>();
        m_initPos = m_rectTransform.anchoredPosition;
        TurnOff();
    }
    public void TurnOn()
    {
        enabled = true;
    }
    public void TurnOff()
    {
        enabled = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

    }

    public void OnDrag(PointerEventData eventData)
    {
        m_rectTransform.position = eventData.position;
        // print("position: " + eventData.position);
        // print("pressPosition: " + eventData.pressPosition);
        // print("pointerCurrentRaycast.worldPosition: " + eventData.pointerCurrentRaycast.worldPosition);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //print("OnEndDrag");
        PileSpot pileSpot = GetPileSpotHit(eventData);

        if (pileSpot != null)
        {
            int cardId = pileSpot.GetCardId();
            Vector2 relativeHitPosition = pileSpot.GetLocalHitPos(eventData);
            m_tokenController.TokenDropped(cardId, relativeHitPosition);
            TurnOff();
        }
        else
        {
            //print("init drag");
            m_rectTransform.anchoredPosition = m_initPos;
        }
    }
    PileSpot GetPileSpotHit(PointerEventData eventData)
    {
        // Perform a raycast for UI elements using EventSystem
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = eventData.position;

        // Create a list to store the raycast results
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);

        // Loop through the results to find the UI objects
        foreach (RaycastResult result in results)
        {
            // Check if the hit object is a UI object
            GameObject hitObject = result.gameObject;
            PileSpot pileSpot = hitObject.GetComponent<PileSpot>();
            if (pileSpot != null)
                return pileSpot;
        }
        return null;
    }
}
