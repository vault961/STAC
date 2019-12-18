using System.Collections.Generic;
using UnityEngine;

public class UIEventManager : MonoBehaviour
{
    //#region 싱글톤 생성
    //private static UIEventManager instance = null;
    //public static void CreateSingleTon(GameManager gameMgr)
    //{
    //    if (instance == null) instance = new GameObject().AddComponent<UIEventManager>();
    //    else gameMgr.UiMgr = instance;

    //    instance.name = "UiEventMgr";
    //}
    //#endregion

    static UIEventManager instance = null;
    private void Awake()
    {
        instance = this;
    }
    public static UIEventManager Instance() { return instance; }

    public Transform sm;

    public enum EventType
    {
        Continent,                                  //대륙 선택(North America=0, Europe=1, Asia=2, South America=3, Africa=4)
        NextTurn,                                   //다음 턴으로 넘어갈때
        PickChar,                                   //케릭터(1번 케릭터, 2번, 3번, 4번, 5번)
        MoveChar,                                   //움직였을때
        AttackChar,                                 //공격할때
        HitChar,                                    //피격당할때
    }

    public delegate void OnEvent(object palyer = null, object data = null);
    Dictionary<EventType, List<OnEvent>> eventDic;

    //public void start()
    //{
    //    eventDic = new Dictionary<EventType, List<OnEvent>>();
    //}

    private void Start()
    {
        eventDic = new Dictionary<EventType, List<OnEvent>>();
        transform.GetChild(0).GetComponent<Capsule>().start();
        transform.GetChild(0).GetComponent<Slider>().start();
        sm.GetComponent<UIController>().start();
        sm.GetComponent<UIManager>().start();
        transform.GetChild(0).GetComponent<Capsule>().Host();
        transform.GetChild(0).GetComponent<Slider>().RemoveEvent();
    }

    public void update()
    {
        
    }

    //이벤트 추가
    public void AddEvent(EventType type, OnEvent func)
    {
        List<OnEvent> newEvent = null;

        if (eventDic.TryGetValue(type, out newEvent))
        {
            if (newEvent.Contains(func)) return;

            newEvent.Add(func);
            eventDic[type] = newEvent;

            return;
        }

        newEvent = new List<OnEvent>();
        newEvent.Add(func);
        eventDic.Add(type, newEvent);
    }

    //이벤트 제거
    public void RemoveEvent(EventType type, OnEvent func)
    {
        List<OnEvent> newEvent = null;

        if(eventDic.TryGetValue(type, out newEvent))
        {
            if (newEvent.Contains(func))
            {
                newEvent.Remove(func);
                eventDic[type] = newEvent;
            }
        }
    }

    //이벤트 호출
    public void PostEvent(EventType type, object player, object data)
    {
        List<OnEvent> newEvent = null;

        if(eventDic.TryGetValue(type, out newEvent))
        {
            for(int i=0; i<newEvent.Count; i++)
            {
                newEvent[i](player, data);
            }
        }
    }
}
