using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawImage : MonoBehaviour
{
    Texture2D tex;
    Sprite sprite;
    public GameObject targetSprite;

    Vector2 DrawPos;

    // Start is called before the first frame update
    void Start()
    {
        tex = new Texture2D(280,280);
        sprite = targetSprite.GetComponent<SpriteRenderer>().sprite;
    }

    // Update is called once per frame
    void Update()
    {
        DrawPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(DrawPos);
        if(Input.GetMouseButton(0)){
            tex.SetPixel((int)DrawPos.x,(int)DrawPos.y,Color.black);
            tex.Apply();
            Debug.Log("<color=red>FUNCIONE</color>");
            targetSprite.GetComponent<SpriteRenderer>().sprite = Sprite.Create(tex,new Rect(0,0,tex.width,tex.height),new Vector2(0.5f,0.5f));
        }    
    }
}
