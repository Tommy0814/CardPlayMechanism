using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{

    public Draggable.Slot typeOfItem = Draggable.Slot.INVENTORY;

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("OnPointerEnter");
        if (eventData.pointerDrag == null)
            return;

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null)
        {
            d.placeholderParent = this.transform;
        }

    }
    
    public void OnPointerExit(PointerEventData eventData)
    {

        if (eventData.pointerDrag == null)
            return;

        //Debug.Log("OnPointerExit");
        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null && d.placeholderParent == this.transform)
        {
            d.placeholderParent = d.parentToReturnTo;
        }

    }
    
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag.name + " was dropped in " + gameObject.name);

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null)
        {
            // Drop card of correct type, drop all cards of type inventory
            // The later half is to allow all cards acceptable
            // Delete the later half otherwise all cards will be acceptable
            if(typeOfItem == d.typeOfItem || typeOfItem == Draggable.Slot.INVENTORY)
                d.parentToReturnTo = this.transform;
        }
    }
    
}
