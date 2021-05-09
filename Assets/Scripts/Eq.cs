using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq : MonoBehaviour
{
    public static Eq instance;

    [SerializeField]
    GameObject[] Slots;

    [SerializeField]
    GameObject[] equiped;

    [SerializeField]
    GameObject item1;

    [SerializeField]
    GameObject item2;

    [SerializeField]
    GameObject item3;

    [SerializeField]
    GameObject item4;


    [SerializeField]
    GameObject itemParent;

    void Awake(){
        if(instance != null){
            Debug.LogError("Error eq");
        }else{
            instance = this;
        }
    }

    public void add_item1(){
        foreach(GameObject slot in Slots){
            if(slot.GetComponent<Eqslot>().isfree){
                GameObject isntnced = Instantiate(item1);
                isntnced.GetComponent<RectTransform>().anchoredPosition = slot.GetComponent<RectTransform>().anchoredPosition;
                isntnced.transform.SetParent(itemParent.transform, false);
                isntnced.GetComponent<EqItem>().Slot = slot;
                isntnced.GetComponent<EqItem>().item = null;

                slot.GetComponent<Eqslot>().isfree = false;
                break;
            }
        }
    }

    public void add_item2(){
        foreach(GameObject slot in Slots){
            if(slot.GetComponent<Eqslot>().isfree){
                GameObject isntnced = Instantiate(item2);
                isntnced.GetComponent<RectTransform>().anchoredPosition = slot.GetComponent<RectTransform>().anchoredPosition;
                isntnced.transform.SetParent(itemParent.transform, false);
                isntnced.GetComponent<EqItem>().Slot = slot;
                isntnced.GetComponent<EqItem>().item = null;

                slot.GetComponent<Eqslot>().isfree = false;
                break;
            }
        }
    }

    public void add_item3(){
        foreach(GameObject slot in Slots){
            if(slot.GetComponent<Eqslot>().isfree){
                GameObject isntnced = Instantiate(item3);
                isntnced.GetComponent<RectTransform>().anchoredPosition = slot.GetComponent<RectTransform>().anchoredPosition;
                isntnced.transform.SetParent(itemParent.transform, false);
                isntnced.GetComponent<EqItem>().Slot = slot;
                isntnced.GetComponent<EqItem>().item = null;

                slot.GetComponent<Eqslot>().isfree = false;
                break;
            }
        }
    }

    public void add_item4(){
        foreach(GameObject slot in Slots){
            if(slot.GetComponent<Eqslot>().isfree){
                GameObject isntnced = Instantiate(item4);
                isntnced.GetComponent<RectTransform>().anchoredPosition = slot.GetComponent<RectTransform>().anchoredPosition;
                isntnced.transform.SetParent(itemParent.transform, false);
                isntnced.GetComponent<EqItem>().Slot = slot;
                isntnced.GetComponent<EqItem>().item = null;

                slot.GetComponent<Eqslot>().isfree = false;
                break;
            }
        }
    }

}
