using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Player : MonoBehaviour
{
    public int HP = 100;
    public GameObject bloodyScreen;

    public TextMeshProUGUI playerHealthUI;
    public GameObject gameOverUI;

    public bool isDead;

    private void Start()
    {
        playerHealthUI.text = $"Health:  {HP}";
    }

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;

        if (HP <= 0)
        {
            print("Player Dead");

            PlayerDead();
            isDead = true;

            //Game over
            //Respawn

        }
        else
        {
            print("Player Hit");
            StartCoroutine(BloodyScreenEffect());
            playerHealthUI.text = $"Health:  {HP}";
            SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerHurt);
        }
    }

    private void PlayerDead()
    {
        SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerDeath);
        DisablePlayerControls();  // Disable controls when player dies
        GetComponent<MouseMovement>().enabled = false;
        GetComponent<PlayerMovement>().enabled = false;
         
        //Dying Animation
        GetComponentInChildren<Animator>().enabled = true;
        playerHealthUI.gameObject.SetActive(false);

        GetComponent<ScreenBlackOut>().StartFade();

        StartCoroutine(ShowGameOverUI());
    }
     private void DisablePlayerControls()
    {
        // Disable all player movement and input components
        GetComponent<MouseMovement>().enabled = false;
        GetComponent<PlayerMovement>().enabled = false;

        // If you have other scripts that manage input, disable them here as well
        // For example, if you have a script for shooting or interacting, disable those components too
    }

    private IEnumerator ShowGameOverUI()
    {
        yield return new WaitForSeconds(1f);
        gameOverUI.gameObject.SetActive(true);
    }

    private IEnumerator BloodyScreenEffect()
    {
        if (bloodyScreen.activeInHierarchy == false)
        {
            bloodyScreen.SetActive(true);

        }

        var image = bloodyScreen.GetComponentInChildren<Image>();

        // Set the initial alpha value to 1 (fully visible).
        Color startColor = image.color;
        startColor.a = 1f;
        image.color = startColor;

        float duration = 3f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Calculate the new alpha value using Lerp.
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);

            // Update the color with the new alpha value.
            Color newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;

            // Increment the elapsed time.
            elapsedTime += Time.deltaTime;

            yield return null; ; // Wait for the next frame.
        }

        if (bloodyScreen.activeInHierarchy)
        {
            bloodyScreen.SetActive(false);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ZombieHand"))
        {
            if(isDead== false)
            {
                TakeDamage(other.gameObject.GetComponent<ZombieHand>().damage);


            }


        }
    }
}
