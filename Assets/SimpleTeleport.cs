using UnityEngine;

public class SimpleTeleport : MonoBehaviour
{
    [Header("传送目标")]
    [Tooltip("玩家将被传送到的目标位置")]
    public Transform teleportDestination;
    
    private void Start()
    {
        // 检查是否设置了传送目标
        if (teleportDestination == null)
        {
            Debug.LogError($"[SimpleTeleport] {gameObject.name} 未设置传送目标!");
        }
        
        // 确保Collider是Trigger
        Collider col = GetComponent<Collider>();
        if (col != null && !col.isTrigger)
        {
            col.isTrigger = true;
            Debug.Log($"[SimpleTeleport] 已将 {gameObject.name} 的Collider设置为Trigger");
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // 检查是否是玩家
        if (other.CompareTag("Player"))
        {
            if (teleportDestination != null)
            {
                // 获取玩家的CharacterController（如果有的话）
                CharacterController controller = other.GetComponent<CharacterController>();
                
                if (controller != null)
                {
                    // 如果有CharacterController，需要先禁用
                    controller.enabled = false;
                    other.transform.position = teleportDestination.position;
                    controller.enabled = true;
                }
                else
                {
                    // 直接传送
                    other.transform.position = teleportDestination.position;
                }
                
                Debug.Log($"玩家已传送到: {teleportDestination.name}");
            }
            else
            {
                Debug.LogError("无法传送：未设置传送目标!");
            }
        }
    }
    
    // 在Scene视图中显示传送连线
    private void OnDrawGizmos()
    {
        if (teleportDestination != null)
        {
            // 绘制从触发器到目标的线
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, teleportDestination.position);
            
            // 绘制触发器位置（绿色）
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, Vector3.one * 0.5f);
            
            // 绘制目标位置（红色）
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(teleportDestination.position, 0.3f);
        }
    }
}