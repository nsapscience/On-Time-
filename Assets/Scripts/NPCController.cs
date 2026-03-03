using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour, Interactable
{
    
    [SerializeField] Dialog dialog;

    public void Interact()
    {
    
        StartCoroutine(DialogManager.Instance.ShowDialog(dialog));

    }

}
