using UnityEngine;

namespace Completed
{
    public class TouchInput : IGetInput
    {
        private Vector2 touchOrigin = -Vector2.one; 

        public Vector2Int GetInput()
        {
            if (Input.touchCount <= 0) return Vector2Int.zero;
            Touch myTouch = Input.touches[0];

            if (myTouch.phase == TouchPhase.Began)
            {
                touchOrigin = myTouch.position;
            }

            else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
            {
                Vector2 touchEnd = myTouch.position;

                float x = touchEnd.x - touchOrigin.x;
                float y = touchEnd.y - touchOrigin.y;

                touchOrigin.x = -1;

                if (Mathf.Abs(x) > Mathf.Abs(y))
                {
                    var horizontal = x > 0 ? 1 : -1;
                    return new Vector2Int(horizontal, 0);
                }
                else
                {
                    var vertical = y > 0 ? 1 : -1;
                    return new Vector2Int(0, vertical);
                }
            }

            return Vector2Int.zero;
        }
    }
}