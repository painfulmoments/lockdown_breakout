using System.Collections.Generic;
using UnityEngine;

// 安全区管理器 - 挂在空物体上，整个场景只需要一个
public class SafeZoneManager : MonoBehaviour
{
    private static SafeZoneManager instance;
    public static SafeZoneManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SafeZoneManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("SafeZoneManager");
                    instance = go.AddComponent<SafeZoneManager>();
                }
            }
            return instance;
        }
    }
    
    [Header("所有安全区")]
    [SerializeField] private List<SafeZoneTrigger> allSafeZones = new List<SafeZoneTrigger>();
    
    [Header("调试模式")]
    [SerializeField] private bool debugMode = true;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        // 自动查找所有安全区
        RefreshSafeZonesList();
    }
    
    // 刷新安全区列表
    public void RefreshSafeZonesList()
    {
        allSafeZones.Clear();
        SafeZoneTrigger[] zones = FindObjectsOfType<SafeZoneTrigger>();
        allSafeZones.AddRange(zones);
        
        if(debugMode)
        {
            Debug.Log($"SafeZoneManager: 找到 {allSafeZones.Count} 个安全区");
            foreach(var zone in allSafeZones)
            {
                Debug.Log($"  - {zone.zoneName}");
            }
        }
    }
    
    // 注册安全区
    public void RegisterSafeZone(SafeZoneTrigger zone)
    {
        if (!allSafeZones.Contains(zone))
        {
            allSafeZones.Add(zone);
            if(debugMode)
                Debug.Log($"注册安全区: {zone.zoneName}");
        }
    }
    
    // 注销安全区
    public void UnregisterSafeZone(SafeZoneTrigger zone)
    {
        if (allSafeZones.Contains(zone))
        {
            allSafeZones.Remove(zone);
            if(debugMode)
                Debug.Log($"注销安全区: {zone.zoneName}");
        }
    }
    
    // 检查玩家是否在任意安全区内
    public bool IsPlayerInAnySafeZone()
    {
        foreach(var zone in allSafeZones)
        {
            if(zone != null && zone.IsPlayerInside())
                return true;
        }
        return false;
    }
    
    // 获取玩家当前所在的安全区
    public SafeZoneTrigger GetPlayerCurrentSafeZone()
    {
        foreach(var zone in allSafeZones)
        {
            if(zone != null && zone.IsPlayerInside())
                return zone;
        }
        return null;
    }
    
    // 获取玩家当前所在安全区的名称
    public string GetCurrentSafeZoneName()
    {
        var zone = GetPlayerCurrentSafeZone();
        return zone != null ? zone.zoneName : "无";
    }
    
    // 重置所有安全区（游戏重启时调用）
    public void ResetAllSafeZones()
    {
        foreach(var zone in allSafeZones)
        {
            if(zone != null)
                zone.ResetSafeZone();
        }
    }
}