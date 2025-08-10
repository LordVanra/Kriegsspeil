using UnityEngine;

public class QuitApplication : MonoBehaviour
{
    public void QuitGame()
    {
        Debug.Log("Quit button pressed!");
        Application.Quit();

        // If running in the Editor, stop playing
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}