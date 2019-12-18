using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
public class JsonManager : MonoBehaviour
{
    /// <summary>
    /// Json 관리 메니져 클래스
    ///  Programmer : 강준호 
    /// </summary>
    private GameManager MGR;

    #region 싱글턴
    private static JsonManager instance = null;
    public static void CreateSingleton(GameManager GameManager)
    {
        if (!instance)
        {
            GameObject container = new GameObject();
            container.name = "JsonManager"; 
            instance = container.AddComponent<JsonManager>();
            GameManager.Json = instance;
        }
        else
        {
            Debug.Log("MapManager가 이미 존재합니다");
        }
    }
    #endregion

    public void start()
    {
        MGR = GameManager.GetInstance();
    }
    List<LeeJaeChul> jaeList = new List<LeeJaeChul>();
    public string[] jaeString;
    public void Start()
    {
        LeeJaeChul leeJaeChul = new LeeJaeChul(1,
            "이재철",
            Application.dataPath,
            100,
            180,
            85,
            "회사",
            3,
            false
            );
        jaeList.Add(leeJaeChul);
    }
#if UNITYEDITOR
#elif UNITY_ANDRIOD || UNITY_IOS
#endif
    public void JsonSave()
    {
        JsonData jsonData = JsonMapper.ToJson(jaeList); ;
        File.WriteAllText(Application.dataPath + "/Resource/Data/JaeChelList.json", jaeList.ToString());
    }

    public string[] JsonLoad()
    {
        if (File.Exists(Application.dataPath + "/Resource/Data/JaeChelList.json"))
        {
            var jsonStr = File.ReadAllText(Application.dataPath + "Resource/Data/JaeChelList.json");
            JsonData jsonData = JsonMapper.ToObject(jsonStr);
            for (int i = 0; i < jsonData.Count; i++)
            {
                jaeString[i] = jsonData[i].ToString();
            }
            return jaeString;
        }
        return null;
    }
}
