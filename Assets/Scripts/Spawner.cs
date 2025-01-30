using UnityEngine;

public class Spawner : MonoBehaviour
{
    
    public GameObject redBlock;
    public GameObject blueBlock;
    public GameObject yellowBlock;
    public GameObject purpleBlock;
    private GameObject newBlock;

    void Update(){
        // Debug.Log(GetMousePos());
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            newBlock = Instantiate(redBlock, new Vector2(GetMousePos().x, GetMousePos().y), Quaternion.identity);
            // newBlock.GetComponent<Drag>().Start();
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)){
            newBlock = Instantiate(yellowBlock, new Vector2(GetMousePos().x, GetMousePos().y), Quaternion.identity);
            newBlock.GetComponent<Drag>().Start();
        }
        if(Input.GetKeyDown(KeyCode.Alpha3)){
            newBlock = Instantiate(blueBlock, new Vector2(GetMousePos().x, GetMousePos().y), Quaternion.identity);
            newBlock.GetComponent<Drag>().Start();
        }
        if(Input.GetKeyDown(KeyCode.Alpha4)){
            newBlock = Instantiate(purpleBlock, new Vector2(GetMousePos().x, GetMousePos().y), Quaternion.identity);
            newBlock.GetComponent<Drag>().Start();
        }
    }

    private Vector2 GetMousePos(){
        Vector2 mousePos = Input.mousePosition;
        
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
