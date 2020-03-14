using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paintable : MonoBehaviour
{
    public GameObject Brush;
    public float BrushSize = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            //cast a ray to the plane
            var Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(Ray, out hit))
            {
                //instanciate a brush
                var go = Instantiate(Brush, hit.point + Vector3.up * 0.1f, this.transform.rotation, transform);
                go.transform.localScale = Vector3.one * BrushSize;
            }

        }
    }
}
