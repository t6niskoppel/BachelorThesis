﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void load(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
