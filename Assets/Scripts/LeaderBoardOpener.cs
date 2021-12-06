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
        open = true;
    }

    public void Close()
    {
        leaderboard.SetActive(false);
        open = false;
    }

    public void OpenClose()
    {
        leaderboard.SetActive(!open);
        open = !open;
    }
}
