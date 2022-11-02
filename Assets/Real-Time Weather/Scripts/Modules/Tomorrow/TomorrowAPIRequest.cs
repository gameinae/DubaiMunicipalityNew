//
// Copyright(c) 2021 Real-Time Weather ASSIST-Software
// https://www.assist.ro
//
// For information about the licensing and copyright of this software please
// contact assist contact@assist.ro
//

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace RealTimeWeather.Tomorrow
{
    /// <summary>
    /// <para>
    /// This class handle the HTTP communication with web server.
    /// </para>
    /// </summary>
    public class TomorrowAPIRequest
    {
        #region Public Delegates
        public delegate void OnSendResponse(string requestedData);
        public delegate void OnErrorRaised(string errorMessage);

        public OnSendResponse onSendResponse;
        public OnErrorRaised onErrorRaised;
        #endregion

        #region Public Methods
        /// <summary>
        /// Create a UnityWebRequest for HTTP GET.
        /// </summary>
        /// <param name="uri">The URI of the resource to retrieve via HTTP GET.</param>
        public IEnumerator GetRequest(string uri)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError || webRequest.isHttpError)
                {
                    onErrorRaised?.Invoke(webRequest.error);
                }
                else
                {
                    onSendResponse?.Invoke(webRequest.downloadHandler.text);
                }
            }
        }
        #endregion
    }
}