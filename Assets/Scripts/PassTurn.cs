using UnityEngine;

public class PassTurn : MonoBehaviour
{

    private int turn = 1;
    private Drag[] blocks;

    void Start(){
        blocks = FindObjectsByType<Drag>(FindObjectsSortMode.None);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextTurn(){
        turn++;
        foreach(Drag block in blocks){
            block.paceLeft = 3f;
            block.startPos = new Vector2(block.transform.position.x, block.transform.position.y);
        }
        Debug.Log(turn);
    }
}
