using UnityEngine;

public class MobileInput : MonoBehaviour
{
    private Vector2 startTouch;
    private Vector2 endTouch;

    public Piece currentPiece;

    private void Update()
    {
        if (currentPiece == null)
            return;

        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            startTouch = touch.position;
        }

        if (touch.phase == TouchPhase.Ended)
        {
            endTouch = touch.position;

            DetectSwipe();
        }
    }

    private void DetectSwipe()
    {
        Vector2 delta = endTouch - startTouch;

        // HORIZONTAL
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            if (delta.x > 50)
            {
                currentPiece.MoveRight();
            }
            else if (delta.x < -50)
            {
                currentPiece.MoveLeft();
            }
        }
        // VERTICAL
        else
        {
            // SWIPE DOWN
            if (delta.y < -50)
            {
                currentPiece.HardDropPiece();
            }
            // TAP
            else
            {
                currentPiece.RotatePiece();
            }
        }
    }
}