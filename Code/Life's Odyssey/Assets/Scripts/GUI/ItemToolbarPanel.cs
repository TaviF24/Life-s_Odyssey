using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemToolbarPanel : ItemPanel
{
    [SerializeField] ToolbarController toolbarController;

    private void Start()
    {
        Init();
        toolbarController.onChange += Highlight;
    }

    public override void OnClick(int id)
    {
        // use item
        toolbarController.Set(id);
        Highlight(id);
    }

    int currentSelectedTool;

    public void Highlight(int id)
    {
        // show the selected item in toolbar
        buttons[currentSelectedTool].Highlight(false);
        currentSelectedTool = id;
        buttons[currentSelectedTool].Highlight(true);
    }

	public override void Show()
	{
		base.Show();
        toolbarController.UpdateHighlightIcon();
	}
}
