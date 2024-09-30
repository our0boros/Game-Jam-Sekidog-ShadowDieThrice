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
        // 子弹飞行5秒自动销毁
        Destroy(gameObject, lifetime);
        // 添加 AudioSource 组件（如果还没有的话）
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        // 设置 AudioSource 为 3D 音效, 0为2D音效
        audioSource.spatialBlend = 1f;
    }

    public void SetShooterInfo(bool isAlive, bool isShadow, bool isDead)
    {
        canDealDamage = isAlive;
        isShooterShadow = isShadow;

        if (isShadow && isDead)
        {
            // 如果是已死亡的影子发射的子弹，立即销毁
            Destroy(gameObject);
        }

        // 可以在这里添加视觉效果，例如改变子弹颜色
        if (!canDealDamage)
        {
            GetComponent<Renderer>().material.color = Color.gray; // 假设子弹有Renderer组件
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;
        Debug.Log(other.name);
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        // 如果玩家生命值不为空且不是己方玩家
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
                // Destroy(gameObject); // 如果这行被注释，那么子弹是幽灵子弹，会穿过物体
            }
            // 销毁子弹
            
        }
        // 如果击中的不是玩家，防止玩家命中自己
        else if (!other.CompareTag("Player"))
        {
            // 播放击中非玩家音效
            PlayHitSound(false);
            // 销毁子弹
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
                Destroy(audioObject, clipToPlay.length); // 确保音效播放完后销毁音效对象
                Debug.Log("Playing sound: " + clipToPlay.name);
            }
            else
            {
                Debug.LogWarning("Hit sound not assigned for " + (hitEnemy ? "enemy" : "non-enemy") + " in BulletBehavior.");
            }
        }
    }
}