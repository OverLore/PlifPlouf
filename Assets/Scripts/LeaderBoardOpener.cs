using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardOpener : MonoBehaviour
{
    [SerializeField] GameObject leaderboard;
    bool open = false;

    public bool isOpen { get => open; }

    private void Start()
    {
        leaderboard.SetActive(false);
        open = false;
    }

    private void OnEnable()
    {
        leaderboard.SetActive(false);
        open = false;
    }

    public void Open()
    {
        leaderboard.SetActive(true);
        leaderboard.GetComponent<Animator>()?.SetTrigger("Show");
        open = true;
        AudioManager.Instance.PlaySound("UIButton");
    }

    public void Close()
    {
        leaderboard.SetActive(false);
        open = false;
        //AudioManager.Instance.PlaySound("UIButton");
    }

    public void OpenClose()
    {
        leaderboard.SetActive(!open);
        open = !open;

        AudioManager.Instance.PlaySound("UIButton");
    }
}
