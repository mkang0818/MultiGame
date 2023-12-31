using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviour, IPunObservable
{
    public Slider HpBar;
    Vector3 curPos;

    public PhotonView PV;
    Rigidbody rb;
    Animator Anim;

    public float moveSp;
    float hAxis;
    float vAxis;
    bool jDown;
    bool isJump;
    bool isRun;

    Vector3 moveVec;

    public int curHP;
    int MaxHP = 10;
    private void Start()
    {
        Anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        HpBar.value = curHP / MaxHP;
    }
    void Update()
    {
        if (PV.IsMine)
        {
            InputMove();
            PV.RPC("Jump", RpcTarget.AllBuffered);
            //Jump();
            Shot();
            LookMouseCursor();
            MinusHP();
        }
        else if ((transform.position - curPos).sqrMagnitude >= 100) transform.position = curPos;
        else transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * 10);
    }
    void InputMove()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        jDown = Input.GetButton("Jump");
        isRun = Input.GetButton("Run");

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        transform.position += moveVec * moveSp * Time.deltaTime;

        //transform.LookAt(transform.position + moveVec);

        Anim.SetBool("walk", moveVec != Vector3.zero);
        Anim.SetBool("run", isRun && moveVec != Vector3.zero);
        if (isRun) moveSp = 10;
        else moveSp = 5;
    }
    public void LookMouseCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitResult;
        if (Physics.Raycast(ray, out hitResult))
        {
            Vector3 mouseDir = new Vector3(hitResult.point.x, transform.position.y, hitResult.point.z) - transform.position;
            transform.forward = mouseDir;
        }
    }

    [PunRPC]
    void Jump()
    {
        if (jDown && !isJump)
        {
            rb.AddForce(Vector3.up * 3, ForceMode.Impulse);
            isJump = true;

            Anim.SetBool("move", isJump);
        }
    }
    void Shot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Anim.SetTrigger("fire");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Anim.SetTrigger("reload");
        }
    }
    void MinusHP()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            curHP -= 1;
        }
        HpBar.value = Mathf.Lerp(HpBar.value,curHP/MaxHP,Time.deltaTime*10);
    }
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Floor")
        {
            isJump = false;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(HpBar.value);
        }
        else
        {
            curPos = (Vector3)stream.ReceiveNext();
            HpBar.value = (float)stream.ReceiveNext();
        }
    }
}