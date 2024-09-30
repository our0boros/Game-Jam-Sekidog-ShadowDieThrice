using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeInAndSwitchScene : MonoBehaviour {
    public string sceneToLoad; // Ҫ���صĳ�������
    public float fadeDuration = 2.0f; // ͸���ȱ仯�ĳ���ʱ��
    public string playerAName = "DogPlayer1";
    public string playerBName = "DogPlayer2";

    private bool isFading = false; // �Ƿ����ڽ���͸���ȱ仯
    private Renderer objectRenderer; // �������Ⱦ��
    private bool playerAInTrigger = false; // ���A�Ƿ��ڴ�������
    private bool playerBInTrigger = false; // ���B�Ƿ��ڴ�������


    private void Start() {
        objectRenderer = GetComponent<Renderer>();
        // ��ʼ��͸����Ϊ0����ȫ͸����
        Color initialColor = objectRenderer.material.color;
        objectRenderer.material.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.name == playerAName) {
            playerAInTrigger = true;
        } else if (other.name == playerBName) {
            playerBInTrigger = true;
        }

        if (playerAInTrigger && playerBInTrigger && !isFading) {
            StartCoroutine(FadeIn());
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.name == playerAName) {
            playerAInTrigger = false;
        } else if (other.name == playerBName) {
            playerBInTrigger = false;
        }

        if (isFading) {
            StopAllCoroutines();
            ResetTransparency();
        }
    }

    private IEnumerator FadeIn() {
        isFading = true;
        float startAlpha = objectRenderer.material.color.a;
        float rate = 1.0f / fadeDuration;
        float progress = 0.0f;

        while (progress < 1.0f) {
            Color tmpColor = objectRenderer.material.color;
            objectRenderer.material.color = new Color(tmpColor.r, tmpColor.g, tmpColor.b, Mathf.Lerp(startAlpha, 1, progress));
            progress += rate * Time.deltaTime;
            yield return null;
        }

        objectRenderer.material.color = new Color(objectRenderer.material.color.r, objectRenderer.material.color.g, objectRenderer.material.color.b, 1);
        isFading = false;

        SceneManager.LoadScene(sceneToLoad);
    }

    private void ResetTransparency() {
        Color tmpColor = objectRenderer.material.color;
        objectRenderer.material.color = new Color(tmpColor.r, tmpColor.g, tmpColor.b, 0.0f);
        isFading = false;
    }
}
