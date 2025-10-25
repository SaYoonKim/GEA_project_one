using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리를 위해 필요합니다.

public class SceneLoader : MonoBehaviour
{
    // 인스펙터 창에서 로드할 씬의 이름을 지정할 수 있도록 public 변수로 선언합니다.
    public string sceneToLoad = "NextSceneName"; 

    // OnTriggerEnter가 호출될 때 콜라이더를 검사합니다.
    private void OnTriggerEnter(Collider other)
    {
        // 1. 충돌한 오브젝트의 태그가 'Player'인지 확인합니다.
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player가 'Clear' 영역에 진입했습니다. 씬을 로드합니다.");

            // 2. 지정된 씬을 로드합니다.
            // 씬 이름이 유효하고, 빌드 설정에 추가되어 있어야 합니다.
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}