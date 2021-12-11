using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitMenu : MonoBehaviour
{
    public GameObject quitCanvas;
    bool open;

    private void Start()
    {
        quitCanvas.SetActive(false);
        open = false;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape) && !open)
        {
            quitCanvas.SetActive(true);
            open = true;
            gameObject.GetComponent<Animator>().SetTrigger("Show");
        }
    }

    public void Confirm()
    {
        Application.Quit();
    }

    public void Close()
    {
        quitCanvas.SetActive(false);
        open = false;
    }
}
