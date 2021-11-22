using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string sceneString)
    {
        AudioManager.Instance.LoadSoundsFromSceneName(sceneString);
        SceneManager.LoadScene(sceneString);
    }

    //DEBUG PURPOSE ONLY
    private void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            if (SceneManager.GetActiveScene().name == "TestNiveaux")
            {
                Debug.Log("leave level with debug key");
                LoadScene("MainMenu");
            }
        }
    }
}
