using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    // Loader Behavior
    private class LoadingBehavior : MonoBehaviour
    {

    }

    // Scene Enums
    public enum Scene
    {
        Gameplay,
        MainMenu,
        Loading,
    }

    // Static Variables
    private static Action onLoadFinish;
    private static AsyncOperation loadingOperation;

    public static void Load(Scene scene)
    {
        // Set Loading Finish and load when loading finishes
        onLoadFinish = () =>
        {
            GameObject loadingObject = new("Loading Object");
            loadingObject.AddComponent<LoadingBehavior>().StartCoroutine(LoadSceneAsync(scene));
        };

        // Load loading scene
        SceneManager.LoadScene(Scene.Loading.ToString());
    }

    public static void LoadFinish()
    {
        // Trigger load finish when scene first load
        if (onLoadFinish != null)
        {
            onLoadFinish();
            onLoadFinish = null;
        }
    }

    public static float GetLoadingProgress()
    {
        // Return progress of loading
        if (loadingOperation != null)
        {
            return loadingOperation.progress;
        }
        else
        {
            return 1f;
        }
    }

    private static IEnumerator LoadSceneAsync(Scene scene)
    {
        // Load Scene Asynchronous Function
        yield return null;
        loadingOperation = SceneManager.LoadSceneAsync(scene.ToString());

        while (!loadingOperation.isDone)
        {
            yield return null;
        }
    }
}