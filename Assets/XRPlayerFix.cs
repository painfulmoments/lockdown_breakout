using UnityEngine;

// 将这个脚本添加到XR Origin上，自动修复设置
public class XRPlayerFix : MonoBehaviour
{
    void Start()
    {
        // 确保有Player标签
        if (!gameObject.CompareTag("Player"))
        {
            gameObject.tag = "Player";
            Debug.Log("已添加Player标签");
        }
        
        // 调整Character Controller
        CharacterController cc = GetComponent<CharacterController>();
        if (cc != null)
        {
            // 设置更合理的值
            cc.height = 1.8f;  // 成人高度
            cc.radius = 0.3f;  // 更大的半径，更容易被检测
            cc.center = new Vector3(0, 0.9f, 0);  // 中心在高度的一半
            
            Debug.Log("已调整Character Controller尺寸");
        }
        
        // 确保在正确的层
        if (gameObject.layer == 0) // Default层
        {
            // 可以设置为特定层，或保持默认
            Debug.Log($"Player在 {LayerMask.LayerToName(gameObject.layer)} 层");
        }
    }
    
    void OnDrawGizmos()
    {
        // 可视化Character Controller
        CharacterController cc = GetComponent<CharacterController>();
        if (cc != null)
        {
            // 绘制胶囊体表示Character Controller
            Gizmos.color = Color.green;
            Vector3 p1 = transform.position + cc.center + Vector3.up * (cc.height / 2 - cc.radius);
            Vector3 p2 = transform.position + cc.center - Vector3.up * (cc.height / 2 - cc.radius);
            Gizmos.DrawWireSphere(p1, cc.radius);
            Gizmos.DrawWireSphere(p2, cc.radius);
        }
    }
}