using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityStandardAssets.CrossPlatformInput
{
    public class LeftJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
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
        public float sensitivity = 0.5f;
        public float maxX = 1.5f, maxY = 1.5f, maxZ = 1.5f;
        public Transform player;
        public Image img;
        private Vector2 newPos = Vector2.zero;


        Vector3 m_StartPos;
        bool m_UseX; // Toggle for using the x axis
        bool m_UseY; // Toggle for using the Y axis
        CrossPlatformInputManager.VirtualAxis m_HorizontalVirtualAxis; // Reference to the joystick in the cross platform input
        CrossPlatformInputManager.VirtualAxis m_VerticalVirtualAxis; // Reference to the joystick in the cross platform input

        void OnEnable()
        {
            //CreateVirtualAxes();
        }

        void Start()
        {
            m_StartPos = transform.position;
            img = GetComponent<Image>();
        }

        void UpdateVirtualAxes(Vector3 value)
        {
            var delta = m_StartPos - value;
            delta.y = -delta.y;
            delta /= MovementRange;
            if (m_UseX)
            {
                m_HorizontalVirtualAxis.Update(-delta.x);
            }

            if (m_UseY)
            {
                m_VerticalVirtualAxis.Update(delta.y);
            }
        }

        void CreateVirtualAxes()
        {
            // set axes to use
            m_UseX = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyHorizontal);
            m_UseY = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyVertical);

            // create new axes based on axes to use
            if (m_UseX)
            {
                if (CrossPlatformInputManager.AxisExists(horizontalAxisName))
                {
                    CrossPlatformInputManager.UnRegisterVirtualAxis(horizontalAxisName);
                }
                m_HorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(horizontalAxisName);
                CrossPlatformInputManager.RegisterVirtualAxis(m_HorizontalVirtualAxis);
            }
            if (m_UseY)
            {
                if (CrossPlatformInputManager.AxisExists(verticalAxisName))
                {
                    CrossPlatformInputManager.UnRegisterVirtualAxis(verticalAxisName);
                }
                m_VerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(verticalAxisName);
                CrossPlatformInputManager.RegisterVirtualAxis(m_VerticalVirtualAxis);
            }
        }


        public void OnDrag(PointerEventData data)
        {
            isClicked = true;
            

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(img.rectTransform,data.position,data.pressEventCamera,out newPos))
            {
                newPos.x = (newPos.x / img.rectTransform.sizeDelta.x)* MovementRange;
                newPos.y = (newPos.y / img.rectTransform.sizeDelta.y)* MovementRange;

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
            //UpdateVirtualAxes(m_StartPos);
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            isClicked = false;
        }

        public void OnPointerDown(PointerEventData data)
        {
           
        }

        public void Update()
        {

            //UpdateVirtualAxes(transform.position);

            Vector3 newPosition = player.transform.position;

            newPosition += (newPos.x * player.right + newPos.y*player.forward) * sensitivity * Time.deltaTime;
            newPosition.x = Mathf.Clamp(newPosition.x, -maxX, maxX);
            newPosition.z = Mathf.Clamp(newPosition.z, -maxZ, maxZ);

            player.transform.position = newPosition;

        }


        //void OnDisable()
        //{
        //    // remove the joysticks from the cross platform input
        //    if (m_UseX)
        //    {
        //        m_HorizontalVirtualAxis.Remove();
        //    }
        //    if (m_UseY)
        //    {
        //        m_VerticalVirtualAxis.Remove();
        //    }
        //}
    }
}