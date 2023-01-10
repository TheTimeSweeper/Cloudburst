using UnityEngine;

namespace Cloudburst.GlobalComponents
{
    public class ProjectileSlow : MonoBehaviour
    {
        private Rigidbody body;
        public float maxVelocityMagnitude;
        public float antiGravity;

        public void Awake()
        {
            body = GetComponent<Rigidbody>();
            if (!body)
            {
                Destroy(this);
                return;
            }
        }
        public void FixedUpdate()
        {

            if (body.velocity.magnitude > this.maxVelocityMagnitude)
            {
                body.velocity = body.velocity.normalized * this.maxVelocityMagnitude;
            }
            if (body.useGravity)
            {
                body.velocity += Physics.gravity * this.antiGravity * Time.fixedDeltaTime;
            }
        }
    }
}
