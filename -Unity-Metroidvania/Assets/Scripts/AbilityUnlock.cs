using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AbilityUnlock : MonoBehaviour
{
    public bool unlockDoubleJump, unlockDash, unlockBecomeBall, unlockDropBomb;
    public GameObject pickupEffect;
    public string unlockMessage;
    public TMP_Text unlockText;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerAbilityTracker player = other.GetComponentInParent<PlayerAbilityTracker>();
            if (unlockDoubleJump)
            {
                player.canDoubleJump = true;
            }
            if (unlockDash)
            {
                player.canDash = true;
            }
            if (unlockBecomeBall)
            {
                player.canBecomeBall = true;
            }
            if (unlockDropBomb)
            {
                player.canDropBomp = true;
            }

            Instantiate(pickupEffect, transform.position, transform.rotation);
            unlockText.transform.parent.SetParent(null);
            unlockText.transform.parent.position = transform.position;
            unlockText.text = unlockMessage;
            unlockText.gameObject.SetActive(true);
            Destroy(unlockText.transform.parent.gameObject, 3f);
            Destroy(gameObject);
        }
    }
}
