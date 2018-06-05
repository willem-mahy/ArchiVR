using UnityEngine;

namespace Assets.WM.UI
{
    public enum SwipeDirection
    {
        up = 0,
        down,
        left,
        right
    }

    public class SwipeEventArgs
    {
        public SwipeDirection dir;

        public SwipeEventArgs(SwipeDirection d) { dir = d; }
    }

    public class InputSwipe : MonoBehaviour
    {
        private Vector3 fp;   //First touch position
        private Vector3 lp;   //Last touch position
        private float dragDistance;  //minimum distance for a swipe to be registered

        // Declare the delegate (if using non-generic pattern).
        public delegate void SwipeEventHandler(object sender, SwipeEventArgs e);

        // Declare the event.
        public event SwipeEventHandler SwipeEvent;

        // Wrap the event in a protected virtual method
        // to enable derived classes to raise the event.
        protected virtual void PublishSwipeEvent(SwipeDirection dir)
        {
            // Raise the event by using the () operator.
            if (SwipeEvent != null)
                SwipeEvent(this, new SwipeEventArgs(dir));
        }

        void Start()
        {
            dragDistance = Screen.height * 15 / 100; //dragDistance is 15% height of the screen
        }

        void Update()
        {
            if (Input.touchCount == 1) // user is touching the screen with a single touch
            {
                Touch touch = Input.GetTouch(0); // get the touch

                if (touch.phase == TouchPhase.Began) //check for the first touch
                {
                    fp = touch.position;
                    lp = touch.position;
                }
                else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
                {
                    lp = touch.position;
                }
                else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
                {
                    lp = touch.position;  //last touch position. Ommitted if you use list

                    //Check if drag distance is greater than 20% of the screen height
                    if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                    {
                        //It's a drag
                        //check if the drag is vertical or horizontal
                        if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
                        {
                            //If the horizontal movement is greater than the vertical movement...
                            if ((lp.x > fp.x))  //If the movement was to the right)
                            {
                                //Right swipe
                                Debug.Log("Right Swipe");
                                PublishSwipeEvent(SwipeDirection.right);
                            }
                            else
                            {
                                //Left swipe
                                Debug.Log("Left Swipe");
                                PublishSwipeEvent(SwipeDirection.left);
                            }
                        }
                        else
                        {
                            //the vertical movement is greater than the horizontal movement
                            if (lp.y > fp.y)  //If the movement was up
                            {   //Up swipe
                                Debug.Log("Up Swipe");
                                PublishSwipeEvent(SwipeDirection.up);
                            }
                            else
                            {   //Down swip
                                Debug.Log("Down Swipe");
                                PublishSwipeEvent(SwipeDirection.down);
                            }
                        }
                    }
                    else
                    {
                        //It's a tap as the drag distance is less than 20% of the screen height
                        Debug.Log("Tap");
                    }
                }
            }
        }
    }
}