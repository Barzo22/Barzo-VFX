using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Entity : MonoBehaviour, IDamageable
{
    public float speed;

    public int maxLife;
    protected int _life;

    public void Start()
    {
        _life = maxLife;
    }

    public virtual void TakeDamage(int dmg)
    {
        _life -= dmg;

        if (_life <= 0)
            Destroy(gameObject);
    }
}

