using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using City;
using UnityEditor;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (CarController))]
    public class CarUserControl : MonoBehaviour
    {
        private CarController m_Car; // the car controller we want to use
        private CityController mCity;
        int direction = 0; // 1 : left | 2 : right | 3: top | 4 : bottom
        Vector2 currentPosition = new Vector2(0,0);
        Vector2 prevPosition = new Vector2(0, 0);
        private Vector3 initPos;
        private void Awake()
        {
            mCity = CityController.GetInstance();
            // get the car controller
            m_Car = GetComponent<CarController>();
            initPos = transform.position;
        }


        private void FixedUpdate()
        {
            Debug.Log(mCity.GetDataIndexOf(2, 2));
            // pass the input to the car!
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
#if !MOBILE_INPUT
            float handbrake = CrossPlatformInputManager.GetAxis("Jump");
            //m_Car.Move(h, v, v, handbrake);

            TrackDirection();
            m_Car.Move(0, 1, 1, 0);
#else
            m_Car.Move(h, v, v, 0f);
#endif
        }

        private void TrackDirection()
        {
            float posX = transform.position.x;
            float posZ = transform.position.z;

            currentPosition = mCity.GetPositionOnMap(posX, posZ);
            Debug.Log(currentPosition.x + "  " + currentPosition.y);
            if (currentPosition.x == prevPosition.x && currentPosition.y == prevPosition.y)
            {
                return;
            }
            if (currentPosition - prevPosition == new Vector2(0,1))
            {
                prevPosition = currentPosition;
                direction = 2;
                Debug.Log("run right");
            }
        }

        private void checkOutMapAndRestore()
        {
            float posX = transform.position.x;
            float posZ = transform.position.z;

            Vector2 currentPos = mCity.GetPositionOnMap(posX, posZ);

            if (currentPos.x < 0 || currentPos.x >= mCity.GetNumberOfChunk() - 1 || currentPos.y < 0 || currentPos.y >= mCity.GetNumberOfChunk() - 1)
            {
                Destroy(this.gameObject);
                UnityEngine.Object prefab = AssetDatabase.LoadAssetAtPath("Assets/Vehicles/Car/Prefabs/Car.prefab", typeof(GameObject));
                GameObject clone = (GameObject)Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
                // Modify the clone to your heart's content
                clone.transform.position = initPos;
            }
        }
    }

}
