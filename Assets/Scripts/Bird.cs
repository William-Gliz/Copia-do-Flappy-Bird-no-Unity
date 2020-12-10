using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bird : MonoBehaviour {

    private static Bird instance;
    public static Bird GetInstance()
    {
        return instance;
    }

    public float jump_force = 60f;

    Rigidbody2D rb;

    public event EventHandler OnDied;
    public event EventHandler OnStartedPlaying;


    private State state;
    private enum State
    {
        WaitingToStart,
        Playing,
        Dead,
    }

    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
        state = State.WaitingToStart;
        rb.bodyType = RigidbodyType2D.Static;
        Debug.Log("Bird.Start");
        Score.Start();  // Passo o evento Bird.OnDied para o script Score
        
    }


    private void Update()
    {

        switch (state)
        {
            default:
            case State.WaitingToStart:
                if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
                {
                    // Start playing
                    state = State.Playing;
                    rb.bodyType = RigidbodyType2D.Dynamic;
                    Jump();
                    if (OnStartedPlaying != null) OnStartedPlaying(this, EventArgs.Empty);
                    FindObjectOfType<AudioManager>().Play(AudioManager.Sounds.Tema);
                }
                break;
            case State.Playing:
                if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) {
                    Jump();
                }

                break;
            case State.Dead:
                break;
        }
    }

    
    private void Jump() {
        rb.velocity = Vector2.up * jump_force;
        FindObjectOfType<AudioManager>().Play(AudioManager.Sounds.BirdJump);
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        Debug.Log("Morri");
        Debug.Log(collider.name);
        FindObjectOfType<AudioManager>().Stop(AudioManager.Sounds.Tema);
        FindObjectOfType<AudioManager>().Play(AudioManager.Sounds.Lose);
        rb.bodyType = RigidbodyType2D.Static;
        if (OnDied != null) OnDied(this, EventArgs.Empty);
    }

}
