using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class ShowStandOnImage : MonoBehaviour
{
    [SerializeField] private ARTrackedImageManager trackedImageManager;
    [SerializeField] private List<StandData> stands;

    private Dictionary<string, GameObject> spawnedStands = new();

    void OnEnable() => trackedImageManager.trackedImagesChanged += OnChanged;
    void OnDisable() => trackedImageManager.trackedImagesChanged -= OnChanged;

    private void Start()
    {
        foreach (var stand in stands)
        {
            var obj = Instantiate(stand.prefab, Vector3.zero, Quaternion.identity);
            obj.name = stand.imageName;
            obj.SetActive(false);
            spawnedStands.Add(stand.imageName, obj);
        }
    }

    private void OnChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (var trackedImage in args.updated)
        {
            if (spawnedStands.TryGetValue(trackedImage.referenceImage.name, out var stand))
            {
                if (trackedImage.trackingState == TrackingState.Tracking)
                {
                    stand.transform.position = trackedImage.transform.position;
                    stand.transform.rotation = trackedImage.transform.rotation;
                    stand.SetActive(true);
                }
                else
                {
                    stand.SetActive(false);
                }
            }
        }
    }
}

[System.Serializable]
public class StandData
{
    public string imageName;
    public GameObject prefab;
}
