using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TouchController : MonoBehaviour
{
    private TouchControls touchControls;
    [SerializeField] GameObject[] FigureArray;
    [SerializeField] GameObject CurrentFigure;
    List<GameObject> FigureList;



    bool doubletapdetected = false;

    Vector2 currentScreenPos;
    bool confirmedswipe = true;

    Vector2 firsttouch;
    Vector2 endtouch;
    [SerializeField] float swipeThreshold = 300f;
    [SerializeField] GameObject trailRenderer;
    bool movementTrail = false; 



    public GameObject currentObject;
    bool isDragging;

    private void Awake()
    {
        touchControls = new TouchControls();

    }
    private void OnEnable()
    {
        touchControls.Enable();
    }
    private void OnDisable()
    {
        touchControls.Disable();
    }
    private void Start()
    {
        FigureList = new List<GameObject>();
        trailRenderer.GetComponent<TrailRenderer>().startColor = CurrentFigure.GetComponent<SpriteRenderer>().color;
        trailRenderer.GetComponent<TrailRenderer>().endColor = CurrentFigure.GetComponent<SpriteRenderer>().color;


        touchControls.Game.TouchTap.started += ctx => StartTouch(ctx);
     


        touchControls.Game.DoubleTap.performed += ctx => DoubleTap(ctx);
        touchControls.Game.DoubleTap.canceled += ctx => { doubletapdetected = false; };


        touchControls.Game.TouchPosition.performed += ctx => { currentScreenPos = ctx.ReadValue<Vector2>(); };
        touchControls.Game.PressAndHold.performed += _ => PressAndDragCheck(_);
        touchControls.Game.PressAndHold.canceled += _ => {isDragging = false; currentObject = null; trailRenderer.SetActive(false); };


        touchControls.Game.Swipe.started += ctx => { firsttouch = Camera.main.ScreenToWorldPoint(touchControls.Game.TouchPosition.ReadValue<Vector2>()); };
        touchControls.Game.Swipe.performed += _ => CheckMovement(_);
        touchControls.Game.Swipe.canceled += ctx => DetectedSwipe(ctx);


    }
    private void StartTouch(InputAction.CallbackContext context)
    {
        Vector2 touchposition = Camera.main.ScreenToWorldPoint(touchControls.Game.TouchPosition.ReadValue<Vector2>());
        StartCoroutine(SingleTap(touchposition));

    }
    private void DoubleTap(InputAction.CallbackContext context)
    {
        doubletapdetected = true;

        Vector2 secondTapPosition = Camera.main.ScreenToWorldPoint(touchControls.Game.TouchPosition.ReadValue<Vector2>());
        RaycastHit2D hitdoubletap = Physics2D.Raycast(secondTapPosition, Vector2.zero);
        if (hitdoubletap.collider != null)
        {
            GameObject figuraHit = hitdoubletap.collider.gameObject;
            FigureList.Remove(figuraHit);
            Destroy(figuraHit);

        }
    }
 
   
    private bool isClickedOn
    {
        get
        {
            RaycastHit2D hitPress = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(currentScreenPos), Vector2.zero);
            if(hitPress.collider != null)
            {
                currentObject = hitPress.collider.gameObject;
                return true;
            }
            return false;
        }
    }
    private void PressAndDragCheck(InputAction.CallbackContext ctx)
    {
        trailRenderer.SetActive(true);
        if (isClickedOn) StartCoroutine(PressAndDrag());
        else confirmedswipe = true;
    }
   private IEnumerator PressAndDrag()
    {
        isDragging = true;
        confirmedswipe = false;
        while (isDragging)
        {
            Vector2 pos = (Camera.main.ScreenToWorldPoint(currentScreenPos));
            currentObject.transform.position = pos;
            trailRenderer.transform.position = pos;

            yield return null;
        }
   }

    private void DetectedSwipe(InputAction.CallbackContext context)
    {
        if(currentObject == null && confirmedswipe == true)
        {
            endtouch = Camera.main.ScreenToWorldPoint(touchControls.Game.TouchPosition.ReadValue<Vector2>());

            Vector2 DiferencePosition = endtouch - firsttouch;
            if (DiferencePosition.magnitude > swipeThreshold)
            {
                for (int i = 0; i < FigureList.Count; i++)
                {
                    Destroy(FigureList[i]);
                }

                FigureList.Clear();

            }
           

        }
        

    }


    private void CheckMovement(InputAction.CallbackContext context)
    {
        StartCoroutine(UpdatePositionTrail());
    }

    private IEnumerator UpdatePositionTrail()
    {
        movementTrail = true;
        while (movementTrail)
        {
            Vector2 pos = (Camera.main.ScreenToWorldPoint(currentScreenPos));
            trailRenderer.transform.position = pos;

            yield return null;
        }
    }


    IEnumerator SingleTap(Vector2 touchposition)
    {
        yield return new WaitForSeconds(0.4f);
        if (!doubletapdetected)
        {
            if (touchposition.y < 3.0f)
            { 
                GameObject newFigure = Instantiate(CurrentFigure, touchposition, Quaternion.identity);
                FigureList.Add(newFigure);
            }

        }
    }



    public void ChangeSquare()
    {
        CurrentFigure = FigureArray[1];
        trailRenderer.GetComponent<TrailRenderer>().startColor = FigureArray[1].GetComponent<SpriteRenderer>().color;
        trailRenderer.GetComponent<TrailRenderer>().endColor = FigureArray[1].GetComponent<SpriteRenderer>().color;
    }
    public void ChangeTriangle()
    {
        CurrentFigure = FigureArray[2];
        trailRenderer.GetComponent<TrailRenderer>().startColor = FigureArray[2].GetComponent<SpriteRenderer>().color;
        trailRenderer.GetComponent<TrailRenderer>().endColor = FigureArray[2].GetComponent<SpriteRenderer>().color;
    }
    public void ChangeCircle()
    {
        CurrentFigure = FigureArray[0];
        trailRenderer.GetComponent<TrailRenderer>().startColor = FigureArray[0].GetComponent<SpriteRenderer>().color;
        trailRenderer.GetComponent<TrailRenderer>().endColor = FigureArray[0].GetComponent<SpriteRenderer>().color;
    }





}
