using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour{
    
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
    private FogMask maskScript;

    void Awake(){
        destroyPrefabs();
    }

    void Start(){

        enable = GetComponent<Slider>();

        maskScript = GameObject.Find("Fog").GetComponent<FogMask>();

        GameObject[] blockArr =  new GameObject[20] {redInf, blueInf, yellowInf, purpleInf, redArt, blueArt, yellowArt, purpleArt, redGen, blueGen, yellowGen, purpleGen, redSkirm, blueSkirm, yellowSkirm, purpleSkirm, redCav, blueCav, yellowCav, purpleCav};


        float n = -15f;

        if (this.name == "Spawn")
        {
            for (int i = 0; i < 20; i++)
            {
                try
                {
                    for (int j = 0; j < SceneChanger.spawnValues[i]; j++)
                    {
                        newBlock = Instantiate(blockArr[i], new Vector2(0f, 0f), Quaternion.identity);
                        if(i % 2 == 0){
                            maskScript.visionSourcesRed.Add(newBlock);
                            newBlock.transform.position = new Vector2(n, 0f);
                        }
                        else{
                            maskScript.visionSourcesBlue.Add(newBlock);
                            newBlock.transform.position = new Vector2(n, 0f);
                        }
                        n += 1.5f;
                    }
                }
                catch (System.NullReferenceException) { }
                catch (System.IndexOutOfRangeException){ }
                catch (System.Exception e)
                {
                    Debug.Log(e.Message);
                }
            }
        }
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

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                newBlock = Instantiate(redToSpawn, new Vector2(0,0), Quaternion.identity);
                maskScript.visionSourcesRed.Add(newBlock);
                newBlock.transform.position = new Vector2(GetMousePos().x, GetMousePos().y);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                newBlock = Instantiate(blueToSpawn, new Vector2(0,0), Quaternion.identity);
                maskScript.visionSourcesRed.Add(newBlock);
                newBlock.transform.position = new Vector2(GetMousePos().x, GetMousePos().y);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                newBlock = Instantiate(yellowToSpawn, new Vector2(0,0), Quaternion.identity);
                maskScript.visionSourcesBlue.Add(newBlock);
                newBlock.transform.position = new Vector2(GetMousePos().x, GetMousePos().y);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                newBlock = Instantiate(purpleToSpawn, new Vector2(0,0), Quaternion.identity);
                maskScript.visionSourcesBlue.Add(newBlock);
                newBlock.transform.position = new Vector2(GetMousePos().x, GetMousePos().y);
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
            blockToSpawn = blockTypes.Artillery;
        }
        if(Input.GetKeyDown(KeyCode.J)){
            blockToSpawn = blockTypes.Cavalry;
        }
        if(Input.GetKeyDown(KeyCode.K)){
            blockToSpawn = blockTypes.Officer;
        }
        if(Input.GetKeyDown(KeyCode.L)){
            blockToSpawn = blockTypes.Skirmisher;
        }
    }

    public void OnSliderClick(){
        
        // Debug.Log(enable.value);
        // enable.value = 1 - enable.value;
        // Debug.Log(enable.value);
    }
    
    private void destroyPrefabs(){
        string[] tags = {"Infantry", "Artillery", "Cavalry", "Officer", "Skirmisher"};
        for(int i = 0; i < 5; i++){
            GameObject[] prefabs = GameObject.FindGameObjectsWithTag(tags[i]);
            foreach (GameObject prefab in prefabs)
            {
                Destroy(prefab);
            }
        }
    }
}
