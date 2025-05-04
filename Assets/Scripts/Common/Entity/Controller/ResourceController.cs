using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    [SerializeField] private float healthChangeDelay = .5f;

    private BaseController baseController;
    private StatHandler statHandler;
    private AnimationHandler animationHandler;

    private float timeSinceLastChange = float.MaxValue;

    public float CurrentHealth { get; private set; }
    public float MaxHealth => statHandler.Health;

    public AudioClip damageClip;

    public Action<float> OnChangeHealth;

    private void Awake()
    {
        baseController = Helper.GetComponent_Helper<BaseController>(gameObject);
        statHandler = Helper.GetComponent_Helper<StatHandler>(gameObject);
        animationHandler = Helper.GetComponent_Helper<AnimationHandler>(gameObject);
    }

    private void Start()
    {
        CurrentHealth = statHandler.Health;
    }

    private void Update()
    {
        if (timeSinceLastChange < healthChangeDelay)
        {
            timeSinceLastChange += Time.deltaTime;
            if (timeSinceLastChange >= healthChangeDelay) animationHandler.InvincibilityEnd();
        }
    }

    public bool ChangeHealth(float change)
    {
        if (change == 0 || timeSinceLastChange < healthChangeDelay) { return false; }

        timeSinceLastChange = 0f;
        CurrentHealth += change;
        CurrentHealth = CurrentHealth > MaxHealth ? MaxHealth : CurrentHealth;
        CurrentHealth = CurrentHealth < 0 ? 0 : CurrentHealth;

        OnChangeHealth?.Invoke(CurrentHealth);

        if (change < 0)
        {
            animationHandler.Damage();
        }

        if (CurrentHealth <= 0f)
        {
            Die();
        }

        return true;
    }

    private void Die() { baseController.Die(); }

    public void AddHealthChangeEvent(Action<float> action)
    {
        OnChangeHealth += action;
    }

    public void RemoveHealthChangeEvent(Action<float> action) { OnChangeHealth -= action; }
}
