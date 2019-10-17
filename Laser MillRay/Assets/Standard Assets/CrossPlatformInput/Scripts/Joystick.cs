using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityStandardAssets.CrossPlatformInput
{
	public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
	{
        
        public static bool isClicked;
		public enum AxisOption
		{
			// Options for which axes to use
			Both, // Use both
			OnlyHorizontal, // Only horizontal
			OnlyVertical // Only vertical
		}

		public int MovementRange = 100;
		public AxisOption axesToUse = AxisOption.Both; // The options for the axes that the still will use
		public string horizontalAxisName = "Horizontal"; // The name given to the horizontal axis for the cross platform input
		public string verticalAxisName = "Vertical"; // The name given to the vertical axis for the cross platform input
        public Camera camera;
        public float maxX = 1.5f, maxY = 1.5f, maxZ = 1.5f;
        public Image img;
        public double sensitivity = 2;
        private Vector2 newPos = Vector2.zero;
        private float rotX;
        private float rotY;
        private float viewRange = 89;
        public Transform player;

        Vector3 m_StartPos;
		bool m_UseX; // Toggle for using the x axis
		bool m_UseY; // Toggle for using the Y axis
		CrossPlatformInputManager.VirtualAxis m_HorizontalVirtualAxis; // Reference to the joystick in the cross platform input
		CrossPlatformInputManager.VirtualAxis m_VerticalVirtualAxis; // Reference to the joystick in the cross platform input

		void OnEnable()
		{
		}

        void Start()
        {
            m_StartPos = transform.position;
        }


		public void OnDrag(PointerEventData data)
		{
            isClicked = true;


            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(img.rectTransform, data.position, data.pressEventCamera, out newPos))
            {
                newPos.x = (newPos.x / img.rectTransform.sizeDelta.x) * MovementRange;
                newPos.y = (newPos.y / img.rectTransform.sizeDelta.y) * MovementRange;

                float x = (img.rectTransform.pivot.x == 1) ? newPos.x * 2 + 1 : newPos.x * 2 - 1;
                float y = (img.rectTransform.pivot.y == 1) ? newPos.y * 2 + 1 : newPos.y * 2 - 1;
            }
            transform.position = Vector3.ClampMagnitude(new Vector3(newPos.x, newPos.y), MovementRange) + m_StartPos;
            newPos = newPos / MovementRange;
        }


		public void OnPointerUp(PointerEventData data)
		{
            isClicked = false;
			transform.position = m_StartPos;
            newPos = Vector2.zero;
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            isClicked = false;
        }

            public void OnPointerDown(PointerEventData data)
        {
            isClicked = true;
            transform.position = m_StartPos;
            newPos = Vector2.zero;
        }

		void OnDisable()
		{

		}
        public void Update()
        {
            rotX -= newPos.y;
            rotY += newPos.x;

            rotX = Mathf.Clamp(rotX, -viewRange, viewRange);

            camera.transform.localRotation = Quaternion.Euler(rotX, 0f, 0f);
            player.transform.rotation = Quaternion.Euler(0f, rotY, 0f);
        }
    }
}