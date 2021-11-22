using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private void LoadMusic(string sceneString)
    {
        switch (sceneString)
        {
            case "MainMenu":
                AudioManager.Instance.SwitchSoundsToMenu();
                break;

            case "TestNiveaux":
                AudioManager.Instance.SwitchSoundsToGame();
                break;

            default:
                break;
        }
    }

    public void LoadScene(string sceneString)
    {
        LoadMusic(sceneString);
        SceneManager.LoadScene(sceneString);
    }
}
