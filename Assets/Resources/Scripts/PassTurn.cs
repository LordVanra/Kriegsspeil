using UnityEngine;

public class PassTurn : MonoBehaviour
{

    public int turn;
    private Drag[] blocks;
    void Start()
    {
        blocks = FindObjectsByType<Drag>(FindObjectsSortMode.None);
        turn = 0;
    }

    public void NextTurn(){
        blocks = FindObjectsByType<Drag>(FindObjectsSortMode.None);
        turn++;
        Debug.Log(turn);
        foreach (Drag block in blocks)
        {
            if (turn != 1)
            {
                block.updateTiredness();
            }
            block.resetInfo();
            GameObject.Find("Combat").GetComponent<CombatHandler>().clearCombat();
        }
    }
}
