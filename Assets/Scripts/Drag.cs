using UnityEngine;
using TMPro;

public class Drag : MonoBehaviour
{

    private bool isDragging = false;
    private TextMeshProUGUI text1;
    private Canvas canvas;
    private Vector2 clickPos;
    private Transform capsuleTransform;
    private SpriteRenderer capsuleColor;

    public float paceLeft = 3f; 
    public int speedMultiplier;
    public Vector2 startPos;
    public int troops;
    
    public void Start(){
        canvas = GetComponentInChildren<Canvas>();
        text1 = canvas.GetComponentInChildren<TextMeshProUGUI>(true);

        capsuleTransform = GetComponentsInChildren<Transform>()[1];

        capsuleColor = GetComponentsInChildren<SpriteRenderer>()[1];

        text1.text = troops.ToString();;
        text1.alignment = TextAlignmentOptions.Center;
        text1.fontStyle = FontStyles.Bold;

        capsuleColor.color = new Color(0f, 1f, 0f);
        
        startPos = new Vector2(transform.position.x, transform.position.y);
    }

    void Update(){
        if(isDragging){
            if(Vector2.Distance(GetMousePos(), startPos) < paceLeft * speedMultiplier + 2f){
                transform.position = GetMousePos();
                paceLeft = 3f-Vector2.Distance(transform.position, startPos);
            }
            if(Input.GetKey(KeyCode.Q)){
                transform.eulerAngles = new Vector3(0f, 0f, transform.eulerAngles.z+0.5f);
            }
            else if(Input.GetKey(KeyCode.E)){
                transform.eulerAngles = new Vector3(0f, 0f, transform.eulerAngles.z-0.5f);
            }
        }
        
        text1.transform.position = transform.position + new Vector3(-0.5f*Mathf.Sin(Mathf.Deg2Rad*transform.eulerAngles.z), 0.5f*Mathf.Cos(Mathf.Deg2Rad*transform.eulerAngles.z), 0f);
        text1.transform.eulerAngles = transform.eulerAngles;

        capsuleTransform.transform.position = transform.position + new Vector3(-0.5f*Mathf.Sin(Mathf.Deg2Rad*transform.eulerAngles.z), 0.5f*Mathf.Cos(Mathf.Deg2Rad*transform.eulerAngles.z), 0f);
        capsuleTransform.transform.eulerAngles = transform.eulerAngles + new Vector3(0, 0, 90f);
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
