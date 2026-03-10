using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog;

    public void Interact()
    {
        StartCoroutine(RunInteractionSequence());
    }

    private IEnumerator RunInteractionSequence()
    {
        // 1. Warte, bis der DialogManager fertig ist
        yield return StartCoroutine(DialogManager.Instance.ShowDialog(dialog));

    }
}