using System.Collections.Generic;
using UnityEngine;

public class CraneRope : MonoBehaviour
{
    public HingeJoint2D parentHinge;
    public float length;
    Rigidbody2D rigid;
    HingeJoint2D hinge;

    private void Start()
    {
        length = transform.localScale.y;
        rigid = GetComponent<Rigidbody2D>();
        hinge = GetComponent<HingeJoint2D>();
    }

    public void Extend(float deltaLength)
    {
        if (deltaLength < -length) return;

        float extendRate = deltaLength / length;
        length = length + deltaLength;
        Vector3 currentCenter = transform.localPosition;
        Vector3 scale = transform.localScale;
        scale.y = length;
        transform.localScale = scale;
        rigid.MovePosition(transform.position + transform.localPosition * extendRate);
        hinge.anchor = new Vector2(0, -0.5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        List<ContactPoint2D> contactPoints = new();
        int contactCounts = collision.GetContacts(contactPoints);
        if (contactCounts == 0) return;

        foreach (var contact in contactPoints)
        {
            if (contact.otherCollider.gameObject == this.gameObject)
            {
                Vector2 hitPosition = contact.point;
                Vector2 hitNormal = contact.normal;
                Debug.Log(hitPosition + " " + hitNormal);
            }
        }
    }
}
