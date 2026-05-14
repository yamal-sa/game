using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBattleSystem : MonoBehaviour
{
    [Header("UI Elements")]
    public Button attackButton;
    public Button startButton;
    public Text attackCounterText;
    public Text battleLogText;
    public Text bossHpText;

    [Header("Boss Settings")]
    public int bossMaxHp = 30;
    public int playerDamage = 5;

    [Header("Shooting Settings")]
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float bulletSpeed = 10f;

    private int currentBossHp;
    private int plannedAttacks = 0;
    private Queue<int> attackQueue;
    private bool isBattleActive = false;

    void Start()
    {
        currentBossHp = bossMaxHp;
        attackQueue = new Queue<int>(); 
        UpdateUI();

        attackButton.onClick.AddListener(OnAttackButtonPressed);
        startButton.onClick.AddListener(OnStartButtonPressed);

        battleLogText.text = "Босс появился! Нажмите Attack для планирования ударов, затем Start.";
    }

    void UpdateUI()
    {
        attackCounterText.text = "Запланировано атак: " + plannedAttacks;
        bossHpText.text = "HP Босса: " + currentBossHp + " / " + bossMaxHp;
    }

    void ShootBullet()
    {
        if (bulletPrefab != null && shootPoint != null)
        {
            // Создаём шар
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);

            // Получаем компонент Rigidbody2D
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                // Задаём скорость полёта шара
                rb.linearVelocity = shootPoint.right * bulletSpeed;
            }

            // УВЕЛИЧИВАЕМ ВРЕМЯ ЖИЗНИ - ТЕПЕРЬ ШАР БУДЕТ ЖИТЬ 5 СЕКУНД
            Destroy(bullet, 5f);

            Debug.Log("Шар создан! Позиция: " + shootPoint.position);
        }
        else
        {
            Debug.LogError("BulletPrefab или ShootPoint не назначены!");
        }
    }

    public void TakeDamageFromBullet(float damage)
    {
        if (currentBossHp > 0 && !isBattleActive)
        {
            currentBossHp -= (int)damage;
            if (currentBossHp < 0) currentBossHp = 0;

            UpdateUI();
            battleLogText.text = $"Пуля нанесла {damage} урона! Осталось HP: {currentBossHp}";

            if (currentBossHp <= 0)
            {
                battleLogText.text = "ПОБЕДА! Босс повержен от пули!";
                bossHpText.text = "HP Босса: 0 / " + bossMaxHp;
            }
        }
    }

    void OnAttackButtonPressed()
    {
        if (isBattleActive)
        {
            battleLogText.text = "Битва уже идёт! Нельзя планировать новые атаки сейчас.";
            return;
        }

        if (currentBossHp <= 0)
        {
            battleLogText.text = "Босс уже побеждён! Начните новую битву (перезапустите сцену).";
            return;
        }

        plannedAttacks++;
        attackQueue.Enqueue(playerDamage);
        UpdateUI();

        ShootBullet();
        StartCoroutine(ShowAttackText());
    }

    IEnumerator ShowAttackText()
    {
        GameObject tempTextObj = new GameObject("AttackTextEffect");
        TextMesh textMesh = tempTextObj.AddComponent<TextMesh>();
        textMesh.text = "ATTACK!";
        textMesh.fontSize = 40;
        textMesh.color = Color.red;
        textMesh.anchor = TextAnchor.MiddleCenter;

        tempTextObj.transform.position = new Vector3(0, 2, 0);

        yield return new WaitForSeconds(0.5f);
        Destroy(tempTextObj);
    }

    void OnStartButtonPressed()
    {
        if (isBattleActive)
        {
            battleLogText.text = "Битва уже идёт!";
            return;
        }

        if (plannedAttacks == 0)
        {
            battleLogText.text = "Нет запланированных атак! Нажмите Attack.";
            return;
        }

        if (currentBossHp <= 0)
        {
            battleLogText.text = "Босс мёртв. Перезапустите сцену.";
            return;
        }

        StartCoroutine(ExecuteAllAttacks());
    }

    IEnumerator ExecuteAllAttacks()
    {
        isBattleActive = true;
        battleLogText.text = "Битва началась! Выполняются атаки...";

        while (attackQueue.Count > 0 && currentBossHp > 0)
        {
            int damage = attackQueue.Dequeue();
            plannedAttacks--;

            currentBossHp -= damage;
            if (currentBossHp < 0) currentBossHp = 0;

            UpdateUI();
            battleLogText.text = $"Нанесено {damage} урона! Осталось HP: {currentBossHp}";

            yield return new WaitForSeconds(0.8f);
        }

        if (currentBossHp <= 0)
        {
            battleLogText.text = "ПОБЕДА! Босс повержен.";
            bossHpText.text = "HP Босса: 0 / " + bossMaxHp;
        }
        else
        {
            battleLogText.text = "Все запланированные атаки выполнены. Можете добавить ещё.";
        }

        isBattleActive = false;
    }
}   