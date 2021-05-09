using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EqItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField]
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    public GameObject Slotfrom;
    public GameObject Slot;

    [SerializeField]
    public GameObject item;

    private void Awake() {
       rectTransform = GetComponent<RectTransform>();
       canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData){

        canvasGroup.blocksRaycasts = false;
        Slotfrom = Slot;
    }

    public void OnDrag(PointerEventData eventData){

        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData){
        Debug.Log("onEND");
        canvasGroup.blocksRaycasts = true;
        if(eventData.pointerCurrentRaycast.gameObject.tag != "Slot"){
            rectTransform.anchoredPosition = Slot.GetComponent<RectTransform>().anchoredPosition;
        }else{
            if(Slotfrom != Slot){
                Slotfrom.GetComponent<Eqslot>().clearslot();
            }else{
                rectTransform.anchoredPosition = Slot.GetComponent<RectTransform>().anchoredPosition;
            }
        }

    }
}
