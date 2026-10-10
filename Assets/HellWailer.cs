using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HellWailer : MonoBehaviour
{
    [Header("武器设置")]
    [SerializeField] private float rayDistance = 20f; // 射线长度
    [SerializeField] private Transform shootOrigin;   // 射线发射点（建议放在枪口）

    [Header("效果")]
    [SerializeField] private ParticleSystem particles; // 粒子效果
    [SerializeField] private AudioSource audioSource;  // 音效源
    [SerializeField] private Light muzzleLight;        // 枪口光效（可选）

    [Header("调试")]
    [SerializeField] private bool showDebugRay = true;  // 显示调试射线
    [SerializeField] private Color debugRayColor = Color.red;

    private bool isShooting = false;
    private DungeonGhost currentTargetGhost = null; // 当前被减速的 Ghost

    void Start()
    {
        // 获取 XR 抓取组件
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.activated.AddListener(OnActivate);   // 扣动扳机
            grabInteractable.deactivated.AddListener(OnDeactivate); // 松开扳机
        }
        else
        {
            Debug.LogWarning("HellWailer: 未找到 XRGrabInteractable 组件！");
        }

        // 自动查找组件
        if (particles == null)
            particles = GetComponentInChildren<ParticleSystem>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (shootOrigin == null)
            shootOrigin = transform;
    }

    private void OnActivate(ActivateEventArgs args)
    {
        StartShoot();
    }

    private void OnDeactivate(DeactivateEventArgs args)
    {
        StopShoot();
    }

    void Update()
    {
        if (!isShooting) return;

        PerformRaycast();
    }

    void PerformRaycast()
    {
        // 射线方向：沿 shootOrigin 的 up 轴（或 forward，根据枪模型调整）
        Vector3 shootDirection = shootOrigin.up; // 或 shootOrigin.forward
        Ray ray = new Ray(shootOrigin.position, shootDirection);

        // 调试射线
        if (showDebugRay)
        {
            Debug.DrawRay(shootOrigin.position, shootDirection * rayDistance, debugRayColor);
        }

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            // 检查是否击中了 DungeonGhost（无论是否为 Trigger，只要能射中）
            DungeonGhost hitGhost = hit.collider?.GetComponent<DungeonGhost>();

            if (hitGhost != null)
            {
                // 如果是新目标，取消旧目标的减速
                if (currentTargetGhost != null && currentTargetGhost != hitGhost)
                {
                    currentTargetGhost.SetSlowMotion(false);
                }

                // 设置新目标并开启减速
                currentTargetGhost = hitGhost;
                if (!currentTargetGhost.isSlowMotion)
                {
                    currentTargetGhost.SetSlowMotion(true);
                }

                if (showDebugRay)
                {
                    Debug.Log($"减速枪命中 Ghost: {hitGhost.name} 距离: {hit.distance:F1}米");
                }
            }
            else
            {
                // 击中了非 Ghost 物体，取消当前 Ghost 的减速
                ClearGhostSlowMotion();
            }
        }
        else
        {
            // 未击中任何物体，取消减速
            ClearGhostSlowMotion();
        }
    }

    public void StartShoot()
    {
        if (isShooting) return;

        isShooting = true;

        // 启动视觉/音效
        if (particles != null) particles.Play();
        if (audioSource != null && !audioSource.isPlaying) audioSource.Play();
        if (muzzleLight != null) muzzleLight.enabled = true;

        Debug.Log("🌀 开始射击：减速射线激活");
    }

    public void StopShoot()
    {
        if (!isShooting) return;

        isShooting = false;

        // 停止特效
        if (particles != null) particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        if (audioSource != null && audioSource.isPlaying) audioSource.Stop();
        if (muzzleLight != null) muzzleLight.enabled = false;

        // ✅ 松开扳机：取消当前 Ghost 的减速
        ClearGhostSlowMotion();

        Debug.Log("🛑 停止射击：减速效果已关闭");
    }

    void ClearGhostSlowMotion()
    {
        if (currentTargetGhost != null)
        {
            currentTargetGhost.SetSlowMotion(false);
            currentTargetGhost = null;
        }
    }

    void OnDisable()
    {
        StopShoot();
    }

    void OnDestroy()
    {
        // 确保销毁时清除减速
        ClearGhostSlowMotion();
    }

    // 编辑器可视化
    void OnDrawGizmosSelected()
    {
        if (shootOrigin == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(shootOrigin.position, shootOrigin.up * rayDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(shootOrigin.position, 0.05f);
    }
}