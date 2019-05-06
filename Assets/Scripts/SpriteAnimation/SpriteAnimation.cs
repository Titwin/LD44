using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimation : ScriptableObject
{
    [SerializeField] public string name;
    [SerializeField] public Sprite[] frames;
    [SerializeField] public float animationDuration;
    [SerializeField] public bool loop = true;
    [SerializeField] public float minDuration = 0;
}
