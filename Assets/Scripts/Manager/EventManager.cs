using System.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/* by 도아
 이벤트 사용 예는 두가지 정도 있을듯 
 1. 단순 이벤트 발생 
    해당 이벤트 발생시 관련된 작업 전체 실행 
    ex) EndOfTurn Event 턴 종료시 카메라 배틀 캐릭터 등에서 해당 이벤트로 종료 함수들 등록해두면 자동 실행 
        or UI에서 많이쓸듯 체력바가 업데이트를 도는게아니고 피가 달떄마다 이벤트 발생 / 버튼눌리면 매니저들에서 동작 실행 
 2. 매니저간 통신 
    매니저간 통신을 직접 접근이 아닌 이벤트를 통해 접근
    ex) 배틀매니저가 캐릭터매니저에게 캐릭터 스텟을 받아야할때 
        1. 작업자가 BC_GetCharacterStat, CB_SetCharacterStat 두개 이벤트를 만들고 내가 작업하는 클래스에 콜백받을 함수를 만듬(필요한 인자 받을수있게)
        2. 만든 함수를 CB_SetCharacterStat와 연결후 BC_GetCharacterStat 이벤트를 발생
        3. 캐릭터매니저에서는 CB_SetCharacterStat이벤트 발생시켜서 스텟을 넘겨주는 함수를 BC_GetCharacterStat로 등록 
        4. 이벤트 목록에 표시해두면 담당자가 보고 해당 함수 만들어주는 식으로 업무가 돌아가면 좋겠는데 너무비효율적일지도 모르겠음 

    더 예쁘게 할수도있을거같은데 모르겠음. 수정이나 더좋은 방식 얘기해보고 
    네이밍 규약은 통신하는 매니저이니셜을 달아서 어디서 사용하는지 써두면 좋을듯 이것도 더 좋은 방식 있는지 고민 
*/

//서기원 작성
public enum EventID
{
    // 이벤트가 추가 될 때 아래에 추가로 작성해주세요.
    // 등록하기 애매한건 단톡방에서 이벤트로 등록할지 상의해주세요.
    // 네이밍 규약
    // 1.On 으로 시작
    // 2.이벤트가 일어나는 주체
    // 3.해당하는 동사
    // ex) 만약 화면 사이즈가 재조정 될 때의 이벤트라면 아래처럼
    OnScreenResize = 0,
    //여기선 On을빼는게 더 예쁘지 않을까? 
    EndOfTurn,
    SelectCharacter,
    SelectTarget,
    Attack,



    

}


// 이벤트가 발생할 때 실행시킬 함수 위에 적어주세요.
// ex) [Event(EventID.OnScreenResize)]
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class EventAttribute : Attribute
{
    public EventID eventID { get; private set; }
    public EventAttribute(EventID id)
    {
        eventID = id;
    }

    public override object TypeId => this;
}

public class EventManager : MonoBehaviour
{
    // 이벤트를 실행할 객체와 함수를 저장할 구조체입니다.
    public struct EventInfo
    {
        public MethodInfo MethodInfo { get; set; }
        public object Instance { get; set; }
    }

    private static EventManager instance = null;
    private HashSet<EventID> _setForDelete = new HashSet<EventID>();
    private Dictionary<EventID, List<EventInfo>> _eventDic = new Dictionary<EventID, List<EventInfo>>(); // 이벤트를 이벤트 형식별로 저장하기 위해서 Dictionary로 사용


    #region 싱글턴
    public static EventManager CreateSingleton()
    {
        if (null == instance)
        {
            GameObject container = new GameObject();
            container.name = "EventManager";
            instance = container.AddComponent<EventManager>();
            GameManager.GetInstance().Event = instance;         // GameManager의 Event 인스턴스에 연결 - by 영균
        }
        else
        {
            Debug.Log("EventManager가 이미 존재합니다"); // by 영균
        }
        return instance;
    }

    // GetInstacne() 사용할 일 없으면 삭제해주세요 - by 영균
    public static EventManager GetInstance()
    {
        return instance;
    }
    #endregion

    // Event 등록 함수입니다.
    // 인자로 함수를 실행시킬 오브젝트와 어떤 이벤트인지 넘겨주세요.
    public void BindEvent(object _instance, EventID _id)
    {
        var methodInfos = _instance.GetType().GetMethods();

        foreach(var methodInfo in methodInfos)
        {
            var customAttributes = methodInfo.GetCustomAttributes(typeof(EventAttribute), false);
            foreach(var customAttribute in customAttributes)
            {
                if (null == customAttribute) continue;

                var attributeInstance = customAttribute as EventAttribute;
                if (attributeInstance.eventID != _id) continue;

                EventInfo info = new EventInfo
                {
                    MethodInfo = methodInfo,
                    Instance = _instance
                };

                if (false == instance._eventDic.ContainsKey(_id))
                    instance._eventDic.Add(_id, new List<EventInfo>());

                instance._eventDic[_id].Add(info);
            }
        }
    }

    // 오브젝트에 연결된 모든 이벤트 삭제입니다.
    // 일부만 삭제하고 싶으면 재등록을 꼭 해주세요.
    // 이벤트를 등록했다면 등록한 객체가 삭제될 때 반드시 호출이 필요합니다.
    public void DeleteEvent(object _instance)
    {
        _setForDelete.Clear();
        foreach(var gameEvent in _eventDic)
        {
            for(int index = 0; index < gameEvent.Value.Count; index++)
            {
                if(gameEvent.Value[index].Instance == _instance)
                {
                    gameEvent.Value.RemoveAt(index);
                    index--;
                }
            }
            if (0 == gameEvent.Value.Count)
                instance._setForDelete.Add(gameEvent.Key);
        }

        foreach (var deleteType in _setForDelete)
            _eventDic.Remove(deleteType);
    }

    // 이벤트 실행 함수 입니다.
    // 등록한 함수의 인자값을 갯수에 맞게 적절히 넣어주세요.
    public void OnTriggerGameEvent(EventID _id, params object[] _parameters)
    {
        List<EventInfo> eventInfos;
        if (false == instance._eventDic.TryGetValue(_id, out eventInfos)) return;

        foreach (var info in eventInfos)
        {
            info.MethodInfo.Invoke(info.Instance, _parameters);
        }
    }
}   