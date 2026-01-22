using UnityEngine;

public class AntiGravityArea : MonoBehaviour
{
    [Tooltip("No Zero!")]
    public float gravityMultiplier = -1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.attachedRigidbody)
        {
            collision.attachedRigidbody.gravityScale *= gravityMultiplier;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.attachedRigidbody)
        {
            collision.attachedRigidbody.gravityScale /= gravityMultiplier;
        }
    }
}
