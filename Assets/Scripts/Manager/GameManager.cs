using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 서영균
public class GameManager : MonoBehaviour
{
    #region 싱글턴
    private static GameManager instance = null;
    public static GameManager GetInstance() { return instance; }
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    #endregion

    public enum Team
    {
        Human,
        Alien,
    }

    public EventManager Event;
    public InputManager Input;
    public CameraManager Camera;
    public BattleManager Battle;
    public MapManager Map;
    public JsonManager Json;
    //public UIEventManager UiEvnMgr;

    private void Start()
    {
        CreateSingleton();
        Camera.start();
        Battle.start();
        Map.start();
        Json.start();
        //UiMgr.start();
    }

    private void Update()
    {
        Input.update();
        Camera.update();
        Map.update();
    }

    private void CreateSingleton()
    {
        EventManager.CreateSingleton();
        InputManager.CreateSingleton(this);
        CameraManager.CreateSingleton(this);
        BattleManager.CreateSingleton(this);
        MapManager.CreateSingleton(this);
        JsonManager.CreateSingleton(this);
    }
}
