using UnityEngine;
[System.Serializable]

public class GameInfo : MonoBehaviour{
    public Vector3 position;
    public Quaternion rotation;
    public string type;

    public GameInfo(Vector3 position, Quaternion rotation, string type)
    {
        this.position = position;
        this.rotation = rotation;
        this.type = type;
    }
}