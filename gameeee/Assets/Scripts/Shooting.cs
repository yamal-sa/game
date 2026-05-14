using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    public GameObject bullet;
    public Transform parent;
    public float bulletSpeed = 10f;
    private float HP = 100f;
    public Image BAR;


    void Start()
    {

    }

    void ShootBullet()
    {

        GameObject clone = Instantiate(bullet, parent);
        Rigidbody2D rb = clone.GetComponent<Rigidbody2D>();


        if (rb != null)
        {
            rb.linearVelocity = transform.right * bulletSpeed;
        }
       
    }
    public void Shoot()
    {
        ShootBullet();
        

    }
    
}