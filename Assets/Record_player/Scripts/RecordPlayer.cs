using UnityEngine;
using System.Collections;

public class RecordPlayer : MonoBehaviour
{
    public bool recordPlayerActive = false;
    private GameObject disc;
    private GameObject arm;

    private int mode;
    private float armAngle;
    private float discAngle;
    private float discSpeed;

    private AudioSource audioSource;  // 음악을 재생할 AudioSource
    public AudioClip[] musicClips;    // 음악 클립 배열

    void Awake()
    {
        disc = gameObject.transform.Find("teller").gameObject;
        arm = gameObject.transform.Find("arm").gameObject;
        audioSource = GetComponent<AudioSource>();  // AudioSource 컴포넌트 참조
    }

    void Start()
    {
        mode = 0;
        armAngle = 0.0f;
        discAngle = 0.0f;
        discSpeed = 0.0f;
    }

    void Update()
    {
        // 음악이 재생 중이면 recordPlayerActive를 true로 설정
        if (audioSource.isPlaying)
        {
            recordPlayerActive = true;
        }
        else
        {
            recordPlayerActive = false;
        }

        // 클릭으로 음악을 재생하거나 정지하는 로직
        if (Input.GetMouseButtonDown(0))  // 마우스 왼쪽 버튼 클릭 시
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == this.gameObject)  // 레코드 플레이어 오브젝트에 클릭한 경우
                {
                    ToggleMusic();  // 음악 재생/정지 토글
                }
            }
        }

        //-- Mode 0: player off
        if (mode == 0)
        {
            if (recordPlayerActive == true)
                mode = 1;
        }
        //-- Mode 1: activation
        else if (mode == 1)
        {
            if (recordPlayerActive == true)
            {
                armAngle += Time.deltaTime * 30.0f;
                if (armAngle >= 30.0f)
                {
                    armAngle = 30.0f;
                    mode = 2;
                }
                discAngle += Time.deltaTime * discSpeed;
                discSpeed += Time.deltaTime * 80.0f;
            }
            else
                mode = 3;
        }
        //-- Mode 2: running
        else if (mode == 2)
        {
            if (recordPlayerActive == true)
                discAngle += Time.deltaTime * discSpeed;
            else
                mode = 3;
        }
        //-- Mode 3: stopping
        else
        {
            if (recordPlayerActive == false)
            {
                armAngle -= Time.deltaTime * 30.0f;
                if (armAngle <= 0.0f)
                    armAngle = 0.0f;

                discAngle += Time.deltaTime * discSpeed;
                discSpeed -= Time.deltaTime * 80.0f;
                if (discSpeed <= 0.0f)
                    discSpeed = 0.0f;

                if ((discSpeed == 0.0f) && (armAngle == 0.0f))
                    mode = 0;
            }
            else
                mode = 1;
        }

        //-- update objects
        arm.transform.localEulerAngles = new Vector3(0.0f, armAngle, 0.0f);
        disc.transform.localEulerAngles = new Vector3(0.0f, discAngle, 0.0f);
    }

    // 음악을 재생하거나 정지하는 함수
    private void ToggleMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();  // 음악 정지
        }
        else
        {
            PlayRandomMusic();  // 랜덤 음악 재생
        }
    }

    // 음악 배열에서 랜덤으로 하나를 선택해 재생
    private void PlayRandomMusic()
    {
        if (musicClips.Length > 0)
        {
            int randomIndex = Random.Range(0, musicClips.Length);  // 0부터 musicClips 배열의 길이 - 1 사이의 값
            audioSource.clip = musicClips[randomIndex];  // 랜덤으로 선택된 음악 클립
            audioSource.Play();  // 음악 재생
        }
    }
}