using System.Collections;
using System.Collections.Generic;
using DWGames;
using UnityEngine;
using UnityEngine.UIElements;

public class FPSUIBehaviour : MonoBehaviour
{
    private DWFPSCounter dwfpsCounter;

    private Label label1, label2;
    // Start is called before the first frame update
    void Start()
    {
        var uiDocument = gameObject.GetComponent<UIDocument>();

        dwfpsCounter = gameObject.GetComponent<DWFPSCounter>();
        label1 = uiDocument.rootVisualElement.Q<Label>("label1");
        label2 = uiDocument.rootVisualElement.Q<Label>("label2");

        label1.text = "FPS:";
    }

    // Update is called once per frame
    void Update()
    {
        label2.text = dwfpsCounter.currentFPS.ToString();
    }
}
