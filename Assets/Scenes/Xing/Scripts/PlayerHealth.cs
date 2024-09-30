using UnityEngine;
using UnityEngine.UI; // 如果您想添加UI显示生命值

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth;
    public int playerNumber;
    public bool isAlive = true;


    // 可选：添加UI Text组件来显示生命值
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
        Debug.Log("玩家" + playerNumber + "生命值-1");
        if (currentHealth <= 0)
        {
            Debug.Log("玩家" + playerNumber + "死亡");
            currentHealth = 0;
            Die();
            isAlive = false;
        }
        UpdateHealthDisplay();
    }

    private void Die()
    {
        // 处理玩家死亡逻辑
        Debug.Log("Player " + playerNumber + " has died!");
        // 加分逻辑
        if(isAlive)
            ScoreManager.Instance.AddScore(playerNumber == 1? 2: 1, 1);
        if (GetComponent<ReplayPlayerData>().replay) {
            gameObject.transform.SetPositionAndRotation(new Vector3(0, -10000, 0), Quaternion.identity);
            //加这行代码是为了让回放停止，否则会被回放功能传回来
            GetComponent<ReplayPlayerData>().replay = false;
            //gameObject.SetActive(false);
        }


        // 可以在这里添加重生逻辑或结束游戏
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
    // 可以添加一个重置方法，用于在回合开始时重置生命值
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        isAlive = true;
        //如果是影子，则会重新出现
        if(GetComponent<ReplayPlayerData>().replay)
            gameObject.SetActive(true);
        //// 为了解决血条消失的问题
        //if (healthBarObject != null)
        //{
        //    healthBarObject.transform.localScale = healthBarOriginalScale;
        //}
        UpdateHealthDisplay();
    }

    private void LateUpdate()
    {
        // 确保血条始终面向相机
        if (healthBarObject != null && Camera.main != null)
        {
            healthBarObject.transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                Camera.main.transform.rotation * Vector3.up);
        }
    }
}