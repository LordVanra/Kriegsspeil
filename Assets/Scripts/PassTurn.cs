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
            Color redden = block.capsuleColor.color;
            redden += new Color(0.1f*(2f-block.paceLeft), -0.1f*(2f-block.paceLeft), 0f, 0f);
            redden.a = 1f;
            block.capsuleColor.color = redden;
            if(block.capsuleColor.color.r < 0){
                block.capsuleColor.color = new Color(0f, 1f, 0f, 1f);
            }
            block.paceLeft = 3f;
            block.startPos = new Vector2(block.transform.position.x, block.transform.position.y);
        }
        Debug.Log(turn);
    }
}
