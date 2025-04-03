using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class SaveLoadSystem : MonoBehaviour{
    private List<Transform> blocks; 

    void setupBlocks(){
        if (FindObjectsByType<Transform>(FindObjectsSortMode.None) != null){
            Transform[] transforms = FindObjectsByType<Transform>(FindObjectsSortMode.None);
            foreach (Transform transform in transforms){
                Debug.Log(transform);
                blocks.Add(transform);
            }
        }
    }

    public void SaveGame(){
        setupBlocks();

        List<GameInfo> blockDataList = new List<GameInfo>();

        foreach (Transform block in blocks)
        {
            Vector3 position = block.position;
            Quaternion rotation = block.rotation;
            string tag = block.gameObject.tag;
            blockDataList.Add(new GameInfo(position, rotation, tag));
        }

        File.WriteAllText(Application.persistentDataPath + "/gameSave.json", JsonUtility.ToJson(new BlockDataList(blockDataList)));
    }

    public void LoadGame(){
        string path = Application.persistentDataPath + "/gameSave.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            BlockDataList blockDataList = JsonUtility.FromJson<BlockDataList>(json);

            // Loop through all blocks and apply their saved position and rotation
            for (int i = 0; i < blockDataList.blocks.Count; i++)
            {
                blocks[i].transform.position = blockDataList.blocks[i].position;
                blocks[i].transform.rotation = blockDataList.blocks[i].rotation;
                blocks[i].tag = blockDataList.blocks[i].type;
            }
        }
        else
        {
            Debug.Log("Save file not found!");
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
