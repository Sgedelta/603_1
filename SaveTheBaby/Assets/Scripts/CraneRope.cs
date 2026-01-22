using System.Collections.Generic;
using UnityEngine;

public class CraneRope : MonoBehaviour
{
    public HingeJoint2D hinge;
    public HingeJoint2D parentHinge;
    public GameObject ropeVisual;
    public float width;
    public float length;
    Rigidbody2D rigid;
    //Rigidbody2D hingeRigid;
    BoxCollider2D colider;

    public GameObject ropePrefab;
    CraneRope parentRope;
    CraneRope childRope;

    private void Awake()
    {
        length = ropeVisual.transform.localScale.y;
        hinge = GetComponent<HingeJoint2D>();
        rigid = GetComponent<Rigidbody2D>();
        //hingeRigid = hinge.gameObject.GetComponent<Rigidbody2D>();
        colider = GetComponent<BoxCollider2D>();
    }

    public void Extend(float deltaLength)
    {
        if (deltaLength < -length) return;
        if (childRope == null)
        {
            SetLength(length + deltaLength);
        }
        else
        {
            SetLength(length + deltaLength);
            //childRope.Extend(deltaLength);
        }
    }

    public void SetLength(float newLength)
    {
        length = newLength;
        //Vector3 currentCenter = transform.localPosition;
        Vector3 scale = transform.localScale;
        scale.y = length;
        ropeVisual.transform.localScale = new Vector3(width, length, width);
        ropeVisual.transform.localPosition = new Vector3(0, length / 2, 0);
        colider.offset = new Vector2(0, length / 2);
        colider.size = new Vector2(width, length);
        hinge.anchor = new Vector2(0, length);
    }

    static int maxSplit = 100;
    static int splitted = 0;
    public void Split(Vector2 splitPosition)
    {
        if (splitted >= maxSplit) return;
        splitted = splitted + 1;
        if (!CraneController.instance.ReadyToSplit()) return;
        CraneController.instance.RefreshCooldown();

        Vector2 localOrigin = transform.position;
        float originalLength = length;
        float t = Vector3.Dot(splitPosition - localOrigin, transform.up) / originalLength;
        if (t > 0 && t < 1)
        {
            Debug.Log("Split");
            Rigidbody2D childRigid = hinge.connectedBody;
            hinge.connectedBody = null;
            var newRopeGO = Instantiate(ropePrefab, transform.parent);
            var newRope = newRopeGO.GetComponent<CraneRope>();

            SetLength(originalLength * t);
            newRopeGO.transform.position = transform.position + length * transform.up;
            newRopeGO.transform.localRotation = transform.localRotation;
            newRope.SetLength(originalLength * (1 - t));

            hinge.connectedBody = newRopeGO.GetComponent<Rigidbody2D>();
            newRope.hinge.connectedBody = childRigid;
            childRope = newRope;
            newRope.parentRope = this;
            CraneController.instance.RegisterRope(childRope);
        }
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
                //Debug.Log(hitPosition + " " + hitNormal);
                //Split(hitPosition);
            }
        }
    }
}
