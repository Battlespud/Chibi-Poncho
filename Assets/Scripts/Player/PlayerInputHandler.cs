using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour {

    private PlayerController playerController;
    private Vector2 move;
    private bool jump, pause;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        pause = Input.GetButtonDown("Submit");
        jump = Input.GetButtonDown("Jump");

        if (pause)
            GameManager.TogglePauseGame();
        if (jump)
            playerController.Jump();

        resetAllInputFlags();
    }

    void FixedUpdate()
    {
        if (!GameManager.IsGamePaused())
        {
            Vector2 move = new Vector2(Input.GetAxisRaw("Horizontal"), 0.0f);

            if(move != Vector2.zero)
                playerController.Move(move);
        }
    }

    private void resetAllInputFlags()
    {
        jump = false;
    }
}
