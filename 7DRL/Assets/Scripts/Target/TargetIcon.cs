using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TargetIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private GameObject popUpPrefab;
    [SerializeField] private GrettingGenerator greetingGenerator;
    private GameObject popUpInstance;
    public void OnPointerEnter(PointerEventData eventData)
    {
        // reveal pop up
        if (popUpPrefab != null && popUpInstance == null)
        {
            popUpInstance = Instantiate(popUpPrefab, transform.position + 150 * Vector3.up, Quaternion.identity, transform);
        }

        TextMeshProUGUI popUpText = popUpInstance.GetComponentInChildren<TextMeshProUGUI>();
        if (popUpText != null)
        {
            popUpText.text = greetingGenerator.GetGreeting();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // hide pop up
        if (popUpInstance != null)
        {
            Destroy(popUpInstance);
            popUpInstance = null;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
