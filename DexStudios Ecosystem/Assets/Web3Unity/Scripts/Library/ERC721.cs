using System.Collections;
using System.Numerics;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameSystems.Scripts;
using Newtonsoft.Json;
using UnityEngine;

public class ERC721
{
    private static string abi = Abis.MotoDEXnftABI;

    public static async Task<int> BalanceOf(string _chain, string _network, string _contract, string _account, string _rpc="")
    {
        string method = "balanceOf";
        string[] obj = { _account };
        string args = JsonConvert.SerializeObject(obj);
        string response = await EVM.Call(_chain, _network, _contract, abi, method, args, _rpc);
        try 
        {
            return int.Parse(response);
        } 
        catch 
        {
            Debug.LogError(response);
            throw;
        }
    }

    public static async Task<string> OwnerOf(string _chain, string _network, string _contract, string _tokenId, string _rpc="")
    {
        string method = "ownerOf";
        string[] obj = { _tokenId };
        string args = JsonConvert.SerializeObject(obj);
        string response = await EVM.Call(_chain, _network, _contract, abi, method, args, _rpc);
        return response; 
    }
    
    public static async Task<string> OwnerOfByIndex(string _chain, string _network, string _contract, string _account, string _index, string _rpc="")
    {
        string method = "tokenOfOwnerByIndex";
#if UNITY_EDITOR
        var cashResult = MockCashResponseSystem.Instance.GetResponseFromCashe(_network, method, _contract);
        if (cashResult.exist)
        {
            return cashResult.response;
        }
#endif
        object[] obj = { _account, _index };
        string args = JsonConvert.SerializeObject(obj);
        string response = await EVM.Call(_chain, _network, _contract, abi, method, args, _rpc);
#if UNITY_EDITOR
        MockCashResponseSystem.Instance.CashValue(_network, new CashPriceData
        {
            Method = method,
            TokenId = _contract,
            Response = response
        });
#endif
        return response; 
    }

    public static async Task<List<string>> OwnerOfBatch(string _chain, string _network, string _contract, string[] _tokenIds, string _multicall = "", string _rpc="")
    {
        string method = "ownerOf";
        // build array of args
        string[][] obj = new string[_tokenIds.Length][];
        for (int i = 0; i < _tokenIds.Length; i++)
        {
            obj[i] = new string[1] { _tokenIds[i] };
        };
        string args = JsonConvert.SerializeObject(obj);
        string response = await EVM.MultiCall(_chain, _network, _contract, abi, method, args, _multicall, _rpc);
        try 
        {
            string[] responses = JsonConvert.DeserializeObject<string[]>(response);
            List<string> owners = new List<string>();
            for (int i = 0; i < responses.Length; i++)
            {
                // clean up address
                string address = "0x" + responses[i].Substring(responses[i].Length - 40);
                owners.Add(address);
            }
            return owners;
        } 
        catch 
        {
            Debug.LogError(response);
            throw;
        }  
    }

    public static async Task<string> URI(string _chain, string _network, string _contract, string _tokenId, string _rpc="")
    {
        string method = "tokenURI";

#if UNITY_EDITOR
        var cashResult = MockCashResponseSystem.Instance.GetResponseFromCashe(_network, method, _tokenId);
        if (cashResult.exist)
        {
            return cashResult.response;
        }
#endif

        object[] obj = { _tokenId };
        string args = JsonConvert.SerializeObject(obj);
  
        string response = await EVM.Call(_chain, _network, _contract, abi, method, args, _rpc);
#if UNITY_EDITOR
        MockCashResponseSystem.Instance.CashValue(_network, new CashPriceData
        {
            Method = method,
            TokenId = _tokenId,
            Response = response
        });
#endif
        return response; 
    }

}
