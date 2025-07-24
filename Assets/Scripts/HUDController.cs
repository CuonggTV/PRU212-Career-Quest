using System.Collections;
using System.Collections.Generic;
using Eflatun.SceneReference;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public static HUDController instance;

    [SerializeField] SceneReference mainMenuScene;
    [SerializeField] SceneReference nextScene;

    [SerializeField] TMP_Text interactionText;
    [SerializeField] TMP_Text announceText;

    [SerializeField] GameObject optionsMenu;
    [SerializeField] List<Toggle> missionToggles;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            announceText.gameObject.SetActive(false);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void EnableInteractionText(string text)
    {
        interactionText.text = text.Trim() + " (F)";
        interactionText.gameObject.SetActive(true);
    }

    public void DisableInteractionText()
    {
        interactionText.gameObject.SetActive(false);
    }

    public void ShowAnnouncement(string text, float duration = 2f)
    {
        StopAllCoroutines();
        StartCoroutine(ShowAnnouncementCoroutine(text, duration));
    }

    private IEnumerator ShowAnnouncementCoroutine(string text, float duration)
    {
        announceText.text = text.Trim();
        announceText.gameObject.SetActive(true);

        yield return new WaitForSeconds(duration);

        announceText.gameObject.SetActive(false);
    }


    public void SetToggleMissionOn(string name)
    {
        name = name.ToLower();

        foreach (var toggle in missionToggles)
        {
            if (toggle.name.ToLower().Contains(name))
            {
                toggle.isOn = true;
                // Optional: break if only one toggle should be turned on
                break;
            }
        }
    }

    public void ActivateOptionsMenu()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        optionsMenu.SetActive(true);
    }

    public void DisableOptionsMenu()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        optionsMenu.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        Loader.Load(mainMenuScene);

    }
    public void LoadNextScene() => Loader.Load(nextScene);

    public void Update()
    {
        bool escPressed = Keyboard.current.escapeKey.isPressed;
        if (escPressed && !optionsMenu.activeSelf)
        {
            ActivateOptionsMenu();
        }
    }
}
