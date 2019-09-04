using UnityEngine;
using System.Collections;
using UnityEngine.Animations.Rigging;

public class player : MonoBehaviour
{
    [Range(0.0f, 1.0F)]
    public float AltLeftHandGrip;
    public GameObject LeftHandGrip;
    public GameObject LeftArm;
    private float smoothDirection;
    private float velocityDir = 0.0F;
    private float smoothSpeed;
    private float velocitySpeed = 0.0F;
    private bool rifleON;
    private bool pistolON;
    private bool gunsOff;
    float m_Timer = 0;
    private bool inLocomotion;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        gunsOff = true;
        pistolON = false;
        rifleON = false;
        anim.SetLayerWeight(3, 1f);
        anim.SetLayerWeight(4, 0f);
        smoothDirection = 0.1f;
        smoothSpeed = 0.1f;
        AltLeftHandGrip = 0;
        inLocomotion = false;
    }
    void Update()
    {
        if((anim.GetFloat("Direction")>0.1f || anim.GetFloat("Direction")<-0.1f) || (anim.GetFloat("Speed")>0.1f || anim.GetFloat("Speed")<-0.1f))
        {
            inLocomotion = true;
        }
        else
        {
            inLocomotion = false;
        }
        
        float h = Input.GetAxis("Horizontal");
        h = Mathf.SmoothDamp(anim.GetFloat("Direction"), h, ref velocityDir, smoothDirection);
        anim.SetFloat("Direction", h);

        float v = Input.GetAxis("Vertical");
        v = Mathf.SmoothDamp(anim.GetFloat("Speed"), v, ref velocitySpeed, smoothSpeed);
        anim.SetFloat("Speed", v);

        if (Input.GetButton("Fire1"))
        {
            anim.SetBool("Fire1", true);
        }
        else
        {
            anim.SetBool("Fire1", false);
        }    

        if (Input.GetButton("Fire2"))
        {
            anim.SetBool("Fire2", true);
        }
        else
        {
            anim.SetBool("Fire2", false);
        }

        if (Input.GetButtonDown("Fire3") && (m_Timer > 0.2f))
        {
            anim.SetTrigger("Reload");
            m_Timer = 0;
        }
        m_Timer += Time.deltaTime;

        if (Input.GetButtonDown("Jump") && (gunsOff == true) && (inLocomotion == false))
        {
            gunsOff = false;
            rifleON = true;
            anim.SetLayerWeight(3, 1f);
            anim.SetLayerWeight(4, 0f);
        }
        else if (Input.GetButtonDown("Jump") && (rifleON == true) && (inLocomotion == false))
        {
            rifleON = false;
            pistolON = true;
            anim.SetLayerWeight(3, 0f);
            anim.SetLayerWeight(4, 1f);
        }
        else if (Input.GetButtonDown("Jump") && (pistolON == true) && (inLocomotion == false))
        {
            pistolON = false;
            rifleON = false;
            gunsOff = true;
            anim.SetLayerWeight(3, 1f);
            anim.SetLayerWeight(4, 0f);
        }

        anim.SetBool("Rifle", rifleON);
        anim.SetBool("Pistol", pistolON);

        var gripWeight = LeftHandGrip.GetComponent<Rig>();
        gripWeight.weight = AltLeftHandGrip;
        var gripPose = LeftArm.GetComponent<MultiParentConstraint>();
        var sourceObjects = gripPose.data.sourceObjects;
        sourceObjects.SetWeight(0, 1f - AltLeftHandGrip);
        sourceObjects.SetWeight(1, AltLeftHandGrip);
        gripPose.data.sourceObjects = sourceObjects;
    }
}
