using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform parentToReturnTo = null;

    public Transform placeholderParent = null;

    GameObject placeholder = null;
    
    public enum Slot
    {
        WEAPON, HEAD, CHEST, LEGS, FEET, INVENTORY
    }

    public Slot typeOfItem = Slot.INVENTORY;
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin Drag");

        // Set Up Placeholder To Allow A Card To Be Added In Between Two Cards Instead Of Only Add To The Tail
        placeholder = new GameObject();
        placeholder.transform.SetParent( this.transform.parent );
        LayoutElement le = placeholder.AddComponent<LayoutElement>();  // add a layout element and store it in le
        le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
        le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
        le.flexibleWidth = 0;
        le.flexibleHeight = 0;

        placeholder.transform.SetSiblingIndex( this.transform.GetSiblingIndex() ); 
        // so the placeholder is holding the original slot while on drag, other cards won't fill out the orginal slot, it will keep empty on drag
        

        parentToReturnTo = this.transform.parent;
        placeholderParent = parentToReturnTo;
        this.transform.SetParent ( this.transform.parent.parent );

        GetComponent<CanvasGroup>().blocksRaycasts = false;

       /*
       // Find all the available dropzones
        DropZone[] zones = GameObject.FindObjectsOfType<DropZone>();
 
        for (int i = 0; i < zones.Length; i++)
        {
            Debug.Log(zones[i]);
        } 
        */
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("On Drag");
        this.transform.position = eventData.position;

        if (placeholder.transform.parent != placeholderParent)
        {

            placeholder.transform.SetParent(placeholderParent);

        }


        int newSiblingIndex = placeholderParent.childCount;

        for(int i = 0; i < placeholderParent.childCount; i++)
        {
            
            if (this.transform.position.x < placeholderParent.GetChild(i).position.x)
            {
                
                newSiblingIndex = i;

                if(placeholder.transform.GetSiblingIndex() < newSiblingIndex)
                    newSiblingIndex--;

                break;

            }

        }

        placeholder.transform.SetSiblingIndex(newSiblingIndex);

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End Drag");
        this.transform.SetParent(parentToReturnTo);
        this.transform.SetSiblingIndex( placeholder.transform.GetSiblingIndex());  // so that the card is put back to the original slot
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        // Instead of letting drop zones check what is just dropped on them
        // this following code let draggable objects check what is beneath it
        //EventSystem.current.RaycastAll(eventData, );

        Destroy(placeholder);
    }

    
}
