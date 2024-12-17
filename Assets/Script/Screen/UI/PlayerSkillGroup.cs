using System;
using System.Collections;
using System.Collections.Generic;
using Sophia.UserInterface;
using UnityEngine;

public class PlayerSkillGroup : MonoBehaviour
{
    [field: SerializeField] public List<PlayerSkillCoolUI> PlayerSkillCoolUIs { get; private set; } = new List<PlayerSkillCoolUI>();
    bool IsVisible = true;
    private bool IsInitialized = false;

    private float transparentAlpha = -1;
    private float originAlpha = 0.125f;
    
    public float TransparentAlpha 
    {
        get 
        {
            if(!IsInitialized) throw new Exception("초기화 안됨");
            return transparentAlpha;
        }
        set 
        {
            if(!IsInitialized) 
            {
                transparentAlpha = value;
                IsInitialized = true;
            }
        }
    }
    public float OriginAlpha 
    {
        get 
        {
            if(!IsInitialized) throw new Exception("초기화 안됨");
            return originAlpha;
        }
        set 
        {
            if(!IsInitialized) 
            {
                originAlpha = value;
                IsInitialized = true;
            }
        }
    }

    private void Awake()
    {
        TransparentAlpha = 0;
        OriginAlpha = PlayerSkillCoolUIs[0].fill.color.a;
    }

    private void OnDisable()
    {
        Debug.Log("Disable");
    }

    public void SetInvisible()
    {
        if(IsVisible == false) return;
        IsVisible = false;
        foreach (var playerSkillCoolUI in PlayerSkillCoolUIs)
        {
            Color transparentFillColor = playerSkillCoolUI.fill.color;
            Color transparentIconColor = playerSkillCoolUI.icon.color;
            Color transparentTextColor = playerSkillCoolUI.textMeshPro.color;
            transparentFillColor.a = TransparentAlpha;
            transparentIconColor.a = TransparentAlpha;
            transparentTextColor.a = TransparentAlpha;
            
            playerSkillCoolUI.fill.color = transparentFillColor;
            playerSkillCoolUI.icon.color = transparentIconColor;
            playerSkillCoolUI.textMeshPro.color = transparentTextColor;
            playerSkillCoolUI.key.SetActive(false);
            playerSkillCoolUI.SetUIDeactivate();
        }
    }
    
    public void SetVisible()
    {
        if(IsVisible == true) return;
        IsVisible = true;
        foreach (var playerSkillCoolUI in PlayerSkillCoolUIs)
        {
            Color vividFillColor = playerSkillCoolUI.fill.color;
            Color vividIconColor = playerSkillCoolUI.icon.color;
            Color vividTextColor = playerSkillCoolUI.textMeshPro.color;
            
            vividFillColor.a = OriginAlpha;
            vividIconColor.a = 1f;
            vividTextColor.a = 1f;
            
            playerSkillCoolUI.fill.color = vividFillColor;
            playerSkillCoolUI.icon.color = vividIconColor;
            playerSkillCoolUI.textMeshPro.color = vividTextColor;
            playerSkillCoolUI.key.SetActive(true);
            playerSkillCoolUI.SetUIActivate();
        }
    }
}
