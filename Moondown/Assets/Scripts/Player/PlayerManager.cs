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

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerManager : MonoBehaviour
{
    // singelton
    public static PlayerManager Instance { get; private set; }

    public delegate void ActionDelegate(int amount);

    public event ActionDelegate OnDamageTaken;
    public event ActionDelegate OnHeal;
    public event ActionDelegate OnCharge;

    public int Health { get; set; } = 5;
    public int MaxHealth { get; set; } = 10;

    public int Charge { get; set; } = 0;
    public int MaxCharge { get; set; } = 3;

    public List<AbstractModule> modules = new List<AbstractModule> { };

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        modules.Add(new BasePlayerModule());
    }

    private void Update()
    {
        EnvironmentInteraction.Modifiers modifiers = EnvironmentInteraction.Instance.CheckCollisions();

        if (modifiers.charge > 0)
            OnCharge(modifiers.charge);

        if (modifiers.health == 0)
            return;

        if (modifiers.health > 0)
            OnHeal(modifiers.health);
        else
            OnDamageTaken(modifiers.health);
    }


}