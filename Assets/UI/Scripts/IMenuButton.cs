﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IMenuButton : MonoBehaviour
{
    [SerializeField] protected GameObject firstPanel;
    [SerializeField] protected GameObject secondPanel;
    [SerializeField] protected int thisIndex;
    [SerializeField] protected MenuButtonController menuButtonController;
    protected Animator animator;
    protected AudioSource selectMenuSFX;
    protected AudioSource changeMenuSFX;
    protected float waitTime = 0.4f;
    protected bool isOver = false;

    protected void AnimatorControls()
    {
        if (menuButtonController.GetIndex() == thisIndex)
        {
            animator.SetBool("selected", true);
            if (Input.GetKeyDown(KeyCode.Return) && menuButtonController.IsAlive() || Input.GetKeyDown(KeyCode.Mouse0) && isOver)
            {
                animator.SetBool("pressed", true);
                OnPressedMenu();
            }
            else if (animator.GetBool("pressed"))
            {
                animator.SetBool("pressed", false);
            }
        }
        else
        {
            animator.SetBool("selected", false);
        }

        if (menuButtonController.GetMaxIndex() == thisIndex)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && menuButtonController.IsAlive())
            {
                animator.SetBool("pressed", true);
                OnPressedMenu();
            }
            else if (animator.GetBool("pressed"))
            {
                animator.SetBool("pressed", false);
            }
        }
    }

    protected void GetStartComponents()
    {
        SFX sfx = FindObjectOfType<SFX>();
        selectMenuSFX = sfx.GetSelectMenuSFX();
        changeMenuSFX = sfx.GetChangeMenuSFX();
        animator = GetComponent<Animator>();
    }

    virtual public void OnPressedMenu() { }
    protected void OnEnablePanel(GameObject objectHide, GameObject objectShow)
    {
        if (objectShow != null)
        {
            objectShow.GetComponent<MenuButtonController>().SetIsAlive(true);
            objectShow.SetActive(true);
        }
        objectHide.SetActive(false);
        objectHide.GetComponent<MenuButtonController>().SetIsAlive(false);
    }
    #region Coroutines
    protected IEnumerator WaitAndQuitGame()
    {
        yield return new WaitForSeconds(waitTime);
        Debug.Log("Quit");
        Application.Quit();
    }
    protected virtual IEnumerator WaitAndEnable()
    {
        yield return new WaitForSeconds(waitTime);
        OnEnablePanel(firstPanel, secondPanel);
    }
    #endregion
}
