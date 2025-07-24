using System.Collections.Generic;
using Eflatun.SceneReference;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] SceneReference mainMenuScene;
    [SerializeField] GameObject gameOverUI;
    [SerializeField] List<BedInteractable> beds;

    public static GameManager Instance { get; private set; }

    public bool IsGameOver()
    {
        foreach (var bed in beds)
        {
            if (!bed.isPersonOnBed) return false;
        }
        return true;
    }

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (IsGameOver())
        {
            if (gameOverUI.activeSelf == false)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                gameOverUI.SetActive(true);
            }
        }
    }
}
