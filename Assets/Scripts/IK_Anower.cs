using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK_Anower : MonoBehaviour
{
    [SerializeField] private float offsetY;
    [SerializeField] private float lookIKWeight;
    [SerializeField] private float headIKWeight;
    [SerializeField] private float bodyWeight;
    [SerializeField] private float clampWeight;
    [SerializeField] private Transform target;

    private Animator animator;
    private Vector3 leftFootPos;
    private Vector3 rightFootPos;
    private Quaternion leftFootRot;
    private Quaternion rightFootRot;
    private float leftFootWeight;
    private float rightFootWeight;
    private float speedOfAnim = 30f;

    private Transform leftFoot;
    private Transform rightFoot;

    private float startTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        leftFoot = animator.GetBoneTransform(HumanBodyBones.LeftFoot);
        rightFoot = animator.GetBoneTransform(HumanBodyBones.RightFoot);
        leftFootRot = leftFoot.rotation;
        rightFootRot = rightFoot.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        startTime += Time.deltaTime;
        
        RaycastHit leftHit;
        Vector3 lpos = leftFoot.position;   
        if(Physics.Raycast(lpos + Vector3.up * 0.9f, Vector3.down, out leftHit, 3))
        {
            leftFootPos = Vector3.Lerp(lpos, leftHit.point + Vector3.up * offsetY, Time.deltaTime * speedOfAnim);
            leftFootRot = Quaternion.FromToRotation(transform.up, leftHit.normal);
        }

        RaycastHit rightHit;
        Vector3 rpos = rightFoot.position;
        if (Physics.Raycast(rpos + Vector3.up * 0.9f, Vector3.down, out rightHit, 3))
        {
            rightFootPos = Vector3.Lerp(rpos, rightHit.point + Vector3.up * offsetY, Time.deltaTime * speedOfAnim);
            rightFootRot = Quaternion.FromToRotation(transform.up, rightHit.normal);
        }

        

    }

    private void OnAnimatorIK()
    {
        animator.SetLookAtWeight(lookIKWeight, bodyWeight, headIKWeight, 1f, clampWeight);
        animator.SetLookAtPosition(target.position);

        if (startTime > 2f)
        {
            

            leftFootWeight = animator.GetFloat("Leg_L");

            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, leftFootWeight);
            animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootPos);

            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, leftFootWeight);
            animator.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootRot);

            rightFootWeight = animator.GetFloat("Leg_R");

            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, rightFootWeight);
            animator.SetIKPosition(AvatarIKGoal.RightFoot, rightFootPos);

            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, rightFootWeight);
            animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootRot);
        }
        


    }
}
