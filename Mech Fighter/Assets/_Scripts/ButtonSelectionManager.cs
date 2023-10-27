using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelectionManager : MonoBehaviour
{
    public GameObject[] Buttons;

    public GameObject LastSelected { get; set; }
    public int LastSelectIndex { get; set; }

    private void OnEnable()
    {
        StartCoroutine(SetSelectedAfterOneFrame());
    }

/*    private void OnDisable()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }*/

    private IEnumerator SetSelectedAfterOneFrame()
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(Buttons[0]);
    }

    private void Update()
    {
        //Move up
        if (MenuInputManager.instance.NavigationInput.y > 0)
        {
            HandleNextButtonSelection(-1);
        }        
        //Move down
        if (MenuInputManager.instance.NavigationInput.y < 0)
        {
            HandleNextButtonSelection(1);
        }

        //Move left
        if (MenuInputManager.instance.NavigationInput.x > 0)
        {
            HandleNextButtonSelection(1);
        }
        //Move right
        if (MenuInputManager.instance.NavigationInput.x < 0)
        {
            HandleNextButtonSelection(-1);
        }
/*        //Enter
        if (MenuInputManager.instance.SubmitInput)
        {
            
        }*/
    }

    private void HandleNextButtonSelection(int addition)
    {
        if(EventSystem.current.currentSelectedGameObject == null && LastSelected != null)
        {
            int newIndex = LastSelectIndex + addition;
            newIndex = Mathf.Clamp(newIndex, 0, Buttons.Length - 1);
            EventSystem.current.SetSelectedGameObject(Buttons[newIndex]);
        }
    }
}
