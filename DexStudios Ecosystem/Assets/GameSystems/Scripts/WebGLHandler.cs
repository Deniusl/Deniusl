using System;
using UnityEngine;

namespace GameSystems.Scripts
{
    public class WebGLHandler : IWebGLHandler
    {
        public void HandleWebGLPlatform(CustomDropdown customDropdown, Action<int> selectNetwork,
            out bool isChainSelectedInUrl)
        {
            var _networksData = AvailableNetworks.networksData;
            var getUrl = new System.Uri(Application.absoluteURL);
            Hosts.currHost = getUrl.GetLeftPart(System.UriPartial.Authority) + "/";
            Debug.Log("#debug #account Main PATH: " + Hosts.currHost);
            string siteUrl = getUrl.ToString();
            Debug.Log("#debug #account Full PATH: " + siteUrl);
            string chain = "";

            if (siteUrl.Contains("64lu4-wiaaa-aaaak-qihra-cai.icp0.io"))
                chain = "icp";
            if (siteUrl.Contains("?chain="))
                chain =
                    siteUrl.Split(new string[] { "?chain=" }, StringSplitOptions.None)[1].Split('?')[0].Split('&')[0];
            Debug.Log("#debug #account Chain: " + chain);
            if (siteUrl.Contains("?tokenId="))
                Invites.tokenId = siteUrl.Split(new string[] { "?tokenId=" }, StringSplitOptions.None)[1].Split('?')[0]
                    .Split('&')[0];
            Debug.Log("#debug #account Invited tokenId: " + Invites.tokenId);
            if (siteUrl.Contains("?bidСombination="))
                Invites.bidCombination =
                    siteUrl.Split(new string[] { "?bidCombination=" }, StringSplitOptions.None)[1].Split('?')[0]
                        .Split('&')[0].Split(',');
            if (Invites.bidCombination.Length > 1)
                Debug.Log("#debug #account Bid motoId: " + Invites.bidCombination[0] + " Bid trackId: " +
                          Invites.bidCombination[1]);

            string nearAccountString = "";
            if (siteUrl.Contains("?account_id="))
                nearAccountString =
                    siteUrl.Split(new string[] { "?account_id=" }, StringSplitOptions.None)[1].Split('?')[0]
                        .Split('&')[0];

            for (int i = 0; i < _networksData.Length; i++)
            {
                if (chain == _networksData[i].uriPathName)
                {
                    Debug.Log(_networksData[i].uriPathName);
                    customDropdown.SetSelectedItem(i);
                    customDropdown.gameObject.SetActive(false);
                    break;
                }
            }

            isChainSelectedInUrl = !string.IsNullOrWhiteSpace(chain);

            Debug.Log(
                $"#debug #switchScene #account _isChainSelectedInUrl {isChainSelectedInUrl} chain {chain} nearAccountString {nearAccountString}");

            Debug.Log($"SelectNetwork in - WebGLHandler - {customDropdown.SelectedItem.Index}");
            selectNetwork(customDropdown.SelectedItem.Index);

            if (nearAccountString.Length > 1)
            {
                customDropdown.SetSelectedItem(nearAccountString.Contains(".testnet")
                    ? Array.FindIndex(AvailableNetworks.networksData,
                        network => network.chain.Equals("near") && network.network.Equals("testnet"))
                    : Array.FindIndex(AvailableNetworks.networksData,
                        network => network.chain.Equals("near") && network.network.Equals("mainnet")));
            }
        }

    }
}

public interface IWebGLHandler
{
     void HandleWebGLPlatform(CustomDropdown customDropdown, Action<int> selectNetwork,
     out bool isChainSelectedInUrl);
}
