﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundNew : MonoBehaviour {

	void Update () {
		transform.RotateAround(transform.position, Vector3.up, 40 * Time.deltaTime);
	}
}
