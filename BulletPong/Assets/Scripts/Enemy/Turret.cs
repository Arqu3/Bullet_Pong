using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Enemy
{

	protected override void Start()
    {
        base.Start();
        BulletPosition = transform.FindChild("Cylinder").FindChild("BulletSpawn");
	}
	
	protected override void Update()
    {
        base.Update();
	}

    protected override void RotationUpdate()
    {
        base.RotationUpdate();
    }
}
