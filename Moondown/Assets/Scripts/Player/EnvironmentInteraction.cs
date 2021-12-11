﻿/*
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

public class EnvironmentInteraction : MonoBehaviour
{
    public struct Modifiers
    {
        public int health;
        public int charge;
    }

    public static EnvironmentInteraction Instance { get; private set;  }

    private List<GameObject> gameObjects = new List<GameObject> { };

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public Modifiers CheckCollisions()
    {
        Modifiers m = new Modifiers();
        m.charge = 0;
        m.health = 0;

        foreach (GameObject collision in gameObjects)
        {
            EnvironmentBehaviour behaviour = collision.GetComponent<EnvironmentBehaviour>();

            m.charge += behaviour.chargeModifier;
            m.health += behaviour.healthModifier;

            if (behaviour.singleUse)
                Destroy(behaviour.gameObject);

            behaviour.isUsable = false;
        }

        gameObjects.Clear();
        return m;

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<EnvironmentBehaviour>() != null)
            gameObjects.Add(collision.gameObject);
    }
}