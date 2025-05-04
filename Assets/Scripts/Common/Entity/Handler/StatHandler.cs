using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatHandler : MonoBehaviour
{
    // Fields
    [Range(1, 100)][SerializeField] private int health = 10;
    [Range(1f, 20f)][SerializeField] private float speed = 5;

    // Properties
    public int Health { get => health; set => Mathf.Clamp(value, 0, 100); }
    public float Speed { get => speed; set => Mathf.Clamp(value, 0f, 20f); }
}
