using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using System.Runtime.InteropServices;
public class Animator : MonoGameLibrary.GameComponent
{
    private Dictionary<string, AnimatedSprite> keyValuePairs;
    public AnimatedSprite curAnimation { get; private set; }
    public string curName { get; private set; }
    /// <summary>
    /// 运行时获得SpriteRender，不需要Clone复制
    /// </summary>
    private SpriteRender spriteRender;
    public Animator()
    {
        keyValuePairs = new Dictionary<string, AnimatedSprite>();
    }
    public override void Update()
    {
        if (spriteRender == null) spriteRender = gameObject.GetComponent<SpriteRender>();
        else if (spriteRender.sprite != curAnimation)
        {
            spriteRender.sprite = curAnimation;
        }
        curAnimation.Update(gameObject.sence.gameTime);
    }

    public bool ChangeAnimation(string name)
    {
        if (keyValuePairs.ContainsKey(name))
        {
            curName = name;
            curAnimation = keyValuePairs[name];
            return true;
        }
        return false;
    }

    public bool RegisterAnimation(string name, AnimatedSprite sprite)
    {
        if (!keyValuePairs.ContainsKey(name))
        {
            keyValuePairs.Add(name, sprite);
            return true;
        }
        return false;
    }

    public void ClearAnimation()
    {
        keyValuePairs.Clear();
        curAnimation = null;
    }

    public override object Clone()
    {
        Animator animator = new Animator();
        foreach (var item in keyValuePairs)
        {
            animator.RegisterAnimation(item.Key, item.Value.Clone() as AnimatedSprite);
        }
        animator.ChangeAnimation(curName);
        return animator;
    }
}