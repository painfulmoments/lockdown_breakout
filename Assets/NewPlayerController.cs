using UnityEngine;
using UnityEngine.InputSystem;

public class NewPlayerController : MonoBehaviour
{
    public bool isDead = false;
    public bool isInRoom = false; // 玩家是否在Room Trigger内

    private Animator anim;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Awake()
    {
        anim = GetComponent<Animator>();
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;
        Debug.Log("Player died!");
        // 触发死亡动画
        anim.SetTrigger("Die");
    }

    public void RespawnAt(Vector3 position, Quaternion rotation, RoomDoorController doorController = null, Transform enemyRespawn = null, GameObject roomKey = null)
    {
        Debug.Log("Respawning player...");

        // 重置死亡状态
        isDead = false;

        // 重置位置
        transform.position = position;
        transform.rotation = rotation;

        // 重置动画
        anim.Rebind();
        anim.Update(0f);

        // 重置钥匙
        if (roomKey != null)
            roomKey.SetActive(true);

        // 重置鬼
        if (enemyRespawn != null)
        {
            GhostController.EnemyChase ghost = FindObjectOfType<GhostController.EnemyChase>();
            if (ghost != null)
            {
                ghost.transform.position = enemyRespawn.position;
                ghost.transform.rotation = enemyRespawn.rotation;
                ghost.currentState = GhostController.EnemyChase.State.Idle;
                ghost.navMeshAgent.ResetPath();
                ghost.isChaseActivated = false;
                ghost.SetMonsterVisible(false);
                ghost.SetCollidersEnabled(false);
            }
        }

        // 重置门
        if (doorController != null)
            doorController.SetDoorClosedState();
    }
}