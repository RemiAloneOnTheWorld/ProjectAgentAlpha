using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private int BoxCount = 0;
    [SerializeField] private int pickupDistance;
    [SerializeField] private int placeDistance;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject boxPrefab;
    [Range(0f, 1f)] public float radiusHalfBox;
    [SerializeField] private RectTransform crosshair;
    private InputAction _collect;
    private bool beingPlaced;
    // Start is called before the first frame update
    void Start()
    {
        playerInput.actions.FindAction("PlaceCollect", true).canceled += Cancel;
        playerInput.actions.FindAction("PlaceCollect", true).started += Started;
   
    }

    private void Started(InputAction.CallbackContext obj)
    {
        beingPlaced = true;
    }


    private void Cancel(InputAction.CallbackContext pContext)
    {
        beingPlaced = false;
        if (!Physics.Raycast(playerCamera.ScreenPointToRay(crosshair.position), out var raycastHit,
           pickupDistance) || !raycastHit.collider.CompareTag("Box"))
        {
            if (BoxCount > 0)
            {

                Vector3 pos = playerCamera.ScreenToWorldPoint(new Vector3(crosshair.position.x, crosshair.position.y, placeDistance));
                if (checkCollision(pos))
                {
                    print("Cant place!");
                }
                else
                {
                    print("box Placed!");
                    Instantiate<GameObject>(boxPrefab, pos, Quaternion.Euler(Vector3.zero));
                    BoxCount--;

                }
            }
         
            return;
        }


        Destroy(raycastHit.collider.gameObject);
        BoxCount++;
        

    }


    private bool checkCollision(Vector3 pos)
    {

        if (Physics.OverlapBox(pos, boxPrefab.transform.localScale * radiusHalfBox).Length > 0)
        {
            return true;
        }
        return false;

    }




    // Update is called once per frame
    void Update()
    {
       

    }

    private void OnDrawGizmos()
    {
        if (beingPlaced)
        {
            print("drawGizmo");
            Gizmos.DrawWireCube(playerCamera.ScreenToWorldPoint(new Vector3(crosshair.position.x, crosshair.position.y, placeDistance)), boxPrefab.transform.localScale * radiusHalfBox);
        }
    }




}
