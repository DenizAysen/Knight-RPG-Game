using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseScript : MonoBehaviour
{
    #region SerializedFields
    [SerializeField] private Texture2D cursorTextureNormal;
    [SerializeField] private Texture2D cursorTextureEnemy;
    [SerializeField] private GameObject mousePoint; 
    #endregion

    private CursorMode _mode = CursorMode.ForceSoftware;
    private Vector2 _hotspot = Vector2.zero;
    

    // Update is called once per frame
    void Update()
    {
        CursorChanger();

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
    private void CursorChanger()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Target"))
            {
                Cursor.SetCursor(cursorTextureEnemy, _hotspot, _mode);
            }
            else
            {
                Cursor.SetCursor(cursorTextureNormal, _hotspot, _mode);
            }
        }
    }
}
