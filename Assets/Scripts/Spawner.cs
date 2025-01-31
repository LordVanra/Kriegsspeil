using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    
    public GameObject redInf, blueInf, yellowInf, purpleInf, redArt, blueArt, yellowArt, purpleArt, redGen, blueGen, yellowGen, purpleGen, redSkirm, blueSkirm, yellowSkirm, purpleSkirm, redCav, blueCav, yellowCav, purpleCav;
    private enum blockTypes{
        Artillery,
        Cavalry,
        Infantry, 
        Officer, 
        Skirmisher
    }
    private blockTypes blockToSpawn = blockTypes.Infantry;
    private GameObject newBlock;
    private Slider enable;
    private GameObject redToSpawn;
    private GameObject blueToSpawn;
    private GameObject yellowToSpawn;
    private GameObject purpleToSpawn;

    void Start(){
        enable = GetComponent<Slider>();

        Debug.Log(enable);
    }

    void Update(){
        changeBlockToSpawn();

        if(blockToSpawn == blockTypes.Infantry){
            redToSpawn = redInf;
            blueToSpawn = yellowInf;
            yellowToSpawn = blueInf;
            purpleToSpawn = purpleInf;
        }
        if(blockToSpawn == blockTypes.Artillery){
            redToSpawn = redArt;
            blueToSpawn = yellowArt;
            yellowToSpawn = blueArt;
            purpleToSpawn = purpleArt;
        }
        if(blockToSpawn == blockTypes.Officer){
            redToSpawn = redGen;
            blueToSpawn = yellowGen;
            yellowToSpawn = blueGen;
            purpleToSpawn = purpleGen;
        }
        if(blockToSpawn == blockTypes.Skirmisher){
            redToSpawn = redSkirm;
            blueToSpawn = yellowSkirm;
            yellowToSpawn = blueSkirm;
            purpleToSpawn = purpleSkirm;
        }
        if(blockToSpawn == blockTypes.Cavalry){
            redToSpawn = redCav;
            blueToSpawn = yellowCav;
            yellowToSpawn = blueCav;
            purpleToSpawn = purpleCav;
        }
        
        if(enable.value == 1){

        if(Input.GetKeyDown(KeyCode.Alpha1)){
            newBlock = Instantiate(redToSpawn, new Vector2(GetMousePos().x, GetMousePos().y), Quaternion.identity);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)){
            newBlock = Instantiate(blueToSpawn, new Vector2(GetMousePos().x, GetMousePos().y), Quaternion.identity);
        }
        if(Input.GetKeyDown(KeyCode.Alpha3)){
            newBlock = Instantiate(yellowToSpawn, new Vector2(GetMousePos().x, GetMousePos().y), Quaternion.identity);
        }
        if(Input.GetKeyDown(KeyCode.Alpha4)){
            newBlock = Instantiate(purpleToSpawn, new Vector2(GetMousePos().x, GetMousePos().y), Quaternion.identity);
        }

        }
    }

    private Vector2 GetMousePos(){
        Vector2 mousePos = Input.mousePosition;
        
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    private void changeBlockToSpawn(){
        if(Input.GetKeyDown(KeyCode.G)){
            blockToSpawn = blockTypes.Infantry;
        }
        if(Input.GetKeyDown(KeyCode.H)){
            blockToSpawn = blockTypes.Skirmisher;
        }
        if(Input.GetKeyDown(KeyCode.J)){
            blockToSpawn = blockTypes.Cavalry;
        }
        if(Input.GetKeyDown(KeyCode.K)){
            blockToSpawn = blockTypes.Artillery;
        }
        if(Input.GetKeyDown(KeyCode.L)){
            blockToSpawn = blockTypes.Officer;
        }
    }

    // void OnMouseDown(){
    //     Debug.Log("Ow");
    // }
}
