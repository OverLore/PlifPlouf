using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class ProfileCreationWindow : MonoBehaviour
{
    [SerializeField] Image errorWindow;
    [SerializeField] Image creationSuccessWindow;
    [SerializeField] Image creationFailedWindow;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] TMP_Dropdown dropdown;

    Coroutine errorWindowShowCoroutine = null;
    Coroutine creationSuccessWindowShowCoroutine = null;
    Coroutine creationFailedWindowShowCoroutine = null;

    private void Start()
    {
        UpdateDropdown();
    }

    private void OnEnable()
    {
        UpdateDropdown();
    }

    public void CreateUser()
    {
        string username = inputField.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrWhiteSpace(username))
        {
            AudioManager.Instance.PlaySound("UIButtonError");
            return;
        }

        if (GameManager.instance.IsExistingUser(username))
        {
            if (errorWindowShowCoroutine != null)
            {
                StopCoroutine(errorWindowShowCoroutine);
            }

            AudioManager.Instance.PlaySound("UIButtonError");
            errorWindowShowCoroutine = StartCoroutine(ShowErrorWindow());

            return;
        }

        GameManager.instance.CreateUser(username);

        if (GameManager.instance.IsExistingUser(username))
        {
            if (creationSuccessWindowShowCoroutine != null)
            {
                StopCoroutine(creationSuccessWindowShowCoroutine);
            }

            AudioManager.Instance.PlaySound("UIButton");
            creationSuccessWindowShowCoroutine = StartCoroutine(ShowCreationSuccessWindow());

            GameManager.instance.ChangeUser(username);

            UpdateDropdown();

            return;
        }
        else
        {
            if (creationFailedWindowShowCoroutine != null)
            {
                StopCoroutine(creationFailedWindowShowCoroutine);
            }


            AudioManager.Instance.PlaySound("UIButtonError");
            creationFailedWindowShowCoroutine = StartCoroutine(ShowCreationFailedWindow());

            return;
        }
    }

    public void OnDropdownValueChanged(int value)
    {
        AudioManager.Instance.PlaySound("UIButton");
        GameManager.instance.ChangeUser(dropdown.options[value].text);
    }

    void UpdateDropdown()
    {
        if (dropdown == null || !dropdown.isActiveAndEnabled)
        {
            return;
        }

        dropdown.ClearOptions();
        dropdown.AddOptions(GameManager.instance.profileNames);

        dropdown.value = GameManager.instance.profileNames.IndexOf(GameManager.instance.profileName);
    }

    IEnumerator ShowErrorWindow()
    {
        float elapsedTime = 0f;

        while (elapsedTime < 3f)
        {
            elapsedTime += Time.deltaTime;
            Color currentColor = Color.Lerp(Color.white, new Color(1,1,1,0), elapsedTime / 3f);
            errorWindow.color = currentColor;
            yield return null;
        }
    }

    IEnumerator ShowCreationSuccessWindow()
    {
        float elapsedTime = 0f;

        while (elapsedTime < 3f)
        {
            elapsedTime += Time.deltaTime;
            Color currentColor = Color.Lerp(Color.white, new Color(1,1,1,0), elapsedTime / 3f);
            creationSuccessWindow.color = currentColor;
            yield return null;
        }
    }

    IEnumerator ShowCreationFailedWindow()
    {
        float elapsedTime = 0f;

        while (elapsedTime < 3f)
        {
            elapsedTime += Time.deltaTime;
            Color currentColor = Color.Lerp(Color.white, new Color(1,1,1,0), elapsedTime / 3f);
            creationFailedWindow.color = currentColor;
            yield return null;
        }
    }
}
