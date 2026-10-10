using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

namespace GhostController
{
    public class EnemyChase : MonoBehaviour
    {
        public Transform player;
        public Transform targetPoint;
        public Transform respawn1; // 玩家未开门复活点
        public Transform respawn2; // 玩家开门复活点
        public Transform respawn3; // 鬼初始出现点

        public float sightRange = 10f;
        public float sightAngle = 45f;
        public float walkSpeed = 2.5f;
        public float runSpeed = 6f;

        public AudioClip roarSound;
        public AudioSource audioSource;

        public NavMeshAgent navMeshAgent;
        public Animator anim;

        public NewPlayerController newPlayerController;
        public RoomDoorController roomDoorController;

        public enum State
        {
            Idle,
            Chasing,
            MovingToTarget,
            Attacking
        }

        public State currentState = State.Idle;

        public bool isChaseActivated = false;
        public bool hasAttacked = false;

        void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            navMeshAgent.speed = walkSpeed;

            newPlayerController = player.GetComponent<NewPlayerController>();

            // 初始不可见不可移动
            SetMonsterVisible(false);
            SetCollidersEnabled(false);
            currentState = State.Idle;
        }

        void Update()
        {
            if (player == null || hasAttacked) return;

            switch (currentState)
            {
                case State.Idle:
                    anim.SetFloat("speed", 0f);
                    if (isChaseActivated && !newPlayerController.isInRoom && IsPlayerInSight())
                        StartChasing();
                    break;

                case State.Chasing:
                    if (newPlayerController.isInRoom)
                        MoveToTargetPoint();
                    else
                        ChasePlayer();
                    break;

                case State.MovingToTarget:
                    anim.SetFloat("speed", walkSpeed);
                    if (navMeshAgent.remainingDistance < 0.1f)
                        StopAtTargetPoint();
                    break;

                case State.Attacking:
                    navMeshAgent.isStopped = true;
                    transform.LookAt(player);
                    anim.SetTrigger("Bite");
                    break;
            }
        }

        public bool IsPlayerInSight()
        {
            Vector3 directionToPlayer = player.position - transform.position;
            float distanceToPlayer = directionToPlayer.magnitude;
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

            if (distanceToPlayer < sightRange && angleToPlayer < sightAngle)
            {
                if (Physics.Raycast(transform.position, directionToPlayer.normalized, out RaycastHit hit, sightRange))
                    return hit.transform.CompareTag("Player");
            }

            return false;
        }

        public void ActivateChase()
        {
            isChaseActivated = true;
            SetMonsterVisible(true);
            SetCollidersEnabled(true);
            transform.position = respawn3.position;
            transform.rotation = respawn3.rotation;
            StartChasing();
        }

        public void SetMonsterVisible(bool isVisible)
        {
            foreach (Renderer rend in GetComponentsInChildren<Renderer>())
                rend.enabled = isVisible;
        }

        public void SetCollidersEnabled(bool isEnabled)
        {
            foreach (Collider col in GetComponentsInChildren<Collider>())
                col.enabled = isEnabled;
        }

        public void StartChasing()
        {
            currentState = State.Chasing;
            navMeshAgent.speed = runSpeed;
            anim.SetFloat("speed", runSpeed);
        }

        public void ChasePlayer()
        {
            navMeshAgent.SetDestination(player.position);
            anim.SetFloat("speed", runSpeed);
        }

        public void MoveToTargetPoint()
        {
            currentState = State.MovingToTarget;
            navMeshAgent.speed = walkSpeed;
            navMeshAgent.SetDestination(targetPoint.position);
            anim.SetFloat("speed", walkSpeed);
        }

        public void StopAtTargetPoint()
        {
            currentState = State.Idle;
            navMeshAgent.ResetPath();
            anim.SetFloat("speed", 0f);
            transform.rotation = Quaternion.LookRotation(-transform.forward);
        }

        public void SetSlowMotion(bool active)
        {
            if (active)
            {
                navMeshAgent.speed = walkSpeed * 0.05f; // 缩慢移动
                anim.speed = 0.05f; // 缩慢动画
            }
            else
            {
                navMeshAgent.speed = (currentState == State.Chasing) ? runSpeed : walkSpeed;
                anim.speed = 1f;
            }
        }
    }
}
