using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Toggle : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    [SerializeField]
	private Player PlayerObject;

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Button Down");
        PlayerObject.IsZooming = true;

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Pointer Up");
        PlayerObject.IsZooming = false;
    }
}
