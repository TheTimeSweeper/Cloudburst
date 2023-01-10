using RoR2.Projectile;
using UnityEngine;

namespace Cloudburst.GlobalComponents
{
    public class ProjectileEffectManager : MonoBehaviour
    {
        private ProjectileController controller;
        private GameObject effectInstance;
        public GameObject effect;

        public void Awake()
        {
            controller = GetComponent<ProjectileController>();
        }

        public void Start()
        {
            effectInstance = Instantiate<GameObject>(effect, controller.ghost.transform.position, controller.ghost.transform.rotation, controller.ghost.transform);
            //effectInstance.transform.localScale = new Vector3(2, 2, 2);
        }
        public void OnDestroy()
        {
            if (effectInstance)
            {
                Destroy(effectInstance);
            }
        }
    }
}
