using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ZoneTrigger : MonoBehaviour
{
    public enum ZoneType
    {
        SafetyZone,
        DangerZone,
        TargetArea,
        MachineArea,
        FinishZone
    }

    [Header("Setting Zone")]
    public ZoneType zoneType;

    [Header("Target Area")]
    [Min(1)] public int requiredLoadCount = 2;
    [Min(0f)] public float requiredStaySeconds = 0.5f;

    [Header("Finish Zone")]
    [SerializeField] private GameObject finishOverlay;
    [SerializeField] private TMP_Text finishMessageText;
    [SerializeField] private string finishMessage = "Terima kasih sudah menyelesaikan training.";
    [SerializeField] private bool pauseOnFinish = true;

    private readonly Dictionary<GameObject, float> loadsInTarget = new Dictionary<GameObject, float>();
    private bool targetDelivered = false;
    private bool finishTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (zoneType == ZoneType.SafetyZone && other.CompareTag("Player"))
        {
            TrainingManager.Instance.SetPlayerInSafeZone(true);
            Debug.Log("Player masuk Safety Zone");
        }

        if (zoneType == ZoneType.MachineArea && other.CompareTag("Player"))
        {
            TrainingManager.Instance.SetPlayerInMachineArea(true);
            Debug.Log("Player masuk Machine Area");
        }

        if (zoneType == ZoneType.DangerZone)
        {
            if (other.CompareTag("Player"))
                TrainingManager.Instance.ReportDangerZoneEntered("Player");
            else if (GetLoadObject(other) != null)
                TrainingManager.Instance.ReportDangerZoneEntered("Load");
        }

        if (zoneType == ZoneType.FinishZone && other.CompareTag("Player"))
        {
            ShowFinishOverlay();
            Debug.Log("Player masuk Finish Zone");
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (zoneType != ZoneType.TargetArea || targetDelivered) return;

        GameObject loadObject = GetLoadObject(other);
        if (loadObject == null) return;

        if (!loadsInTarget.ContainsKey(loadObject))
            loadsInTarget.Add(loadObject, Time.time);

        int stableLoadCount = CountStableLoads();

        if (stableLoadCount >= requiredLoadCount)
        {
            targetDelivered = true;

            if (TrainingManager.Instance != null)
                TrainingManager.Instance.ReportLoadDelivered();

            Debug.Log("Target Area complete: " + stableLoadCount + "/" + requiredLoadCount + " load berhasil masuk.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (zoneType == ZoneType.SafetyZone && other.CompareTag("Player"))
        {
            TrainingManager.Instance.SetPlayerInSafeZone(false);
            Debug.Log("Player keluar Safety Zone");
        }

        if (zoneType == ZoneType.MachineArea && other.CompareTag("Player"))
        {
            TrainingManager.Instance.SetPlayerInMachineArea(false);
            Debug.Log("Player keluar Machine Area");
        }

        if (zoneType == ZoneType.TargetArea)
        {
            GameObject loadObject = GetLoadObject(other);

            if (loadObject != null)
                loadsInTarget.Remove(loadObject);
        }
    }

    private int CountStableLoads()
    {
        int count = 0;

        foreach (float enterTime in loadsInTarget.Values)
        {
            if (Time.time - enterTime >= requiredStaySeconds)
                count++;
        }

        return count;
    }

    private GameObject GetLoadObject(Collider other)
    {
        if (other == null) return null;

        Rigidbody attachedRigidbody = other.attachedRigidbody;

        if (attachedRigidbody != null && IsLoadObject(attachedRigidbody.gameObject))
            return attachedRigidbody.gameObject;

        if (IsLoadObject(other.gameObject))
            return attachedRigidbody != null ? attachedRigidbody.gameObject : other.gameObject;

        Transform root = other.transform.root;
        if (root != null && IsLoadObject(root.gameObject))
            return root.gameObject;

        return null;
    }

    private bool IsLoadObject(GameObject obj)
    {
        if (obj == null) return false;

        return obj.tag == "Load" || obj.tag == "Cargo";
    }

    private void ShowFinishOverlay()
    {
        if (finishTriggered) return;
        finishTriggered = true;

        if (finishOverlay == null)
            CreateFinishOverlay();

        if (finishMessageText != null)
            finishMessageText.text = finishMessage;

        if (finishOverlay != null)
            finishOverlay.SetActive(true);

        if (pauseOnFinish)
            Time.timeScale = 0f;
    }

    private void CreateFinishOverlay()
    {
        GameObject canvasObject = new GameObject("Finish Overlay Canvas");
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1000;

        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<GraphicRaycaster>();

        finishOverlay = new GameObject("Finish Overlay");
        finishOverlay.transform.SetParent(canvasObject.transform, false);

        Image background = finishOverlay.AddComponent<Image>();
        background.color = Color.white;

        RectTransform overlayRect = finishOverlay.GetComponent<RectTransform>();
        overlayRect.anchorMin = Vector2.zero;
        overlayRect.anchorMax = Vector2.one;
        overlayRect.offsetMin = Vector2.zero;
        overlayRect.offsetMax = Vector2.zero;

        GameObject textObject = new GameObject("Finish Message");
        textObject.transform.SetParent(finishOverlay.transform, false);

        finishMessageText = textObject.AddComponent<TextMeshProUGUI>();
        finishMessageText.text = finishMessage;
        finishMessageText.color = Color.black;
        finishMessageText.fontSize = 42f;
        finishMessageText.alignment = TextAlignmentOptions.Center;

        RectTransform textRect = finishMessageText.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;
    }
}
