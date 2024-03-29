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

namespace Moondown.Graphics {
    
    using Moondown.Player;
    
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        [SerializeField] private Vector3 pos;
        [SerializeField] private float speed;

        private void Update()
        {
            cam = Camera.main;
            pos = new Vector3(Player.Instance.gameObject.transform.position.x, Player.Instance.gameObject.transform.position.y, -10);
        }

        private void FixedUpdate()
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, pos, speed * Time.fixedDeltaTime);
        }
    } 
}