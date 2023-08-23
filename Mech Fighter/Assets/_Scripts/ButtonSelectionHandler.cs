using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelectionHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] private float _horizontalMoveAmount = 20;
    [SerializeField] private float _moveTime = 0.1f;
    [SerializeField] private float _scaleAmount = 1.1f;

    private Vector3 _startPos;
    private Vector3 _startScale;

    [SerializeField] ButtonSelectionManager buttonSelectionManager;

    public void OnDeselect(BaseEventData eventData)
    {
        StartCoroutine(MoveButton(false));
    }

    public void OnDeselect()
    {
        StartCoroutine(MoveButton(false));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        eventData.selectedObject = gameObject;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        eventData.selectedObject = null;
    }

    public void OnSelect(BaseEventData eventData)
    {
        StartCoroutine(MoveButton(true));
        buttonSelectionManager.LastSelected = gameObject;
        SoundManager.Instance.PlaySound("menuSelection");
        //find the index
        for (int i = 0; i < buttonSelectionManager.Buttons.Length; i++)
        {
            if(buttonSelectionManager.Buttons[i] == gameObject)
            {
                buttonSelectionManager.LastSelectIndex = i;
                return;
            }
        }
    }

    private void Start()
    {
        _startPos = transform.position;
        _startScale = transform.localScale;
    }

    private IEnumerator MoveButton(bool startingAnimation)
    {
        Vector3 endPosition;
        Vector3 endScale;

        float elapsedTime = 0f;

        while(elapsedTime < _moveTime)
        {
            elapsedTime += Time.deltaTime;

            if (startingAnimation)
            {
                endPosition = _startPos + new Vector3(_horizontalMoveAmount, 0f , 0f);
                endScale = _startScale * _scaleAmount;
            }
            else
            {
                endPosition = _startPos;
                endScale = _startScale;
            }

            Vector3 lerpedPos = Vector3.Lerp(transform.position, endPosition, (elapsedTime / _moveTime));
            Vector3 lerpedScale = Vector3.Lerp(transform.localScale, endScale, (elapsedTime / _moveTime)); ;

            transform.position = lerpedPos;
            transform.localScale = lerpedScale;

            yield return null;
        }
    }
}
