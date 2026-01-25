using UnityEngine;

public class LockAngleArea : MonoBehaviour
{
    public void LockAngle(HingeJoint2D hinge,bool locked)
    {

        if (locked)
        {
            var limits = hinge.limits;
            limits.max = hinge.jointAngle;
            limits.min = hinge.jointAngle;
        }
        hinge.useLimits = locked;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HingeJoint2D hinge;
        if (hinge=collision.attachedRigidbody.GetComponent<HingeJoint2D>())
        {
            LockAngle(hinge,true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        HingeJoint2D hinge;
        if (hinge = collision.attachedRigidbody.GetComponent<HingeJoint2D>())
        {
            LockAngle(hinge, false);
        }
    }
}
