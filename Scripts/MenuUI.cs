using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    public int firstLevelBuildIndex = 1; 

    public void Play()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(firstLevelBuildIndex);
    }
    public void Quit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; 
#endif
    }
}
