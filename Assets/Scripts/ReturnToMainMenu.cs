using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMainMenu : MonoBehaviour
{
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        SceneManager.LoadScene("MainMenu");
    }
}