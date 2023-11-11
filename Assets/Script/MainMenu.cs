using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Play Game
    public void ActionPlay()
    {
        SceneLoader.Load(SceneLoader.Scene.Gameplay);
    }

    // Exit Game
    public void ActionExit()
    {
        Application.Quit();
    }
}