//-----------------------------------------------------------------------
// <copyright file="CardboardStartup.cs" company="Google LLC">
// Copyright 2020 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//-----------------------------------------------------------------------

using Google.XR.Cardboard;
using UnityEngine;
using UnityEngine.XR.Management;

/// <summary>
/// Initializes Cardboard XR Plugin.
/// </summary>
public class CardboardStartup : MonoBehaviour
{
    private bool _isInitialized = false;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        Debug.Log("CardboardStartup Awake");

        // Initialize XR loader.
        var xrManager = XRGeneralSettings.Instance.Manager;
        if (xrManager != null)
        {
            Debug.Log("XR Manager found.");
            xrManager.InitializeLoaderSync();
            if (xrManager.activeLoader == null)
            {
                Debug.LogError("Initializing XR Loader failed.");
                return;
            }
            else
            {
                Debug.Log("XR Loader initialized successfully.");
                xrManager.StartSubsystems();
                _isInitialized = true;
            }
        }
        else
        {
            Debug.LogError("XR Manager instance is null.");
        }
    }

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    void Start()
    {
        Debug.Log("CardboardStartup Start");

        // Configures the app to not shut down the screen and sets the brightness to maximum.
        // Brightness control is expected to work only in iOS, see:
        // https://docs.unity3d.com/ScriptReference/Screen-brightness.html.
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.brightness = 1.0f;

        // Checks if the device parameters are stored and scans them if not.
        if (!Api.HasDeviceParams())
        {
            Api.ScanDeviceParams();
        }
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    void Update()
    {
        if (!_isInitialized)
        {
            Debug.LogError("Cardboard XR Loader not initialized.");
            return;
        }

        if (Api.IsGearButtonPressed)
        {
            Api.ScanDeviceParams();
        }

        if (Api.IsCloseButtonPressed)
        {
            Application.Quit();
        }

        if (Api.IsTriggerHeldPressed)
        {
            Api.Recenter();
        }

        if (Api.HasNewDeviceParams())
        {
            Api.ReloadDeviceParams();
        }

        Api.UpdateScreenParams();
    }
}
