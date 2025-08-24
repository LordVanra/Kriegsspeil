using UnityEngine;
[System.Serializable]

public class GameInfo{
    public Vector3 position;
    public Quaternion rotation;
    public string type;
    public Color tiredness;
    public Color ammo;
    public string health;

    public GameInfo(Vector3 position, Quaternion rotation, string type, Color tiredness, Color ammo, string health)
    {
        this.position = position;
        this.rotation = rotation;
        this.type = type;
        this.tiredness = tiredness;
        this.ammo = ammo;
        this.health = health;
    }
}