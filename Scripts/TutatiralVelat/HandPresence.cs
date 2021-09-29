using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{
    public bool showController = false;
    public InputDeviceCharacteristics controllreCharacteristics;
    public List<GameObject> controllerPrefabs;
    public GameObject handModelPrefab;


    private InputDevice targetDevice;

    private GameObject spawnedController;
    private GameObject spawnedHandModel;
    private Animator handAnimator;
        void Start()
    {
        TryInitializze();

    }
    void TryInitializze()
    {
        List<InputDevice> devices = new List<InputDevice>();

        InputDevices.GetDevicesWithCharacteristics(controllreCharacteristics, devices);
        foreach (var item in devices)
        {
            Debug.Log(item.name + item.characteristics);
        }
        if (devices.Count > 0)
        {
            targetDevice = devices[0];
            GameObject prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name);
            if (prefab)
            {
                spawnedController = Instantiate(prefab, transform);
            }
            else
            {
                Debug.LogError("Did not find correspending controller model / Model referansı bulunamadı!!");
                spawnedController = Instantiate(controllerPrefabs[0], transform);
            }
            spawnedHandModel = Instantiate(handModelPrefab, transform);
            handAnimator = spawnedHandModel.GetComponent<Animator>();

        }
    }
    void UpdateHandAnimation()

    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger,out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            handAnimator.SetFloat("Trigger", 0);
        }
        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!targetDevice.isValid)
        {
            TryInitializze();
        }
        else
        {
            if (showController)
            {
                spawnedHandModel.SetActive(false);
                spawnedController.SetActive(true);

            }
            else
            {
                spawnedHandModel.SetActive(true);
                spawnedController.SetActive(false);
                UpdateHandAnimation();
            }
        }
       
        /*  if (targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue)&&primaryButtonValue)
               Debug.Log("Tuşa Basıyorsunnnn");

           if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue)&&triggerValue > 0.1f)
               Debug.Log("Trigger Pressed" + triggerValue);

           if (targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxisValue)&&primary2DAxisValue != Vector2.zero)
               Debug.Log("primary touchpad " + primary2DAxisValue); 
         */

    }
}
