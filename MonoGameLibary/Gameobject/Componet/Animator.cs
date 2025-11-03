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
}