﻿using UnityEngine;
using System.Collections;

public class Explosion : Bullet {

    void Start()
    {
        SelfDestroy();
    }
	
}
