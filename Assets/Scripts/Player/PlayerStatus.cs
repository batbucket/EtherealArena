﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour {

    private int maxHealth = 3;
    private int currentHealth;
    private float invulnerabilityTimer;
    private const float baseInvulnerabilityTime = 4f;
    // Whether or not player sprite should flicker for invulnerability, negative for invisible frames, positive for visibile
    private int flickerFrames;
    private const int baseFlickerRate = 3;
    private SpriteRenderer playerSpriteRenderer;
    private SpriteRenderer playerArmSpriteRenderer;
    // TODO: replace with event system
    [SerializeField] SceneManagement sceneManagement;

    void Start() {
        currentHealth = maxHealth;
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        playerArmSpriteRenderer = transform.Find("Arm").GetComponent<SpriteRenderer>();
    }

    void Update() {
        if (invulnerabilityTimer > 0) {
            invulnerabilityTimer -= Time.deltaTime;
            if (invulnerabilityTimer > 0) {
                if (flickerFrames < 0) {
                    flickerFrames++;
                    if (flickerFrames == 0) {
                        playerSpriteRenderer.enabled = true;
                        playerArmSpriteRenderer.enabled = true;
                        flickerFrames = baseFlickerRate;
                    }
                } else if (flickerFrames > 0) {
                    flickerFrames--;
                    if (flickerFrames == 0) {
                        playerSpriteRenderer.enabled = false;
                        playerArmSpriteRenderer.enabled = false;
                        flickerFrames = -baseFlickerRate;
                    }
                }
            } else {
                playerSpriteRenderer.enabled = true;
                playerArmSpriteRenderer.enabled = true;
                gameObject.layer = 0;
            }
        }
    }

    public void TakeHit() {
        if (invulnerabilityTimer <= 0) {
            currentHealth--;
            if (currentHealth <= 0) {
                KO();
            }
            invulnerabilityTimer = baseInvulnerabilityTime;
            flickerFrames = - baseFlickerRate;
            gameObject.layer = 8;
        }
    }

    void RestoreHealth() {
        currentHealth = maxHealth;
    }

    void KO() {
        playerSpriteRenderer.enabled = false;
        playerArmSpriteRenderer.enabled = false;
        sceneManagement.OpponentVictory();
    }

    public int GetHealth() {
        return currentHealth;
    }

}
