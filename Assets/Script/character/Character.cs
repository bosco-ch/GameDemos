using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.Events;
public class Character : MonoBehaviour
{
    [Header("����")]
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float currentHealth;
    public UnityEvent<float, float> bloodUI;
    [Header("�޵�")]
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
            //ִ�����˶���
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
        //Destroy(this.gameObject);//�ݻٵ�ǰ����
        OnDie?.Invoke();
    }
    //�޵�
    protected virtual IEnumerator invulnerableCoroutine()
    {
        invulnerable = true;
        //�޵�ʱ��
        yield return new WaitForSeconds(invulnerableDuration);
        invulnerable = false;
    }
}
