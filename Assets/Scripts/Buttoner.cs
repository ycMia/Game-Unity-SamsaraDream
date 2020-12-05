using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Buttoner : MonoBehaviour
{
    public bool pressed;

    void Awake()
    {
        AddTriggersListener(gameObject, EventTriggerType.PointerDown, MPress);
        AddTriggersListener(gameObject, EventTriggerType.PointerUp, MUnPress);
    }

    public void MPress(BaseEventData bEData)
    {
        pressed = true;
        //Debug.Log("MyOnPressed Called.");
    }

    public void MUnPress(BaseEventData bEData)
    {
        pressed = false;
        //Debug.Log("MyLeavingButton Called.");
    }

    private void AddTriggersListener(GameObject obj, EventTriggerType eventID, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = obj.AddComponent<EventTrigger>();
        }

        if (trigger.triggers.Count == 0)
        {
            trigger.triggers = new List<EventTrigger.Entry>();
        }

        UnityAction<BaseEventData> callback = new UnityAction<BaseEventData>(action);
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventID;
        entry.callback.AddListener(callback);

        trigger.triggers.Add(entry);
    }
}