using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DWGames;
using DWGames.com.darkwing_games.core.Runtime.Util;
using UnityEngine;
using UnityEngine.UIElements;

public class FPSUIBehaviour : MonoBehaviour
{
    private Label label1, label2;


    [SerializeField] private bool isVisible = true;
    private bool lastVisibleSetting = true;
    private UIDocument uiDocument;


    [SerializeField] private ScreenLocation screenLocation = ScreenLocation.BOTTOM_LEFT;
    private ScreenLocation lastScreenLocation = ScreenLocation.BOTTOM_LEFT;
    private VisualElement rootPanel;

    void Start()
    {
        uiDocument = gameObject.GetComponent<UIDocument>();
        rootPanel = uiDocument.rootVisualElement.Query<VisualElement>("rootPanel");
        label2 = uiDocument.rootVisualElement.Q<Label>("label2");
        DWFPSCounter.FPSUpdateEvent += (fps) => label2.text = fps.ToString();
    }

    private void Update()
    {
        HandleVisibleFlag();
        HandleLocationSetting();
    }

    private void HandleLocationSetting()
    {
        if (lastScreenLocation != screenLocation)
        {
            lastScreenLocation = screenLocation;
            switch (screenLocation)
            {
                case ScreenLocation.BOTTOM_LEFT:
                    setDistances(-1, -1, 0, 0);
                    break;
                case ScreenLocation.BOTTOM_RIGHT:
                    setDistances(-1, 0, -0, -1);
                    break;
                // case ScreenLocation.MIDDLE_LEFT:
                //     setDistances(Screen.height / 2, -1, Screen.height / 2, 0);
                //
                //     break;
                // case ScreenLocation.MIDDLE_RIGHT:
                //     setDistances(Screen.height / 2, 0, Screen.height / 2, -1);

                    break;
                case ScreenLocation.TOP_LEFT:
                    setDistances(0, -1, -1, 0);
                    break;
                case ScreenLocation.TOP_RIGHT:
                    setDistances(0, 0, -1, -1);
                    break;
            }
        }
    }

    private void setDistances(int top, int right, int bottom, int left)
    {
        if (top == -1)
        {
            rootPanel.style.top = new StyleLength(StyleKeyword.Auto);
        }
        else
        {
            rootPanel.style.top = new Length(top, LengthUnit.Pixel);
        }

        if (right == -1)
        {
            rootPanel.style.right = new StyleLength(StyleKeyword.Auto);
        }
        else
        {
            rootPanel.style.right = new Length(top, LengthUnit.Pixel);
        }

        if (bottom == -1)
        {
            rootPanel.style.bottom = new StyleLength(StyleKeyword.Auto);
        }
        else
        {
            rootPanel.style.bottom = new Length(top, LengthUnit.Pixel);
        }

        if (left == -1)
        {
            rootPanel.style.left = new StyleLength(StyleKeyword.Auto);
        }
        else
        {
            rootPanel.style.left = new Length(top, LengthUnit.Pixel);
        }
    }


    private void HandleVisibleFlag()
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