using UnityEngine;
using UnityEngine.Events;

namespace TowerAttack.Combat
{
    //Attach to all the projectile.
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 0.5f;
        //the distance this projectile can reach.
        [SerializeField] float maxDistance = 20;
        //the gameObect will be destroy after this time.
        [SerializeField] float lifeAfterImpact = 0.2f;
        // [SerializeField] UnityEvent onHit=null;    //hit sound

        private CombatTarget target;
        //the type of the target should be.
        private CombatTargetType targetType;
        private Vector3 targetPoint;
        private Vector3 startPoint;
        private GameObject instigator;
        private float damage;
        private Color color;

        private void Start()
        {
            startPoint = transform.position;
        }

        //The projectile must look at in the direction of target. 
        public void AimAt(GameObject instigator, CombatTarget target, CombatTargetType targetType, float damage,Color color)
        {
            this.damage = damage;
            this.targetType=targetType;
            this.target = target;
            this.instigator = instigator;
            this.color=color;

            //the position of the target is on the ground, so adjust the position by using the collider.
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            targetPoint = (targetCapsule == null) ? target.GetComponent<Transform>().position : 
            target.GetComponent<Transform>().position + Vector3.up * targetCapsule.height / 2;
            transform.LookAt(targetPoint);
        }

        void FixedUpdate()
        {
            //move toward the target.
            transform.Translate(Vector3.forward * speed);
            //the projectile will be destroyed once reach the maxDistance.
            if (Vector3.Distance(transform.position, startPoint) >= maxDistance)
                Destroy(gameObject);
        }

        //Shoot event is different from hit event. 
        //The damage will not be taken until the projectile hit the target.
        private void OnTriggerEnter(Collider other)
        {
            CombatTarget combatTarget=other.GetComponent<CombatTarget>();
            if(combatTarget==null) return;
            
            //the projectile will only hit the target with right type.
            //projectile comes from the allies will not deal a damage.
            if (combatTarget.combatTargetType == targetType)
            {
                if (target.IsDead) return;

                //stop the projectile and deal damage.
                //onHit.Invoke();
                speed = 0;
                combatTarget.TakeDamage(damage,color);

                Destroy(gameObject, lifeAfterImpact);
            }
        }

        public void DestroyObject()
        {
            Destroy(gameObject, lifeAfterImpact);
        }
    }
}