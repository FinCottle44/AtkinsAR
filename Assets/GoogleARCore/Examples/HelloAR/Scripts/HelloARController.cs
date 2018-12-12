//-----------------------------------------------------------------------
// <copyright file="HelloARController.cs" company="Google">
//
// Copyright 2017 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

using System.Configuration.Assemblies;
using System.Linq.Expressions;
using TMPro;
using UnityEngine.UI;

namespace GoogleARCore.Examples.HelloAR
{
    using System.Collections.Generic;
    using GoogleARCore;
    using GoogleARCore.Examples.Common;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;
    using TMPro;

#if UNITY_EDITOR
    // Set up touch input propagation while using Instant Preview in the editor.
    using Input = InstantPreviewInput;
#endif

    /// <summary>
    /// Controls the HelloAR example.
    /// </summary>
    public class HelloARController : MonoBehaviour
    {
        /// <summary>
        /// The first-person camera being used to render the passthrough camera image (i.e. AR background).
        /// </summary>
        public Camera FirstPersonCamera;

        /// <summary>
        /// A prefab for tracking and visualizing detected planes.
        /// </summary>
        public GameObject DetectedPlanePrefab;

        /// <summary>
        /// A model to place when a raycast from a user touch hits a plane.
        /// </summary>
        public GameObject AndyPlanePrefab;

        /// <summary>
        /// A model to place when a raycast from a user touch hits a feature point.
        /// </summary>
        public GameObject AndyPointPrefab;

        /// <summary>
        /// A gameobject parenting UI for displaying the "searching for planes" snackbar.
        /// </summary>
        public GameObject SearchingForPlaneUI;

        /// <summary>
        /// Fin's Public vars
        /// </summary>
        public GameObject Ring;
        public GameObject Cam;
        public GameObject DebugSnack;
        public GameObject OriginMarker;
        public GameObject marker;
        public GameObject cone;
        public Transform Origin;
        public DisplacementDisplay displacement;
        public List<GameObject> rings;
        public TMP_Dropdown ddObject;
        public int SelectionValue = 0;
        

        /// <summary>
        /// The rotation in degrees need to apply to model when the Andy model is placed.
        /// </summary>
        private const float k_ModelRotation = 180.0f;

        /// <summary>
        /// A list to hold all planes ARCore is tracking in the current frame. This object is used across
        /// the application to avoid per-frame allocations.
        /// </summary>
        private List<DetectedPlane> m_AllPlanes = new List<DetectedPlane>();

        /// <summary>
        /// True if the app is in the process of quitting due to an ARCore connection error, otherwise false.
        /// </summary>
        private bool m_IsQuitting = false;

        /// <summary>
        /// The Unity Update() method.
        /// </summary>
        public void Update()
        {

            _UpdateApplicationLifecycle();
            
            // Hide snackbar when currently tracking at least one plane.
            Session.GetTrackables<DetectedPlane>(m_AllPlanes);
            bool showSearchingUI = true;
            for (int i = 0; i < m_AllPlanes.Count; i++)
            {
                if (m_AllPlanes[i].TrackingState == TrackingState.Tracking)
                {
                    showSearchingUI = false;
                    break;
                }
            }

            SearchingForPlaneUI.SetActive(showSearchingUI);
            
            
            DebugPos();
            // If the player has not touched the screen, we are done with this update.
            Touch touch;
            if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
            {
                return;
            }

            //If user touched UI then do not register touch on plane underneath
            if (EventSystem.current.IsPointerOverGameObject() || EventSystem.current.currentSelectedGameObject != null)
            {
                return;
            }
            
            // Raycast against the location the player touched to search for planes.
            TrackableHit hit;
            TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
                TrackableHitFlags.FeaturePointWithSurfaceNormal;

            if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
            {
                // Use hit pose and camera pose to check if hittest is from the
                // back of the plane, if it is, no need to create the anchor.
                if ((hit.Trackable is DetectedPlane) &&
                    Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position,
                        hit.Pose.rotation * Vector3.up) < 0)
                {
                    Debug.Log("Hit at back of the current DetectedPlane");
                }
                else
                {
                    // Choose the Andy model for the Trackable that got hit.
                    GameObject prefab;
                    if (hit.Trackable is FeaturePoint)
                    {
                        prefab = AndyPointPrefab;
                    }
                    else
                    {
                        prefab = AndyPlanePrefab;
                    }

                    // Instantiate Andy model at the hit pose.
                    
                    // Create an anchor to allow ARCore to track the hitpoint as understanding of the physical
                    // world evolves.
                    var anchor = hit.Trackable.CreateAnchor(hit.Pose);
                    
                    
                    if (SelectionValue == 0)
                    {
                        var measureRing = Instantiate(Ring, hit.Pose.position, hit.Pose.rotation);
                        measureRing.transform.Rotate(0, k_ModelRotation, 0, Space.Self);
                        measureRing.SetActive(true);
                        measureRing.tag = "point";
                        rings.Add(measureRing);
                        measureRing.transform.parent = anchor.transform;
                        
                        var andyObject = Instantiate(prefab, hit.Pose.position, hit.Pose.rotation);
                        // Compensate for the hitPose rotation facing away from the raycast (i.e. camera).
                        andyObject.transform.Rotate(0, k_ModelRotation, 0, Space.Self);
                        //measureRing.transform.localScale = new Vector3(0.2f, 0, 0.2f);
                        andyObject.tag = "point";
                        // Make Andy model a child of the anchor.
                        andyObject.transform.parent = anchor.transform;
                        //DebugSnack.GetComponent<Text>().text = "DISPLAY SUPPOSEDLY CALLED";
                        displacement.Display(andyObject);
                    }
                    else
                    {
                        Vector3 ConeCorrection = new Vector3(0f, 0.25f, 0f);
                        var andyObject = Instantiate(prefab, hit.Pose.position + ConeCorrection, hit.Pose.rotation);
                        // Compensate for the hitPose rotation facing away from the raycast (i.e. camera).
                        andyObject.transform.Rotate(0, k_ModelRotation, 0, Space.Self);
                        //measureRing.transform.localScale = new Vector3(0.2f, 0, 0.2f);
                        andyObject.tag = "point";
                        // Make Andy model a child of the anchor.
                        andyObject.transform.parent = anchor.transform;
                        //DebugSnack.GetComponent<Text>().text = "DISPLAY SUPPOSEDLY CALLED";
                        displacement.Display(andyObject);
                    }
                }
            }

