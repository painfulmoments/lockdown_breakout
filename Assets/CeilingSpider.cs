using UnityEngine;
using System.Collections;

public class CeilingCrawler : MonoBehaviour
{
    public float crawlSpeed = 5f; // 爬行速度
    public float circleRadius = 5f; // 圆周半径
    public float detectionRange = 10f; // 检测玩家的范围
    public LayerMask playerLayer; // 玩家层
    public GameEnding gameEnding; // 游戏结束脚本
    public float moveSpeed = 5f; // 移动速度
    public GameObject firstPersonCamera; // 玩家摄像头的GameObject
    public Transform tran; // 怪物的Transform
    public AudioClip attackSound; // 怪物攻击时的叫声
    public float roarRange = 5f;

    private Animator anim;
    private Vector3 centerPoint; // 圆周运动的中心点
    private Vector3 lastPoint; // 上一个位置
    private float angle = 0f; // 当前角度
    private float ceilingHeight; // 天花板的高度
    private Transform playerTransform; // 玩家位置
    private Camera playerCamera; // 玩家摄像头
    private bool isAttacking = false; // 是否正在攻击
    private Renderer monsterRenderer;
    private AudioSource audioSource; // 音频源组件
   

    void Start()
    {
        anim = GetComponent<Animator>();
        monsterRenderer = GetComponent<Renderer>();
        if (monsterRenderer == null)
        {
           
        }
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        ceilingHeight = transform.position.y; // 初始化天花板高度为当前怪物位置的高度
        centerPoint = new Vector3(transform.position.x, ceilingHeight, transform.position.z);
        lastPoint = transform.position;
        anim.SetFloat("speed", 1f); // 切换到爬行动画

        // 获取玩家摄像头
        playerCamera = Camera.main;
        Debug.Log("Camera assigned: " + (playerCamera != null));
    }

    void Update()
    {
        if (!isAttacking)
        {
            Patrol(); // 进行圆周运动
            AdjustOrientation(); // 调整朝向
            DetectPlayer(); // 检测玩家
        }
    }

    void AdjustOrientation()
    {
        // 保持怪物的方向与天花板一致，并朝向运动方向
        Vector3 direction = transform.position - lastPoint;
        if (direction.sqrMagnitude > 0.1f) // 确保方向向量的平方大小大于一个小阈值
        {
            Vector3 ceilingUp = -Vector3.up; // 天花板方向
            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized, ceilingUp);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * crawlSpeed);
        }
    }

    void Patrol()
    {
        // 根据当前角度计算怪物的位置
        float x = Mathf.Cos(angle) * circleRadius;
        float z = Mathf.Sin(angle) * circleRadius;

        // 计算新的位置，并设置y为天花板高度
        Vector3 newPosition = new Vector3(centerPoint.x + x, ceilingHeight, centerPoint.z + z);

        // 更新怪物的位置
        transform.position = newPosition;

        // 更新上一个位置
        lastPoint = newPosition;

        // 更新角度
        angle += (crawlSpeed / circleRadius) * Time.deltaTime;

        // 确保角度在0到2π之间
        if (angle > Mathf.PI * 2)
        {
            angle -= Mathf.PI * 2;
        }
    }

    void DetectPlayer()
    {
        Collider[] players = Physics.OverlapSphere(transform.position, detectionRange, playerLayer);
        if (players.Length > 0)
        {
            // 检测玩家是否按下Ctrl键（蹲下）
            if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))
            {
                Debug.Log("Player detected and not crouching"); // 调试信息
                playerTransform = players[0].transform;
                isAttacking = true;
                StartCoroutine(DisappearAndReappear());
            }
            else
            {
                Debug.Log("Player detected but crouching, not attacking"); // 调试信息
            }
        }
    }

    IEnumerator DisappearAndReappear()
    {
        Debug.Log("Starting DisappearAndReappear coroutine"); // 调试信息

        // 使怪物消失
        if (monsterRenderer != null)
        {
            monsterRenderer.enabled = false;
        }
        Debug.Log("Monster disappeared");

        // 等待一秒
        yield return new WaitForSeconds(1f);

        // 获取玩家输入并在摄像头前出现
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal != 0 || vertical != 0)
        {
            Vector3 targetDirection = new Vector3(horizontal, 0, vertical);
            float y = firstPersonCamera.transform.rotation.eulerAngles.y;
            targetDirection = Quaternion.Euler(0, y, 0) * targetDirection;

            // 移动摄像头和怪物
            firstPersonCamera.transform.Translate(targetDirection * Time.deltaTime * moveSpeed, Space.World);
            tran.Translate(targetDirection * Time.deltaTime * moveSpeed, Space.World);
        }

        // 设置怪物在玩家摄像头前的位置
        Vector3 attackPosition = playerCamera.transform.position + playerCamera.transform.forward * 0.3f; // 调整0.3f以控制怪物与玩家的距离
        Debug.Log("Calculated attack position: " + attackPosition);
        transform.position = attackPosition;
        if (monsterRenderer != null)
        {
            monsterRenderer.enabled = true;
        }
        Debug.Log("Monster reappeared in front of player at position: " + attackPosition);



        // 等待一秒钟以确保玩家看到怪物出现
        yield return new WaitForSeconds(2.5f);

        // 触发攻击动画
        anim.SetTrigger("Attack");

        // 再等待两秒钟，让玩家看到怪物的攻击
        yield return new WaitForSeconds(2f);

        TriggerGameEnding();
    }

    void FacePlayer()
    {
        if (playerCamera != null)
        {
            Vector3 lookDirection = (playerCamera.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = lookRotation; // 确保正面朝向玩家摄像头
        }
    }

    void TriggerGameEnding()
    {
        Debug.Log("Triggering game ending");

        // 触发游戏结束
        gameEnding.Caught();
        Debug.Log("Game ending triggered");

        // 重置攻击状态
        isAttacking = false;
    }

    void OnDrawGizmosSelected()
    {
        // 在编辑器中显示检测范围
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
