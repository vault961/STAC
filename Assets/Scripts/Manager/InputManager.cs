using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 서영균
public class InputManager : MonoBehaviour
{
    #region 싱글턴
    private static InputManager instance = null;
    public static void CreateSingleton(GameManager GameManager)
    {
        if(!instance)
        {
            GameObject container = new GameObject();
            container.name = "InputManager";
            instance = container.AddComponent<InputManager>();
            GameManager.Input = instance;
        }
        else
        {
            Debug.Log("InputManager가 이미 존재합니다");
        }
    }
    #endregion

    public float mouseX { get; private set;} = 0.0f;
    public float mouseY { get; private set; } = 0.0f;
    public float mouseWheel { get; private set; } = 0.0f;
    public bool mouseL { get; private set; } = false;
    public bool mouseLUp { get; private set; } = false;
    public bool mouseR { get; private set; } = false;
    public bool mouseRUp { get; private set; } = false;
    public Vector3 mousePos { get; private set; } = Vector3.zero;
    public Touch touchOne { get; private set; } // 첫번째 터치 좌표
    public Touch touchTwo { get; private set; } // 두번재 터치 좌표
    public int touchCount { get; private set; } = 0; // 입력된 터치 카운트
    public bool KeyQ { get; private set; } = false;
    public bool KeyE { get; private set; } = false;

    public void update()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        mouseWheel = Input.GetAxis("Mouse ScrollWheel");
        mousePos = Input.mousePosition;
        mouseL = Input.GetMouseButton(0);
        mouseLUp = Input.GetMouseButtonUp(0);
        mouseR = Input.GetMouseButton(1);
        mouseRUp = Input.GetMouseButtonUp(1);
        touchCount = Input.touchCount;
        if(1 <= touchCount) touchOne = Input.GetTouch(0);
        if(2 <= touchCount) touchTwo = Input.GetTouch(1);
        KeyQ = Input.GetKeyDown(KeyCode.Q);
        KeyE = Input.GetKeyDown(KeyCode.E);
    }
}
