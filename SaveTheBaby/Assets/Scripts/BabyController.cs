using System;
using UnityEngine;
using UnityEngine.UIElements;

public class BabyController : MonoBehaviour
{
    [SerializeField] private float footHeight = -0.5f;
    [SerializeField] private Vector2 groundedSize = new Vector2(1f, 0.2f);
    [SerializeField] private Vector2 deathSize = new Vector2(1.1f, 1.1f);
    [SerializeField] private Vector2 goalSize = new Vector2(1.2f, 1.2f);
    private int groundLayerMask;
    private int goalLayerMask;
    private bool grounded = false;
    private bool hitSomething = false;
    private GameObject lastGoalHit = null;

    [SerializeField] private float crawlSpeed = 1;
    [SerializeField] private int crawlDir = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        groundLayerMask = LayerMask.GetMask("Obstacle");
        goalLayerMask = LayerMask.GetMask("Goal");
    }

    // Update is called once per frame
    void Update()
    {
        CheckGrounded();
        CheckDeathCollision();
        CheckGoalCollision();

        if(grounded)
        {
            transform.position += new Vector3(crawlDir * crawlSpeed * Time.deltaTime, 0, 0);
        }

        if (hitSomething)
        {
            KillBaby();
        }
    }

    /// <summary>
    /// checks if there is an obstacle beneath the baby and sets grounded appropriately 
    /// </summary>
    private void CheckGrounded()
    {
        Collider2D[] groundOverlaps = Physics2D.OverlapBoxAll(
            new Vector2(transform.position.x, transform.position.y + footHeight), 
            groundedSize, 0, groundLayerMask);


        grounded = groundOverlaps.Length > 0;
    }

    private void CheckDeathCollision()
    {
        Collider2D[] sidesAndTopOverlaps = Physics2D.OverlapBoxAll(
            new Vector2(transform.position.x, transform.position.y + ((deathSize.y - 1) / 2) + 0.3f), 
            deathSize, 0, groundLayerMask);

        hitSomething = sidesAndTopOverlaps.Length > 0;
    }

    private void CheckGoalCollision()
    {
        Collider2D[] goalOverlap = Physics2D.OverlapBoxAll(
            transform.position, goalSize, 0, goalLayerMask);

        if (goalOverlap.Length > 0)
        {
            lastGoalHit = goalOverlap[0].gameObject;
            Debug.Log(goalOverlap[0].gameObject.name);
        }
    }

    //destroys the baby
    private void KillBaby()
    {
        Debug.Log("Baby Died!");
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y + footHeight, 0), groundedSize);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y + ((deathSize.y - 1) / 2) + 0.3f), deathSize);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, goalSize);
    }


}
