using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Drag : MonoBehaviour
{

    private bool isDragging = false;
    private TextMeshProUGUI text1;
    private Canvas canvas;
    private Transform capsuleTransform;
    private Transform ammoTransform;
    public SpriteRenderer capsuleColor;
    public SpriteRenderer ammoColor;
    private const float MAXPACE = 3f;
    private int roadMult = 1;
    public float speedMultiplier = 1f;
    public float disOrgMult = 1f;
    public Vector2 startPos;
    public int troops;
    public bool loaded;
    public int shots;
    public int MAXSHOTS;
    
    public void Start()
    {

        canvas = GetComponentInChildren<Canvas>();
        text1 = canvas.GetComponentInChildren<TextMeshProUGUI>(true);

        capsuleTransform = GetComponentsInChildren<Transform>()[1];
        ammoTransform = GetComponentsInChildren<Transform>()[2];

        capsuleColor = GetComponentsInChildren<SpriteRenderer>()[1];
        ammoColor = GetComponentsInChildren<SpriteRenderer>()[2];

        text1.text = troops.ToString(); ;
        text1.alignment = TextAlignmentOptions.Center;
        text1.fontStyle = FontStyles.Bold;

        if (!loaded)
        {
            capsuleColor.color = new Color(0.1f, 0.9f, 0f, 1f);
            ammoColor.color = new Color(0.1f, 0.9f, 0f, 1f);
        }

        string type = this.tag;

        switch (type)
        {
            case "Infantry":
                MAXSHOTS = shots = 18;
                break;
            case "Artillery":
                MAXSHOTS = shots = 40;
                break;
            case "Cavalry":
                MAXSHOTS = shots = 1000;
                break;
            case "Officer":
                MAXSHOTS = shots = 1000;
                break;
            case "Skirmisher":
                MAXSHOTS = shots = 12;
                break;
        }

        startPos = new Vector2(transform.position.x, transform.position.y);
    }

    void Update()
    {
        if (isDragging && GameObject.Find("Combat").GetComponent<Slider>().value == 0f &&  GameObject.Find("FogMode").GetComponent<Slider>().value == 1f)
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                roadMult = 2;
            }
            else
            {
                roadMult = 1;
            }
            if (Vector2.Distance(GetMousePos(), startPos) < MAXPACE * speedMultiplier * roadMult * disOrgMult || GameObject.Find("PassTurn").GetComponent<PassTurn>().turn == 0)
            {
                transform.position = GetMousePos();
                // paceLeft = MAXPACE-Vector2.Distance(transform.position, startPos);
            }
            if (Input.GetKey(KeyCode.Q))
            {
                transform.eulerAngles = new Vector3(0f, 0f, transform.eulerAngles.z + 0.5f);
            }
            else if (Input.GetKey(KeyCode.E))
            {
                transform.eulerAngles = new Vector3(0f, 0f, transform.eulerAngles.z - 0.5f);
            }
        }

        text1.transform.position = transform.position + new Vector3(-0.5f * Mathf.Sin(Mathf.Deg2Rad * transform.eulerAngles.z), 0.5f * Mathf.Cos(Mathf.Deg2Rad * transform.eulerAngles.z), 0f);
        text1.transform.eulerAngles = transform.eulerAngles;

        capsuleTransform.transform.position = transform.position + new Vector3(0.5f * Mathf.Sin(Mathf.Deg2Rad * transform.eulerAngles.z), -0.5f * Mathf.Cos(Mathf.Deg2Rad * transform.eulerAngles.z), 0f);
        capsuleTransform.transform.eulerAngles = transform.eulerAngles + new Vector3(0, 0, 90f);
        
        ammoTransform.transform.position = transform.position + new Vector3(0.75f * Mathf.Sin(Mathf.Deg2Rad*transform.eulerAngles.z), -0.75f * Mathf.Cos(Mathf.Deg2Rad*transform.eulerAngles.z), 0f);
        ammoTransform.transform.eulerAngles = transform.eulerAngles + new Vector3(0, 0, 90f);
    }

    public void resetInfo(){
        // paceLeft = MAXPACE;
        startPos = new Vector2(transform.position.x, transform.position.y);
    }

    public void updateTiredness(){

        Color redden = capsuleColor.color;
        if ((Vector2.Distance(transform.position, startPos) - 1.5f) > 1){
            redden += new Color(0.12f * (Vector2.Distance(transform.position, startPos) - 1.5f), -0.12f * (Vector2.Distance(transform.position, startPos) - 1.5f), 0f, 0f);
        }
        else{
            redden += new Color(0.04f * (Vector2.Distance(transform.position, startPos) - 1.5f), -0.04f * (Vector2.Distance(transform.position, startPos) - 1.5f), 0f, 0f);
        }
        redden.a = 1f;

        capsuleColor.color = redden;
        if(capsuleColor.color.r < 0.1){
            capsuleColor.color = new Color(0.1f, 0.9f, 0f, 1f);
        }
        else if(capsuleColor.color.r > 0.9){
            capsuleColor.color = new Color(0.9f, 0.1f, 0f, 1f);
        }
    }

    private Vector2 GetMousePos(){
        Vector2 mousePos = Input.mousePosition;
        
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    void OnMouseDown(){
        isDragging = true;
    }

    void OnMouseUp(){
        isDragging = false;
    }
}
