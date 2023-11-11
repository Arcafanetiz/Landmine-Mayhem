using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadCallback : MonoBehaviour
{
    // Internal Variable
    private bool isLoaded = false;

    private void Update()
    {
        if (!isLoaded)
        {
            isLoaded = true;
            SceneLoader.LoadFinish();
        }
    }
}