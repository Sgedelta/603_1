using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float transitionSpeed = 1;
    [SerializeField] float snapDistanceSquared = 0.05f;

    [SerializeField] List<Vector2> cameraPositions;
    [SerializeField] List<string> positionNames;

    private string currentPosition;
    public string CurrentPosition { get { return currentPosition; } set { 
        if(value != currentPosition) { currentPosition = value; SetCameraTarget(value); }
        } }

    private Vector2 targetPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(((Vector2)transform.position - targetPos).sqrMagnitude < snapDistanceSquared)
        {
            transform.position = new Vector3(targetPos.x, targetPos.y, transform.position.z);
        }

        Vector2 lerpedPos = Vector2.Lerp(transform.position, targetPos, transitionSpeed * Time.deltaTime);
        transform.position = new Vector3(lerpedPos.x, lerpedPos.y, transform.position.z);

    }

    private void SetCameraTarget(string positionName)
    {
        targetPos = cameraPositions[positionNames.IndexOf(positionName)];
    }
}
