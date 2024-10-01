using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.GameSystems.Scripts.Utils
{
    public class InvitationData
    {
        public string InviterWallet { get; set; }
        public string Network { get; set; }
        public string TokenId { get; set; }
        public bool IsOwner { get; set; }
    }

    public class ChainData
    {
        public string Chain { get; set; }
    }
    
    internal class URLValidator
    {
        public InvitationData InvitationParsedData { get; private set; }
        
        public ChainData ChainData { get; private set; }
        
        public bool Validate(string input)
        {
            Debug.Log($"URLValidator Validate invoked with string: {input}");

            try
            {
                Uri uri = new Uri(input);
                string query = uri.Query;
                
                Debug.Log($"#debug #query - {query}");

                if (string.IsNullOrEmpty(query))
                {
                    Debug.Log("URLValidator: No query parameters found.");
                    return false;
                }

                string adjLabel = GetQueryParameter(query, "?adj_label=");
                
                Debug.Log($"#debug #adjLabel - {adjLabel}");
                
                if (!string.IsNullOrEmpty(adjLabel))
                {
                    InvitationParsedData = ParseAdjLabel(adjLabel);
                    if (InvitationParsedData != null)
                    {
                        Debug.Log($"URLValidator: Successfully parsed adj_label: {adjLabel}");
                        return true;
                    }
                }
                
                string chain = GetQueryParameter(query, "?chain=");
                if (!string.IsNullOrEmpty(chain))
                {
                    ChainData = new ChainData
                    {
                        Chain = chain
                    };
                    Debug.Log($"URLValidator: Successfully parsed chain parameter: {chain}");
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Debug.LogError($"URLValidator: An error occurred while parsing the URL. {ex.Message}");
                return false;
            }
        }

        private string GetQueryParameter(string query, string parameter)
        {
            string prefix = parameter;
            if (query.StartsWith(prefix))
            {
                query = query.Substring(prefix.Length);
                
                
                Debug.Log($"#debug #trimmedQuery - {query}");

                return Uri.UnescapeDataString(query);
            }

            return string.Empty;
        }

        private InvitationData ParseAdjLabel(string adjLabel)
        {
            string[] parts = adjLabel.Split(',');
            Debug.Log($"#debug #parts");
            if (parts.Length < 4)
            {
                return null;
            }

            return new InvitationData
            {
                InviterWallet = parts[0],
                Network = parts[1],
                TokenId = parts[2],
                IsOwner = parts[3].EndsWith("=1")
            };
        }
    }
}
