using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CraneController : MonoBehaviour
{
    InputAction moveAction;
    InputAction interactAction;
    InputAction raiseLowerAction;

    [SerializeField]
    GameObject pivotObject;
    Rigidbody2D pivotRigidbody;

    [SerializeField]
    GameObject magnetObject;
    SpriteRenderer magnetSprite;
    Rigidbody2D magnetRigidbody;

    [SerializeField]
    CraneRope firstRope;

    public Vector2 moveSpeed = new Vector2(5, 2);
    public float extendSpeed = 5;
    public float attractAcceleration;

    int obstacleLayerMask;
    bool magnetActive = false;
    List<Rigidbody2D> attractedObstacles = new();
    List<CraneRope> ropes = new();

    private void Start()
    {
        // 3. Find the references to the "Move" and "Jump" actions
        moveAction = InputSystem.actions.FindAction("Move");
        interactAction = InputSystem.actions.FindAction("Interact");
        raiseLowerAction = InputSystem.actions.FindAction("Rope");
        magnetSprite = magnetObject.GetComponent<SpriteRenderer>();
        magnetRigidbody = magnetObject.GetComponent<Rigidbody2D>();
        pivotRigidbody = pivotObject.GetComponent<Rigidbody2D>();
        obstacleLayerMask = LayerMask.GetMask("Obstacle");
        ropes.Add(firstRope);
    }

    // Update is called once per frame
    void Update()
    {
        if (interactAction.WasPressedThisFrame())
        {
            Toggle();
        }

    }

    private void FixedUpdate()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        float raiseValue = raiseLowerAction.ReadValue<float>();


        Attract();
        Vector2 currentPosition = new Vector2(pivotRigidbody.transform.position.x, pivotRigidbody.transform.position.y);
        Vector2 deltaPosition = moveValue * Time.deltaTime * moveSpeed;
        float deltaVertical = raiseValue * Time.deltaTime * extendSpeed;
        pivotRigidbody.MovePosition(currentPosition + deltaPosition);
        ropes[0].Extend(-deltaVertical);

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
                direction *= rigid.gameObject.GetComponent<Magnetic>().Repellent ? -1 : 1;
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
