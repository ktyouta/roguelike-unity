using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandButton : MonoBehaviour
{
    private Button commandButton;
    private PanelManager pManager;

    // Start is called before the first frame update
    void Start()
    {
        commandButton = GetComponent<Button>();
        commandButton.onClick.AddListener(clickCommandButton);
        pManager = GameObject.Find("GameController").GetComponent<PanelManager>();
    }

    private void clickCommandButton()
    {
        if (GManager.instance.doingSetup)
        {
            return;
        }
        pManager.isCloseCommand = !pManager.isCloseCommand;
    }
}
