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
    int sumGoPoints = 0; // ��ȸ Ƚ��
    Transform curTransform;
    CharacterController curCharacterController;
    Camera curPlayerCamera;

    private const float _maxDistance = 10;
    private GameObject _gazedAtObject = null;

    // Start is called before the first frame update
    void Start()
    {
        curGoPointIdx = 0; // ���� GoPoint�� �ε���
        curTransform = GetComponent<Transform>(); // ���� Player�� Transform
        curCharacterController = GetComponent<CharacterController>(); // ���� Player�� CharacterController ���
        curPlayerCamera = Camera.main; // ���� Camera ���
        goPointList = GameObject.FindGameObjectsWithTag("GoPoint"); // ������ GoPoints
        numGoPoints = goPointList.Length; // GoPoints �� ����
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
        // Player(��)�� ������ ���� ����
        Vector3 goDirection = goPointList[curGoPointIdx].transform.position - curTransform.position;
        // Player�� �ٶ� ������ Rot���� ���ʹϾ��� ���ؼ� ����.
        Quaternion goRotation = Quaternion.LookRotation(goDirection);
        // �ش� ������ ȸ��
        curTransform.rotation = Quaternion.Slerp(curTransform.rotation, goRotation, Time.deltaTime * rotSpeed);
        // �ش� �������� �̵�
        curTransform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
    }
    void LookAtMovePlayer()
    {
        if (Input.touchCount > 0)
        {
            // ���� �ٶ󺸰� �ִ� Camera�� ������ �÷��̾ �����̴� ����
            Vector3 ForwardDir = curPlayerCamera.transform.forward;

            // �ش� ������ ���� ���� 0���� �־ �����϶� ���̿� ��� ���� ���⼺ ����
            ForwardDir.y = 0;

            // ���� CharacterController�� SimpleMove�� ���ؼ� �̵�
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

                testingPos = Camera.main.worldToCameraMatrix * testingPos; // �� ���� ��ȯ
                // �밢���̸�
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
                testingPos = Camera.main.cameraToWorldMatrix * testingPos; // �ٽ� ���� �������� ��ȯ

                Debug.DrawLine(positionCamera, testingPos, Color.red);

                if (Physics.Raycast(testingPos, ForwardDir, out hit, _maxDistance, mask))
                {
                    Debug.DrawRay(testingPos, ForwardDir);
                    // GameObject detected in front of the camera.
                    if (i == 0 && j == 0)
                    {
                        // ? �� �ǹ̴� �� ������ null�� �ƴҶ� ȣ��
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
