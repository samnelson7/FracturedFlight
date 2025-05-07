using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int baseHP;
    private int currentHP;

    private void Awake()
    {
        currentHP = baseHP;
    }
    public void TakeDamage()
    {

    }
}
