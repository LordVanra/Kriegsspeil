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
            block.updateTiredness();
            block.resetInfo();
        }
        Debug.Log(turn);
    }
}
