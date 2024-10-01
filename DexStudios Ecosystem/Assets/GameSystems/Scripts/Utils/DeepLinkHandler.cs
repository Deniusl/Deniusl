using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class DeepLinkHandler : MonoBehaviour
{
    private static bool isObjectCreated = false;
    
    private void Start()
    {
        if (!isObjectCreated)
        {
            isObjectCreated = true;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        Application.deepLinkActivated += HandleDeepLink;
    }

    private void HandleDeepLink(string deepLink)
    {
        Debug.Log("Received deep link: " + deepLink);
        
        string response = ExtractResponseFromDeepLink(deepLink);
        GUIUtility.systemCopyBuffer = response;
    }
    
    private string ExtractResponseFromDeepLink(string deepLink)
    {
        string pattern = @"response=(\w+)";
        
        Match match = Regex.Match(deepLink, pattern);
        
        if (match.Success && match.Groups.Count > 1)
        {
            string response = match.Groups[1].Value;
            
            return response;
        }
        
        return string.Empty;
    }
    
    private void OnDestroy()
    {
        Application.deepLinkActivated -= HandleDeepLink;
    }
}
