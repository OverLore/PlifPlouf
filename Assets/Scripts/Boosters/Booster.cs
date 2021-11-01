using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New booster", menuName = "Scriptables/Booster")]
public class Booster : ScriptableObject
{
    public float chance = 0;
    public float duration = 0;
    public float speed = 0;
    public string boosterName = string.Empty;

    public Sprite sprite = null;

    public Color frontColor = Color.white;

    public bool useRandomBackColor = false;
    public Color backColor = Color.white;

    public UnityEvent PickUpEvent = null;
}