using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CraneController : MonoBehaviour
{
    InputAction moveAction;
    InputAction interactAction;

    [SerializeField]
    GameObject magnetObject;
    SpriteRenderer magnetSprite;
    Rigidbody2D magnetRigidbody;

    public float moveSpeed;
    public float attractAcceleration;

    int obstacleLayerMask;
    bool magnetActive = false;
    List<Rigidbody2D> attractedObstacles = new();

    private void Start()
    {
        // 3. Find the references to the "Move" and "Jump" actions
        moveAction = InputSystem.actions.FindAction("Move");
        interactAction = InputSystem.actions.FindAction("Interact");
        magnetSprite = magnetObject.GetComponent<SpriteRenderer>();
        magnetRigidbody = magnetObject.GetComponent<Rigidbody2D>();
        obstacleLayerMask = LayerMask.GetMask("Obstacle");
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
        

        Attract();
        //Vector2 currentPosition = new Vector2(magnetRigidbody.transform.position.x, magnetRigidbody.transform.position.y);
        Vector2 deltaPosition = moveValue * moveSpeed * Time.deltaTime;
        //magnetRigidbody.MovePosition(currentPosition + deltaPosition);
        magnetRigidbody.linearVelocity = moveValue * moveSpeed;
        //transform.position = currentPosition + deltaPosition;

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
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        var rigid = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rigid != null)
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
