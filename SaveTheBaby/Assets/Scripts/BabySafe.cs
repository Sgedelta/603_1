using UnityEngine;

public class BabySafe : MonoBehaviour
{

    [SerializeField] Collider2D safeCollider;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<BabyController>() != null)
        {
            collision.gameObject.GetComponent<BabyController>().SwapDir();
        }
    }


}
