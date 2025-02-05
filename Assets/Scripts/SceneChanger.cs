using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour{

    public void openGame(){
        SceneManager.LoadScene("Scenes/Game");
    }

    public void openSpawner(){
        SceneManager.LoadScene("Scenes/SpawnMenu");
    }
}
