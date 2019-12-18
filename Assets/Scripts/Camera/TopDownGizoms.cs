using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 서영균
public class TopDownGizoms : MonoBehaviour
{
    //private void OnDrawGizmosSelected()
    private void OnDrawGizmos()
    {
        Transform[] camPositions;
        camPositions = GetComponentsInChildren<Transform>();
        Gizmos.color = Color.green;

        foreach(Transform tr in camPositions)
        {
            if(tr == transform)
                continue;
            Gizmos.DrawWireSphere(tr.position, 0.5f);
            Gizmos.DrawLine(tr.position, transform.position);
        }
    }
}
