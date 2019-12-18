using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 서영균
public class TopDownController : MonoBehaviour
{
    private DoublyCircularLinkedList<Transform> camList;
    private DCLNode<Transform> iter;

    public void start()
    {
        ListSetUp();
    }

    public Transform center { get { return transform; } }
    public Transform rigTr { get { return iter.data; } }
    public Transform prev { get { return iter.prev.data; } }
    public Transform next { get { return iter.next.data; } }
    public void Prev()
    {
        iter = iter.prev;
    }
    public void Next()
    {
        iter = iter.next;
    }

    // 동서남북에 있는 탑다운카메라리그를 리스트에 넣어줌
    private void ListSetUp()
    {
        Transform[] transforms = GetComponentsInChildren<Transform>();
        camList = new DoublyCircularLinkedList<Transform>();
        for (int i = 1; i < transforms.Length; ++i)
        {
            camList.Add(transforms[i]);
        }
        iter = camList.front;
    }
}
