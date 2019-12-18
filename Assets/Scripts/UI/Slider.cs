using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : MonoBehaviour
{
    UIEventManager uiMgr;

    public void start()
    {
        //uiMgr = GameManager.GetInstance().UiMgr;
        uiMgr = UIEventManager.Instance();
        uiMgr.AddEvent(UIEventManager.EventType.PickChar, OnPickCharacter);
    }

    public void OnPickCharacter(object player, object playerNum)
    {
        uiMgr.PostEvent(UIEventManager.EventType.MoveChar, this.gameObject, 3);
    }

    public void RemoveEvent()
    {
        uiMgr.RemoveEvent(UIEventManager.EventType.PickChar, OnPickCharacter);
    }
}
