using RoR2;
using UnityEngine;

namespace Cloudburst.Items.Green.RestlessRings
{
    //Original From ROR2's ItemFollower, Modified to smooth
    [RequireComponent(typeof(ItemDisplay))]
    public class ItemFollowerSmooth : MonoBehaviour
    {
        public GameObject followerPrefab;
        public GameObject targetObject;
        public BezierCurveLine followerCurve;
        public LineRenderer followerLineRenderer;
        public float distanceDampTime;
        public float distanceMaxSpeed;
        public ItemDisplay itemDisplay;
        public Vector3 velocityDistance;
        public Vector3 v0;
        public Vector3 v1;
        public float SmoothingNumber;

        [HideInInspector]
        public GameObject followerInstance;

        public void Start()
        {
            itemDisplay = GetComponent<ItemDisplay>();
            followerLineRenderer = GetComponent<LineRenderer>();
            Rebuild();
        }

        public void Rebuild()
        {
            if (itemDisplay.GetVisibilityLevel() == VisibilityLevel.Invisible)
            {
                if (followerInstance)
                {
                    Destroy(followerInstance);
                }
                if (followerLineRenderer)
                {
                    followerLineRenderer.enabled = false;
                }
            }
            else
            {
                if (!followerInstance)
                {
                    followerInstance = Instantiate(followerPrefab, targetObject.transform.position, Quaternion.identity);
                    followerInstance.transform.localScale = transform.localScale;
                    if (followerCurve)
                    {
                        v0 = followerCurve.v0;
                        v1 = followerCurve.v1;
                    }
                }
                if (followerLineRenderer)
                {
                    followerLineRenderer.enabled = true;
                }
            }
        }

        public void Update()
        {
            Rebuild();
            if (followerInstance)
            {
                Transform followerTransform = followerInstance.transform;
                Transform targetTransform = targetObject.transform;
                followerTransform.position = Vector3.SmoothDamp(followerTransform.position, targetTransform.position, ref velocityDistance, distanceDampTime);
                followerTransform.rotation = Quaternion.Slerp(followerTransform.rotation, targetTransform.rotation, SmoothingNumber);
                if (followerCurve)
                {
                    followerCurve.v0 = transform.TransformVector(v0);
                    followerCurve.v1 = followerTransform.TransformVector(v1);
                    followerCurve.p1 = followerTransform.position;
                }
            }
        }

        public void OnDestroy()
        {
            if (followerInstance)
            {
                Destroy(followerInstance);
            }
        }
    }
}
