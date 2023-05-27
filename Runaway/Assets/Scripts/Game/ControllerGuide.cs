using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGuide : MonoBehaviour
{
    Animator anim;
    Player player;
    UIManager uimanager;

    private void Start()
    {
        anim = GetComponent<Animator>();
        player = FindObjectOfType<Player>();
        uimanager = FindObjectOfType<UIManager>();

        anim.Play("Blink");
    }

    private void Update()
    {
        if (player.isMoved)
            Destroy(gameObject);
        if (uimanager.activeMenu)
            Destroy(gameObject);
    }
}
