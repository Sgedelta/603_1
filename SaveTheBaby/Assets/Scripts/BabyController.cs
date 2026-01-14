using UnityEngine;
using UnityEngine.UIElements;

public class BabyController : MonoBehaviour
{
    [SerializeField] private float footHeight = -0.5f;
    [SerializeField] private Vector2 groundedSize = new Vector2(1f, 0.2f);
    int groundLayerMask;
    bool grounded = false;

    [SerializeField] private float crawlSpeed = 1;
    [SerializeField] private int crawlDir = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        groundLayerMask = LayerMask.GetMask("Obstacle");
    }

    // Update is called once per frame
    void Update()
    {
        CheckGrounded();

        if(grounded)
        {
            transform.position += new Vector3(crawlDir * crawlSpeed * Time.deltaTime, 0, 0);
        }
    }

    private void CheckGrounded()
    {
        Collider2D[] groundOverlaps = Physics2D.OverlapBoxAll(
            new Vector2(transform.position.x, transform.position.y + footHeight), groundedSize, 0, groundLayerMask);


        grounded = groundOverlaps.Length > 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y + footHeight, 0), groundedSize);
    }
}
