using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceMove : MonoBehaviour
{
    [SerializeField]
    GameObject[] goPointList;
    [SerializeField]
    GameObject m_teleportCylinder;
    [SerializeField]
    float rotSpeed = 2.0f;
    [SerializeField]
    float moveSpeed = 0.8f;
    [SerializeField]
    int viewMode = 0; // forecemove = 0, lookat = 1, teleport = 2
    [SerializeField]
    FadeInOut m_fadeObject;
    [SerializeField]
    GameObject m_teleportCavePos;
    [SerializeField]
    GameObject m_teleportCaveExitPos;

    int curGoPointIdx = 0;
    int numGoPoints = 0;
    int sumGoPoints = 0; // 순회 횟수
    Transform curTransform;
    CharacterController curCharacterController;
    Camera curPlayerCamera;

    private const float _maxDistance = 10;
    private GameObject _gazedAtObject = null;

    // Start is called before the first frame update
    void Start()
    {
        curGoPointIdx = 0; // 현재 GoPoint의 인덱스
        curTransform = GetComponent<Transform>(); // 현재 Player의 Transform
        curCharacterController = GetComponent<CharacterController>(); // 현재 Player의 CharacterController 얻기
        curPlayerCamera = Camera.main; // 현재 Camera 얻기
        goPointList = GameObject.FindGameObjectsWithTag("GoPoint"); // 지정한 GoPoints
        numGoPoints = goPointList.Length; // GoPoints 총 개수
    }

    // Update is called once per frame
    void Update()
    {
        if (viewMode == 0) MovePlayer();
        else if (viewMode == 1) LookAtMovePlayer();
        else if (viewMode == 2) LookAtTeleportPlayer();
    }
    void MovePlayer()
    {
        // Player(이)가 가야할 방향 결정
        Vector3 goDirection = goPointList[curGoPointIdx].transform.position - curTransform.position;
        // Player가 바라볼 방향의 Rot값을 쿼터니언을 통해서 구함.
        Quaternion goRotation = Quaternion.LookRotation(goDirection);
        // 해당 방향을 회전
        curTransform.rotation = Quaternion.Slerp(curTransform.rotation, goRotation, Time.deltaTime * rotSpeed);
        // 해당 방향으로 이동
        curTransform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
    }
    void LookAtMovePlayer()
    {
        if (Input.touchCount > 0)
        {
            // 현재 바라보고 있는 Camera의 방향이 플레이어가 움직이는 방향
            Vector3 ForwardDir = curPlayerCamera.transform.forward;

            // 해당 방향의 높이 값을 0으로 주어서 움직일때 높이와 상관 없는 방향성 제시
            ForwardDir.y = 0;

            // 현재 CharacterController를 SimpleMove를 통해서 이동
            curCharacterController.SimpleMove(ForwardDir * moveSpeed);
        }
    }
    bool isFading = false;
    void LookAtTeleportPlayer()
    {
        int mask = 1 << 5; // UI Culling
        mask = ~mask;
        Vector3 ForwardDir = curPlayerCamera.transform.forward;
        Vector3 positionCamera = curPlayerCamera.transform.position;
        RaycastHit hit = new RaycastHit();
        Vector3 hitPoint = Vector3.zero;
        bool testing = false;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                Vector3 testingPos = positionCamera;

                testingPos = Camera.main.worldToCameraMatrix * testingPos; // 뷰 공간 전환
                // 대각선이면
                if (Mathf.Abs(Mathf.Abs(i) - 1.0f) < float.Epsilon && Mathf.Abs(Mathf.Abs(j) - 1.0f) < float.Epsilon)
                {
                    testingPos.x += (float)i * 7 / 100f;
                    testingPos.y += (float)j * 7 / 100f;
                }
                else
                {
                    testingPos.x += i / 10f;
                    testingPos.y += j / 10f;
                }
                testingPos.x += i * 0.02f;
                testingPos.y += j * 0.02f;
                testingPos = Camera.main.cameraToWorldMatrix * testingPos; // 다시 월드 공간으로 전환

                Debug.DrawLine(positionCamera, testingPos, Color.red);

                if (Physics.Raycast(testingPos, ForwardDir, out hit, _maxDistance, mask))
                {
                    Debug.DrawRay(testingPos, ForwardDir);
                    // GameObject detected in front of the camera.
                    if (i == 0 && j == 0)
                    {
                        // ? 의 의미는 앞 변수가 null이 아닐때 호출
                        _gazedAtObject = hit.transform.gameObject;
                        hitPoint = hit.point;
                    }
                    if (hit.transform.gameObject.layer != 6)
                    {
                        testing = true;
                        break;
                    }
                }
            }
        }
        if (testing || hitPoint == Vector3.zero)
        {
            m_teleportCylinder.SetActive(false);
        }
        else if (!testing)
        {
            m_teleportCylinder.SetActive(true);
            hitPoint.y += 0.5f;
            m_teleportCylinder.transform.position = hitPoint;
        }
        if (Application.platform != RuntimePlatform.Android)
        {
            if ((Input.GetMouseButtonDown(0) && m_teleportCylinder.activeSelf && m_fadeObject.alpha <= 0f && !isFading))
            {
                StartCoroutine(FadeInMove(m_teleportCylinder.transform.position));
            }
        }
        else
        {
            if ((Input.GetTouch(0).phase == TouchPhase.Ended && m_teleportCylinder.activeSelf && m_fadeObject.alpha <= 0f && !isFading))
            {
                StartCoroutine(FadeInMove(m_teleportCylinder.transform.position));
            }
        }
    }
    IEnumerator FadeInMove(Vector3 pos)
    {
        isFading = true;
        m_fadeObject.FadeIn(0.7f);
        yield return new WaitForSeconds(1.0f);
        curTransform.position = pos;
        m_fadeObject.FadeOut(0.7f);
        isFading = false;
    }
    public void TeleportToCave()
    {
        StartCoroutine(FadeInMove(m_teleportCavePos.transform.position));
    }
    public void CaveToTeleport()
    {
        StartCoroutine(FadeInMove(m_teleportCaveExitPos.transform.position));
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("GoPoint"))
        {
            curGoPointIdx++;
            if(curGoPointIdx == numGoPoints)
            {
                curGoPointIdx = 0;
                sumGoPoints++;
            }
            if (sumGoPoints >= 3 && curGoPointIdx == 1)
            {
                viewMode = 1;
                Debug.Log("View Mode Changed!");
            }
            Debug.Log("Trigger Enter : " + other.name);
        }
    }
}
