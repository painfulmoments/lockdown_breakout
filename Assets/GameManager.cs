using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;

public class GameManager : MonoBehaviour
{
    [Header("游戏对象引用")]
    [SerializeField] private GameObject player; // XR Origin Player
    [SerializeField] private DungeonGhost dungeonGhost; // 地牢Ghost
    [SerializeField] private Transform playerSpawnPoint; // Player重生点
    [SerializeField] private Transform ghostSpawnPoint; // Ghost重生点
    
    [Header("游戏设置")]
    [SerializeField] private float restartDelay = 3f; // 重启延迟
    [SerializeField] private bool debugMode = false; // 调试模式
    
    [Header("UI反馈（可选）")]
    [SerializeField] private GameObject deathUI; // 死亡UI面板
    [SerializeField] private TMPro.TextMeshProUGUI countdownText; // 倒计时文本
    
    // 单例模式
    private static GameManager instance;
    public static GameManager Instance
    {
        get { return instance; }
    }
    
    // 游戏状态
    private bool isGamePaused = false;
    private bool isRestarting = false;
    private Vector3 playerInitialPosition;
    private Quaternion playerInitialRotation;
    
    // XR组件引用
    private XROrigin xrOrigin;
    private CharacterController characterController;
    private ContinuousMoveProviderBase moveProvider;
    
    void Awake()
    {
        // 设置单例
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        // 自动查找对象
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                GameObject xrOriginObj = GameObject.Find("XR Origin");
                if (xrOriginObj != null)
                {
                    player = xrOriginObj;
                }
            }
        }
        
        if (dungeonGhost == null)
        {
            dungeonGhost = FindObjectOfType<DungeonGhost>();
        }
    }
    
    void Start()
    {
        // 获取XR组件
        if (player != null)
        {
            xrOrigin = player.GetComponent<XROrigin>();
            characterController = player.GetComponent<CharacterController>();
            moveProvider = player.GetComponentInChildren<ContinuousMoveProviderBase>();
            
            // 保存初始位置
            if (playerSpawnPoint != null)
            {
                playerInitialPosition = playerSpawnPoint.position;
                playerInitialRotation = playerSpawnPoint.rotation;
            }
            else
            {
                playerInitialPosition = player.transform.position;
                playerInitialRotation = player.transform.rotation;
            }
        }
        
        // 隐藏死亡UI
        if (deathUI != null)
        {
            deathUI.SetActive(false);
        }
    }
    
    void Update()
    {
        // 调试快捷键
        if (debugMode)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartGame();
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                TogglePause();
            }
        }
    }
    
    // 重启游戏
    public void RestartGame()
    {
        if (isRestarting) return;
        
        StartCoroutine(RestartGameCoroutine());
    }
    
    IEnumerator RestartGameCoroutine()
    {
        isRestarting = true;
        
        // 显示死亡UI
        if (deathUI != null)
        {
            deathUI.SetActive(true);
        }
        
        // 倒计时
        float timer = restartDelay;
        while (timer > 0)
        {
            if (countdownText != null)
            {
                countdownText.text = $"重新开始: {Mathf.Ceil(timer)}";
            }
            
            timer -= Time.deltaTime;
            yield return null;
        }
        
        // 重置Player位置
        ResetPlayer();
        
        // 重置Ghost
        ResetGhost();
        
        // 重置其他游戏元素
        ResetGameElements();
        
        // 隐藏死亡UI
        if (deathUI != null)
        {
            deathUI.SetActive(false);
        }
        
        // 确保时间恢复正常
        Time.timeScale = 1f;
        
        isRestarting = false;
        isGamePaused = false;
        
        Debug.Log("游戏已重置");
    }
    
    // 重置Player
    void ResetPlayer()
    {
        if (player != null)
        {
            // 禁用CharacterController以便传送
            if (characterController != null)
            {
                characterController.enabled = false;
            }
            
            // 重置位置和旋转
            player.transform.position = playerInitialPosition;
            player.transform.rotation = playerInitialRotation;
            
            // 对于XR Origin，使用MoveCameraToWorldLocation方法
            if (xrOrigin != null)
            {
                xrOrigin.MoveCameraToWorldLocation(playerInitialPosition);
                
                // 如果需要调整摄像机高度偏移，可以使用：
                // xrOrigin.CameraYOffset = 1.6f; // 设置合适的高度值
            }
            
            // 重新启用CharacterController
            if (characterController != null)
            {
                characterController.enabled = true;
            }
            
            // 清除速度
            if (moveProvider != null)
            {
                moveProvider.enabled = false;
                moveProvider.enabled = true;
            }
        }
    }
    
    // 重置Ghost
    void ResetGhost()
    {
        if (dungeonGhost != null)
        {
            dungeonGhost.ResetGhost();
            
            // 如果有Ghost重生点
            if (ghostSpawnPoint != null)
            {
                dungeonGhost.transform.position = ghostSpawnPoint.position;
                dungeonGhost.transform.rotation = ghostSpawnPoint.rotation;
            }
        }
    }
    
    // 重置其他游戏元素
    void ResetGameElements()
    {
        // 重置所有可拾取物品
        PickupItem[] pickups = FindObjectsOfType<PickupItem>();
        foreach (var pickup in pickups)
        {
            pickup.gameObject.SetActive(true);
        }
        
        // 重置门的状态
        Door[] doors = FindObjectsOfType<Door>();
        foreach (var door in doors)
        {
            door.ResetDoor();
        }
        
        // 可以添加其他需要重置的元素
    }
    
    // 暂停/继续游戏
    public void TogglePause()
    {
        isGamePaused = !isGamePaused;
        Time.timeScale = isGamePaused ? 0f : 1f;
        
        Debug.Log($"游戏{(isGamePaused ? "暂停" : "继续")}");
    }
    
    // 游戏结束（被Ghost抓住）
    public void OnPlayerCaught()
    {
        Debug.Log("Player被抓住了！");
        
        // 震动控制器（如果有）
        if (player != null)
        {
            var controllers = player.GetComponentsInChildren<ActionBasedController>();
            foreach (var controller in controllers)
            {
                controller.SendHapticImpulse(1f, 0.5f);
            }
        }
        
        // 触发重启
        RestartGame();
    }
    
    // 获取游戏状态
    public bool IsGamePaused()
    {
        return isGamePaused;
    }
    
    public bool IsRestarting()
    {
        return isRestarting;
    }
}

// 辅助类：可拾取物品
public class PickupItem : MonoBehaviour
{
    // 简单的重置逻辑
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    
    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }
    
    public void Reset()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        gameObject.SetActive(true);
    }
}

// 辅助类：门
public class Door : MonoBehaviour
{
    private bool isOpen = false;
    private Vector3 closedPosition;
    private Vector3 openPosition;
    
    void Start()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + Vector3.up * 2f; // 向上开门
    }
    
    public void ResetDoor()
    {
        isOpen = false;
        transform.position = closedPosition;
    }
}