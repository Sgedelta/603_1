using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private Button controlButton;
    [SerializeField] private Button controlBackButton;

    /// <summary>
    /// Loads the 1st scene in the build
    /// </summary>
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Starts the game and resets baby position
    /// </summary>
    public void PlayFromStart()
    {
        Destroy(GameObject.Find("BabyLoader"));
        StartGame();
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
}
