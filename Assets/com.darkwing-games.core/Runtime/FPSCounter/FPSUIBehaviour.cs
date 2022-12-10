using System;
using System.Collections;
using System.Collections.Generic;
using DWGames;
using UnityEngine;
using UnityEngine.UIElements;

public class FPSUIBehaviour : MonoBehaviour
{
    private Label label1, label2;


    [SerializeField] private bool isVisible = true;
    private bool lastVisibleSetting = true;
    private UIDocument uiDocument;

    void Start()
    {
        this.uiDocument = gameObject.GetComponent<UIDocument>();
        label2 = uiDocument.rootVisualElement.Q<Label>("label2");
        DWFPSCounter.FPSUpdateEvent += (fps) => label2.text = fps.ToString();
    }

    private void Update()
    {
        if (isVisible != lastVisibleSetting)
        {
            lastVisibleSetting = isVisible;
            if (!isVisible)
            {
                uiDocument.rootVisualElement.style.display = DisplayStyle.None;
            }
            else
            {
                uiDocument.rootVisualElement.style.display = DisplayStyle.Flex;
            }
        }
    }
}