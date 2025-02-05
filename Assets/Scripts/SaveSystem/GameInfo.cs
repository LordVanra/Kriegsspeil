using UnityEngine;
[System.Serializable]

public class GameInfo : MonoBehaviour{
    public Vector3 position;
    public Quaternion rotation;

    public GameInfo(Vector3 position, Quaternion rotation)
    {
        this.position = position;
        this.rotation = rotation;
    }
}