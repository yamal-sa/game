using System.Threading;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
public class HEALTH : MonoBehaviour

{
    public float HP = 100f;
    public Image BAR;
    public float maxHealth = 10f;
    public GameObject boss;
  
    void Start()
    {
        
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            TakeDamage(10);
        }
    }
    private void Update()
    {
        BAR.fillAmount = HP / 100;
    }
    public void TakeDamage(int damage)
    {
        HP -= damage;
        BAR.fillAmount = HP / maxHealth;
        if (HP <= 0)
        {
            HP = 0;
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

   
    }
