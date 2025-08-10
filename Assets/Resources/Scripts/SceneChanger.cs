using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneChanger : MonoBehaviour{
    public TMP_InputField[] inputs;
    public static int[] spawnValues;
    public Sprite[] menus = new Sprite[5];
    private GameObject bg;

    void Start(){
        spawnValues = new int[inputs.Length];
        bg = GameObject.Find("MainBackground");

        int a = Random.Range(0, 5);
        Debug.Log(bg.GetComponent<SpriteRenderer>().sprite);
        bg.GetComponent<SpriteRenderer>().sprite = menus[a];

    }

    private void collectInputs(){
        for(int i = 0; i < inputs.Length; i++){
            try{
                spawnValues[i] = int.Parse(inputs[i].text);
            }
            catch(System.NullReferenceException){
                spawnValues[i] = 0;
            }
            catch(System.FormatException){}
            catch(System.Exception e){
                Debug.Log(e.Message);
            }
        }
    }

    public void openGame(){
        collectInputs();
        SceneManager.LoadScene("Scenes/Game");
    }

    public void openSpawner(){
        SceneManager.LoadScene("Scenes/SpawnMenu");
    }
}
