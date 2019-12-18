using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUIInfo : MonoBehaviour
{
    UIEventManager uiEvnMgr;
    int idx;
    GameObject[] charInfo;
    GameObject[] charName;
    GameObject[] charHP;
    GameObject[] charVigor;


    public void start()
    {
        uiEvnMgr = UIEventManager.Instance();
        uiEvnMgr.AddEvent(UIEventManager.EventType.NextTurn, OnNextTurn);
        charInfo = new GameObject[4];
        charName = new GameObject[4];
        charHP = new GameObject[4];
        charVigor = new GameObject[4];
    }

    public void Init()
    {
        
    }

    public void OnNextTurn(object prevChar, object nextChar)
    {
        idx = (prevChar as GameObject).GetComponent<CharacterInfo>().index;
        charName[idx].SetActive(false);
        charInfo[idx].GetComponent<Image>().color = new Color(50, 50, 50, 255);

        idx = (nextChar as GameObject).GetComponent<CharacterInfo>().index;
        charName[idx].SetActive(false);
        charInfo[idx].GetComponent<Image>().color = Color.white;
    }

}
