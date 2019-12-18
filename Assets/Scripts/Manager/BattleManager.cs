using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//김도아
public class BattleManager : MonoBehaviour
{
    #region 싱글턴
    private static BattleManager instance = null;
    public static void CreateSingleton(GameManager GameManager)
    {
        if (!instance)
        {
            GameObject container = new GameObject();
            container.name = "BattleManager";   // "CameraManager"라고 적혀 있던 걸 "BattleManager"로 수정 - by 영균
            instance = container.AddComponent<BattleManager>();
            GameManager.Battle = instance;
        }
        else
        {
            Debug.Log("BattleManager가 이미 존재합니다");
        }
    }
    #endregion

    private GameManager MGR;
    //캐릭터 메인클래스로 수정, 게임매니저에 있어야하지않을까? 
    private List<Actor> humanTeam = new List<Actor>();
    private List<Actor> alienTeam = new List<Actor>();

    private GameManager.Team currentTurn;
    private Actor activeCharacter = null;
    private int currentHitRate;

    public void start()
    {
        MGR = GameManager.GetInstance();
        currentTurn = GameManager.Team.Human;

        BindEvents();
    }

    public void update()
    {
        
    }

    public void CleanUp()
    {
        //EventManager.GetInstance().DeleteEvent(this);
        MGR.Event.DeleteEvent(this); // GameManager 통해서 EventManager 접근하도록 수정 - by 영균
    }

    private void BindEvents()
    {
        MGR.Event.BindEvent(this, EventID.SelectCharacter);
        MGR.Event.BindEvent(this, EventID.SelectTarget);
        MGR.Event.BindEvent(this, EventID.Attack);
    }

    [Event(EventID.SelectCharacter)]
    private void OnSelectCharacter(Actor character)
    {
        if (currentTurn != character.team)
            return;

        activeCharacter = character;
    }

    [Event(EventID.SelectTarget)]
    private void OnSelectTarget(Actor target)//인덱스? 
    {
        //명중률 계산
    }

    [Event(EventID.Attack)]
    private void OnAttack(Actor target)
    {
        //명중률 적용 후 공격
    }

    private int GetHitRate(Actor target)
    {
        currentHitRate = activeCharacter.hitRate;

        Vector3 startPosition = activeCharacter.transform.position;
        Vector3 finishPosition = target.transform.position; // 레이쏠 위치 받을 위치 필요

        Vector3 direction = finishPosition - startPosition;
        float distance = Vector3.Distance(startPosition, finishPosition);

        RaycastHit[] obstacles = Physics.RaycastAll(startPosition, direction, distance, 3);  //layer 체크해야함

        for (int i = 0; i < obstacles.Length; ++i) 
        {
            if (obstacles[i].transform.gameObject.layer == 1)
                currentHitRate -= 10;
            else if (obstacles[i].transform.gameObject.layer == 2)
                currentHitRate -= 20;
            //완방 반방 폭발물 등등 체크
            //폭발물체크는 콜라이더로? 게임오브젝트 선택해놓고 공격시에 체크? 
        }

        return currentHitRate;
    }


}
