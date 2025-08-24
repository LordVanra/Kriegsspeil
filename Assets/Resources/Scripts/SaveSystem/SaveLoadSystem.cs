using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using System.Collections.Generic;

public class SaveLoadSystem : MonoBehaviour
{
    private List<Transform> blocks = new List<Transform>();

    public GameObject saveQuit;

    public static bool loadOnScene;

    public static string prevScene;

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

                blockDataList.Add(new GameInfo(position, rotation, name, tiredness.color, ammo.color, health));
            }
        }

        File.WriteAllText(Application.persistentDataPath + "/gameSave.json", JsonUtility.ToJson(new BlockDataList(blockDataList)));
        blocks = new List<Transform>();
    }

    public void LoadGame()
    {
        destroyPrefabs();
        string path = Application.persistentDataPath + "/gameSave.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            BlockDataList blockDataList = JsonUtility.FromJson<BlockDataList>(json);

            foreach (GameInfo block in blockDataList.blocks)
            {

                block.type = block.type.Split('(')[0];
                GameObject b = Instantiate(Resources.Load<GameObject>("Images/Pieces/Prefabs/" + block.type), block.position, block.rotation);

                b.GetComponent<BoxCollider2D>().enabled = true;
                b.GetComponent<Drag>().enabled = true;
                b.GetComponentInChildren<Canvas>().enabled = true;

                b.gameObject.GetComponent<Drag>().loaded = true;

                b.GetComponentsInChildren<SpriteRenderer>()[1].color = block.tiredness;
                b.GetComponentsInChildren<SpriteRenderer>()[2].color = block.ammo;

                b.gameObject.GetComponentInChildren<TextMeshProUGUI>(true).text = block.health;
                b.gameObject.GetComponent<Drag>().troops = int.Parse(block.health);
            }
        }
        else
        {
            Debug.Log("Save file not found!");
        }
    }

    private void destroyPrefabs()
    {
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

    [System.Serializable]
    private class BlockDataList
    {
        public List<GameInfo> blocks;

        public BlockDataList(List<GameInfo> blockList)
        {
            blocks = blockList;
        }
    }
    

}
