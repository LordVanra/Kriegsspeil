using UnityEngine;

public class SettingsManager : MonoBehaviour
{

    private GameObject settings;
    private GameObject settingsBG;
    private GameObject mainCamera;
    private Camera cameraObj;

    void Start(){
        settings = GameObject.Find("Settings");
        settingsBG = GameObject.Find("SettingsBG");
        mainCamera = GameObject.Find("Main Camera");
        cameraObj = GameObject.Find("Main Camera").GetComponent<Camera>();

        settings.SetActive(false);
        settingsBG.SetActive(false);
    }

    void Update(){
        settingsBG.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z+2f);
        settingsBG.transform.localScale = new Vector3(3f * cameraObj.orthographicSize, 1.5f * cameraObj.orthographicSize, 1f);
    }

    public void toggleSettings(){
        settings.SetActive(!settings.activeSelf);
        settingsBG.SetActive(!settingsBG.activeSelf);

        CameraBehavior.settingsClosed = !CameraBehavior.settingsClosed;
    }

}
