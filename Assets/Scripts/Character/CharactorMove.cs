using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Node: System.IComparable<Node>  {
    public int row = 0;
    public int col = 0;
    public Node parentNode = null;
    public int F = 0;
    public int G = 0;
    public int H = 0;

    public Node(Vector2 nodeIndex){
        this.row = (int)nodeIndex.x;
        this.col = (int)nodeIndex.y;
    }
    public void calWeight(Node previousNode, Vector2 endIndex){
        int H_dx = (int)Mathf.Abs(this.row - endIndex.x);
        int H_dy = (int)Mathf.Abs(this.col - endIndex.y);

        int G_dx = (int)Mathf.Abs(this.row - previousNode.row);
        int G_dy = (int)Mathf.Abs(this.col - previousNode.col);
        
        this.H = previousNode.H + (H_dx + H_dy)*10;
        this.G = previousNode.G + Mathf.Min(G_dx,G_dy)*14 + Mathf.Abs(G_dx-G_dy)*10;
        this.F = this.G + this.H; 
    }
    public void changePrevious(Node previousNode){
        this.parentNode = previousNode;
        int G_dx = (int)Mathf.Abs(this.row - previousNode.row);
        int G_dy = (int)Mathf.Abs(this.col - previousNode.col);
        this.G = previousNode.G + Mathf.Min(G_dx,G_dy)*14 + Mathf.Abs(G_dx-G_dy)*10;
        this.F = this.G + this.H; 
    }

    public int CompareTo(Node other)
    {
        if(other == null){
            return 1;
        }
        if(other.F > this.F){
            return -1;
        }
        if(other.F == this.F){
            return 0;
        }
        return 1;
    }
}
public class CharactorMove : MonoBehaviour
{   
    public Vector2 size = new Vector2(6,10);
    public int tileSize = 10;

    public Vector2 startIndex;
    public Vector2 endIndex;

    public int[,] mapMat = {
        {0,1,1,1,0,0,0,0,1,1},
        {0,1,1,1,0,0,1,0,0,0},
        {0,0,0,0,0,1,1,1,0,0},
        {0,0,0,0,0,1,1,0,0,0},
        {0,0,1,1,0,0,0,0,1,1},
        {1,1,1,1,0,0,1,1,1,1},
    };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector3 getPosWithIndex(int row, int col){
        Vector3 retPos = new Vector3(0,0,0);
        retPos.x = tileSize/2 + tileSize * col;
        retPos.z = -1*(tileSize/2 + tileSize * row);
        return retPos;
    }

    bool isValidIndex(Vector2 index){
        if(index.x < 0 || index.y < 0 || index.x >= size.x || index.y >= size.y){
            return false;
        }
        else{
            return true;
        } 
    }
    
