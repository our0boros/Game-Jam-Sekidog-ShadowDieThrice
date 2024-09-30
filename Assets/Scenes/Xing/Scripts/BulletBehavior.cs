using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public float lifetime = 5f;
    public int damage = 1;
    public int playerNumber; // 1 for Player 1, 2 for Player 2

    [Header("Audio")]
    public AudioClip hitEnemySound;
    public AudioClip hitNonEnemySound;
    [Range(0f, 1f)]
    public float hitSoundVolume = 1f;

    private AudioSource audioSource;
    private bool canDealDamage = true;
    private bool isShooterShadow = false;
    private bool hasHit = false; // New variable to track if the bullet has hit something
    private void Start()
    {   
        // �ӵ�����5���Զ�����
        Destroy(gameObject, lifetime);
        // ��� AudioSource ����������û�еĻ���
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        // ���� AudioSource Ϊ 3D ��Ч, 0Ϊ2D��Ч
        audioSource.spatialBlend = 1f;
    }

    public void SetShooterInfo(bool isAlive, bool isShadow, bool isDead)
    {
        canDealDamage = isAlive;
        isShooterShadow = isShadow;

        if (isShadow && isDead)
        {
            // �������������Ӱ�ӷ�����ӵ�����������
            Destroy(gameObject);
        }

        // ��������������Ӿ�Ч��������ı��ӵ���ɫ
        if (!canDealDamage)
        {
            GetComponent<Renderer>().material.color = Color.gray; // �����ӵ���Renderer���
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;
        Debug.Log(other.name);
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        // ����������ֵ��Ϊ���Ҳ��Ǽ������
        if (playerHealth != null && playerHealth.playerNumber != playerNumber)
        {
            if (canDealDamage)
            {
                playerHealth.TakeDamage(damage);
                PlayHitSound(true);
                hasHit = true; // Mark the bullet as having hit something
                Destroy(gameObject);
            }
            else
            {
                hasHit = true; // Mark the bullet as having hit something
                PlayHitSound(false);
                // Destroy(gameObject); // ������б�ע�ͣ���ô�ӵ��������ӵ����ᴩ������
            }
            // �����ӵ�
            
        }
        // ������еĲ�����ң���ֹ��������Լ�
        else if (!other.CompareTag("Player"))
        {
            // ���Ż��з������Ч
            PlayHitSound(false);
            // �����ӵ�
            hasHit = true; // Mark the bullet as having hit something
            Destroy(gameObject);
        }


    }

    private void PlayHitSound(bool hitEnemy)
    {
        if (audioSource != null)
        {
            AudioClip clipToPlay = hitEnemy ? hitEnemySound : hitNonEnemySound;
            if (clipToPlay != null)
            {
                //audioSource.PlayOneShot(clipToPlay, hitSoundVolume);
                GameObject audioObject = new GameObject("BulletHitSound");
                AudioSource audioSource = audioObject.AddComponent<AudioSource>();
                audioSource.spatialBlend = 1f;
                audioSource.PlayOneShot(clipToPlay, hitSoundVolume);
                Destroy(audioObject, clipToPlay.length); // ȷ����Ч�������������Ч����
                Debug.Log("Playing sound: " + clipToPlay.name);
            }
            else
            {
                Debug.LogWarning("Hit sound not assigned for " + (hitEnemy ? "enemy" : "non-enemy") + " in BulletBehavior.");
            }
        }
    }
}