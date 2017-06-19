// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using HoloToolkit.Unity.InputModule;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VR.WSA.Input;

namespace HoloToolkit
{
    public class GazeableColorPicker : MonoBehaviour, IFocusable, IInputClickHandler
    {
        public Renderer rendererComponent;
        public AudioSource voice;

        [System.Serializable]
        public class PickedColorCallback : UnityEvent<Color> { }

        public PickedColorCallback OnColorChanged = new PickedColorCallback();

        private bool gazing = false;
        private Texture2D texture;
        private GestureRecognizer tapGestureRecognizer;
        private void Start()
        {
            texture = rendererComponent.material.mainTexture as Texture2D;

            tapGestureRecognizer = new GestureRecognizer();
            tapGestureRecognizer.SetRecognizableGestures(GestureSettings.Tap);
            tapGestureRecognizer.TappedEvent += TapGestureRecognizer_TappedEvent;
        }
        private void OnEnable()
        {
            ActiveGestureManager.Instance.ActiveGesture = tapGestureRecognizer;
        }
        private void OnDisable()
        {
            ActiveGestureManager.Instance.ActiveGesture = null;
        }
        private void OnDestroy()
        {
            ActiveGestureManager.Instance.RemoveAll(tapGestureRecognizer);
            
            tapGestureRecognizer.TappedEvent -= TapGestureRecognizer_TappedEvent;
            
            tapGestureRecognizer.Dispose();
        }
        private void TapGestureRecognizer_TappedEvent(InteractionSourceKind source, int tapCount, Ray headRay)
        {
            UpdatePickedColor(OnColorChanged);
            //close me
            gameObject.SetActive(false);
        }
        
        void UpdatePickedColor(PickedColorCallback cb)
        {
            RaycastHit hit = GazeManager.Instance.HitInfo;
            if (hit.transform!=null&&hit.transform.gameObject != rendererComponent.gameObject) return;

            voice.Play();

            Vector2 pixelUV = hit.textureCoord;
            pixelUV.x *= texture.width;
            pixelUV.y *= texture.height;

            Color col = texture.GetPixel((int)pixelUV.x, (int)pixelUV.y);
            cb.Invoke(col);
        }

        public void OnFocusEnter()
        {
            gazing = true;
        }

        public void OnFocusExit()
        {
            gazing = false;
        }

        public void OnInputClicked(InputClickedEventData eventData)
        {
            UpdatePickedColor(OnColorChanged);
            //close me
            gameObject.SetActive(false);
        }
    }
}