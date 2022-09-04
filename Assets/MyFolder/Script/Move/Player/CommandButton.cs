using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandButton : MonoBehaviour
{
    private Button commandButton;

    // Start is called before the first frame update
    void Start()
    {
        commandButton = GetComponent<Button>();
        commandButton.onClick.AddListener(clickCommandButton);
    }

    private void clickCommandButton()
    {
        GManager.instance.isCloseCommand = !GManager.instance.isCloseCommand;
    }
}
