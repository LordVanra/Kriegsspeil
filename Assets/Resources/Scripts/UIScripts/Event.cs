using UnityEngine;
[System.Serializable]

public class Event{
    public string id;
    public string title;
    public string desc;
    public int length;

    public Event(string id, string title, string desc, int length)
    {
        this.id = id;
        this.title = title;
        this.desc = desc;
        this.length = length;
    }
}