using UnityEngine;
[System.Serializable]

public class GameInfo : MonoBehaviour{
    public Vector3 position;
    public Quaternion rotation;
    public string tag;

    public GameInfo(Vector3 position, Quaternion rotation, string tag)
    {
        this.position = position;
        this.rotation = rotation;
        this.tag = tag;
    }
}