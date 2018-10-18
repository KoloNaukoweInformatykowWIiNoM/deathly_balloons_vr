﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRaycast : MonoBehaviour {

    public  float       raycast_delay = 5f;
    public  LayerMask[] ignore_layer;
    private GameObject  last_object;
    private float       raycast_timer;

	/// <summary> Funkcja Inicjalizacji </summary>
	void Start () {
		
	}
	
	/// <summary> Funkcja Update </summary>
	void Update () {
		RaycastHit();
	}

    /// <summary> Wybór obiektu przez spoglądanie na niego przez określoną ilość czasu </summary>
    private void RaycastHit() {
        Ray ray = new Ray( transform.position, transform.forward );
        RaycastHit hit;

        if ( Physics.Raycast( ray, out hit, 5.0f ) ) {
            GameObject hitObject = hit.collider.gameObject;
            if ( hitObject != last_object ) {
                raycast_timer = 0f;
                last_object = hitObject;
            }
            raycast_timer += Time.deltaTime;
            if ( raycast_timer > raycast_delay ) { raycast_timer = raycast_delay; }
        } else {
            raycast_timer = 0f;
            last_object = null;
        }
    }

    public GameObject GetSelectedObject() { return last_object; }
    public bool GetSelectedActive() { return raycast_timer >= raycast_delay; }
}
