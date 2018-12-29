using UnityEngine;

public class EndTriggerCollision : MonoBehaviour
{
    [SerializeField]
    private GameObject FinishLevelUI;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("END");
        Instantiate(FinishLevelUI).SetActive(true);
    }
}
