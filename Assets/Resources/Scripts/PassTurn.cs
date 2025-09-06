using UnityEngine;
using System.IO;
using System.Collections.Generic;
public class PassTurn : MonoBehaviour
{

    public int turn;
    private Drag[] blocks;
    private FogMask maskScript;
    public GameObject slidePopup;
    public GameObject randomEvent;
    private List<Event> eventList = new List<Event>();
    private List<GameObject> activeEvents = new List<GameObject>();
    int n = 0;
    void Start()
    {
        blocks = FindObjectsByType<Drag>(FindObjectsSortMode.None);
        maskScript = GameObject.Find("Fog").GetComponent<FogMask>();
        turn = 0;
        LoadEvents();
    }

    public void NextTurn()
    {
        blocks = FindObjectsByType<Drag>(FindObjectsSortMode.None);

        turn++;
        // Debug.Log(turn);

        maskScript.ClearFog();
        foreach (Drag block in blocks)
        {
            if (turn != 1)
            {
                block.updateTiredness();
            }
            block.resetInfo();
            GameObject.Find("Combat").GetComponent<CombatHandler>().clearCombat();
        }

        CalcRandomEvent();
    }

    public void CalcRandomEvent()
    {
        if (turn > 2 && UnityEngine.Random.Range(1, 5) == 1)
        {
            GameObject newEvent = Instantiate(randomEvent);
            activeEvents.Add(newEvent);

            SlideoutMenu slideout = newEvent.AddComponent<SlideoutMenu>();
            slideout.popupPrefab = slidePopup;

            int eventNum = UnityEngine.Random.Range(1, eventList.Count);

            slideout.eventID = eventList[eventNum].id;
            slideout.eventTitle = eventList[eventNum].title;
            slideout.eventText = eventList[eventNum].desc;
            slideout.eventDuration = eventList[eventNum].length;
            slideout.eventStart = turn;

            newEvent.transform.SetParent(GameObject.Find("UI").transform, false);
            newEvent.transform.SetSiblingIndex(0);

            newEvent.GetComponent<RectTransform>().offsetMax = new Vector2(newEvent.GetComponent<RectTransform>().offsetMax.x, -100 * n - 200);
            newEvent.GetComponent<RectTransform>().offsetMin = new Vector2(newEvent.GetComponent<RectTransform>().offsetMin.x, -100 * n + 840);
            n += 1;
        }
    }

    public void LoadEvents()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("events"); // no extension
        string json = jsonFile.text;
        EventDataList eventDataList = JsonUtility.FromJson<EventDataList>(json);
        foreach (Event events in eventDataList.events)
        {
            eventList.Add(events);
        }
    }

    public void RecalculateEventPositions(float destroyedN)
    {
        n -= 1;

        Debug.Log(activeEvents.Count);
        foreach (GameObject activeEvent in activeEvents)
        {
            if (activeEvents.IndexOf(activeEvent) > destroyedN)
            {
                Debug.Log("Pre: " + activeEvent.GetComponent<RectTransform>().offsetMax.y);
                activeEvent.GetComponent<RectTransform>().offsetMax = new Vector2(activeEvent.GetComponent<RectTransform>().offsetMax.x, -100 * (activeEvents.IndexOf(activeEvent) - 1) - 200);
                activeEvent.GetComponent<RectTransform>().offsetMin = new Vector2(activeEvent.GetComponent<RectTransform>().offsetMin.x, -100 * (activeEvents.IndexOf(activeEvent) - 1) + 840);

                activeEvent.GetComponent<SlideoutMenu>().hiddenPosition = activeEvent.GetComponent<RectTransform>().anchoredPosition;
                activeEvent.GetComponent<SlideoutMenu>().revealedPosition = activeEvent.GetComponent<SlideoutMenu>().hiddenPosition + Vector3.right * activeEvent.GetComponent<SlideoutMenu>().slideDistance;
            }
        }
        
        activeEvents.RemoveAt((int)destroyedN);
    }
    
    [System.Serializable]
    private class EventDataList
    {
        public List<Event> events;
        public int turn;

        public EventDataList(List<Event> events, int turn)
        {
            this.turn = turn;
            this.events = events;
        }
    }
}
