using UnityEngine;

public class Goal : MonoBehaviour
{

    [SerializeField] Vector3 magPos = Vector3.zero;

    public void UpdateCheckpointPos()
    {
        LoadBabyPos.INSTANCE.updateBabySpawn(transform.position, (Vector2)magPos);
    }
}
