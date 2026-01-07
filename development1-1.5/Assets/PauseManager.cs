using UnityEngine;
using UnityEngine.EventSystems;
using StarterAssets; 
public class UltimateGameController : MonoBehaviour
{
    public GameObject pauseMenuUI;

    [Header("Starter Assets 引用")]
    public MonoBehaviour characterController; 
    public MonoBehaviour starterAssetsInputs; 

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }

        if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        SetPlayerControl(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        SetPlayerControl(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void SetPlayerControl(bool state)
    {
        if (characterController != null) characterController.enabled = state;
        if (starterAssetsInputs != null) starterAssetsInputs.enabled = state;
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}