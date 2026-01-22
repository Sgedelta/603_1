using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CraneController : MonoBehaviour
{
    public static CraneController instance;

    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    InputAction moveAction;
    InputAction interactAction;

    [SerializeField]
    GameObject pivotObject;
    Rigidbody2D pivotRigidbody;

    [SerializeField]
    GameObject magnetObject;
    SpriteRenderer magnetSprite;
    Rigidbody2D magnetRigidbody;

    [SerializeField]
    CraneRope firstRope;

    public Vector2 moveSpeed;
    public float extendSpeed = 5;
    public float attractAcceleration;

    int obstacleLayerMask;
    bool magnetActive = false;
    List<Rigidbody2D> attractedObstacles = new();
    List<CraneRope> ropes = new();

    public int initialSegments = 1;
    public float maxSplitRate;
    float splitCooldown;

    private void Start()
    {
        // 3. Find the references to the "Move" and "Jump" actions
        moveAction = InputSystem.actions.FindAction("Move");
        interactAction = InputSystem.actions.FindAction("Interact");
        magnetSprite = magnetObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        magnetRigidbody = magnetObject.GetComponent<Rigidbody2D>();
        pivotRigidbody = pivotObject.GetComponent<Rigidbody2D>();
        obstacleLayerMask = LayerMask.GetMask("Obstacle");
        RegisterRope(firstRope);

        float totalLength = firstRope.length;
        float segmentLength = totalLength / initialSegments;
        Vector2 totalVector = (magnetObject.transform.position - firstRope.transform.position).normalized * totalLength;
        for (int i = 1; i < initialSegments; i++)
        {
            ropes[^1].Split((Vector2)firstRope.transform.position + totalVector * i / initialSegments);
        }
    }

    public void RegisterRope(CraneRope newRope)
    {
        ropes.Add(newRope);
    }

    // Update is called once per frame
    void Update()
    {
        if (interactAction.WasPressedThisFrame())
        {
            Toggle();
        }

        if (splitCooldown > 0)
        {
            splitCooldown -= Time.deltaTime;
            if (splitCooldown < 0) splitCooldown = 0;
        }

    }
    public bool ReadyToSplit() => splitCooldown <= 0;
    public void RefreshCooldown()
    {
        splitCooldown = 0; return;
        if (maxSplitRate == 0)
        {
            splitCooldown = -1; return;
        }
        splitCooldown = 1 / maxSplitRate;
    }

    private void FixedUpdate()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();

        //TODO: Input change
        float ropeExtendValue = 0;
        if (Input.GetKey(KeyCode.R)) ropeExtendValue = 1;
        if (Input.GetKey(KeyCode.F)) ropeExtendValue = -1;


        Attract();
        Vector2 currentPosition = new Vector2(pivotRigidbody.transform.position.x, pivotRigidbody.transform.position.y);
        Vector2 deltaPosition = moveValue * Time.deltaTime * moveSpeed;
        float ropeExtend = ropeExtendValue * Time.deltaTime * extendSpeed;
        //Vector2 deltaHorizontal = new Vector2(deltaPosition.x * moveSpeed, 0);
        //float deltaVertical = deltaPosition.y * extendSpeed;
        pivotRigidbody.MovePosition(currentPosition + deltaPosition);

        foreach (CraneRope rope in ropes)
        {
            rope.Extend(-ropeExtend / ropes.Count);
        }
        //ropes[0].Extend(-ropeExtend);

    }

    void Toggle()
    {
        if (magnetActive)
        {
            magnetActive = false;
            magnetSprite.color = Color.red;
        }
        else
        {
            magnetActive = true;
            magnetSprite.color = Color.green;
        }
    }

    void Attract()
    {
        if (magnetActive)
        {
            foreach (var rigid in attractedObstacles)
            {
                Vector2 direction = (magnetRigidbody.transform.position - rigid.transform.position).normalized;
                rigid.AddForce(direction * rigid.mass * attractAcceleration);
                magnetRigidbody.AddForce(-direction * rigid.mass * attractAcceleration);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        var rigid = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rigid != null && rigid.gameObject.GetComponent<Magnetic>() != null)
        {
            attractedObstacles.Add(rigid);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        var rigid = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rigid != null && attractedObstacles.Contains(rigid))
        {
            attractedObstacles.Remove(rigid);
        }
    }
}