    List<Node> getAstarPaths(){
        bool isFind = false;
        List<Node> nodes = new List<Node>();
        List<Node> openList = new List<Node>();
        List<Node> closeList = new List<Node>();
        List<Node> retList = new List<Node>();
        Node lastNode = null;
        Node startNode = new Node(startIndex);
        nodes.Add(startNode);
        closeList.Add(startNode);

        Vector2 nearNodeIndex = new Vector2();
        //처음 8개 넣기
        for(int i = -1; i <= 1; i++){
            for(int j = -1 ; j <= 1; j++){
                if(i != 0 || j != 0){
                    nearNodeIndex.x = startNode.row + i;
                    nearNodeIndex.y = startNode.col + j;
                    if(isValidIndex(nearNodeIndex)){
                        if(mapMat[(int)nearNodeIndex.x,(int)nearNodeIndex.y] == 0){
                            Node nearNode = new Node(nearNodeIndex);
                            nearNode.parentNode = startNode;
                            nearNode.calWeight(startNode,endIndex);
                            nodes.Add(nearNode);
                            openList.Add(nearNode);
                        }
                    }
                }
            }
        }
        
        while(!openList.Exists(node => node.row == endIndex.x && node.col == endIndex.y) 
            && openList.Count != 0){

            Node focusedNode = openList.Min(node=>node);
            openList.Remove(focusedNode);
            closeList.Add(focusedNode);

            //8방향 평가
            for(int i = -1 ; i<=1 ; i++){
                for(int j=-1;j<=1;j++){
                    if(i != 0 || j!= 0){
                        nearNodeIndex.x = focusedNode.row + i;
                        nearNodeIndex.y = focusedNode.col + j;
                        if(isValidIndex(nearNodeIndex)){
                            if(!closeList.Exists(node => node.row == nearNodeIndex.x && node.col == nearNodeIndex.y) 
                                && mapMat[(int)nearNodeIndex.x,(int)nearNodeIndex.y] == 0){
                                if(!openList.Exists(node => node.row == nearNodeIndex.x && node.col == nearNodeIndex.y)){
                                    
                                    Node nearNode = new Node(nearNodeIndex);
                                    nearNode.parentNode = focusedNode;
                                    nearNode.calWeight(focusedNode,endIndex);
                                    openList.Add(nearNode);

                                    if(nearNodeIndex.x == endIndex.x && nearNodeIndex.y == endIndex.y){
                                        lastNode = nearNode;
                                        isFind = true;
                                        goto FIND_SUCCESS;
                                    }
                                }
                                else{
                                    //포커스 노드의 근처 탐색중인 노드가 이미 열린 목록에 있을경우 G값이 더 작으면 이게 부모다.
                                    Debug.LogWarning(focusedNode.parentNode.row + "," +focusedNode.parentNode.col);
                                    Node newParentNode = openList.Find(node => node.row == nearNodeIndex.x 
                                                                        && node.col == nearNodeIndex.y);
                                    Debug.LogWarning(newParentNode.row + "," +newParentNode.col);                            
                                    if(newParentNode.G < focusedNode.parentNode.G){
                                        focusedNode.parentNode = newParentNode;
                                        focusedNode.changePrevious(newParentNode);
                                    }
                                }
                            }
                        } 
                    }
                }
            }
            
        }

        FIND_SUCCESS:
        Stack<Node> tempStack = new Stack<Node>();
        if(isFind){
            Debug.Log("founded!");
            Node tempNode = lastNode; 
            while(tempNode != null){
                tempStack.Push(tempNode);
                tempNode = tempNode.parentNode;
            }
            while(tempStack.Count != 0){
                retList.Add(tempStack.Pop());
            }

            foreach(Node node in retList){
                Debug.Log("("+node.row+""+node.col+")");
            }
            return retList;
        }
        else{
            Debug.Log("can't go there!");
            return null;
        }

    }

    void OnDrawGizmos() {
        int height = (int)(tileSize * size.x); 
        int width = (int)(tileSize * size.y);

        Gizmos.DrawLine(Vector3.zero, new Vector3(10,0,11));
        Gizmos.color = new Color(1, 1, 0);
        for(int i=0;i<size.y+1;i++){
            Gizmos.DrawLine(new Vector3(0+tileSize*i ,0,0), new Vector3(0+tileSize*i,0,-1*height));
        }
        for(int i=0;i<size.x+1;i++){
            Gizmos.DrawLine(new Vector3(0,0,0-tileSize*i), new Vector3(width,0,0-tileSize*i));
        }
        Gizmos.color = Color.red;
        for(int col=0;col<size.y;col++){
            for(int row=0;row<size.x;row++){
                if(mapMat[row,col] != 0){
                    Gizmos.DrawCube(getPosWithIndex(row,col), new Vector3(8, 1, 8));
                }
            }
        }
        Gizmos.color = Color.green;
        Gizmos.DrawCube(getPosWithIndex((int)startIndex.x,(int)startIndex.y), new Vector3(4, 1, 4));

        Gizmos.color = Color.blue;
        Gizmos.DrawCube(getPosWithIndex((int)endIndex.x,(int)endIndex.y), new Vector3(4, 1, 4));

        Gizmos.color = Color.white;

        List<Node> path = getAstarPaths();
        foreach(Node node in path){
            Gizmos.DrawCube(getPosWithIndex(node.row, node.col), new Vector3(2, 2, 2));
        }


    }
}
