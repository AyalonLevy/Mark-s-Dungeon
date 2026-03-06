using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthBar;

    private Entity entity;


    private void Awake()
    {
        entity = GetComponent<Entity>();
    }

    private void Update()
    {
        healthBar.value = entity.CurrentHealth / entity.MaxHealth;
    }
}
