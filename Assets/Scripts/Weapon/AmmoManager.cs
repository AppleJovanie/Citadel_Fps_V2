using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoManager : MonoBehaviour
{
    public static AmmoManager Instance { get; private set; }

    // UI
    public TextMeshProUGUI ammoDisplay;

    // Variables to track ammo
    private int currentAmmo; // Current ammo count
    private int magazineSize; // Maximum ammo count in the magazine

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Method to initialize ammo counts
    public void InitializeAmmo(int startingAmmo, int magazineSize)
    {
        this.currentAmmo = startingAmmo;
        this.magazineSize = magazineSize;
        UpdateAmmoDisplay();
    }

    // Method to deduct ammo based on weapon type
    public void UseAmmo(int amount)
    {
        currentAmmo -= amount;
        currentAmmo = Mathf.Max(currentAmmo, 0); // Ensure ammo doesn't go negative
        UpdateAmmoDisplay();
    }

    // Method to reload ammo
    public void Reload(int ammoToAdd)
    {
        currentAmmo += ammoToAdd;
        currentAmmo = Mathf.Min(currentAmmo, magazineSize); // Ensure ammo doesn't exceed magazine size
        UpdateAmmoDisplay();
    }

    // Update the UI display
    private void UpdateAmmoDisplay()
    {
        if (ammoDisplay != null)
        {
            ammoDisplay.text = $"{currentAmmo} / {magazineSize}";
        }
    }

    // Get current ammo for external access
    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }
}
