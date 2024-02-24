using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void SetChoiceAndLoadScene(string scriptChoice)
    {
        if (DataManager.Instance != null)
        {
            DataManager.Instance.selectedScript = scriptChoice;
        }
        SceneManager.LoadScene("VR_Submarine"); // Replace "MainScene" with the name of your main scene
    }
}
