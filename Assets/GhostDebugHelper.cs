using UnityEngine;

// Ghost调试助手 - 用于诊断追逐问题
[RequireComponent(typeof(DungeonGhost))]
public class GhostDebugHelper : MonoBehaviour
{
    private DungeonGhost ghost;
    private GameObject player;
    
    [Header("调试信息")]
    [SerializeField] private bool showDebugInfo = true;
    [SerializeField] private bool autoFixSetup = true;
    
    void Start()
    {
        ghost = GetComponent<DungeonGhost>();
        
        // 诊断Player设置
        DiagnosePlayerSetup();
        
        // 如果需要，自动修复
        if (autoFixSetup)
        {
            AutoFixSetup();
        }
    }
    
    void DiagnosePlayerSetup()
    {
        Debug.Log("========== Ghost调试诊断开始 ==========");
        
        // 1. 查找Player
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("? 未找到带'Player'标签的对象！");
            
            // 尝试其他方式查找
            GameObject xrOrigin = GameObject.Find("XR Origin");
            if (xrOrigin != null)
            {
                Debug.LogWarning("?? 找到XR Origin，但没有Player标签");
                player = xrOrigin;
            }
        }
        else
        {
            Debug.Log($"? 找到Player: {player.name}");
        }
        
        if (player == null) return;
        
        // 2. 检查Character Controller
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null)
        {
            Debug.Log($"? Character Controller存在");
            Debug.Log($"  - Center: {cc.center}");
            Debug.Log($"  - Height: {cc.height}");
            Debug.Log($"  - Radius: {cc.radius}");
            
            if (cc.height < 0.5f)
            {
                Debug.LogWarning("?? Character Controller高度太小，可能检测不到");
            }
        }
        else
        {
            Debug.LogError("? Player没有Character Controller!");
            
            // 检查其他碰撞体
            Collider col = player.GetComponent<Collider>();
            if (col != null)
            {
                Debug.Log($"?? Player有{col.GetType().Name}而不是Character Controller");
            }
        }
        
        // 3. 检查层级设置
        Debug.Log($"? Player层级: {LayerMask.LayerToName(player.layer)} (Layer {player.layer})");
        
        // 4. 检查子对象
        Transform cameraOffset = player.transform.Find("Camera Offset");
        if (cameraOffset != null)
        {
            Debug.Log("? 找到Camera Offset");
            Camera mainCam = cameraOffset.GetComponentInChildren<Camera>();
            if (mainCam != null)
            {
                Debug.Log("? 找到Main Camera");
            }
        }
        
        Debug.Log("========== 诊断结束 ==========");
    }
    
    void AutoFixSetup()
    {
        if (player == null) return;
        
        // 1. 确保有Player标签
        if (!player.CompareTag("Player"))
        {
            player.tag = "Player";
            Debug.Log("自动修复: 添加Player标签");
        }
        
        // 2. 确保Character Controller设置合理
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null)
        {
            if (cc.height < 1.0f)
            {
                cc.height = 1.8f;
                cc.center = new Vector3(0, 0.9f, 0);
                cc.radius = 0.4f;
                Debug.Log("自动修复: 调整Character Controller尺寸");
            }
        }
    }
    
    void Update()
    {
        if (!showDebugInfo || player == null) return;
        
        // 显示距离
        float distance = Vector3.Distance(transform.position, player.transform.position);
        
        // 显示角度
        Vector3 dirToPlayer = player.transform.position - transform.position;
        float angle = Vector3.Angle(transform.forward, dirToPlayer.normalized);
        
        // 射线检测测试
        Vector3 rayStart = transform.position + Vector3.up;
        RaycastHit hit;
        bool canSee = false;
        
        if (Physics.Raycast(rayStart, dirToPlayer.normalized, out hit, distance))
        {
            canSee = hit.collider.CompareTag("Player") || 
                    hit.collider.GetComponent<CharacterController>() != null;
            
            Debug.DrawRay(rayStart, dirToPlayer.normalized * hit.distance, canSee ? Color.green : Color.red);
        }
    }
    
    void OnGUI()
    {
        if (!showDebugInfo || player == null) return;
        
        // 在屏幕上显示调试信息
        float distance = Vector3.Distance(transform.position, player.transform.position);
        Vector3 dirToPlayer = player.transform.position - transform.position;
        float angle = Vector3.Angle(transform.forward, dirToPlayer.normalized);
        
        GUIStyle style = new GUIStyle();
        style.fontSize = 16;
        style.normal.textColor = Color.white;
        
        string info = $"Ghost调试信息:\n";
        info += $"距离Player: {distance:F1}米\n";
        info += $"角度: {angle:F1}度\n";
        info += $"是否在检测范围: {(distance <= 5f ? "是" : "否")}\n";
        info += $"是否在视野内: {(angle < 60f ? "是" : "否")}";
        
        GUI.Label(new Rect(10, 10, 300, 100), info, style);
    }
    
    // 编辑器按钮
    [ContextMenu("手动诊断")]
    void ManualDiagnose()
    {
        DiagnosePlayerSetup();
    }
    
    [ContextMenu("强制追逐测试")]
    void ForceChaseTest()
    {
        if (ghost != null && player != null)
        {
            ghost.StartChasing();
            Debug.Log("强制开始追逐！");
        }
    }
}