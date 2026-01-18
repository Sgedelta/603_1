using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] string newCameraPositionName;

    

    public void UpdateCameraPosition()
    {
        Camera.main.GetComponent<CameraController>().CurrentPosition = newCameraPositionName;
    }
}
