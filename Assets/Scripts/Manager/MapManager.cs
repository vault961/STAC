using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//김도아
public class MapManager : MonoBehaviour
{
    #region 싱글턴
    private static MapManager instance = null;
    public static void CreateSingleton(GameManager GameManager)
    {
        if (!instance)
        {
            GameObject container = new GameObject();
            container.name = "MapManager";   // "CameraManager"라고 적혀 있던 걸 "BattleManager"로 수정 - by 영균
            instance = container.AddComponent<MapManager>();
            GameManager.Map = instance;
        }
        else
        {
            Debug.Log("MapManager가 이미 존재합니다");
        }
    }
    #endregion


    private GameManager MGR;

    private Vector2 tileSize = new Vector2();
    private Vector2 coordinate = new Vector2();
    private Vector3 tilePosition = new Vector3();

    public void start()
    {
        MGR = GameManager.GetInstance();

        BindEvents();

        //임시
        tileSize = new Vector2(10f, 10f);
    }

    public void update()
    {
        
    }

    public void CleanUp()
    {
        MGR.Event.DeleteEvent(this); // GameManager 통해서 EventManager 접근하도록 수정 - by 영균
    }

    private void BindEvents()
    {
    }

    private Vector2 GetCoordinate(Vector3 selectPosition)
    {
        coordinate.x = (selectPosition.x / tileSize.x) - 1f;
        coordinate.y = (selectPosition.z / tileSize.y) - 1f;

        return coordinate;
    }

    private Vector3 GetTilePosition(Vector2 coordinate)
    {
        tilePosition.x = (coordinate.x * tileSize.x) + (tileSize.x * 0.5f);
        tilePosition.y = 0f;
        tilePosition.z = (coordinate.y * tileSize.y) + (tileSize.y * 0.5f);

        return tilePosition;
    }
}
