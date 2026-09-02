using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ColouredButtonTrigger : MonoBehaviour
{
    IAttachedButtonTo attachTo;

    void Start()
    {
        attachTo = GetComponentInParent<IAttachedButtonTo>();

        if (attachTo == null)
        {
            Debug.LogError("ColouredButtonTrigger.cs does not have IAttachedButtonTo component as a parent.");
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        var newButton = collision.GetComponentInParent<ColouredButton>();

        if (newButton == null) return;

        newButton.RemoveButtonTrigger(); // Stop this from being called multiple times

        newButton.AddAttachedTo(attachTo);
    }
}
