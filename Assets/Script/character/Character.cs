using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.Events;
public class Character : MonoBehaviour
{
    [Header("属性")]
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float currentHealth;
    public UnityEvent<float, float> bloodUI;
    [Header("无敌")]
    [SerializeField] public bool invulnerable = false;
    public UnityEvent OnHurt;
    public UnityEvent OnDie;
    public float invulnerableDuration;
    public string layer;

    protected virtual void OnEnable()
    {
        currentHealth = maxHealth;
    }
    public virtual void taskDamage(float damage)
    {
        if (invulnerable)
            return;
        // float res = currentHealth - damage;

        if (currentHealth - damage > 0f)
        {
            currentHealth -= damage;

            StartCoroutine(nameof(invulnerableCoroutine));
            //执行受伤动画
            OnHurt?.Invoke();
            bloodUI?.Invoke(currentHealth, maxHealth);
        }
        else
        {
            //die
            Die();
        }
    }
    public virtual void Die()
    {
        //
        currentHealth = 0f;
        //Destroy(this.gameObject);//摧毁当前对象
        OnDie?.Invoke();
    }
    //无敌
    protected virtual IEnumerator invulnerableCoroutine()
    {
        invulnerable = true;
        //无敌时间
        yield return new WaitForSeconds(invulnerableDuration);
        invulnerable = false;
    }
}
