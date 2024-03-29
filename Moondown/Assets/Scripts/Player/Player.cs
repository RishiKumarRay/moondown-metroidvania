﻿/*
    A script to manage the player's progress
    Copyright (C) 2021 Moondown Project

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moondown.WeaponSystem;
using Moondown.Inventory;
using Moondown.Player.Modules;
using Moondown.Environment;
using Moondown.UI.Localization;
using Moondown.UI;
using System;
using System.Threading.Tasks;
using Moondown.Player.Movement;

namespace Moondown.Player
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Player : MonoBehaviour
    {
        // singelton
        public static Player Instance { get; private set; }

        public delegate void BlankDelegate();

        public event Action<int> OnDamageTaken;
        public event Action<int> OnHeal;

        public event Action<int> OnCharge;

        public event BlankDelegate OnDeath;
        public event BlankDelegate OnRespawn;
        public event Action<GameObject> OnHazardRespawn;

        public event BlankDelegate OnApplyLowHealth;
        public event BlankDelegate OnClearVignette;

        public RespawnLocation LocalRespawn { get; set; }
        public RespawnLocation DeathRespawn { get; set; }

        public int health;
        public int MaxHealth { get; set; } = 5;

        public int charge;
        public int MaxCharge { get; set; } = 3;

        public List<AbstractModule> modules = new List<AbstractModule> { };

        [SerializeField] private Sprite baseSprite;

        [SerializeField] private GameObject LowHealthPP;

        [SerializeField] private Material shaderMaterial;

        public GameObject LowHealthPostProcessing => LowHealthPP;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        private void Start()
        {
            DisplayHUD.Init(GameObject.FindGameObjectWithTag("hp bar"), GameObject.FindGameObjectWithTag("charge bar"));

            modules.Add(new BasePlayerModule());

            #region weapon
            Weapon weapon = new Weapon(
                LocalizationManager.Get("BASIC_SWORD_NAME"),
                LocalizationManager.Get("BASIC_SWORD_DESC"),
                "UI/Inventory/Placeholder",
                1,
                0.3f,
                AttackMode.NORMAL,
                ItemType.MEELE_WEAPON,
                baseSprite,
                EquipmentManager.Instance.NextFreeSlot
            );

            EquipmentManager.Instance.Inventory.Add(weapon);
            #endregion
            #region item
            Item item = new Item(
                LocalizationManager.Get("MISC_NAME"),
                LocalizationManager.Get("MISC_DESC"),
                "UI/Inventory/PlaceholderItem",
                ItemType.ITEM,
                baseSprite,
                EquipmentManager.Instance.NextFreeSlot
            );

            EquipmentManager.Instance.Inventory.Add(item);
            #endregion
            #region otherWeapon
            Weapon otherWeapon = new Weapon(
                LocalizationManager.Get("OTHER_SWORD_NAME"),
                LocalizationManager.Get("OTHER_SWORD_DESC"),
                "UI/Inventory/other placeholder",
                2,
                0.2f,
                AttackMode.DASH,
                ItemType.MEELE_WEAPON,
                baseSprite,
                EquipmentManager.Instance.NextFreeSlot
            );

            EquipmentManager.Instance.Inventory.Add(otherWeapon);

            #endregion

            EquipmentManager.Instance.FirstLoading = false;
            OnRespawn();
        }

        private async void Update()
        {
            EnvironmentInteraction.Modifiers modifiers = EnvironmentInteraction.Instance.CheckCollisions();

            if (modifiers.charge > 0)
                OnCharge(modifiers.charge);

            if (modifiers.health != 0)
            {

                if (modifiers.health > 0)
                    OnHeal(modifiers.health);
                else
                {
                    OnDamageTaken(modifiers.health);

                    if (health == 1)
                        OnApplyLowHealth();
                    else
                        OnClearVignette();
                }
            }

            if (modifiers.hasBeenHit)
            {
                OnHazardRespawn(gameObject);
            }

            if (health <= 0)
                Die();

            DisplayHUD.UpdateCharge(charge);
            DisplayHUD.UpdateHealth(health);

        }

        private void Die()
        {
            OnDeath();
            OnRespawn();
        }

        public async Task AnimateHazardFade()
        {
            PlayerMovement.Instance.controls.Disable();
            await Animate();
            
            Task delay = Task.Delay(2000);

            await delay;
            PlayerMovement.Instance.controls.Enable();

        }

        public async Task AnimateReverse()
        {
            float i = 1;
            while (i > 0)
            {
                shaderMaterial.SetFloat("_FadeValue", i);
                await Task.Delay(10);
                i -= 0.01f;
            }
        }

        private async Task Animate()
        {
            float i = 0;
            while (i < 1)
            {
                shaderMaterial.SetFloat("_FadeValue", i);
                await Task.Delay(10);
                i += 0.01f;
            }
        }
    }
}