using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capsule : MonoBehaviour
{
    UIEventManager uiMgr;
    int x = 10;

    public GameObject obj;

    public void start()
    {
        //uiMgr = GameManager.GetInstance().UiMgr;
        uiMgr = UIEventManager.Instance();
        uiMgr.AddEvent(UIEventManager.EventType.PickChar, OnPickCharacter);
    }

    public void OnPickCharacter(object player, object playerNum)
    {
        x = 20;
        Debug.Log("   데이터 : " + playerNum);
        x += 20;
    }

    public void Host()
    {
        //uiMgr.PostEvent(UIEventManager.EventType.NextTurn, gameObject, obj);
    }
}
