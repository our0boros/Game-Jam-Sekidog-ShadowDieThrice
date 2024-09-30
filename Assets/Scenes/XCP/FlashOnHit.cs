using System.Collections;
using UnityEngine;

public class FlashOnHit : MonoBehaviour
{
    public Color flashColor = Color.red; // ��˸ʱ����ɫ
    public float flashDuration = 0.1f; // ÿ����˸�ĳ���ʱ��
    public SkinnedMeshRenderer meshRenderer; // ʹ�� MeshRenderer
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
        if (collision.relativeVelocity.magnitude > 1) // �ж���ײ��ǿ��
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
        meshRenderer.material.color = flashColor; // �ı���ɫΪ��˸ɫ
        yield return new WaitForSeconds(flashDuration); // �ȴ���˸ʱ��
        meshRenderer.material.color = originalColor; // �ָ�ԭɫ
        isFlashing = false;
    }
}
