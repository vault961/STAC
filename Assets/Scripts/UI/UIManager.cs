using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    UIEventManager uiEvnMgr;
    GameObject highlightImg;                    //선택 케릭터 하이라이팅
    Transform effectPool;

    public void start()
    {
        uiEvnMgr = UIEventManager.Instance();
        uiEvnMgr.AddEvent(UIEventManager.EventType.NextTurn, OnNextTurn);
        effectPool = new GameObject("EffectPool").transform;
        effectPool.position = new Vector3(0, 50000, 0);
    }

    public void OnNextTurn(object prevChar, object nextChar)
    {
        if (nextChar == null)
            highlightImg.SetActive(false);
        else
            highlightImg.transform.SetParent(nextChar as Transform);
    }

    public void CreateEffect()
    {
        highlightImg = Resources.Load<GameObject>("highlightObj");
        highlightImg.transform.SetParent(effectPool);
        highlightImg.SetActive(false);
    }
}
