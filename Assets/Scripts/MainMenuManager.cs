using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] SceneReference scene1;
    [SerializeField] SceneReference scene2;

    [SerializeField] GameObject optionsMenu;
    [SerializeField] GameObject levelsMenu;

    [SerializeField] Button startButton;
    [SerializeField] Button optionsButton;
    [SerializeField] Button quitButton;

    public static void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
    public void DisableOptionsMenu() => optionsMenu.SetActive(false);
    public void DisableLevelsMenu() => levelsMenu.SetActive(false);
    public void LoadScene1() => Loader.Load(scene1);
    public void LoadScene2() => Loader.Load(scene2);
    void Awake()
    {
        startButton.onClick.AddListener(() => levelsMenu.SetActive(true));
        optionsButton.onClick.AddListener(() => optionsMenu.SetActive(true));
        quitButton.onClick.AddListener(() => QuitGame());

        Time.timeScale = 1f;
    }
}
