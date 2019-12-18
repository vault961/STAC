using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 서영균
public class CameraManager : MonoBehaviour
{
    #region 싱글턴
    private static CameraManager instance = null;
    public static void CreateSingleton(GameManager GameManager)
    {
        if (!instance)
        {
            GameObject container = new GameObject();
            container.name = "CameraManager";
            instance = container.AddComponent<CameraManager>();
            GameManager.Camera = instance;
        }
        else
        {
            Debug.Log("CameraManager가 이미 존재합니다");
        }
    }
    #endregion

    private enum ViewMode { TOPDOWN = 0, TPS }; // 탑다운, TPS 시점
    private ViewMode viewMode = ViewMode.TOPDOWN;
    private GameManager MGR;
    private Transform mainCameraTr;

    // 탑다운카메라리그 관리 스크립트
    private TopDownController TopDown;
    
    private Vector3 startPos = Vector3.zero;
    private Vector3 endPos = Vector3.zero;
    public void start()
    {
        MGR = GameManager.GetInstance();
        TopDown = GameObject.Find("TopDownCameraRig").GetComponent<TopDownController>();
        TopDown.start();
        
        mainCameraTr = Camera.main.transform;
        startPos = TopDown.rigTr.position;   
    }

    public void update()
    {
        switch (viewMode)
        {
            case ViewMode.TOPDOWN: { TopDownView(); break; }
        }
    }

    private void TopDownView()
    {
        mainCameraTr.LookAt(TopDown.center);
        if (MGR.Input.KeyQ) 
        {
            StartCoroutine(CameraSlerp(mainCameraTr.position, TopDown.prev.position));
            TopDown.Prev();
        }
        if (MGR.Input.KeyE) 
        {
            StartCoroutine(CameraSlerp(mainCameraTr.position, TopDown.next.position));
            TopDown.Next();
        }
        
        //EdgeScrolling();

        // 터치 확대
        if (2 <= MGR.Input.touchCount)
        {
            CameraZoom(TouchZoom());
        }
        // 마우스 휠 확대
        if (0 != MGR.Input.mouseWheel)
        {
            CameraZoom(MGR.Input.mouseWheel);
        }
    }

    private IEnumerator CameraSlerp(Vector3 startPos, Vector3 endPos)
    {
        float elapsedTime = 0.0f;
        while(elapsedTime < 1f)
        {
            mainCameraTr.position = Vector3.Slerp(startPos,endPos, elapsedTime / 1f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    // 모서리 스크롤 (마우스, 키보드)
    private void EdgeScrolling()
    {
        float EdgeScrollingSpeed = 10f;
        //float edgeSize = 10f;

        if (Input.GetKey(KeyCode.W)) //|| MGR.Input.mousePos.y > Screen.height - edgeSize)
        {
            TopDown.rigTr.Translate(Vector3.forward * EdgeScrollingSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D)) //|| MGR.Input.mousePos.x > Screen.width - edgeSize)
        {
            TopDown.rigTr.Translate(Vector3.right * EdgeScrollingSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A)) //|| MGR.Input.mousePos.x < edgeSize)
        {
            TopDown.rigTr.Translate(-Vector3.right * EdgeScrollingSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S)) //|| MGR.Input.mousePos.y < edgeSize)
        {
            TopDown.rigTr.Translate(-Vector3.forward * EdgeScrollingSpeed * Time.deltaTime);
        }
    }

    // 터치 줌
    private float TouchZoom()
    {
        Touch touchOne = MGR.Input.touchOne;
        Touch touchTwo = MGR.Input.touchOne;

        // 움직이기 전의 위치 계산 (현재 position에서 delta값을 빼주면 움직이기 전의 position이 된다)
        Vector2 touchZeroPrePos = touchOne.position - touchOne.deltaPosition;
        Vector2 touchOnePrePos = touchOne.position - touchOne.deltaPosition;

        // 첫번째 터치와 두번째 터치 사이의 거리를 프레임 단위로 계산
        float prevTouchDeltaMag = (touchZeroPrePos - touchOnePrePos).magnitude;
        float touchDeltaMag = (touchOne.position - touchOne.position).magnitude; // magnitude를 sqrMagnitude로 바꿀수는 없을까?

        // 터치의 이동 거리 계산
        return prevTouchDeltaMag - touchDeltaMag;
    }

    // (전투 시스템 완성되면 턴 선택 화면에서만 작동하도록 수정할것)
    // 카메라 줌
    private void CameraZoom(float zoomValue)
    {
        //float zoom = zoomValue * zoomSpeed * Time.deltaTime;
        //Vector3 TopDownPos = mainCameraTr.position.y;

        // float scrollSpeed = 100f; 
        // TopDownPos.y += zoomValue * scrollSpeed * Time.deltaTime;
        // TopDownCameraRig.localPosition = TopDownPos;

        // this.zoomValue -= zoomValue * zoomSpeed * Time.deltaTime;
        // this.zoomValue = Mathf.Clamp(this.zoomValue, minZoom, maxZoom);
        // //Camera.main.fieldOfView -= zoomSpeed * zoomValue * Time.deltaTime; // zoomValue에 따라 카메라 배율 변경
        // //Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, minZoom, maxZoom); // minZoom, maxZoom 범위 안에서만 배율 변경 허용
        // if(zoomValue > 0)
        // {
        //     //Camera.main.fieldOfView -= 100f * Time.deltaTime;
        // }
        // else if(zoomValue < 0)
        // {
        //     //Camera.main.fieldOfView += 100f * Time.deltaTime;
        // }
    }
}
