using UnityEngine;

public class SettingsManager : MonoBehaviour
{

    private GameObject settings;
    private GameObject settingsBG;
    private GameObject saveQuit;
    private GameObject mainCamera;
    private Camera cameraObj;

    void Start()
    {
       
        settings = GameObject.Find("Settings");
        saveQuit = GameObject.Find("SaveQuit");
        settingsBG = GameObject.Find("SettingsBG");
        mainCamera = GameObject.Find("Main Camera");
        cameraObj = mainCamera.GetComponent<Camera>();
        if (this.name == "Button")
        {
            saveQuit.SetActive(false);
            settings.SetActive(false);
            settingsBG.SetActive(false);
        }
    }

    void Update()
    {
        if (this.name == "Button")
        {
            settingsBG.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z + 2f);
            settingsBG.transform.localScale = new Vector3(0.2f * cameraObj.orthographicSize, 0.15f * cameraObj.orthographicSize, 1f);

            if (Input.GetKey(KeyCode.Escape) && settings.activeSelf)
            {
                toggleSettings();
            }
        }
    }

    public void toggleSettings()
    {
        settings.SetActive(!settings.activeSelf);
        settingsBG.SetActive(!settingsBG.activeSelf);
        if(GameObject.Find("CombatCanv")){
            GameObject.Find("Combat").GetComponent<CombatHandler>().toggleCombat();
        }
        CameraBehavior.settingsClosed = !CameraBehavior.settingsClosed;

        // GameObject.Find("ToMenu").SetActive(false);
        // GameObject.Find("ToDesktop").SetActive(false);
        // GameObject.Find("SaveQuitBG").SetActive(false);
        // GameObject.Find("SaveQuitX").SetActive(false);
    }

}
