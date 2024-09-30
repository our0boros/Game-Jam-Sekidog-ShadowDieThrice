using System.Collections;
using UnityEngine;

public class FlashOnHit : MonoBehaviour
{
    public Color flashColor = Color.red; // 闪烁时的颜色
    public float flashDuration = 0.1f; // 每次闪烁的持续时间
    public SkinnedMeshRenderer meshRenderer; // 使用 MeshRenderer
    private Color originalColor;
    private bool isFlashing = false;

    private void Start()
    {
        //meshRenderer = GetComponentInChildren<MeshRenderer>();
        if (meshRenderer != null)
        {
            originalColor = meshRenderer.material.color;
        }
        else
        {
            Debug.LogError("MeshRenderer component not found!");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 1) // 判断碰撞的强度
        {
            if (!isFlashing)
            {
                StartCoroutine(FlashEffect());
            }
        }
    }
    public void Flash()
    {
        if (!isFlashing)
        {
            StartCoroutine(FlashEffect());
        }
    }

    private IEnumerator FlashEffect()
    {
        isFlashing = true;
        meshRenderer.material.color = flashColor; // 改变颜色为闪烁色
        yield return new WaitForSeconds(flashDuration); // 等待闪烁时间
        meshRenderer.material.color = originalColor; // 恢复原色
        isFlashing = false;
    }
}
