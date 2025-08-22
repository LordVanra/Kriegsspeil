using UnityEngine;

public class PassTurn : MonoBehaviour
{

    public int turn;
    private Drag[] blocks;
    private FogMask maskScript;
    void Start()
    {
        blocks = FindObjectsByType<Drag>(FindObjectsSortMode.None);
        maskScript = GameObject.Find("Fog").GetComponent<FogMask>();
        turn = 0;
    }

    public void NextTurn(){
        blocks = FindObjectsByType<Drag>(FindObjectsSortMode.None);
        turn++;
        maskScript.ClearFog();
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
