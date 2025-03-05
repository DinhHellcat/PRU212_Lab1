using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector2 direction;
    void Start()
    {
        
    }
    void Update()
    {
        transform.Translate(direction * Time.deltaTime * moveSpeed);
    }
    public void handleProjectile()
    {
        Destroy(gameObject, 1f);
    }
}
