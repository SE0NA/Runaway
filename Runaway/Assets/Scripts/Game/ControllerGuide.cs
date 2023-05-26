using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGuide : MonoBehaviour
{
    Animator anim;
    Player player;

    private void Start()
    {
        anim = GetComponent<Animator>();
        player = FindObjectOfType<Player>();

        anim.Play("Blink");
    }

    private void Update()
    {
        if (player.isMoved)
            Destroy(gameObject);
    }
}
