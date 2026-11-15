using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneSwitch : MonoBehaviour
{
    public TMP_Dropdown sceneDropdown;

    public void SwitchScene(int index)
    {
        if (sceneDropdown == null)
        {
            Debug.LogError("Scene Dropdown reference is missing in the Inspector!");
            return;
        }

        string sceneName = sceneDropdown.options[index].text;

        // Reset time scale just in case the game was paused
        Time.timeScale = 1f;

        // Check if the scene is valid before trying to load it
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Cannot load scene: '" + sceneName + "'. Make sure it is added to File > Build Settings and spelled correctly!");
        }
    }
}