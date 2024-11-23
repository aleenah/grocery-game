using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Return : MonoBehaviour
{

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
    }

}