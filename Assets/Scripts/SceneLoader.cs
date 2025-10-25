using UnityEngine;
using UnityEngine.SceneManagement; // �� ������ ���� �ʿ��մϴ�.

public class SceneLoader : MonoBehaviour
{
    // �ν����� â���� �ε��� ���� �̸��� ������ �� �ֵ��� public ������ �����մϴ�.
    public string sceneToLoad = "NextSceneName"; 

    // OnTriggerEnter�� ȣ��� �� �ݶ��̴��� �˻��մϴ�.
    private void OnTriggerEnter(Collider other)
    {
        // 1. �浹�� ������Ʈ�� �±װ� 'Player'���� Ȯ���մϴ�.
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player�� 'Clear' ������ �����߽��ϴ�. ���� �ε��մϴ�.");

            // 2. ������ ���� �ε��մϴ�.
            // �� �̸��� ��ȿ�ϰ�, ���� ������ �߰��Ǿ� �־�� �մϴ�.
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}