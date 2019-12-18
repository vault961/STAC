using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 서영균
class DCLNode<T>
{
    public DCLNode<T> prev { get; private set; }
    public DCLNode<T> next { get; private set; }
    public T data { get; private set; }

    // 생성자
    public DCLNode(T _data = default)
    {
        data = _data;
        prev = this;
        next = this;
    }

    // 노드 데이터 출력
    public void Display()
    {
        //Console.Write(data);
        Debug.Log(data);
    }

    // 노드 뒤에 삽입
    public DCLNode<T> InsertNext(DCLNode<T> node)
    {
        if (null != node)
        {
            node.prev = this;
            node.next = next;
            next.prev = node;
            next = node;
        }
        return node;
    }

    // 노드 삭제
    public DCLNode<T> Remove()
    {
        // prev가 this인 경우 = 리스트에 노드가 하나만 있는 경우
        if (this == prev)
        {
            prev = next = null;
        }
        else
        {
            prev.next = next;
            next.prev = prev;
        }
        data = default;
        return this;
    }
}

class DoublyCircularLinkedList<T> : IEnumerable<T>, IEnumerator<T>
{
    // 헤드 노드
    private DCLNode<T> tail = null;

    // 생성자
    public DoublyCircularLinkedList() { }

    // 리스트가 비었는지 확인
    public bool isEmpty { get { return null == tail; } }
    // 리스트 길이
    public int Length { get; private set; }
    // 리스트 첫번째 원소 반환
    public DCLNode<T> front { get { return tail.next; } }

    // []인덱서 오퍼레이터로 노드 데이터 반환
    public T this[int index] { get { return Find(index).data; } }

    // 해당 인덱스 노드 찾기
    public DCLNode<T> Find(int index)
    {
        // 인덱스 범위 벗어날 경우
        if (0 > index || index >= Length)
        {
            //Console.WriteLine("해당 번지의 노드는 존재하지 않습니다");
            Debug.Log("해당 번지의 노드는 존재하지 않습니다");
            return null;
        }

        // 해당 인덱스 노드 탐색
        DCLNode<T> node = tail.next;
        while (0 != index)
        {
            --index;
            node = node.next;
        }
        return node;
    }

    // 리스트 마지막에 노드 삽입
    public void Add(T _data)
    {
        ++Length;
        if (null == tail)
        {
            tail = new DCLNode<T>(_data);
            return;
        }
        tail = tail.InsertNext(new DCLNode<T>(_data));
    }

    // 인덱스 위치에 삽입
    public void Insert(T _data, int index)
    {
        ++Length;
        if (null == tail)
        {
            tail = new DCLNode<T>(_data);
            return;
        }
        Find(index).InsertNext(new DCLNode<T>(_data));
    }

    // 노드 삭제
    public void Remove(DCLNode<T> n)
    {
        if (isEmpty)
        {
            //Console.WriteLine("삭제할 노드가 없습니다");
            Debug.Log("삭제할 노드가 없습니다");
            return;
        }

        --Length;
        // 삭제할 노드가 tail인 경우
        if (n == tail)
        {
            tail = tail.Remove().prev;
            return;
        }
        for (DCLNode<T> node = tail.next; node != tail; node = node.next)
        {
            if (n == node)
            {
                node.Remove();
                return;
            }
        }
    }

    // 노드 출력
    public void Display()
    {
        if (null == tail)
        {
            //Console.WriteLine("리스트가 비어있습니다!");
            Debug.Log("리스트가 비어있습니다!");
            return;
        }
        DCLNode<T> n = tail.next;
        for (int i = 0; i < Length; ++i, n = n.next)
        {
            //Console.Write(i + "번지 원소 : ");
            Debug.Log(i + "번지 원소 : ");
            n.Display();
        }
        //Console.WriteLine("Tail : " + tail.data);
        Debug.Log("Tail : " + tail.data);
    }

    // ================================================
    // ------------------- 열거자 ------------------- 
    // ================================================
    private DCLNode<T> current = null;
    public IEnumerator<T> GetEnumerator()
    {
        if (current == null)
            current = front;
        do
        {
            yield return current.data;
            current = current.next;
        } while (current != front);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        if (current == null)
            current = front;
        do
        {
            yield return current.data;
            current = current.next;
        } while (current != front);
    }

    public T Current { get { return current.data; } }
    object IEnumerator.Current { get { return current.data; } }

    public bool MoveNext()
    {
        if (current == tail)
        {
            Reset();
            return false;
        }
        return true;
    }

    public void Reset()
    {
        current = tail;
    }

    public void Dispose()
    {
    }
}