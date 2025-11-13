using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TP2 - Ariadna Delpiano

public abstract class Entity : MonoBehaviour, IDamageable
{
    public float speed;

    public int maxLife;
    protected int _life;

    private void Start()
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

