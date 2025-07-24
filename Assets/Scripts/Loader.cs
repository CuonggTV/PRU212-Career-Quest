using System.Collections.Generic;
using Eflatun.SceneReference;
using MEC;
using UnityEngine.SceneManagement;

public static class Loader
{
    static readonly SceneReference loadingScene = new(SceneGuidToPathMapProvider.ScenePathToGuidMap["Assets/Scenes/Loading.unity"]);
    static SceneReference targetScene;

    public static void Load(SceneReference scene)
    {
        targetScene = scene;

        SceneManager.LoadScene(loadingScene.Name);
        Timing.RunCoroutine(LoadTargetScene());
    }

    static IEnumerator<float> LoadTargetScene()
    {
        yield return Timing.WaitForOneFrame;
        SceneManager.LoadScene(targetScene.Name);
    }
}

