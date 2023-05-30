using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToastAd : MonoBehaviour
{
    public TextMeshProUGUI txt_toast;

    Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void ToastUp()
    {
        anim.Play("toast");
    }

    public void EndToast()
    {
        gameObject.SetActive(false);
    }
}
