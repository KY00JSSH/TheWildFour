using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIcon_LookControl : MonoBehaviour
{
    public RectTransform playerIconRect;
    public RectTransform arrowIconRect;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = Input.mousePosition;

        Vector2 playerIconPosition = playerIconRect.position;

        Vector2 direction = mousePosition - playerIconPosition;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        //arrowIconRect.position = playerIconRect.position;

        arrowIconRect.rotation = Quaternion.Euler(new Vector3(0, 0, angle));        
    }
}
