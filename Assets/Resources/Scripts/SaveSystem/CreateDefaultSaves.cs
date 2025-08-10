using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class CreateDefaultSaves : MonoBehaviour
{
    void Start()
    {
        List<GameInfo> blockDataList = new List<GameInfo>();

        File.WriteAllText(Application.persistentDataPath + "/emptyGameSave.json", JsonUtility.ToJson(new BlockDataList(blockDataList)));

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
