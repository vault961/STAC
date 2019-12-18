using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    UIEventManager uiEvnMgr;
    GameObject destnationImg;                       //목적지 하이라이팅 이미지
    GameObject moveBtn;
    GameObject dashBtn;
    GameObject acceptBtn;
    GameObject cancelBtn;
    GameObject explainView;

    public void start()
    {
        uiEvnMgr = UIEventManager.Instance();
        uiEvnMgr.AddEvent(UIEventManager.EventType.MoveChar, OnActionCharacter);
    }

    public void OnActionCharacter(object player, object data)
    {
        //destnationImg.SetActive(false);
        Debug.Log(player);

        
    }

    

    #region 버튼 콜백함수
    public void OnClickPauseBtn()
    {

    }

    public void OnClickCloseBtn()
    {

    }

    public void OnClickRecordingBtn()
    {

    }

    public void OnClickLPassBtn()
    {

    }

    public void OnClickRPassBtn()
    {

    }

    public void OnClickAimingBtn()
    {

    }

    public void OnClickLookAroundBtn()
    {

    }

    public void OnClickGrenadeBtn()
    {

    }

    public void OnClickDefenceBtn()
    {

    }

    public void OnClickRifleBtn()
    {

    }

    public void OnClickPistolBtn()
    {

    }
    #endregion
}
