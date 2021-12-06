using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] UnityEvent OnNotEnoughtLife;

    public void LoadScene(string sceneString)
    {
        if (GameManager.instance.lives > 0)
        {
            AudioManager.Instance.LoadSoundsFromSceneName(sceneString);
            SceneManager.LoadScene(sceneString);
            GameManager.instance.ResetScore();
        }
        else
        {
            OnNotEnoughtLife?.Invoke();
        }
    }

    public void PlayButtonEffect(string sceneString)
    {
        if (GameManager.instance.lives > 0)
        {
            LoadScene(sceneString);
        }
    }

    //DEBUG PURPOSE ONLY
    //private void Update()
    //{
    //    //if (Input.GetKeyDown("escape"))
    //    //{
    //    //    if (SceneManager.GetActiveScene().name == "TestNiveaux")
    //    //    {
    //    //        Debug.Log("leave level with debug key");
    //    //        LoadScene("MainMenu");
    //    //    }
    //    //}
    //}
}
