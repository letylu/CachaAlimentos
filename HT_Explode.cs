﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HT_Explode : MonoBehaviour {

    public UnityEngine.GameObject explosion;
    //public Transform explosion;
    //public ParticleSystem[] effects;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Hat")
        {
            Instantiate(explosion, transform.position, transform.rotation);
           // foreach (var effect in effects)
           // {
                //effect.transform.parent = null;
                //effect.Stop();
                //Destroy(effect.gameObject , 1.0f);
            //}
            Destroy(gameObject);
        }
    }
}
