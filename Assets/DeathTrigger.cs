using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathTrigger : MonoBehaviour
{
    [Header("死亡设置")]
    [SerializeField] private float restartDelay = 3f; // 重启延迟时间
    [SerializeField] private bool freezeOnDeath = true; // 死亡时是否冻结游戏
    
    [Header("可选：死亡效果")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip deathSound; // 死亡音效
    [SerializeField] private GameObject deathEffectPrefab; // 死亡特效预制体
    
    private bool isProcessingDeath = false; // 防止重复触发
    private static DeathTrigger instance; // 单例，用于管理所有死亡触发器
    
    void Awake()
    {
        // 确保触发器设置正确
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = true;
        }
        
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }
    
    void OnTriggerEnter(Collider other)
    {
        // 检查是否碰到了Player
        if (other.CompareTag("Player") && !isProcessingDeath)
        {
            Debug.Log("Ghost抓到了Player！触发死亡");
            TriggerDeath();
        }
    }
    
    void TriggerDeath()
    {
        if (isProcessingDeath) return;
        isProcessingDeath = true;
        
        // 播放死亡音效
        if (audioSource && deathSound)
        {
            audioSource.PlayOneShot(deathSound);
        }
        
        // 生成死亡特效
        if (deathEffectPrefab != null)
        {
            GameObject effect = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, 5f); // 5秒后清理特效
        }
        
        // 冻结游戏
        if (freezeOnDeath)
        {
            // 暂停所有物理和动画
            Time.timeScale = 0f;
            
            // 因为Time.timeScale = 0会暂停协程，我们需要使用真实时间
            StartCoroutine(RestartGameWithRealTime());
        }
        else
        {
            StartCoroutine(RestartGame());
        }
    }
    
    IEnumerator RestartGameWithRealTime()
    {
        float pauseEndTime = Time.realtimeSinceStartup + restartDelay;
        
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return 0;
        }
        
        // 恢复时间流速
        Time.timeScale = 1f;
        
        // 重置游戏
        ResetGame();
    }
    
    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(restartDelay);
        ResetGame();
    }
    
    void ResetGame()
    {
        // 方法1：重新加载当前场景（简单但会重置所有东西）
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
        // 方法2：调用GameManager的重置方法（推荐）
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.RestartGame();
        }
        else
        {
            // 如果没有GameManager，重新加载场景
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        isProcessingDeath = false;
    }
    
    // 在编辑器中显示触发器范围
    void OnDrawGizmos()
    {
        Collider col = GetComponent<Collider>();
        if (col != null && col.isTrigger)
        {
            Gizmos.color = new Color(1, 0, 0, 0.3f); // 半透明红色
            
            // 根据不同类型的碰撞器绘制
            if (col is BoxCollider)
            {
                BoxCollider box = col as BoxCollider;
                Matrix4x4 oldMatrix = Gizmos.matrix;
                Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
                Gizmos.DrawCube(box.center, box.size);
                Gizmos.matrix = oldMatrix;
            }
            else if (col is SphereCollider)
            {
                SphereCollider sphere = col as SphereCollider;
                Gizmos.DrawSphere(transform.position + sphere.center, sphere.radius * transform.lossyScale.x);
            }
        }
    }
}