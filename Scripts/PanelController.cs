using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelController : MonoBehaviour
{
    public GameObject failPanel, winPanel;
    SpawnPath[] sp; bool ended;
    void Start(){
        Time.timeScale = 1f;
        if (failPanel) failPanel.SetActive(false);
        if (winPanel) winPanel.SetActive(false);
        sp = Object.FindObjectsByType<SpawnPath>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
    }

    void Update()
    {
        if (ended) return;

        foreach (var s in sp) if (!s.done) return;      
        if (Object.FindAnyObjectByType<Enemy>() != null) return; 

        if (winPanel) winPanel.SetActive(true);          
        Time.timeScale = 0f; ended = true;
    }

    void OnTriggerEnter2D(Collider2D other){
        if (ended || !other.GetComponent<Enemy>()) return;
        if (failPanel) failPanel.SetActive(true);        // FailPanel Control.
        Time.timeScale = 0f; ended = true;
    }

    public void Retry() { Time.timeScale = 1f; 
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
    }
    public void NextLevel() { Time.timeScale = 1f; 
    var i = SceneManager.GetActiveScene().buildIndex; 
    var n = i + 1; if (n >= SceneManager.sceneCountInBuildSettings) n = 0; SceneManager.LoadScene(n); }
    public void MainMenu() { Time.timeScale = 1f; 
    SceneManager.LoadScene(0); 
    }
}
