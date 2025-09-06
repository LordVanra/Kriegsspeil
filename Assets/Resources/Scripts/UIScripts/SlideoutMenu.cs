using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SlideoutMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public float slideDistance = 400f; // How far the panel slides out
    private float animationSpeed = 1f; // Speed of slide animation
    private AnimationCurve slideCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    public GameObject popupPrefab; // Assign your popup prefab here
    private Canvas popupCanvas; // Canvas to spawn popup on

    private RectTransform rectTransform;
    public Vector3 hiddenPosition;
    public Vector3 revealedPosition;
    private GameObject currentPopup;
    private bool isMouseOver = false;
    private bool shouldBeRevealed = false; // Track desired state
    private bool popped = false;

    public string eventID;
    public string eventTitle;
    public string eventText;
    public int eventDuration;
    public int eventStart;
    private GameObject passButton;

    // Animation coroutine reference
    private Coroutine slideCoroutine;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        hiddenPosition = rectTransform.anchoredPosition;
        revealedPosition = hiddenPosition + Vector3.right * slideDistance;

        popupCanvas = FindFirstObjectByType<Canvas>();

        passButton = GameObject.Find("PassTurn");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && currentPopup && !isMouseOver)
        {
            ClosePopup();
        }
        if (passButton.GetComponent<PassTurn>().turn == eventStart + eventDuration)
        {
            DestroyEvent();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
        shouldBeRevealed = true;

        // Always start slide out animation, even if currently animating
        if (slideCoroutine != null)
        {
            StopCoroutine(slideCoroutine);
        }

        slideCoroutine = StartCoroutine(AnimateToPosition(revealedPosition, true));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
        shouldBeRevealed = false;

        // Always start slide in animation, even if currently animating
        if (slideCoroutine != null)
        {
            StopCoroutine(slideCoroutine);
        }

        slideCoroutine = StartCoroutine(AnimateToPosition(hiddenPosition, false));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!popped)
        {
            currentPopup = Instantiate(popupPrefab, new Vector3(GameObject.Find("Main Camera").GetComponent<Camera>().transform.position.x, GameObject.Find("Main Camera").GetComponent<Camera>().transform.position.y, -1), Quaternion.identity);
            currentPopup.transform.localScale = new Vector3(GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize * 1.2f, GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize * 1.2f, 1);
            currentPopup.GetComponentsInChildren<TextMeshProUGUI>(true)[0].text = eventText;
            currentPopup.GetComponentsInChildren<TextMeshProUGUI>(true)[1].text = eventTitle;
        }
        else
        {
            ClosePopup();
        }
        popped = !popped;
        CameraBehavior.popupClosed = !CameraBehavior.popupClosed;
    }

    private System.Collections.IEnumerator AnimateToPosition(Vector3 targetPosition, bool revealingPanel)
    {
        Vector3 startPosition = rectTransform.anchoredPosition;
        float elapsedTime = 0f;
        float animationDuration = 1f / animationSpeed;

        while (elapsedTime < animationDuration)
        {
            // Check if the desired state changed during animation
            if (shouldBeRevealed != revealingPanel)
            {
                yield break;
            }

            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / animationDuration;
            float curveValue = slideCurve.Evaluate(progress);

            rectTransform.anchoredPosition = Vector3.Lerp(startPosition, targetPosition, curveValue);
            yield return null;
        }

        rectTransform.anchoredPosition = targetPosition;
    }

    public void ClosePopup()
    {
        if (currentPopup != null)
        {
            // Destroy the blocker first
            Transform blocker = popupCanvas.transform.Find("PopupBlocker");
            if (blocker != null)
            {
                DestroyImmediate(blocker.gameObject);
            }

            Destroy(currentPopup);
            currentPopup = null;
        }
    }

    public void DestroyEvent()
    {
        passButton.GetComponent<PassTurn>().RecalculateEventPositions((GetComponent<RectTransform>().offsetMax.y + 200) / -100);
        Destroy(this.gameObject);
    }
}