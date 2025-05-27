using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIController : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject panelToDestroy; // ������ ���� ���� ������
    public GameObject panelToActivate; // ������ ���� ���� ������
    public float destroyDelay = 2f; // ����� ���� ���� ���� ����� ������

    [Header("Keyboard Inputs")]
    public List<KeyCode> activationKeys; // ����� ������� ���� ���� ������ ����

    private bool isPanelDestroyed = false; // ����� ��� �� ����� ������ �� ��

    void Start()
    {
        // ������ �� �� ������ ������ ����� ���� ������
        if (panelToActivate != null)
        {
            panelToActivate.SetActive(false); // ����� ������ ���� ���� ������
        }
    }

    void Update()
    {
        // ������ ��� �� ����� ��� �� �� ������� ������� �� ��� Inspector
        foreach (var key in activationKeys)
        {
            if (Input.GetKeyDown(key) && !isPanelDestroyed) // ��� �� ����� ��� �� ��� ��� ����� ������ ���
            {
                StartCoroutine(DestroyPanelAfterDelay());
                break; // �� ����� ������ �� ������� ������ ��� ����� �����
            }
        }
    }

    // Coroutine ������ ��� Panel ��� ������� ������
    IEnumerator DestroyPanelAfterDelay()
    {
        // �������� ���� ������� ������
        yield return new WaitForSeconds(destroyDelay);

        // ����� ��� Panel
        if (panelToDestroy != null)
        {
            panelToDestroy.SetActive(false);
            isPanelDestroyed = true; // �� ����� ������
        }

        // ����� ������ ������
        if (panelToActivate != null)
        {
            panelToActivate.SetActive(true);
        }
    }
}
