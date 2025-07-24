using System;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class ThirdPersonAimingController : MonoBehaviour
{
    [SerializeField] private CinemachineCamera aimCinemachineCamera;
    [SerializeField] private CinemachineCamera defaultCinemachineCamera;

    public ThirdPersonController thirdPersonController;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private UnityEngine.UI.Image crossHair;
    [SerializeField] private Transform waterNozzle;

    
    private void Update()
    {
        Vector2 screenCenter = new(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter); // 👈 fixed this line
        Vector3 mouseWorldPosition = Vector3.zero;

        if (Physics.Raycast(ray, out RaycastHit hit, 999f, aimColliderLayerMask))
        {
            mouseWorldPosition = hit.point;
        }

        // Aiming
        if (Input.GetMouseButton(1)) // Right mouse button held
        {
            aimCinemachineCamera.Priority = 20;
            defaultCinemachineCamera.Priority = 10;
            crossHair.enabled = true;
            thirdPersonController.SetRotationOnMove(false);

            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
            waterNozzle.LookAt(mouseWorldPosition); // rotate nozzle to aim

        }
        else
        {
            aimCinemachineCamera.Priority = 10;
            defaultCinemachineCamera.Priority = 20;
            crossHair.enabled = false;

            thirdPersonController.SetRotationOnMove(true);

        }
    }
}
