using UnityEngine;
using UnityEngine.UI; // ����������UI��ʾ����ֵ

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth;
    public int playerNumber;
    public bool isAlive = true;


    // ��ѡ�����UI Text�������ʾ����ֵ
    public Text healthText;
    public GameObject healthBarObject;
    private Vector3 healthBarOriginalScale = new Vector3(1,0.1f,0.1f);

    private void Start()
    {
        currentHealth = maxHealth;
        if (healthBarObject != null)
        {
            //healthBarOriginalScale = healthBarObject.transform.localScale;
        }
        UpdateHealthDisplay();
        //ResetHealth();
    }
    //private void OnEnable()
    //{
    //    currentHealth = maxHealth;
    //    UpdateHealthDisplay();
    //}

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (GetComponent<FlashOnHit>() != null)
        {
            GetComponent<FlashOnHit>().Flash();
        }
        Debug.Log("���" + playerNumber + "����ֵ-1");
        if (currentHealth <= 0)
        {
            Debug.Log("���" + playerNumber + "����");
            currentHealth = 0;
            Die();
            isAlive = false;
        }
        UpdateHealthDisplay();
    }

    private void Die()
    {
        // ������������߼�
        Debug.Log("Player " + playerNumber + " has died!");
        // �ӷ��߼�
        if(isAlive)
            ScoreManager.Instance.AddScore(playerNumber == 1? 2: 1, 1);
        if (GetComponent<ReplayPlayerData>().replay) {
            gameObject.transform.SetPositionAndRotation(new Vector3(0, -10000, 0), Quaternion.identity);
            //�����д�����Ϊ���ûط�ֹͣ������ᱻ�طŹ��ܴ�����
            GetComponent<ReplayPlayerData>().replay = false;
            //gameObject.SetActive(false);
        }


        // ������������������߼��������Ϸ
    }

    private void UpdateHealthDisplay()
    {
        if (healthText != null)
        {
            healthText.text = "P" + playerNumber + " Health: " + currentHealth;
        }
        if (healthBarObject != null)
        {
            float healthPercentage = (float)currentHealth / maxHealth;
            Vector3 newScale = healthBarOriginalScale;
            newScale.x = healthBarOriginalScale.x * healthPercentage;
            healthBarObject.transform.localScale = newScale;
        }
    }
    // �������һ�����÷����������ڻغϿ�ʼʱ��������ֵ
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        isAlive = true;
        //�����Ӱ�ӣ�������³���
        if(GetComponent<ReplayPlayerData>().replay)
            gameObject.SetActive(true);
        //// Ϊ�˽��Ѫ����ʧ������
        //if (healthBarObject != null)
        //{
        //    healthBarObject.transform.localScale = healthBarOriginalScale;
        //}
        UpdateHealthDisplay();
    }

    private void LateUpdate()
    {
        // ȷ��Ѫ��ʼ���������
        if (healthBarObject != null && Camera.main != null)
        {
            healthBarObject.transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                Camera.main.transform.rotation * Vector3.up);
        }
    }
}