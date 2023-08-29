using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public GameObject MenuButtons;

    public static void Load(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void LoadMenuButtons()
    {
        MenuButtons.SetActive(!MenuButtons.activeSelf);
    }
}