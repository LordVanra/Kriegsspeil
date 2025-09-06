using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneChanger : MonoBehaviour
{
    public TMP_InputField[] inputs;
    public static int[] spawnValues;
    public Sprite[] menus = new Sprite[5];
    private GameObject bg;

    void Start()
    {
        spawnValues = new int[inputs.Length];
        bg = GameObject.Find("MainBackground");

        int a = Random.Range(0, 5);
        bg.GetComponent<SpriteRenderer>().sprite = menus[a];

    }

    private bool CollectInputs()
    {
        for (int i = 0; i < inputs.Length; i++)
        {
            try
            {
                spawnValues[i] = int.Parse(inputs[i].text);
                if (spawnValues[i] < 0)
                {
                    return false;
                }
            }
            catch (System.NullReferenceException)
            {
                spawnValues[i] = 0;
            }
            catch (System.FormatException) { }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
            }
        }
        return true;
    }

    public void openGame(bool load)
    {
        if (CollectInputs())
        {
            SceneManager.LoadScene("Scenes/Game");
            SaveLoadSystem.loadOnScene = load;
            CameraBehavior.settingsClosed = true;
        }
        else
        {
            Debug.Log("Bad Input");
        }
    }

    public void openSpawner()
    {
        SceneManager.LoadScene("Scenes/SpawnMenu");
    }

    public void openMainMenu()
    {
        SceneManager.LoadScene("Scenes/MainMenu");
        
        int a = Random.Range(0, 5);
        bg.GetComponent<SpriteRenderer>().sprite = menus[a];
    }
}
