using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New booster", menuName = "Scriptables/Booster")]
public class Booster : ScriptableObject
{
    public float chance = 0;
    public float duration = 0;
    public string boosterName = string.Empty;

    public Sprite sprite = null;

    public UnityEvent PickUpEvent = null;
}