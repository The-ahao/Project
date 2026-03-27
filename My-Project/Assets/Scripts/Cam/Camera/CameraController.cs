
using UnityEngine;
namespace CameraControl
{
    public class CameraController : MonoBehaviour
    {
        #region 第一步是获取组件
        private Transform cam;
        #endregion

        #region 角度旋转参数
        private float Y_Pivot;//控制y轴旋转参数
        private float X_Pivot;//控制x轴旋转参数
        #endregion

        private Vector3 currentEulerAngler;
        private Vector3 currentVelocity = Vector3.zero;
        private Vector3 targetPosition;
        [SerializeField] Transform lookAt;
        //控制参数
        [SerializeField] private Vector2 angleRange;
        [SerializeField] float distance;
        [SerializeField] private float rotationTime;
        [SerializeField] private float followSpeed;
        [SerializeField] private float X_Sensitivity;
        [SerializeField] private float Y_Sensitivity;
        private void Awake()
        {
            cam = Camera.main.transform;
        }
        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        private void Update()
        {
            UpdateCameraInput();
        }
        
        private void LateUpdate()
        {
            //通过将改变的角度旋转，每一帧执行
            CameraPosition();
            CameraRotation();
        }
        /// <summary>
        /// 旋转逻辑
        /// </summary>
        private void CameraRotation()
        {
            currentEulerAngler = Vector3.SmoothDamp(currentEulerAngler, new Vector3(X_Pivot, Y_Pivot, 0), ref currentVelocity, rotationTime);
            cam.eulerAngles = currentEulerAngler;
            //也可以旋转四元数
            //cam.rotation=Quaternion.Euler(currentEulerAngler);
        }

        /// <summary>
        /// 相机位置
        /// </summary>
        private void CameraPosition()
        {
            //计算目标位置
            targetPosition = lookAt.transform.position - cam.forward * distance;
            //lerp在系数*时间补充状态下，每一帧都一个值，以便接近目标值
            cam.position = Vector3.Lerp(cam.position, targetPosition, followSpeed * Time.deltaTime);
        }

        /// <summary>
        /// 更新输入
        /// </summary>
        private void UpdateCameraInput()
        {
            //获取鼠标移动的输入值
            //然后使用输入系统封装好的输入系统
            Y_Pivot += CharacterInputSystem.MainInstance.CameraLook.x* X_Sensitivity;
            X_Pivot -= CharacterInputSystem.MainInstance.CameraLook.y* Y_Sensitivity;
            //用Clamp限制一下
            X_Pivot = Mathf.Clamp(X_Pivot, angleRange.x, angleRange.y);
        }
    }
}
