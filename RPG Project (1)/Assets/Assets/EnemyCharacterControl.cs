using System;
using UnityEngine;
using System.Collections;

namespace Enemy
{
    [RequireComponent(typeof (UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof (EnemyCharacter))]
    public class EnemyCharacterControl : MonoBehaviour
    {
        public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
        public EnemyCharacter character { get; private set; } // the character we are controlling
        public Transform target;                                    // target to aim for

		public enum Behavior
		{
			CIRCULAR,
			RANDOM,
			PATROL,
			CHASE,
			STAND,
			WANDER
		}

		private enum PatrolDirection
		{
			PATROL_FORWARD,
			PATROL_BACKWARDS
		}

		public Behavior behavior;
		private Behavior oldBehavior;
		private Vector3 oldDestination;
		public Transform[] waypoints;
		public int currentWaypoint = 0;
		public Rect wanderArea;


		public float walkSpeed = 0.5f;
		public float runSpeed = 3.0f;
		public bool pauseAtWaypoint = false;

		private Vector3 destination;
		private bool paused = false;

		private PatrolDirection direction = PatrolDirection.PATROL_FORWARD;
		private const bool crouch = false;
		private const bool jump = false;

		public float distance;

		private Transform headTrans;

        private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
			character = GetComponent<EnemyCharacter>();

			headTrans = transform.FindDeepChild ("Head");

	        agent.updateRotation = false;
	        agent.updatePosition = true;

			switch (behavior) 
			{
			case Behavior.CIRCULAR:
			case Behavior.PATROL:
			case Behavior.RANDOM:
				destination = waypoints [currentWaypoint].position;
				agent.SetDestination (destination);
				break;
			case Behavior.WANDER:
				destination = RandomWanderPoint ();
				agent.SetDestination (destination);
				break;
			case Behavior.STAND:
				break;
			case Behavior.CHASE:
				break;
			}
        }


        private void Update()
        {
			if(!paused)
			{
				switch (behavior) 
				{
				case Behavior.CIRCULAR:
				case Behavior.PATROL:
				case Behavior.RANDOM:
					Patrol ();
					break;
				case Behavior.STAND:
					break;
				case Behavior.WANDER:
					Wander();
					break;
				case Behavior.CHASE:
					Chase ();
					break;
				}
			}
        }


        public void SetTarget(Transform target)
        {
            this.target = target;
        }

		private void Patrol()
		{
			agent.speed = walkSpeed;	

			float distance = Vector3.Distance (this.transform.position, destination);

			if (distance > 2.0f) 
			{
				character.Move (agent.desiredVelocity, crouch, jump);
			} 
			else 
			{
				switch (behavior) 
				{
				case Behavior.CIRCULAR:
					//change to the next waypoint with wrap 
					currentWaypoint = (currentWaypoint+1) % waypoints.Length;
					destination = waypoints[currentWaypoint].position;
					break;

				case Behavior.RANDOM:
					currentWaypoint = UnityEngine.Random.Range(0,waypoints.Length);
					destination = waypoints[currentWaypoint].position;
					break;

				case Behavior.PATROL:
					if (direction == PatrolDirection.PATROL_FORWARD)
					{
						if (currentWaypoint == waypoints.Length - 1)
						{
							direction = PatrolDirection.PATROL_BACKWARDS;
							currentWaypoint = currentWaypoint - 1;
						}
						else
						{
							currentWaypoint++;	
						}
					} 
					else
					{
						if (currentWaypoint == 0) 
						{
							direction = PatrolDirection.PATROL_FORWARD;
							currentWaypoint++;
						} 
						else 
						{
							currentWaypoint--;
						}
					}
					destination = waypoints [currentWaypoint].position;
					break;
				}
				agent.SetDestination (destination);
				if (pauseAtWaypoint) 
				{
					RandomWait();
				}
			}
		}

		private void Wander()
		{
			agent.speed = walkSpeed;
			float distance = Vector3.Distance (this.transform.position, destination);

			if (distance > 2.0f) 
			{
				character.Move(agent.desiredVelocity, crouch, jump);
			} 
			else 
			{
				destination = RandomWanderPoint ();
				agent.SetDestination (destination);
			}
		}

		private void Chase()
		{
			agent.speed = runSpeed;
			agent.SetDestination (target.position);
			float distance = Vector3.Distance (this.transform.position, destination);

			if (distance > 40.0f) 
			{
				behavior = oldBehavior;
				destination = oldDestination;
				agent.SetDestination (destination);
				Debug.Log ("reverting to old behavior" + behavior);
				return;
			}

			if (distance > 2.0f) 
			{
				character.Move (agent.desiredVelocity, crouch, jump);
			} 
			else 
			{
				//attack
			}
		}

		private void RandomWait()
		{
			paused = true;
			int waitTime = UnityEngine.Random.Range (0, 10);
			if (waitTime > 0) 
			{
				agent.speed = 0f;
				character.Move (Vector3.zero, crouch, jump);

				StartCoroutine (PauseFor (waitTime));
			}
		}

		private IEnumerator PauseFor(int waitTime)
		{
			yield return new WaitForSeconds(waitTime);	
			paused = false;
		}

		private Vector3 RandomWanderPoint()
		{
			int newX = (int)wanderArea.x + UnityEngine.Random.Range (0, (int)wanderArea.width);
			int newZ = (int)wanderArea.y + UnityEngine.Random.Range (0, (int)wanderArea.height);

			Vector3 pos = new Vector3 (newX, 0f, newZ);
			pos.y = Terrain.activeTerrain.SampleHeight (pos) + Terrain.activeTerrain.GetPosition().y + 1.0f;
			return pos;
		}

		public void OnTriggerStay (Collider other)
		{
			if (behavior == Behavior.CHASE)
			{
				return;
			}

			if (other.tag == "Player") 
			{
				RaycastHit hit;

				if (Physics.Raycast (headTrans.position, other.transform.position = headTrans.position, out hit)) 
				{
					if (hit.collider.gameObject.tag == "Player") 
					{
						Debug.Log ("enetering chase mode");
						oldBehavior = behavior;
						oldDestination = destination;
						behavior = Behavior.CHASE;
						target = other.transform;
						agent.SetDestination (target.position);
					}
				}

			}
		}
    }
}
