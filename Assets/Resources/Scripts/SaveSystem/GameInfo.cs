using UnityEngine;
[System.Serializable]

public class GameInfo{
    public Vector3 position;
    public Quaternion rotation;
    public string type;
    public SpriteRenderer tiredness;
    public string health;

    public GameInfo(Vector3 position, Quaternion rotation, string type, SpriteRenderer tiredness, string health)
    {
        this.position = position;
        this.rotation = rotation;
        this.type = type;
        this.tiredness = tiredness;
        this.health = health;
    }
}