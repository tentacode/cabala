﻿using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour
{
    public float rotationSpeed;

    void Update ()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
	}
}
