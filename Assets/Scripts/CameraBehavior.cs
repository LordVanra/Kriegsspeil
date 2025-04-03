using UnityEngine;

public class CameraBehavior : MonoBehaviour
{

    private Camera cam;
    private float speedMultiplier = 8f;
    public static bool settingsClosed = true;
    public static bool combatClosed = true;

    void Start()
    {
        cam = GetComponent<Camera>();

        transform.position = new Vector3(0f, 0f, -10f);
        cam.orthographicSize = 10f;
    }

    void Update()
    {
        if (settingsClosed && combatClosed)
        {

            float scroll = Input.GetAxis("Mouse ScrollWheel");

            if (scroll != 0)
            {
                cam.orthographicSize -= scroll * 2f;
                cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, 1f, 40f);
            }

            Vector2 movement = new Vector2(0f, 0f);

            if (Input.GetKey(KeyCode.W)) movement.y += 1;
            if (Input.GetKey(KeyCode.S)) movement.y -= 1;
            if (Input.GetKey(KeyCode.A)) movement.x -= 1;
            if (Input.GetKey(KeyCode.D)) movement.x += 1;

            if (Input.GetKey(KeyCode.LeftControl))
            {
                speedMultiplier = 4f;
            }
            else
            {
                speedMultiplier = Input.GetKey(KeyCode.LeftShift) ? 16f : 8f;
            }

            movement = movement.normalized * Time.deltaTime * speedMultiplier;

            transform.position = new Vector3(transform.position.x + movement.x, transform.position.y + movement.y, transform.position.z);

        }
    }
}