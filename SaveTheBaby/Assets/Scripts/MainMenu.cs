using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(1);
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
