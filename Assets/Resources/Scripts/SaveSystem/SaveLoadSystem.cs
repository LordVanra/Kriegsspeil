using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;

public class SaveLoadSystem : MonoBehaviour{
    private List<Transform> blocks = new List<Transform>(); 

    void setupBlocks(){
        if (FindObjectsByType<Transform>(FindObjectsSortMode.None) != null){
            Transform[] transforms = FindObjectsByType<Transform>(FindObjectsSortMode.None);
            if(transforms != null){
                foreach (Transform transform in transforms){
                    if(transform.gameObject.name.Split(' ')[0] == "R" || transform.gameObject.name.Split(' ')[0] == "B"){
                        blocks.Add(transform);
                    }
                }
            }
        }
    }

    public void SaveGame(){
        setupBlocks();

        List<GameInfo> blockDataList = new List<GameInfo>();

        if(blocks != null){
            foreach (Transform block in blocks)
            {
                Vector3 position = block.position;
                Quaternion rotation = block.rotation;
                string name = block.gameObject.name;
                SpriteRenderer tiredness = block.gameObject.GetComponentsInChildren<SpriteRenderer>()[1];
                string health = block.gameObject.GetComponentInChildren<TextMeshProUGUI>(true).text;

                blockDataList.Add(new GameInfo(position, rotation, name, tiredness, health));
            }
        }

        File.WriteAllText(Application.persistentDataPath + "/gameSave.json", JsonUtility.ToJson(new BlockDataList(blockDataList)));
        blocks = new List<Transform>();
    }

    public void LoadGame(){
        destroyPrefabs();
        string path = Application.persistentDataPath + "/gameSave.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            BlockDataList blockDataList = JsonUtility.FromJson<BlockDataList>(json);

            // Loop through all blocks and apply their saved position and rotation
            // for (int i = 0; i < blockDataList.blocks.Count; i++)
            // {
            //     blocks[i].transform.position = blockDataList.blocks[i].position;
            //     blocks[i].transform.rotation = blockDataList.blocks[i].rotation;
            //     blocks[i].tag = blockDataList.blocks[i].type;
            // }
            foreach (GameInfo block in blockDataList.blocks)
            {
                block.type = block.type.Split('(')[0];
                GameObject b = Instantiate(Resources.Load<GameObject>("Images/Pieces/Prefabs/" + block.type), block.position, block.rotation);

                b.GetComponent<BoxCollider2D>().enabled = true;
                b.GetComponent<Drag>().enabled = true;
                b.GetComponentInChildren<Canvas>().enabled = true;

                b.gameObject.GetComponent<Drag>().loaded = true;
                b.GetComponentsInChildren<SpriteRenderer>()[1].color = block.tiredness.color;
                Debug.Log(b.GetComponentsInChildren<SpriteRenderer>()[1].color);

                b.gameObject.GetComponentInChildren<TextMeshProUGUI>(true).text = block.health;
                b.gameObject.GetComponent<Drag>().troops = int.Parse(block.health);
            }
        }
        else
        {
            Debug.Log("Save file not found!");
        }
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
    
    [System.Serializable]
    private class BlockDataList{
        public List<GameInfo> blocks;

        public BlockDataList(List<GameInfo> blockList)
        {
            blocks = blockList;
        }
    }

}
