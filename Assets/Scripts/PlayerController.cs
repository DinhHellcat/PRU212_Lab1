using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private ProjectileController projectile;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private float firingCooldown;
    private float tempCooldown;
    void Start()
    {
        
    }
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 direction = new Vector2(horizontal, vertical);
        transform.Translate(direction * Time.deltaTime * moveSpeed);

        if (Input.GetKey(KeyCode.Space))
        {
            if (tempCooldown <= 0)
            {
                Fire();
                tempCooldown = firingCooldown;
            }
        }
        tempCooldown -= Time.deltaTime;
    }
    private void Fire()
    {
        ProjectileController p = Instantiate(projectile, firingPoint.position, Quaternion.identity, null);
        p.handleProjectile();
    }
}
