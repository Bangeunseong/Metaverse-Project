using System;
using UnityEditor.Animations;
using UnityEngine;

[Serializable]public class CharacterData
{
    public int id;
    public Sprite characterSprite;
    public AnimatorController controller;
    public AnimationClip idleClip;
    public AnimationClip damageClip;
    public AnimationClip moveClip;
    public Color trailColor;
}
