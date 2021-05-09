using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Eqslot : MonoBehaviour, IDropHandler
{
    [SerializeField]
    public bool isfree;

    public GameObject item;

    private void Start() {
        isfree = true;
    }

    public void OnDrop(PointerEventData eventData){
        Debug.Log(isfree);
        if(isfree){
            //setingitem()
            Debug.Log("dropped");
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            eventData.pointerDrag.GetComponent<EqItem>().Slot = this.gameObject;
            item = eventData.pointerDrag.GetComponent<EqItem>().Slot;
            isfree = false;
        }
    }

    public void setingitem(){
        //wad
    }

    public void clearslot(){
        isfree = true;
        item = null;
    }
}
