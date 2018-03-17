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
        private int[,] mapData;
        int direction = 0; // 1 : left | 2 : right | 3: top | 4 : bottom
        Vector2 currentPosition = new Vector2(0,0);
        Vector2 prevPosition = new Vector2(0, 0);
        private int stoppingCount = 0;
        private Vector3 initPos;
        private Vector3 stoppingVelocity = new Vector3(0, 0, 0);
        private Quaternion initRotation;
        private Rigidbody rigidbody;

        private void Awake()
        {
            mCity = CityController.GetInstance();
            // get the car controller
            m_Car = GetComponent<CarController>();
            mapData = mCity.GetMapCityData();
            rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            initPos = this.transform.position;
            initRotation = this.transform.rotation;
        }

        public void setInitPos(Vector3 pos)
        {
            initPos = pos;
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
            checkOutMapAndRestore();
#else
            m_Car.Move(h, v, v, 0f);
#endif
        }

        private void checkOutMapAndRestore()
        {
            float posX = transform.position.x;
            float posZ = transform.position.z;

            Vector2 currentPos = mCity.GetPositionOnMap(posX, posZ);

            if (currentPos.x < 0 || currentPos.x > mCity.GetNumberOfChunkHeight() - 1 || currentPos.y < 0 || currentPos.y > mCity.GetNumberOfChunkWidth() - 1 || 
                isNotRoad(currentPos) || isStopping())
            {
                Destroy(this.gameObject);
                GameObject clone = (GameObject)Instantiate(Resources.Load("Car"));
                // Modify the clone to your heart's content
                clone.transform.position = initPos;
                clone.transform.rotation = initRotation;
            }
        }

        private bool isNotRoad(Vector2 currentPos)
        {
            if (initRotation.eulerAngles.y >-2 && initRotation.eulerAngles.y < 2 && currentPos.y < mCity.GetNumberOfChunkWidth() - 1 && mapData[(int) currentPos.x, (int) currentPos.y + 1] != 1)
            {
                return true;
            }

            if (initRotation.eulerAngles.y > 88 && initRotation.eulerAngles.y < 92 && currentPos.x < mCity.GetNumberOfChunkHeight() - 1 && mapData[(int) currentPos.x + 1, (int) currentPos.y] != 1)
            {
                return true;
            }

            if (initRotation.eulerAngles.y > -92 && initRotation.eulerAngles.y < -88  && currentPos.x > 1 && mapData[(int)currentPos.x - 1, (int)currentPos.y] != 1)
            {
                return true;
            }

            return false;
        }

        private bool isStopping()
        {
            if (rigidbody.velocity.magnitude <= 0.001f)
            {
                return true;
            }

            return false;
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
    }

}
