using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONINFO
{
    public int ID;
    public string Name;
    public string Dis;
}

public class LeeJaeChul : JSONINFO
{
    /// <summary>
    /// Lee JaeChul state Class
    ///  Programmer : 강준호 
    /// </summary>

    public int HP; // 체력
    public float Tall; // 키
    public float Weight; // 몸무게
    public string Company; // 회사
    public int Annual; // 연차 여자친구
    public bool GirlFriend; // 여자친구 존재여부

    public LeeJaeChul(int id, string name, string dis, int hp, float tall, float weight, string company, int annual, bool girfriend)
    {
        ID = id;
        Name = name;
        Dis = dis;
        HP = hp;
        Tall = tall;
        Weight = weight;
        Company = company;
        Annual = annual;
        GirlFriend = girfriend;
    }
}
