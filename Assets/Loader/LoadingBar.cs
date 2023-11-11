using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingBar : MonoBehaviour
{
    // Attach GameObject
    [Header("Runtime Debug")]
    [SerializeField] private Transform loadBar;

    private void Update()
    {
        loadBar.localScale = new Vector3(SceneLoader.GetLoadingProgress(), 1, 1);
    }
}
