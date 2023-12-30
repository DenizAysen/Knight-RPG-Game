using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseScript : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture;
    [SerializeField] private GameObject mousePoint;

    private CursorMode _mode = CursorMode.ForceSoftware;
    private Vector2 _hotspot = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.SetCursor(cursorTexture, _hotspot, _mode);

        CreateMoveParticles();
    }
    private void CreateMoveParticles()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    Vector3 lastPos = hit.point;
                    lastPos.y = 0.35f;
                    Instantiate(mousePoint, lastPos, Quaternion.identity);
                }
            }
        }
    }
}
