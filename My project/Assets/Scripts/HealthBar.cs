using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider; // HP 바 UI 슬라이더
    public float maxHealth = 100f; // 최대 HP
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    // 데미지를 입었을 때 호출될 함수
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // 좀비가 죽었을 때 처리할 로직 (예: 오브젝트 파괴, 애니메이션 재생 등)
        Debug.Log(gameObject.name + " died!");
        Destroy(gameObject);
    }

    // 좀비가 활성화될 때 HP 바도 함께 활성화되도록 (옵션)
    void OnEnable()
    {
        if (healthSlider != null) healthSlider.gameObject.SetActive(true);
    }

    // 좀비가 비활성화될 때 HP 바도 함께 비활성화되도록 (옵션)
    void OnDisable()
    {
        if (healthSlider != null) healthSlider.gameObject.SetActive(false);
    }
}
