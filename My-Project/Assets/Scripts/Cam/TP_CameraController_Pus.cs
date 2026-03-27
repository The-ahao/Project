using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP_CameraController_Pus : MonoBehaviour
{

    ///  <summary>
    ///  射线检测逻辑说明，这不仅仅是简单的检测，
    ///  在初始化相机位置之后寻找相机点，
    ///  直到找到可以看见角色的点
    ///  游戏玩家可以控制相机随意旋转
    ///  </summary>

    
        public Transform player;     //角色位置信息
        Vector3[] v3;         //用于远程寻找位置的
        public int num;              //射线检测时的个数
        public Vector3 start;        //相机开始时的位置
        public Vector3 end;          //相机结束时的位置
        Vector3 tagetPostion;        //相机最终的目标
        Vector3 ve3;                 //平滑移动的ref参数
        Quaternion angel;            //相机目标旋转值
        public float speed;          //相机移动速度
        void Start()
        {
            //给数组赋值数组长度
            v3 = new Vector3[num];
        }
        void LateUpdate()
        {
            //记录相机初始位置
            start = player.position + player.up * 2.0f - player.forward * 3.0f;
            //记录相机结束位置
            end = player.position + player.up * 5.0f;
            //处理鼠标控制相机旋转
            if (Input.GetMouseButton(1))
            {
                //记录相机的初始位置和旋转角度
                Vector3 pos = transform.position;
                Vector3 rot = transform.eulerAngles;
                //根据鼠标输入左右旋转
                transform.RotateAround(transform.position, Vector3.up, Input.GetAxis("Mouse X") * 10);
                transform.RotateAround(transform.position, Vector3.left, -Input.GetAxis("Mouse Y") * 10);
                //限制相机X轴旋转的角度
                if (transform.eulerAngles.x < -60 || transform.eulerAngles.x > 60)
                {
                    transform.position = pos;
                    transform.eulerAngles = rot;
                }
                return;
            }
            //设置目标位置，开始在初始位置
            tagetPostion = start;
            v3[0] = start;
            v3[num - 1] = end;
            //动态获取相机的候选点
            for (int i = 1; i < num; i++)
            {
                v3[i] = Vector3.Lerp(start, end, i / num);
            }
            //判断哪个点可以看见角色
            for (int i = 0; i < num; i++)
            {
                if (Function(v3[i]))
                {
                    tagetPostion = v3[i];
                    break;
                }
                if (i == num - 1)
                {
                    tagetPostion = end;
                }
            }
            //处理我们的移动和看
            transform.position = Vector3.SmoothDamp(transform.position, tagetPostion, ref ve3, 0);
            angel = Quaternion.LookRotation(player.position - tagetPostion);
            transform.rotation = Quaternion.Slerp(transform.rotation, angel, speed);
        }
        ///   <summary>
        ///   射线检测，判断是否有看到物体
        ///   </summary>
        ///   <param name=" v3 "> 射线发射的方向 </param>
        ///   <returns> 是否检测到 </returns>
        bool Function(Vector3 v3)
        {
            RaycastHit hit;
            if (Physics.Raycast(v3, player.position - v3, out hit))
            {
                if (hit.collider.tag == "Player")
                {
                    return true;
                }
            }
            return false;
        }
    }