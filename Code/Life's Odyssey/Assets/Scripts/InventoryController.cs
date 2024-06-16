using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject statusPanel;
    [SerializeField] GameObject toolbarPanel;
    [SerializeField] GameObject additionalPanel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            // open inventory on key press I
            // show inventory panel, hide toolbar panel
            if( panel.activeInHierarchy == false)
            {
                Open();
            }
            else
            {
                Close();
            }
        }
    }

    public void Open()
    {
		panel.SetActive(true);
		statusPanel.SetActive(true);
		toolbarPanel.SetActive(false);
	}

    public void Close()
    {
		panel.SetActive(false);
		statusPanel.SetActive(false);
		toolbarPanel.SetActive(true);
        additionalPanel.SetActive(false);
	}
}
