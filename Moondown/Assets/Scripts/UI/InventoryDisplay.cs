﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryDisplay
{
    enum LastViewed
    {
        MEELE_WEAPON,
        RANGED_WEAPON,
        ARMOUR,
        GENERAL
    }

    public static InventoryDisplay Instance { get; private set; } = new InventoryDisplay();

    private LastViewed lastViewed;

    public void Load(GameObject[] slots, GameObject[] quickBarSlots, Sprite baseSprite, GameObject UI)
    {
        if (lastViewed == LastViewed.GENERAL)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                Sprite itemSprite = (
                    from IInventoryItem item in EquipmentManager.Instance.Inventory
                    where item.SlotNumber == i
                    select item.Image
                ).First();

                Sprite sprite = MergeTextures(
                    new Sprite[]
                    {
                        baseSprite,
                        itemSprite
                    }
                );
            }          
        }

        UI.SetActive(true);
    }

    private Sprite MergeTextures(Sprite[] sprites)
    {
        Texture2D newTexture = new Texture2D(100, 100);

        for (int y = 0; y < newTexture.height; y++)
        {
            for (int x = 0; x < newTexture.width; x++)
            {
                newTexture.SetPixel(x, y, new Color(1, 1, 1, 0));
            }
        }

        for (int i = 0; i < sprites.Length; i++)
        {
            for (int y = 0; y < newTexture.height; y++)
            {
                for (int x = 0; x < newTexture.width; x++)
                {
                    Color color = sprites[i].texture.GetPixel(x, y).a == 0 ?
                        newTexture.GetPixel(x, y) :
                        sprites[i].texture.GetPixel(x, y);

                    newTexture.SetPixel(x, y, color);
                }
            }
        }

        newTexture.Apply();
        Sprite finalSprite = Sprite.Create(newTexture, new Rect(0, 0, newTexture.width, newTexture.height), new Vector2(0.5f, 0.5f));
        finalSprite.name = "Inevntory slot";
        return finalSprite;
    }



}