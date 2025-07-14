using UnityEngine;
using UnityEngine.SceneManagement; // この行が必須

public class SceneLoader : MonoBehaviour
{
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}