using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SliderController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent<bool> onPointEvent;
    public void OnPointerDown(PointerEventData eventData)
    {
        onPointEvent.Invoke(true);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        onPointEvent.Invoke(false);
    }
}
