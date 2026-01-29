using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    InputAction pauseAction;
    [SerializeField] GameObject _pauseMenuUI;
    [SerializeField] private Button controlButton;
    [SerializeField] private Button controlBackButton;

    private void Start()
    {
        pauseAction = InputSystem.actions.FindAction("Pause");
        pauseAction.performed += ctx => Pause();
    }
    /// <summary>
    /// Closes the game both in editor and in build
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif 
    }

    /// <summary>
    /// Opens the controls menu
    /// </summary>
    /// <param name="_controlsPanel">Panel that contains the controls</param>
    public void ToggleControls(GameObject _controlsPanel)
    {
        _controlsPanel.SetActive(!_controlsPanel.activeSelf);
        if (_controlsPanel.activeSelf)
        {
            controlBackButton.Select();
        }
        else
        {
            controlButton.Select();
        }
    }

    /// <summary>
    /// Loads main menu
    /// </summary>
    public void MainMenu()
    {
        Resume();
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Hover effect enter
    /// </summary>
    /// <param name="_transform">Rect Transform of button</param>
    public void HoverEnter(RectTransform _transform)
    {
        _transform.localScale = _transform.localScale * 2;
    }

    /// <summary>
    /// Hover effect exit
    /// </summary>
    /// <param name="_transform">Rect Transform of button</param>
    public void HoverExit(RectTransform _transform)
    {
        _transform.localScale = _transform.localScale / 2;
    }

    /// <summary>
    /// Pauses the game when called and sets pause menu as active
    /// </summary>
    /// <param name="_pauseMenu">Empty parent game object that has the pause menu</param>
    public void Pause()
    {
        if (_pauseMenuUI != null) 
        {
            if (_pauseMenuUI.activeSelf)
            {
                Resume();
            }
            else
            {
                Time.timeScale = 0f;
                _pauseMenuUI.SetActive(true);
            }
        }
        
    }

    /// <summary>
    /// Resumes the game when called and sets pause menu as unactive
    /// </summary>
    /// <param name="_pauseMenu">Empty parent game object that has the pause menu</param>
    public void Resume()
    {
        Time.timeScale = 1.0f;
        _pauseMenuUI.SetActive(false);
    }
}
