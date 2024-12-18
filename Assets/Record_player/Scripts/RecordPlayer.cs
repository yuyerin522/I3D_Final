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

    private AudioSource audioSource;  // ������ ����� AudioSource
    public AudioClip[] musicClips;    // ���� Ŭ�� �迭

    void Awake()
    {
        disc = gameObject.transform.Find("teller").gameObject;
        arm = gameObject.transform.Find("arm").gameObject;
        audioSource = GetComponent<AudioSource>();  // AudioSource ������Ʈ ����
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
        // ������ ��� ���̸� recordPlayerActive�� true�� ����
        if (audioSource.isPlaying)
        {
            recordPlayerActive = true;
        }
        else
        {
            recordPlayerActive = false;
        }

        // Ŭ������ ������ ����ϰų� �����ϴ� ����
        if (Input.GetMouseButtonDown(0))  // ���콺 ���� ��ư Ŭ�� ��
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == this.gameObject)  // ���ڵ� �÷��̾� ������Ʈ�� Ŭ���� ���
                {
                    ToggleMusic();  // ���� ���/���� ���
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

    // ������ ����ϰų� �����ϴ� �Լ�
    private void ToggleMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();  // ���� ����
        }
        else
        {
            PlayRandomMusic();  // ���� ���� ���
        }
    }

    // ���� �迭���� �������� �ϳ��� ������ ���
    private void PlayRandomMusic()
    {
        if (musicClips.Length > 0)
        {
            int randomIndex = Random.Range(0, musicClips.Length);  // 0���� musicClips �迭�� ���� - 1 ������ ��
            audioSource.clip = musicClips[randomIndex];  // �������� ���õ� ���� Ŭ��
            audioSource.Play();  // ���� ���
        }
    }
}