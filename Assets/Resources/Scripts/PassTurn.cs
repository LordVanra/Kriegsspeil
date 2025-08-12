using UnityEngine;

public class PassTurn : MonoBehaviour
{

    public int turn = 1;
    private Drag[] blocks;
    void Start(){
        blocks = FindObjectsByType<Drag>(FindObjectsSortMode.None);
    }

    public void NextTurn(){
        blocks = FindObjectsByType<Drag>(FindObjectsSortMode.None);
        turn++;
        foreach(Drag block in blocks){
            block.updateTiredness();
            block.resetInfo();
            GameObject.Find("Combat").GetComponent<CombatHandler>().clearCombat();
        }
    }
}
