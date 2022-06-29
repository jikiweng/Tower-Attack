using UnityEngine;
using UnityEngine.Events;

namespace TowerAttack.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 3;
        [SerializeField] float maxDistance = 20;
        [SerializeField] float lifeAfterImpact = 0.2f;
        // [SerializeField] GameObject hitImpact = null;
        // [SerializeField] GameObject[] destroyOnHit = null;
        // [SerializeField] UnityEvent onHit=null;    //hit sound

        private CombatTarget target;
        private CombatTargetType targetType;
        private Vector3 targetPoint;
        private Vector3 startPoint;
        private GameObject instigator = null;
        private float damage;
        //private isHoming=false;

        private void Start()
        {
            startPoint = transform.position;
        }

        public void AimAt(GameObject instigator, CombatTarget target, CombatTargetType targetType, float damage)
        {
            this.damage = damage;
            this.targetType=targetType;
            this.target = target;
            this.instigator = instigator;
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            targetPoint = (targetCapsule == null) ? target.GetComponent<Transform>().position : 
            target.GetComponent<Transform>().position + Vector3.up * targetCapsule.height / 2;
            transform.LookAt(targetPoint);
        }

        // Update is called once per frame
        void Update()
        {
            transform.Translate(Vector3.forward * speed);
            if (Vector3.Distance(transform.position, startPoint) >= maxDistance)
                Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            CombatTarget combatTarget=other.GetComponent<CombatTarget>();
            if(combatTarget==null) return;
            
            if (combatTarget.combatTargetType == targetType)
            {
                if (target.IsDead) return;

                //onHit.Invoke();
                speed = 0;
                combatTarget.TakeDamage(damage);

                // foreach (GameObject toDestroy in destroyOnHit)
                //     Destroy(toDestroy);

                //show the hit impact
                // if (hitImpact != null)
                //     Instantiate(hitImpact, targetPoint, transform.rotation);
                Destroy(gameObject, lifeAfterImpact);
            }
        }

        public void DestroyObject()
        {
            Destroy(gameObject, lifeAfterImpact);
        }
    }
}