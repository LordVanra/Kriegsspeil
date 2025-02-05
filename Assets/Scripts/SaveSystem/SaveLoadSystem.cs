using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class SaveLoadSystem : MonoBehaviour{
    public List<Transform> blocks; 

    void Start(){
        Transform[] transforms = FindObjectsByType<Transform>(FindObjectsSortMode.None);
        foreach (Transform transform in transforms){
            blocks.Add(transform);
        }
    }

    public void SaveGame()
    {
        List<GameInfo> blockDataList = new List<GameInfo>();

        foreach (Transform block in blocks)
        {
            Vector3 position = block.position;
            Quaternion rotation = block.rotation;
            blockDataList.Add(new GameInfo(position, rotation));
        }
        
        Debug.Log("Saving");
        Debug.Log(Application.persistentDataPath);
        File.WriteAllText(Application.persistentDataPath + "/gameSave.json", JsonUtility.ToJson(new BlockDataList(blockDataList)));
        Debug.Log("Saved");
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
