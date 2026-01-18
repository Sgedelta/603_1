using UnityEngine;

public class RobotController : MonoBehaviour
{
    [SerializeField] private float speed = 3;
    [SerializeField] private Vector2 wallDetectSize = new Vector2(0.1f, 0.8f);
    [SerializeField] private Vector2 wallDetectOffset = new Vector2(0.5f, 0.1f);

    [SerializeField] private bool crushable = true;
    [SerializeField] private Vector2 crushDetectSize = new Vector2(.9f, 0.1f);
    [SerializeField] private Vector2 crushDetectOffset = new Vector2(0, 0.6f);

    private int bounceLayerMask;
    private int crushLayerMask;
    private bool goingLeft = false;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bounceLayerMask = LayerMask.GetMask("Obstacle");
        crushLayerMask = LayerMask.GetMask("Obstacle");

    }

    // Update is called once per frame
    void Update()
    {
        CheckBounce();
        CheckCrush();

        transform.position += new Vector3((goingLeft ? -1 : 1) * speed * Time.deltaTime, 0, 0);
    }

    private void CheckBounce()
    {
        Collider2D[] goalOverlap = Physics2D.OverlapBoxAll(
            transform.position + new Vector3(( goingLeft ? -1 : 1 ) * wallDetectOffset.x, wallDetectOffset.y, 0), wallDetectSize, 0, bounceLayerMask);

        foreach(Collider2D collider in goalOverlap)
        {
            if(collider.gameObject != gameObject)
            {
                goingLeft = !goingLeft;
            }
        }

    }

    private void CheckCrush()
    {
        //if we're not crushable, don't worry
        if(!crushable)
        {
            return;
        }

        Collider2D[] goalOverlap = Physics2D.OverlapBoxAll(
            transform.position + (Vector3)crushDetectOffset, crushDetectSize, 0, crushLayerMask);

        foreach (Collider2D collider in goalOverlap)
        {
            if (collider.gameObject != gameObject)
            {
                Destroy(gameObject);
                //todo: replace with animation or similar
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + (Vector3)wallDetectOffset, wallDetectSize);
        Gizmos.DrawWireCube(transform.position + new Vector3(-wallDetectOffset.x, wallDetectOffset.y, 0), wallDetectSize);


        if(crushable)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position + (Vector3)crushDetectOffset, crushDetectSize);
        }
    }

}
