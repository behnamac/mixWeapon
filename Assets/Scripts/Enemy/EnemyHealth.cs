using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private Renderer[] renderers;
    [SerializeField] private Color hitColor;

    public EnemyWaveController waveController { get; set; }
    public bool IsDead { get; private set; }
    public UnityAction OnDead;

    private float _currentHealth;
    private bool _hit;
    private void Awake()
    {
    }
    private void Start()
    {
        _currentHealth = maxHealth;
    }
    
    public void TakeDamage(float value)
    {
        _currentHealth -= value;
        if (_currentHealth <= 0)
        {
            Dead();
        }
        if (!_hit)
            StartCoroutine(HitReact());
    }
    
    public void Dead()
    {
        if (IsDead) return;
        IsDead = true;
        int money = 5;
        UIController.instance.AddCoin(money, transform.position);
        waveController.OnEnemyDead();
        OnDead?.Invoke();
        Destroy(gameObject);
    }

    public void ChangeMaxHealth(float value) 
    {
        maxHealth = value;
        _currentHealth = maxHealth;
    }

    private IEnumerator HitReact() 
    {
        _hit = true;
        List<ColorHolder> colorsHolder = new List<ColorHolder>();
        for (int i = 0; i < renderers.Length; i++)
        {
            ColorHolder colorHolder = new ColorHolder();
            colorHolder.colors = new List<Color>();
            for (int j = 0; j < renderers[i].materials.Length; j++)
            {
                colorHolder.colors.Add(renderers[i].materials[j].color);
                renderers[i].materials[j].color = hitColor;
            }
            colorsHolder.Add(colorHolder);
        }
        yield return new WaitForSeconds(0.7f);
        for (int i = 0; i < renderers.Length; i++)
        {
            for (int j = 0; j < renderers[i].materials.Length; j++)
            {
                renderers[i].materials[j].color = colorsHolder[i].colors[j];
            }
        }
        _hit = false;
    }

    struct ColorHolder 
    {
        public List<Color> colors;
    }
}
