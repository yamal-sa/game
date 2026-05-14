using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 10f;
    public float lifetime = 5f;  // УВЕЛИЧИЛ ВРЕМЯ ЖИЗНИ ДО 5 СЕКУНД

    void Start()
    {
        Destroy(gameObject, lifetime);
        Debug.Log("Пуля создана, будет жить: " + lifetime + " секунд");
    }

    void Update()
    {
        // Опционально: добавляем свечение или частицы для видимости
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Пуля столкнулась с: " + collision.gameObject.name);

        if (collision.gameObject.tag == "boss")
        {
            BossBattleSystem boss = collision.gameObject.GetComponent<BossBattleSystem>();
            if (boss != null)
            {
                boss.TakeDamageFromBullet(damage);
                Debug.Log("Попадание в босса! Урон: " + damage);
            }
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag != "Player")
        {
            Destroy(gameObject);
        }
    }
}