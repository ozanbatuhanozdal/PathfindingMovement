using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckVictory : MonoBehaviour
{
    public void VictoryButton()
    {
        SceneManager.LoadScene("Menu");
    }

    public void LoseButton()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ExitButton()
    {

    }
}
