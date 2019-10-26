using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {
    // variable for sound files
    public AudioClip OkSound, BoomSound;

    public Text scoreText;
    public int ballValue;
    public UnityEngine.GameObject explosion;

    private int score;

    public static Score instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Use this for initialization
    void Start () {
        score = 0;
        UpdateScore();
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Bomb")
        {
            AudioSource.PlayClipAtPoint(BoomSound, other.transform.position);
            Instantiate(explosion, transform.position, transform.rotation);
            //Destroy(explosion);
            return;
        }
        score += ballValue;
        UpdateScore();
        // play audio file at position from catched gameobject
        AudioSource.PlayClipAtPoint(OkSound, other.transform.position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Bomb")
        {
            AudioSource.PlayClipAtPoint(BoomSound, collision.gameObject.transform.position);
            Instantiate(explosion, transform.position, transform.rotation);
            //score -= ballValue * 1;
            //score = score;
            //UpdateScore();
        }
    }
    void UpdateScore ()    
    {
        scoreText.text = "Puntaje:\n" + score;
    }

    public int GetScoreHat()
    {
        return score;
    }
}
