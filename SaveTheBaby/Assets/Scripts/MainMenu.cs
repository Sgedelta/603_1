using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private Button controlButton;
    [SerializeField] private Button controlBackButton;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif 
    }
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

    public void HoverEnter(RectTransform _transform)
    {
        _transform.localScale = _transform.localScale * 2;
    }

    public void HoverExit(RectTransform _transform)
    {
        _transform.localScale = _transform.localScale / 2;
    }
}
