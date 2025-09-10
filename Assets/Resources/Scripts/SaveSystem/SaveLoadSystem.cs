using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using System.Collections.Generic;

public class SaveLoadSystem : MonoBehaviour
{
    private List<Transform> blocks = new List<Transform>();

    public GameObject saveQuit;
    public GameObject saveMenu;
    public GameObject loadMenu;

    public static bool loadOnScene;
    public static string prevScene;

    private int slot;

    void Awake(){
        if(gameObject.name == "Load" || gameObject.name == "Button" ){
            Debug.Log(prevScene);
            Debug.Log(loadOnScene);
            if(SceneManager.GetActiveScene().name == "Game" && (loadOnScene || prevScene == "Game")){
                LoadGame();
                loadOnScene = false;
            }

            prevScene = SceneManager.GetActiveScene().name;
        }
    }

    void setupBlocks()
    {
        if (FindObjectsByType<Transform>(FindObjectsSortMode.None) != null)
        {
            Transform[] transforms = FindObjectsByType<Transform>(FindObjectsSortMode.None);
            if (transforms != null)
            {
                foreach (Transform transform in transforms)
                {
                    if (transform.gameObject.name.Split(' ')[0] == "R" || transform.gameObject.name.Split(' ')[0] == "B")
                    {
                        blocks.Add(transform);
                    }
                }
            }
        }
    }

    public void SaveGame()
    {
        setupBlocks();

        List<GameInfo> blockDataList = new List<GameInfo>();

        if (blocks != null)
        {
            foreach (Transform block in blocks)
            {
                Vector3 position = block.position;
                Quaternion rotation = block.rotation;
                string name = block.gameObject.name;
                SpriteRenderer tiredness = block.gameObject.GetComponentsInChildren<SpriteRenderer>()[1];
                SpriteRenderer ammo = block.gameObject.GetComponentsInChildren<SpriteRenderer>()[2];
                string health = block.gameObject.GetComponentInChildren<TextMeshProUGUI>(true).text;
                Vector2 startPos = block.gameObject.GetComponent<Drag>().startPos;

                blockDataList.Add(new GameInfo(position, rotation, name, tiredness.color, ammo.color, health, startPos));
            }
        }
        string fileName;

        #if UNITY_EDITOR
            fileName = "/gameSave_Editor";
        #elif UNITY_STANDALONE
            fileName = "/gameSave_Standalone";
        #elif UNITY_ANDROID
            fileName = "/gameSave_Android";
        #elif UNITY_IOS
            fileName = "/gameSave_iOS";
        #endif

        File.WriteAllText(Application.persistentDataPath + fileName + slot + ".json", JsonUtility.ToJson(new BlockDataList(blockDataList, GameObject.Find("PassTurn").GetComponent<PassTurn>().turn)));
        blocks = new List<Transform>();
    }

    public void LoadGame()
    {
        destroyPrefabs();

        string fileName;

#if UNITY_EDITOR
            fileName = "/gameSave_Editor";
#elif UNITY_STANDALONE
            fileName = "/gameSave_Standalone";
#elif UNITY_ANDROID
            fileName = "/gameSave_Android";
#elif UNITY_IOS
            fileName = "/gameSave_iOS";
#endif

        Debug.Log("Slot set to: " + slot);
        Debug.Log("Slot set to: " + this.slot);

        string path = Application.persistentDataPath + fileName + slot + ".json";

        Debug.Log(path);

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Debug.Log(json);
            BlockDataList blockDataList = JsonUtility.FromJson<BlockDataList>(json);

            FogMask fog = GameObject.Find("Fog").GetComponent<FogMask>();

            foreach (GameInfo block in blockDataList.blocks)
            {

                block.type = block.type.Split('(')[0];
                GameObject b = Instantiate(Resources.Load<GameObject>("Images/Pieces/Prefabs/" + block.type), block.position, block.rotation);

                if (block.type.Split(' ')[0] == "R")
                {
                    fog.visionSourcesRed.Add(b);
                }
                else
                {
                    fog.visionSourcesBlue.Add(b);
                }

                b.GetComponent<BoxCollider2D>().enabled = true;
                b.GetComponent<Drag>().enabled = true;
                b.GetComponentInChildren<Canvas>().enabled = true;

                b.gameObject.GetComponent<Drag>().loaded = true;

                b.GetComponentsInChildren<SpriteRenderer>()[1].color = block.tiredness;
                b.GetComponentsInChildren<SpriteRenderer>()[2].color = block.ammo;

                b.gameObject.GetComponentInChildren<TextMeshProUGUI>(true).text = block.health;
                b.gameObject.GetComponent<Drag>().troops = int.Parse(block.health);
                b.gameObject.GetComponent<Drag>().startPos = block.startPos;
                Debug.Log(b.gameObject.GetComponent<Drag>().startPos);
            }

            GameObject.Find("PassTurn").GetComponent<PassTurn>().turn = blockDataList.turn;
        }
        else
        {
            Debug.Log("Save file not found!");
        }
    }

    private void destroyPrefabs()
    {
        GameObject.Find("Fog").GetComponent<FogMask>().visionSourcesRed.Clear();
        GameObject.Find("Fog").GetComponent<FogMask>().visionSourcesBlue.Clear();
        
        string[] tags = { "Infantry", "Artillery", "Cavalry", "Officer", "Skirmisher" };
        for (int i = 0; i < 5; i++)
        {
            GameObject[] prefabs = GameObject.FindGameObjectsWithTag(tags[i]);
            foreach (GameObject prefab in prefabs)
            {
                Destroy(prefab);
            }
        }
    }

    public void saveAndQuit()
    {
        saveQuit.SetActive(!saveQuit.activeSelf);
    }

    public void toggleSaveMenu(){
        saveMenu.SetActive(!saveMenu.activeSelf);
    }

    public void toggleLoadMenu(){
        loadMenu.SetActive(!loadMenu.activeSelf);
    }

    public void setSlot(int slot)
    {
        this.slot = slot;
        Debug.Log("Slot Changed to:" + slot);
        Debug.Log("Slot Changed to: " + this.slot);
    }

    [System.Serializable]
    private class BlockDataList
    {
        public List<GameInfo> blocks;
        public int turn;

        public BlockDataList(List<GameInfo> blockList, int turn)
        {
            this.turn = turn;
            blocks = blockList;
        }
    }
    
}