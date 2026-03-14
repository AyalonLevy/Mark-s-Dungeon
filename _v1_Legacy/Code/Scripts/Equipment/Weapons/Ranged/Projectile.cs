using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _speed = 20.0f;
    private float _damage;
    private LayerMask _targetLayer;
    private Vector3 _direction;

    private float _selfDestructTime = 5.0f;

    public void Launch(Vector3 targetPos, float damage, LayerMask targetLayer)
    {
        _damage = damage;
        _targetLayer = targetLayer;
        _direction = (targetPos - transform.position).normalized;

        Destroy(gameObject, _selfDestructTime);
    }

    private void Update()
    {
        transform.position += _direction * _speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & _targetLayer.value) != 0)
        {
            if (other.TryGetComponent(out IDamagable victim))
            {
                victim.Damage(_damage);
            }
            Destroy(gameObject);
        }
    }
}
