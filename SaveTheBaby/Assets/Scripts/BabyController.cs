using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class BabyController : MonoBehaviour
{
    [SerializeField] private float footHeight = -0.5f;
    [SerializeField] private Vector2 groundedSize = new Vector2(1f, 0.2f);
    [SerializeField] private Vector2 deathSize = new Vector2(1.1f, 1.1f);
    [SerializeField] private Vector2 goalSize = new Vector2(1.2f, 1.2f);
    [SerializeField] private Vector2 electricDeathSize = new Vector2(1, 1);
    [SerializeField] private float stepDetectWidth = .1f;
    private int groundLayerMask;
    private int goalLayerMask;
    private bool grounded = false;
    private bool hitSomething = false;
    private GameObject lastGoalHit = null;
    private SpriteRenderer sr;
    bool babyAlive = true;

    [SerializeField] private float crawlSpeed = 1;
    [SerializeField] private int crawlDir = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        groundLayerMask = LayerMask.GetMask("Obstacle");
        goalLayerMask = LayerMask.GetMask("Goal");
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckGrounded();
        CheckStep();
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
            new Vector2(transform.position.x, transform.position.y + footHeight - groundedSize.y/2), 
            groundedSize, 0, groundLayerMask);


        grounded = groundOverlaps.Length > 0;
    }

    private void CheckStep()
    {
        if(!grounded)
        {
            return;
        }

        Collider2D[] stepCollider = (from obj in Physics2D.OverlapBoxAll(
            new Vector2(transform.position.x + groundedSize.x/2 * crawlDir, transform.position.y + footHeight + groundedSize.y / 2),
            new Vector2(stepDetectWidth, groundedSize.y), 0, groundLayerMask
            ) where !obj.isTrigger select obj).ToArray();

        for (int i = 0; i < stepCollider.Length; i++)
        {

            transform.position += new Vector3(0, groundedSize.y, 0);
        }
    }

    private void CheckDeathCollision()
    {
        List<Collider2D> deathOverlaps = (from obj in Physics2D.OverlapBoxAll(
            new Vector2(transform.position.x, transform.position.y + ((deathSize.y - 1) / 2) + 0.3f),
            deathSize, 0, groundLayerMask) where (!obj.isTrigger) select obj).ToList<Collider2D>();

        deathOverlaps.AddRange(from obj in Physics2D.OverlapBoxAll(transform.position, electricDeathSize, 0) 
                               where (obj.GetComponent<Magnetic>() != null && obj.GetComponent<Magnetic>().IsElectric) select obj);

        

        hitSomething = deathOverlaps.Count > 0;
    }

    private void CheckGoalCollision()
    {
        Collider2D[] goalOverlap = Physics2D.OverlapBoxAll(
            transform.position, goalSize, 0, goalLayerMask);

        if (goalOverlap.Length > 0)
        {
            lastGoalHit = goalOverlap[0].gameObject;
            lastGoalHit.GetComponent<Goal>().UpdateCheckpointPos();
        }
    }

    //destroys the baby
    private void KillBaby()
    {
        if(!babyAlive)
        {
            return;
        }
        babyAlive = false;
        Debug.Log("Baby Died!");
        crawlDir = 0;
        IEnumerator babyRoutine = ReloadSceneAfterDelay(5);
        StartCoroutine(babyRoutine);
        GameObject.Find("AudioManager").GetComponent<AudioSource>().Play();

    }



    private IEnumerator ReloadSceneAfterDelay(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(1);
        yield return null;
    }

    public void SwapDir()
    {
        crawlDir *= -1;
        Debug.Log("Swap Dir");
        sr.flipX = !sr.flipX;
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y + footHeight - groundedSize.y/2, 0), groundedSize);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y + ((deathSize.y - 1) / 2) + 0.3f), deathSize);
        Gizmos.DrawWireCube(transform.position, electricDeathSize);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, goalSize);
    }


}
