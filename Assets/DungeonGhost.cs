using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DungeonGhost : MonoBehaviour
{
    [Header("巡逻设置")]
    [SerializeField] private Transform[] patrolPoints; // 巡逻点数组
    [SerializeField] private float patrolSpeed = 2f; // 巡逻速度
    [SerializeField] private float waitTime = 1f; // 到达巡逻点后的等待时间
    
    [Header("追逐设置")]
    [SerializeField] private float chaseSpeed = 5f; // 追逐速度
    [SerializeField] private float detectionRange = 5f; // 检测范围
    [SerializeField] private float fieldOfView = 120f; // 视野角度
    [SerializeField] private LayerMask playerLayer; // Player层级
    [SerializeField] private LayerMask obstacleLayer; // 障碍物层级
    
    [Header("动画设置")]
    [SerializeField] private Animator animator; // 动画控制器
    [SerializeField] private string speedParam = "speed"; // 速度参数名
    [SerializeField] private string biteParam = "Bite"; // 攻击触发器名
    
    [Header("音频")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip roarSound; // 咆哮音效
    [SerializeField] private AudioClip patrolSound; // 巡逻时的声音
    [SerializeField] private AudioClip chaseSound; // 追逐时的声音
    
    [Header("死亡设置")]
    [SerializeField] private GameObject deathTrigger; // 死亡触发器
    
    [Header("减速设置")]
    [SerializeField] private float slowMotionMultiplier = 0.05f; // 减速倍数
    public bool isSlowMotion = false; // 是否处于减速状态
    private float originalAnimSpeed = 1f; // 原始动画速度
    
    private NavMeshAgent navAgent;
    private GameObject player;
    private int currentPatrolIndex = 0;
    private bool isChasing = false;
    private bool isWaiting = false;
    private bool isDead = false;
    private bool hasAttacked = false; // 是否已经攻击过
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    
    // 状态枚举
    private enum GhostState
    {
        Idle,       // 闲置
        Patrolling, // 巡逻
        Chasing,    // 追逐
        Returning,  // 返回
        Attacking   // 攻击
    }
    private GhostState currentState = GhostState.Patrolling;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        
        // 查找Player - 尝试多种方式
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            // 尝试查找XR Origin
            GameObject xrOrigin = GameObject.Find("XR Origin");
            if (xrOrigin != null)
            {
                player = xrOrigin;
                Debug.Log("找到XR Origin作为Player");
            }
            else
            {
                // 尝试查找带有Camera的对象
                Camera mainCam = Camera.main;
                if (mainCam != null)
                {
                    player = mainCam.transform.root.gameObject;
                    Debug.Log("找到Main Camera的根对象作为Player");
                }
            }
        }
        
        if (player == null)
        {
            Debug.LogError("DungeonGhost: 未找到Player！请确保Player对象有'Player'标签");
        }
        else
        {
            Debug.Log($"DungeonGhost: 找到Player - {player.name}");
        }
        
        // 获取组件
        if (animator == null)
            animator = GetComponent<Animator>();
        
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        
        // 保存初始位置和动画速度
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        originalAnimSpeed = animator ? animator.speed : 1f;
        
        // 设置初始速度
        navAgent.speed = patrolSpeed;
        
        // 验证层级设置
        Debug.Log($"Player Layer设置为: {playerLayer.value}");
        Debug.Log($"Obstacle Layer设置为: {obstacleLayer.value}");
        
        // 开始巡逻
        if (patrolPoints.Length > 0)
        {
            StartPatrolling();
            Debug.Log($"开始巡逻，共有 {patrolPoints.Length} 个巡逻点");
        }
        else
        {
            Debug.LogWarning("没有设置巡逻点！");
        }
    }

    void Update()
    {
        if (isDead || hasAttacked) return;
        
        // 更新动画速度参数（基于当前移动速度）
        UpdateAnimationSpeed();
        
        switch (currentState)
        {
            case GhostState.Idle:
                HandleIdleState();
                break;
            case GhostState.Patrolling:
                Patrol();
                CheckForPlayer();
                break;
            case GhostState.Chasing:
                ChasePlayer();
                break;
            case GhostState.Returning:
                ReturnToPatrol();
                break;
            case GhostState.Attacking:
                HandleAttackState();
                break;
        }
    }
    
    // 更新动画速度
    void UpdateAnimationSpeed()
    {
        if (animator == null) return;
        
        // 根据NavMeshAgent的速度设置动画速度参数
        float normalizedSpeed = 0f;
        
        if (navAgent.velocity.magnitude > 0.1f)
        {
            if (currentState == GhostState.Chasing)
            {
                normalizedSpeed = 1f; // 追逐时满速
            }
            else if (currentState == GhostState.Patrolling || currentState == GhostState.Returning)
            {
                normalizedSpeed = 0.5f; // 巡逻时半速
            }
        }
        
        animator.SetFloat(speedParam, normalizedSpeed);
    }
    
    // 处理闲置状态
    void HandleIdleState()
    {
        if (animator) animator.SetFloat(speedParam, 0f);
        
        // 可以添加条件让Ghost从闲置状态开始巡逻
        if (patrolPoints.Length > 0)
        {
            currentState = GhostState.Patrolling;
            StartPatrolling();
        }
    }
    
    // 处理攻击状态
    void HandleAttackState()
    {
        navAgent.isStopped = true;
        
        // 面向玩家
        if (player != null)
        {
            Vector3 lookPos = player.transform.position - transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);
        }
        
        // 触发攻击动画
        if (animator && !hasAttacked)
        {
            animator.SetTrigger(biteParam);
            hasAttacked = true;
            
            // 可以在这里添加攻击完成后的逻辑
            StartCoroutine(AttackComplete());
        }
    }
    
    IEnumerator AttackComplete()
    {
        yield return new WaitForSeconds(1.5f); // 等待攻击动画完成
        
        // 攻击完成后的处理
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPlayerCaught();
        }
    }
    
    // 巡逻逻辑
    void Patrol()
    {
        if (patrolPoints.Length == 0) return;
        
        if (!navAgent.pathPending && navAgent.remainingDistance < 0.5f)
        {
            if (!isWaiting)
            {
                StartCoroutine(WaitAtPoint());
            }
        }
    }
    
    IEnumerator WaitAtPoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        navAgent.SetDestination(patrolPoints[currentPatrolIndex].position);
        isWaiting = false;
    }
    
    // 检测玩家（使用射线检测）
    void CheckForPlayer()
    {
        if (player == null) 
        {
            Debug.LogWarning("CheckForPlayer: Player为空！");
            return;
        }
        
        // 获取Player的实际中心位置（考虑Character Controller）
        Vector3 playerCenter = player.transform.position;
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null)
        {
            playerCenter += new Vector3(0, cc.height / 2, 0); // 使用高度的一半作为中心
        }
        
        // 从Ghost到Player的方向
        Vector3 dirToPlayer = playerCenter - (transform.position + Vector3.up * 1f);
        float distToPlayer = dirToPlayer.magnitude;
        
        // 检查是否在检测范围内
        if (distToPlayer <= detectionRange)
        {
            // 检查是否在视野角度内
            float angle = Vector3.Angle(transform.forward, dirToPlayer.normalized);
            if (angle < fieldOfView / 2)
            {
                // 射线检测是否有障碍物遮挡
                Vector3 rayStart = transform.position + Vector3.up * 1f; // 从Ghost眼睛高度发射
                RaycastHit hit;
                
                // 调试射线 - 黄色表示检测中
                Debug.DrawRay(rayStart, dirToPlayer, Color.yellow, 0.1f);
                
                // 直接检测是否能看到Player
                if (Physics.Raycast(rayStart, dirToPlayer.normalized, out hit, distToPlayer))
                {
                    Debug.Log($"射线击中: {hit.collider.name}, Tag: {hit.collider.tag}, Layer: {LayerMask.LayerToName(hit.collider.gameObject.layer)}");
                    
                    // 检查是否击中Player（通过标签或组件）
                    if (hit.collider.CompareTag("Player") || 
                        hit.collider.GetComponent<CharacterController>() != null ||
                        hit.collider.transform.root == player.transform)
                    {
                        Debug.Log("检测到Player！开始追逐");
                        StartChasing();
                    }
                    else
                    {
                        // 击中了其他物体（可能是墙）
                        Debug.Log($"视线被 {hit.collider.name} 遮挡");
                    }
                }
            }
            else
            {
                // 不在视野内，绘制灰色线表示角度超出
                Debug.DrawRay(transform.position + Vector3.up * 1f, dirToPlayer, Color.gray, 0.1f);
            }
        }
        
        // 额外绘制Ghost的前方方向（蓝色）用于调试
        Debug.DrawRay(transform.position + Vector3.up * 1f, transform.forward * detectionRange, Color.blue, 0.1f);
    }
    
    // 开始追逐 - 改为public以便调试
    public void StartChasing()
    {
        if (currentState == GhostState.Chasing) return;
        
        currentState = GhostState.Chasing;
        isChasing = true;
        
        // 设置追逐速度（考虑减速效果）
        navAgent.speed = isSlowMotion ? chaseSpeed * slowMotionMultiplier : chaseSpeed;
        navAgent.isStopped = false;
        
        // 播放咆哮音效
        if (audioSource && roarSound)
        {
            audioSource.clip = chaseSound ? chaseSound : roarSound;
            audioSource.Play();
        }
        
        Debug.Log("Ghost开始追逐Player!");
    }
    
    // 追逐玩家
    void ChasePlayer()
    {
        if (player == null)
        {
            Debug.LogWarning("ChasePlayer: Player为空！");
            StopChasing();
            return;
        }
        
        // 设置目标为玩家位置
        navAgent.SetDestination(player.transform.position);
        
        // 获取Player中心位置
        Vector3 playerCenter = player.transform.position;
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null)
        {
            playerCenter += new Vector3(0, cc.height / 2, 0);
        }
        
        // 检查是否还能看到玩家
        Vector3 dirToPlayer = playerCenter - (transform.position + Vector3.up * 1f);
        float distToPlayer = dirToPlayer.magnitude;
        
        // 调试信息 - 减少频率
        if (Time.frameCount % 30 == 0) // 每30帧输出一次
        {
            Debug.Log($"追逐中 - 距离: {distToPlayer:F1}米");
        }
        
        Vector3 rayStart = transform.position + Vector3.up * 1f;
        RaycastHit hit;
        bool canSeePlayer = false;
        
        // 红色射线表示追逐中
        Debug.DrawRay(rayStart, dirToPlayer, Color.red, 0.1f);
        
        if (Physics.Raycast(rayStart, dirToPlayer.normalized, out hit, distToPlayer))
        {
            if (hit.collider.CompareTag("Player") || 
                hit.collider.GetComponent<CharacterController>() != null ||
                hit.collider.transform.root == player.transform)
            {
                canSeePlayer = true;
            }
        }
        
        // 如果看不到玩家了，或距离太远，停止追逐
        if (!canSeePlayer || distToPlayer > detectionRange * 1.5f)
        {
            Debug.Log($"失去目标 - 能看到: {canSeePlayer}, 距离: {distToPlayer:F1}");
            StopChasing();
        }
    }
    
    // 停止追逐 - 改为public以便其他脚本调用
    public void StopChasing()
    {
        currentState = GhostState.Returning;
        isChasing = false;
        
        // 恢复巡逻速度（考虑减速效果）
        navAgent.speed = isSlowMotion ? patrolSpeed * slowMotionMultiplier : patrolSpeed;
        
        // 切换回巡逻音效
        if (audioSource && patrolSound)
        {
            audioSource.clip = patrolSound;
            audioSource.Play();
        }
        
        // 返回最近的巡逻点
        if (patrolPoints.Length > 0)
        {
            navAgent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
        
        Debug.Log("Ghost停止追逐，返回巡逻");
    }
    
    // 返回巡逻
    void ReturnToPatrol()
    {
        if (!navAgent.pathPending && navAgent.remainingDistance < 0.5f)
        {
            currentState = GhostState.Patrolling;
            StartPatrolling();
        }
    }
    
    // 开始巡逻
    void StartPatrolling()
    {
        if (patrolPoints.Length > 0)
        {
            navAgent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
    }
    
    // 进入安全区时被调用
    public void OnPlayerEnterSafeZone()
    {
        if (currentState == GhostState.Chasing)
        {
            StopChasing();
        }
    }
    
    // 减速效果（被枪击中时调用）
     public void SetSlowMotion(bool active)
    {
        if (isDead || hasAttacked) return;

        if (active == isSlowMotion) return; // 避免重复设置

        isSlowMotion = active;

        float targetSpeed = currentState == GhostState.Chasing ? chaseSpeed : patrolSpeed;
        navAgent.speed = active ? targetSpeed * slowMotionMultiplier : targetSpeed;

        if (animator != null)
        {
            animator.speed = active ? slowMotionMultiplier : originalAnimSpeed;
        }

        Debug.Log($"Ghost {name} {(active ? "进入" : "退出")} 减速状态");
    }
    
        // 触发攻击（当抓到玩家时）
        public void TriggerAttack()
        {
            if (currentState != GhostState.Attacking && !hasAttacked)
            {
                currentState = GhostState.Attacking;
                HandleAttackState();
            }
        }
    
    // 重置Ghost
    public void ResetGhost()
    {
        isDead = false;
        hasAttacked = false;
        isSlowMotion = false;
        currentState = GhostState.Patrolling;
        isChasing = false;
        isWaiting = false;
        currentPatrolIndex = 0;
        
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        
        navAgent.speed = patrolSpeed;
        navAgent.isStopped = false;
        
        if (animator)
        {
            animator.speed = originalAnimSpeed;
            animator.SetFloat(speedParam, 0f);
        }
        
        if (patrolPoints.Length > 0)
            StartPatrolling();
    }
    
    // 在编辑器中绘制检测范围
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        // 绘制视野角度
        Vector3 fovLine1 = Quaternion.AngleAxis(fieldOfView / 2, transform.up) * transform.forward * detectionRange;
        Vector3 fovLine2 = Quaternion.AngleAxis(-fieldOfView / 2, transform.up) * transform.forward * detectionRange;
        
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, fovLine1);
        Gizmos.DrawRay(transform.position, fovLine2);
    }
}