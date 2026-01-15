using UnityEngine;

public class CraneSensor : MonoBehaviour
{
    [SerializeField]
    CraneController craneController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        craneController.OnTriggerEnter2D(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        craneController.OnTriggerExit2D(collision);
    }
}
