using System.Collections.Generic;
using UnityEngine;

// 安全区触发器 - 挂在每个安全区物体上
public class SafeZoneTrigger : MonoBehaviour
{
    [Header("安全区设置")]
    public string zoneName = "安全区";
    
    [Header("影响的Ghost")]
    [Tooltip("哪些Ghost会被这个安全区影响")]
    [SerializeField] private AffectMode affectMode = AffectMode.AllGhosts;
    [SerializeField] private List<GameObject> specificGhosts = new List<GameObject>();
    
    [Header("音效")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip enterSound;
    [SerializeField] private AudioClip exitSound;
    
    [Header("视觉效果")]
    [SerializeField] private GameObject visualEffectObject;
    [SerializeField] private Light safeZoneLight;
    [SerializeField] private Color gizmoColor = new Color(0, 1, 0, 0.3f);
    
    private bool playerInside = false;
    private Collider safeZoneCollider;
    
    public enum AffectMode
    {
        AllGhosts,      // 影响所有Ghost
        DungeonGhost,   // 只影响地牢Ghost
        FirstFloorGhost,// 只影响一楼Ghost  
        SpecificGhosts  // 只影响指定的Ghost
    }
    
    void Start()
    {
        // 确保有Collider且是Trigger
        safeZoneCollider = GetComponent<Collider>();
        if (safeZoneCollider == null)
        {
            Debug.LogError($"安全区 {zoneName} 缺少Collider组件！自动添加BoxCollider");
            safeZoneCollider = gameObject.AddComponent<BoxCollider>();
        }
        safeZoneCollider.isTrigger = true;
        
        // 获取AudioSource
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        
        // 注册到管理器
        if (SafeZoneManager.Instance != null)
        {
            SafeZoneManager.Instance.RegisterSafeZone(this);
        }
        
        // 初始化视觉效果
        if (visualEffectObject != null)
            visualEffectObject.SetActive(false);
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !playerInside)
        {
            playerInside = true;
            OnPlayerEnterSafeZone();
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && playerInside)
        {
            playerInside = false;
            OnPlayerExitSafeZone();
        }
    }
    
    void OnPlayerEnterSafeZone()
    {
        Debug.Log($"玩家进入安全区: {zoneName}");
        
        // 播放进入音效
        if (audioSource && enterSound)
            audioSource.PlayOneShot(enterSound);
        
        // 激活视觉效果
        if (visualEffectObject != null)
            visualEffectObject.SetActive(true);
        
        if (safeZoneLight != null)
        {
            safeZoneLight.intensity *= 1.5f;
        }
        
        // 通知相关的Ghost停止追逐
        NotifyGhostsToStop();
    }
    
    void OnPlayerExitSafeZone()
    {
        Debug.Log($"玩家离开安全区: {zoneName}");
        
        // 播放离开音效
        if (audioSource && exitSound)
            audioSource.PlayOneShot(exitSound);
        
        // 关闭视觉效果
        if (visualEffectObject != null)
            visualEffectObject.SetActive(false);
        
        if (safeZoneLight != null)
        {
            safeZoneLight.intensity /= 1.5f;
        }
    }
    
    void NotifyGhostsToStop()
    {
        switch(affectMode)
        {
            case AffectMode.AllGhosts:
                NotifyAllGhosts();
                break;
                
            case AffectMode.DungeonGhost:
                NotifyDungeonGhosts();
                break;
                
            case AffectMode.FirstFloorGhost:
                NotifyFirstFloorGhosts();
                break;
                
            case AffectMode.SpecificGhosts:
                NotifySpecificGhosts();
                break;
        }
    }
    
    void NotifyAllGhosts()
    {
        // 通知所有地牢Ghost
        DungeonGhost[] dungeonGhosts = FindObjectsOfType<DungeonGhost>();
        foreach(var ghost in dungeonGhosts)
        {
            ghost.OnPlayerEnterSafeZone();
        }
        
        // 如果有一楼Ghost类
        // FirstFloorGhost[] firstFloorGhosts = FindObjectsOfType<FirstFloorGhost>();
        // foreach(var ghost in firstFloorGhosts)
        // {
        //     ghost.OnPlayerEnterSafeZone();
        // }
    }
    
    void NotifyDungeonGhosts()
    {
        DungeonGhost[] dungeonGhosts = FindObjectsOfType<DungeonGhost>();
        foreach(var ghost in dungeonGhosts)
        {
            ghost.OnPlayerEnterSafeZone();
        }
    }
    
    void NotifyFirstFloorGhosts()
    {
        // 当您创建FirstFloorGhost脚本后启用这部分
        // FirstFloorGhost[] firstFloorGhosts = FindObjectsOfType<FirstFloorGhost>();
        // foreach(var ghost in firstFloorGhosts)
        // {
        //     ghost.OnPlayerEnterSafeZone();
        // }
        Debug.Log("FirstFloorGhost脚本尚未创建");
    }
    
    void NotifySpecificGhosts()
    {
        foreach(var ghostObj in specificGhosts)
        {
            if(ghostObj != null)
            {
                var dungeonGhost = ghostObj.GetComponent<DungeonGhost>();
                if(dungeonGhost != null)
                {
                    dungeonGhost.OnPlayerEnterSafeZone();
                }
                
                // var firstFloorGhost = ghostObj.GetComponent<FirstFloorGhost>();
                // if(firstFloorGhost != null)
                // {
                //     firstFloorGhost.OnPlayerEnterSafeZone();
                // }
            }
        }
    }
    
    // 重置安全区
    public void ResetSafeZone()
    {
        playerInside = false;
        
        if (visualEffectObject != null)
            visualEffectObject.SetActive(false);
            
        if (safeZoneLight != null)
        {
            // 重置光照强度
        }
    }
    
    // 获取玩家是否在安全区内
    public bool IsPlayerInside()
    {
        return playerInside;
    }
    
    void OnDestroy()
    {
        // 从管理器中注销
        if(SafeZoneManager.Instance != null)
            SafeZoneManager.Instance.UnregisterSafeZone(this);
    }
    
    // 在编辑器中绘制安全区范围
    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            // Box Collider
            if (col is BoxCollider)
            {
                BoxCollider box = col as BoxCollider;
                Matrix4x4 oldMatrix = Gizmos.matrix;
                Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
                Gizmos.DrawCube(box.center, box.size);
                Gizmos.color = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, 1f);
                Gizmos.DrawWireCube(box.center, box.size);
                Gizmos.matrix = oldMatrix;
            }
            // Sphere Collider
            else if (col is SphereCollider)
            {
                SphereCollider sphere = col as SphereCollider;
                float radius = sphere.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);
                Gizmos.DrawSphere(transform.position + sphere.center, radius);
                Gizmos.color = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, 1f);
                Gizmos.DrawWireSphere(transform.position + sphere.center, radius);
            }
        }
        
        // 显示安全区名称
        #if UNITY_EDITOR
        UnityEditor.Handles.Label(transform.position + Vector3.up * 2, $"{zoneName}\n[{affectMode}]");
        #endif
    }
    
    // 编辑器测试功能
    [ContextMenu("测试 - 模拟玩家进入")]
    void TestPlayerEnter()
    {
        playerInside = true;
        OnPlayerEnterSafeZone();
    }
    
    [ContextMenu("测试 - 模拟玩家离开")]
    void TestPlayerExit()
    {
        playerInside = false;
        OnPlayerExitSafeZone();
    }
}