            foreach (GameObject ring in rings)
            {
                RingLookAt(ring.transform, Cam.transform);
            }

            GetSelectedPrefab();
        }

        public void GetSelectedPrefab()
        {
            int Selection = SelectionValue;
            if (Selection == 0)
            {
                AndyPlanePrefab = marker;
            }
            else if (Selection == 1)
            {
                AndyPlanePrefab = cone;
            }
        }
        public void SelectCone()
        {
            SelectionValue = 1;
        }
        public void SelectRing()
        {
            SelectionValue = 0;
        }

        /// <summary>
        /// Check and update the application lifecycle.
        /// </summary>
        private void _UpdateApplicationLifecycle()
        {
            // Exit the app when the 'back' button is pressed.
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            // Only allow the screen to sleep when not tracking.
            if (Session.Status != SessionStatus.Tracking)
            {
                const int lostTrackingSleepTimeout = 15;
                Screen.sleepTimeout = lostTrackingSleepTimeout;
            }
            else
            {
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
            }

            if (m_IsQuitting)
            {
                return;
            }

            // Quit if ARCore was unable to connect and give Unity some time for the toast to appear.
            if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
            {
                _ShowAndroidToastMessage("Camera permission is needed to run this application.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
            else if (Session.Status.IsError())
            {
                _ShowAndroidToastMessage("ARCore encountered a problem connecting.  Please start the app again.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
        }

        /// <summary>
        /// Actually quit the application.
        /// </summary>
        private void _DoQuit()
        {
            Application.Quit();
        }

        /// <summary>
        /// Show an Android toast message.
        /// </summary>
        /// <param name="message">Message string to show in the toast.</param>
        private void _ShowAndroidToastMessage(string message)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            if (unityActivity != null)
            {
                AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity,
                        message, 0);
                    toastObject.Call("show");
                }));
            }
        }

        public void Clear()
        {
            GameObject[] gos = GameObject.FindGameObjectsWithTag("point");
            foreach (var go in gos)
            {
                Destroy(go);
            }
            GameObject[] texts = GameObject.FindGameObjectsWithTag("pointText");
            foreach (var t in texts)
            {
                t.GetComponent<TextMeshPro>().text = "";
                t.tag = "cleared";
            }
        }

 

        public void PlaceBelowUser()

        {
            Session.GetTrackables<DetectedPlane>(m_AllPlanes);
            TrackableHit hit2;
            float Distance;
            Vector3 down = transform.TransformDirection(Vector3.down);
            if (Frame.Raycast(transform.position, (down), out hit2))
            {
                Distance = hit2.Distance;
                var newring = Instantiate(Ring, Cam.transform);
                newring.transform.position = Cam.transform.position - new Vector3(0, Distance, 0);
            }
        }  

        void DebugPos()
        {
            //OriginMarker.transform.position = Origin.position;
            
            DebugSnack.GetComponent<Text>().text = Cam.transform.position.ToString();
        }

        public void RingLookAt(Transform ring, Transform target)
        {
            var lookPos = target.position - ring.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
        }
    }    
}
