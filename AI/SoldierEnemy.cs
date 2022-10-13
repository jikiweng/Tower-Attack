using UnityEngine;
using TowerAttack.Combat;

namespace TowerAttack.AI
{
    //Attach to the enemy soldier.
    public class SoldierEnemy : Soldier 
    { 
        //there are some soldiers patroling at the beginning of the game.
        [SerializeField] PatrolPath patrolPath=null;
        //the range of the waypoint.
        [SerializeField] float waypointTolerence = 0.1f;
        //the staying time before move on to next waypoint.
        [SerializeField] float waypointDwellTime = 3f;
        //the patrol speed is slower than running speed.
        [SerializeField] float patrolSpeed=1f;

        //to record how much time does the soldier stay on current waypoint.
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        //the waypoint is an array under patrolpath, use the index to get current waypoint.
        int currentWaypointIndex = 0;

        //Override the controller in Soldier script. Will be called in FixedUpdate().
        protected override void controller()
        {
            //the tag is to tell the type of enemy soldiers.
            //there are 3 kind of enemies, normal, shooter killer and the one bully fewest HP soldier.
            //only normal enemy will keep attack the same target. other enemies will keep finding their favor.
            if(gameObject.tag=="normal enemy"&&combatTarget!=null&&!combatTarget.IsDead) 
            {
                attackEnemy(combatTarget);
                return;
            }

            //if DetectTarget did not catch any soldier, or the previous target is dead, find a new one.
            //if the enemy is not a normal one, he also need to compare the HP or shooter type if there is a new invader.
            CombatTarget target = detectTarget.combatTarget;
            if(target!=null) 
            {
                combatTarget=target;
                attackEnemy(target);
                return;
            }

            //if the soldier has patrolpath, he needs to following the path when he has no target.
            if(patrolPath!=null)
            {
                //get the distance between current position and the next waypoint.
                Vector3 nextPosition = currentTransform.position;
                //finalSpeed is 0 while staying; equals to patrolSpeed while patroling.
                float finalSpeed=patrolSpeed;
                float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());

                //if the distance is shorter than tolerence, stay for a while and then move on.
                if (distanceToWaypoint < waypointTolerence)
                {
                    //record the staying time.
                    timeSinceArrivedAtWaypoint += Time.fixedDeltaTime;
                    //move on after stay enough time.
                    if (timeSinceArrivedAtWaypoint >= waypointDwellTime)
                    {
                        currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
                        timeSinceArrivedAtWaypoint = 0;
                    }
                    else finalSpeed=0f;
                }

                nextPosition = GetCurrentWaypoint();
                //cancel attack action before move on.
                if (currentAction == "attack") cancelAttack();
                move(nextPosition,finalSpeed);                
            }
        }

        //Method to get current waypoint from PatrolPath script.
        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetCurrentWaypoint(currentWaypointIndex);
        }
    }
}