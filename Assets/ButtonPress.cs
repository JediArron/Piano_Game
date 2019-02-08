using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    SpriteRenderer theSR;
    public Sprite defaultImage;
    public Sprite pressedImage;

    // Start is called before the first frame update
    void Start()
    {
        theSR = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        theSR.sprite = pressedImage;
    }

    private void OnMouseUp()
    {
        theSR.sprite = defaultImage;
    }
}

    //Update is called once per frame
//    void update()
//    {
//        if (input.touchcount > 0)
//        {
//            touch touch = input.gettouch(0);
//            if (touch.phase == touchphase.began)
//            {
//                theSR.sprite = pressedimage;
//            }

//            if (touch.phase == touchphase.ended)
//            {
//                theSR.sprite = defaultimage;
//            }
//        }
//    }
//}
