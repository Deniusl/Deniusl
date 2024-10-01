#if UNITY_ANDROID
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameSystems.Scripts;
using NearClientUnity;
using NearClientUnity.Utilities;
using NearClientUnityTests.Utils;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class AndroidNear
{
    private static ConfirmationMenu _confirmationMenu;
    
    public static async Task<string> View(string method, string args = default)
    {
        string response = null;
        switch (method)
        {
            case "nftTokensForOwners":
                response = await NftTokensForOwners(args);
                break;
            case "tokenIdsAndOwners":
                response = await TokenIdsAndOwners();
                break;
            case "valueInMainCoin":
                response = await ValueInMainCoin(int.Parse(args));
                break;
            case "valueInMainCoinCorrect":
                response = await ValueInMainCoin(int.Parse(args), false);
                break;
            case "getPriceForType":
                response = await GetPriceForType(int.Parse(args));
                break;
            case "getHealthForId":
                response = await GetHealthForId(args);
                break;
            case "getPercentForTrack":
                response = await GetPercentForTrack(args);
                break;
            case "getGameSessions":
                response = await GetGameSessions();
                break;
            case "getAllGameBids":
                response = await GetAllGameBids();
                break;
            case "getMinimalFee":
                response = await GetMinimalFee();
                break;
            case "getMinimalFeeRate":
                response = await GetMinimalFeeRate();
                break;
            case "latestEpochUpdate":
                response = await LatestEpochUpdate();
                break;
            case "getLatestPrice":
                response = await GetLatestPrice();
                break;
            case "syncEpochResultsBidsFinal":
                response = await SyncEpochResultsBidsFinal();
                break;
            case "syncEpochResultsMotosFinal":
                response = await SyncEpochResultsMotosFinal();
                break;
            default:
                Debug.Log("View method not set");
                break;
        }
        return response;
    }

    public static async Task<string> Change(string method, string args = default, string value = default, ConfirmationMenu confirmationMenu = null)
    {
        string response = null;
        _confirmationMenu = confirmationMenu;
        switch (method)
        {
            case "purchase":
                response = await Purchase(int.Parse(args), value);
                break;
            case "addMoto":
                response = await Add_NFT(args, value, true);
                break;
            case "addTrack":
                response = await Add_NFT(args, value, false);
                break;
            case "returnMoto":
                response = await Return_NFT(args, value, true);
                break;
            case "returnTrack":
                response = await Return_NFT(args, value, false);
                break;
            case "addHealthNFT":
                string[] addHealrhNftArgs = JsonConvert.DeserializeObject<string[]>(args);
                response = await AddHealthNFT(addHealrhNftArgs[0],  addHealrhNftArgs[1], value);
                break;
            case "addHealthMoney":
                response = await AddHealthMoney(args, value);
                break;
            case "bidFor": 
                string[] bidForArgs = JsonConvert.DeserializeObject<string[]>(args);
                response = await BidFor(bidForArgs[0], bidForArgs[1], value);
                break;
            default:
                Debug.Log("Change method not set");
                break;
        }
        return response;
    }

    public static async Task<string> GetPriceForType(int typeNft)
    {
        var walletAccountId = NearPersistentManager.Instance.WalletAccount.GetAccountId();
        Account account = await NearPersistentManager.Instance.Near.AccountAsync(walletAccountId);
        string contractId = AvailableNetworks.CurrentNetworkData.contracts[0];
        ContractOptions contractOptions = new ContractOptions()
        {
            sender = walletAccountId,
            viewMethods = new [] {"get_price_for_type"}
        };
        ContractNear _contractNear = new ContractNear(account, contractId, contractOptions);
        
        var args = new Dictionary<string, object>
        {
            { "type_nft", typeNft }
        };

        var response = JObject.FromObject(await _contractNear.View("get_price_for_type", args));
        string responseString = response["result"].ToString();
        return responseString;
    }
    public static async Task<string> GetMinimalFee()
    {
        var walletAccountId = NearPersistentManager.Instance.WalletAccount.GetAccountId();
        Account account = await NearPersistentManager.Instance.Near.AccountAsync(walletAccountId);
        string contractId = AvailableNetworks.CurrentNetworkData.contracts[0];
        ContractOptions contractOptions = new ContractOptions()
        {
            sender = walletAccountId,
            viewMethods = new [] {"get_minimal_fee"}
        };
        ContractNear _contractNear = new ContractNear(account, contractId, contractOptions);
        
        
        var response = JObject.FromObject(await _contractNear.View("get_minimal_fee", null));
        string responseString = response["result"].ToString();
        return responseString;
    }

    public static async Task<string> GetMinimalFeeRate()
    {
        var walletAccountId = NearPersistentManager.Instance.WalletAccount.GetAccountId();
        Account account = await NearPersistentManager.Instance.Near.AccountAsync(walletAccountId);
        string contractId = AvailableNetworks.CurrentNetworkData.contracts[0];
        ContractOptions contractOptions = new ContractOptions()
        {
            sender = walletAccountId,
            viewMethods = new [] {"get_minimal_fee_rate"}
        };
        ContractNear _contractNear = new ContractNear(account, contractId, contractOptions);
        
        
        var response = JObject.FromObject(await _contractNear.View("get_minimal_fee_rate", null));
        string responseString = response["result"].ToString();
        return responseString;
    }
    
    public static async Task<string> GetLatestPrice() 
    {
        var walletAccountId = NearPersistentManager.Instance.WalletAccount.GetAccountId();
        Account account = await NearPersistentManager.Instance.Near.AccountAsync(walletAccountId);
        string contractId = AvailableNetworks.CurrentNetworkData.contracts[0];
        ContractOptions contractOptions = new ContractOptions()
        {
            sender = walletAccountId,
            viewMethods = new [] {"get_latest_price"}
        };
        ContractNear _contractNear = new ContractNear(account, contractId, contractOptions);

        string responseString;
        try
        {
            var response = JObject.FromObject(await _contractNear.View("get_latest_price", null));
            responseString = response["result"].ToString();
            responseString = "{\"0\":\"" + responseString + "\",\"1\":\"1\"}";

        }
        catch (Exception e)
        {
            Debug.Log(e);
            responseString = "[]";
        }
        return responseString;
    }
    
    public static async Task<string> SyncEpochResultsBidsFinal() 
    {
        var walletAccountId = NearPersistentManager.Instance.WalletAccount.GetAccountId();
        Account account = await NearPersistentManager.Instance.Near.AccountAsync(walletAccountId);
        string contractId = AvailableNetworks.CurrentNetworkData.contracts[0];
        ContractOptions contractOptions = new ContractOptions()
        {
            sender = walletAccountId,
            viewMethods = new [] {"sync_epoch_results_bids_final"}
        };
        ContractNear _contractNear = new ContractNear(account, contractId, contractOptions);

        string responseString = "[]";
        try
        {
            var response = JObject.FromObject(await _contractNear.View("sync_epoch_results_bids_final", null));
            responseString = response["result"].ToString();

        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        return responseString;
    }
    
    public static async Task<string> SyncEpochResultsMotosFinal() 
    {
        var walletAccountId = NearPersistentManager.Instance.WalletAccount.GetAccountId();
        Account account = await NearPersistentManager.Instance.Near.AccountAsync(walletAccountId);
        string contractId = AvailableNetworks.CurrentNetworkData.contracts[0];
        ContractOptions contractOptions = new ContractOptions()
        {
            sender = walletAccountId,
            viewMethods = new [] {"sync_epoch_results_motos_final"}
        };
        ContractNear _contractNear = new ContractNear(account, contractId, contractOptions);

        string responseString;
        try
        {
            var response = JObject.FromObject(await _contractNear.View("sync_epoch_results_motos_final", null));
            responseString = response["result"].ToString();

        }
        catch (Exception e)
        {
            Debug.Log(e);
            responseString = "[]";
        }
        return responseString;
    }
    
    public static async Task<string> LatestEpochUpdate() 
    {
        var walletAccountId = NearPersistentManager.Instance.WalletAccount.GetAccountId();
        Account account = await NearPersistentManager.Instance.Near.AccountAsync(walletAccountId);
        string contractId = AvailableNetworks.CurrentNetworkData.contracts[0];
        ContractOptions contractOptions = new ContractOptions()
        {
            sender = walletAccountId,
            viewMethods = new [] {"get_latest_epoch_update"}
        };
        ContractNear _contractNear = new ContractNear(account, contractId, contractOptions);
        
        
        var response = JObject.FromObject(await _contractNear.View("get_latest_epoch_update", null));
        string responseString = response["result"].ToString();
        return responseString;
    }
    
    public static async Task<string> ValueInMainCoin(int typeNft, bool multiply = true)
    {
        var walletAccountId = NearPersistentManager.Instance.WalletAccount.GetAccountId();
        Account account = await NearPersistentManager.Instance.Near.AccountAsync(walletAccountId);
        string contractId = AvailableNetworks.CurrentNetworkData.contracts[0];
        ContractOptions contractOptions = new ContractOptions()
        {
            sender = walletAccountId,
            viewMethods = new [] {"value_in_main_coin"}
        };
        ContractNear _contractNear = new ContractNear(account, contractId, contractOptions);
        
        
        var args = new Dictionary<string, object>
        {
            { "type_nft", typeNft }
        };
        
        var response = JObject.FromObject(await _contractNear.View("value_in_main_coin", args));
        string responseString = multiply
            ? (float.Parse(response["result"].ToString()) * 1.02f).ToString("0")
            : response["result"].ToString();
        return responseString;
    }
    
    public static async Task<string> GetHealthForId(string tokenId)
    {
        var walletAccountId = NearPersistentManager.Instance.WalletAccount.GetAccountId();
        Account account = await NearPersistentManager.Instance.Near.AccountAsync(walletAccountId);
        string contractId = AvailableNetworks.CurrentNetworkData.contracts[0];
        ContractOptions contractOptions = new ContractOptions()
        {
            sender = walletAccountId,
            viewMethods = new [] {"get_token_health"}
        };
        ContractNear _contractNear = new ContractNear(account, contractId, contractOptions);
        
        var args = new Dictionary<string, object>
        {
            { "token_id", tokenId }
        };
        
        var response = JObject.FromObject(await _contractNear.View("get_token_health", args));
        string responseString = response["result"].ToString();
        return responseString;
    }
    
    public static async Task<string> GetPercentForTrack(string tokenId)
    {
        var walletAccountId = NearPersistentManager.Instance.WalletAccount.GetAccountId();
        Account account = await NearPersistentManager.Instance.Near.AccountAsync(walletAccountId);
        string contractId = AvailableNetworks.CurrentNetworkData.contracts[0];
        ContractOptions contractOptions = new ContractOptions()
        {
            sender = walletAccountId,
            viewMethods = new [] {"get_percent_for_track"}
        };
        ContractNear _contractNear = new ContractNear(account, contractId, contractOptions);
        
        var args = new Dictionary<string, object>
        {
            { "token_id", tokenId }
        };
        
        var response = JObject.FromObject(await _contractNear.View("get_percent_for_track", args));
        string responseString = response["result"].ToString();
        return responseString;
    }    

    public static async Task<string> GetTokenTypeNft(string tokenId)
    {
        var walletAccountId = NearPersistentManager.Instance.WalletAccount.GetAccountId();
        Account account = await NearPersistentManager.Instance.Near.AccountAsync(walletAccountId);
        string contractId = AvailableNetworks.CurrentNetworkData.contracts[0];
        ContractOptions contractOptions = new ContractOptions()
        {
            sender = walletAccountId,
            viewMethods = new [] {"get_token_type_nft"}
        };
        ContractNear _contractNear = new ContractNear(account, contractId, contractOptions);
        
        
        var args = new Dictionary<string, object>
        {
            { "token_id", tokenId }
        };
        
        var response = JObject.FromObject(await _contractNear.View("get_token_type_nft", args));
        string responseString = response["result"].ToString();
        return responseString;
    }
    
    public static async Task<string> GetGameSessions()
    {
        var walletAccountId = NearPersistentManager.Instance.WalletAccount.GetAccountId();
        Account account = await NearPersistentManager.Instance.Near.AccountAsync(walletAccountId);
        string contractId = AvailableNetworks.CurrentNetworkData.contracts[0];
        ContractOptions contractOptions = new ContractOptions()
        {
            sender = walletAccountId,
            viewMethods = new [] {"get_active_sessions_full_view"}
        };
        ContractNear _contractNear = new ContractNear(account, contractId, contractOptions);
        
        var response = JObject.FromObject(await _contractNear.View("get_active_sessions_full_view", null));
        string responseString = response["result"].ToString();
        return responseString;
    }

    public static async Task<string> GetAllGameBids()
    {
        var walletAccountId = NearPersistentManager.Instance.WalletAccount.GetAccountId();
        Account account = await NearPersistentManager.Instance.Near.AccountAsync(walletAccountId);
        string contractId = AvailableNetworks.CurrentNetworkData.contracts[0];
        ContractOptions contractOptions = new ContractOptions()
        {
            sender = walletAccountId,
            viewMethods = new[] {"get_game_bids_paged"}
        };
        ContractNear _contractNear = new ContractNear(account, contractId, contractOptions);

        var response = JObject.FromObject(await _contractNear.View("get_game_bids_paged", null));
        string responseString = response["result"].ToString();
        return responseString;
    }
    public static async Task<string> NftTokensForOwners(string accountId)
    {
        var walletAccountId = NearPersistentManager.Instance.WalletAccount.GetAccountId();
        Account account = await NearPersistentManager.Instance.Near.AccountAsync(walletAccountId);
        string contractId = AvailableNetworks.CurrentNetworkData.contracts[0];
        ContractOptions contractOptions = new ContractOptions()
        {
            sender = walletAccountId,
            viewMethods = new [] {"nft_tokens_for_owners"}
        };
        ContractNear _contractNear = new ContractNear(account, contractId, contractOptions);
        
        var args = new Dictionary<string, object>
        {
            { "account_id", accountId }
        };
        
        var response = JObject.FromObject(await _contractNear.View("nft_tokens_for_owners", args));
        string responseString = response["result"].ToString();
        return responseString;
    }
    
    public static async Task<string> TokenIdsAndOwners()
    {
        var walletAccountId = NearPersistentManager.Instance.WalletAccount.GetAccountId();
        Account account = await NearPersistentManager.Instance.Near.AccountAsync(walletAccountId);
        string contractId = AvailableNetworks.CurrentNetworkData.contracts[0];
        ContractOptions contractOptions = new ContractOptions()
        {
            sender = walletAccountId,
            viewMethods = new [] {"token_ids_and_owners"}
        };
        ContractNear _contractNear = new ContractNear(account, contractId, contractOptions);
        
        
        var response = JObject.FromObject(await _contractNear.View("token_ids_and_owners", null));
        string responseString = response["result"].ToString();
        return responseString;
    }
    
    public static async Task<string> Purchase(int typeNft, string value)
    {
        var walletAccountId = NearPersistentManager.Instance.WalletAccount.GetAccountId();
        Account account = await NearPersistentManager.Instance.Near.AccountAsync(walletAccountId);
        string contractId = AvailableNetworks.CurrentNetworkData.contracts[0];
        ContractOptions contractOptions = new ContractOptions()
        {
            changeMethods = new [] {"purchase"},
            sender = walletAccountId
        };
        ContractNear _contractNear = new ContractNear(account, contractId, contractOptions);

        var args = new Dictionary<string, object>
        {
            { "type_nft", typeNft }
        };
        
        UInt128 valueInMainCoinResult;
        if (value != default)
        {
            valueInMainCoinResult = UInt128.Parse(value);
        }
        else
        {
            if (value == default)
            {
                string valueInMainCoinString =  await ValueInMainCoin(int.Parse(typeNft.ToString()));
                valueInMainCoinResult = UInt128.Parse(valueInMainCoinString);
            }
            else
            {
                valueInMainCoinResult = UInt128.Parse(value);
            }
            
        }

        try
        {
            bool shouldExecute = await _confirmationMenu.ShowConfirmationMenu((double)valueInMainCoinResult);
            if (!shouldExecute)
            {
                Debug.Log("Declined");
                return "fail";
            }
            
            JObject buyResponse = JObject.FromObject(await _contractNear.Change("purchase", args, 300000000000000, valueInMainCoinResult));
            string[] response = new string[2];
            var buyResponseString = buyResponse.ToString();
            if (buyResponse.TryGetValue("status", StringComparison.OrdinalIgnoreCase, out JToken eventTypeJsonProperty)/* ContainsKey("status")*/ && buyResponse["status"].ToString().Contains("SuccessValue"))
            {
                response[0] = "success";
                if (buyResponseString.Contains("status") && buyResponseString.Contains("EVENT_JSON") &&
                    buyResponseString.Contains("token_ids"))
                {
                    var eventJson = JObject.Parse("{" + buyResponse["receipts_outcome"][0]["outcome"]["logs"][0] + "}");
                    response[1] = eventJson["EVENT_JSON"]["data"][0]["token_ids"][0].ToString();
                }

            }
            else
            {
                response[0] = "fail";
            }

            return response[0] == "fail" ? response[0] : string.Join(",", response);
        }catch (Exception e) {
            Debug.LogException(e);
            return "fail";
        }
    }
    
    public static async Task<string> Add_NFT(string tokenId, string value, bool addMoto)
    {
        var walletAccountId = NearPersistentManager.Instance.WalletAccount.GetAccountId();
        Account account = await NearPersistentManager.Instance.Near.AccountAsync(walletAccountId);
        string contractId = AvailableNetworks.CurrentNetworkData.contracts[0];
        ContractOptions contractOptions = new ContractOptions()
        {
            changeMethods = new [] {"add_moto", "add_track"},
            sender = walletAccountId
        };
        ContractNear _contractNear = new ContractNear(account, contractId, contractOptions);

        var args = new Dictionary<string, object>
        {
            { "token_id", tokenId }
        };
        
        UInt128 getMinimalFeeResult;
        if (value != default)
        {
            getMinimalFeeResult = UInt128.Parse(value);
        }
        else
        {
            string getMinimalFeeString =  await GetMinimalFee();
            getMinimalFeeResult = UInt128.Parse(getMinimalFeeString);
        }

        try
        {
            bool shouldExecute = await _confirmationMenu.ShowConfirmationMenu((double)getMinimalFeeResult);
            if (!shouldExecute)
            {
                Debug.Log("Declined");
                return "fail";
            }
            
            JObject buyResponse = JObject.FromObject(await _contractNear.Change(addMoto ? "add_moto" : "add_track", args, 50000000000000, getMinimalFeeResult));
            string response;
            if (buyResponse.TryGetValue("status", StringComparison.OrdinalIgnoreCase, out JToken eventTypeJsonProperty) 
                && buyResponse["status"].ToString().Contains("SuccessValue"))
            {
                response = "success";
            }
            else
            {
                response = "fail";
            }

            return response;
        }catch (Exception e) {
            Debug.LogException(e);
            return "fail";
        }
    }
    
    public static async Task<string> Return_NFT(string tokenId, string value, bool returnMoto)
    {
        var walletAccountId = NearPersistentManager.Instance.WalletAccount.GetAccountId();
        Account account = await NearPersistentManager.Instance.Near.AccountAsync(walletAccountId);
        string contractId = AvailableNetworks.CurrentNetworkData.contracts[0];
        ContractOptions contractOptions = new ContractOptions()
        {
            changeMethods = new [] {"return_moto", "return_track"},
            sender = walletAccountId
        };
        ContractNear _contractNear = new ContractNear(account, contractId, contractOptions);

        var args = new Dictionary<string, object>
        {
            { "token_id", tokenId }
        };
        
        UInt128 getMinimalFeeResult;
        if (value != default)
        {
            getMinimalFeeResult = UInt128.Parse(value);
        }
        else
        {
            getMinimalFeeResult = UInt128.One;
        }

        try
        {
            bool shouldExecute = await _confirmationMenu.ShowConfirmationMenu((double)getMinimalFeeResult);
            if (!shouldExecute)
            {
                Debug.Log("Declined");
                return "fail";
            }
            
            JObject buyResponse = JObject.FromObject(await _contractNear.Change(returnMoto ? "return_moto" : "return_track", args, 50000000000000, getMinimalFeeResult));
            string response;
            if (buyResponse.TryGetValue("status", StringComparison.OrdinalIgnoreCase, out JToken eventTypeJsonProperty)
                && buyResponse["status"].ToString().Contains("SuccessValue"))
            {
                response = "success";

            }
            else
            {
                response = "fail";
            }

            return response;
        }catch (Exception e) {
            Debug.LogException(e);
            return "fail";
        }
    }
    
    public static async Task<string> AddHealthNFT(string tokenId, string healthPillTokenId, string value)
    {
        var walletAccountId = NearPersistentManager.Instance.WalletAccount.GetAccountId();
        Account account = await NearPersistentManager.Instance.Near.AccountAsync(walletAccountId);
        string contractId = AvailableNetworks.CurrentNetworkData.contracts[0];
        ContractOptions contractOptions = new ContractOptions()
        {
            changeMethods = new [] {"add_health_nft"},
            sender = walletAccountId
        };
        ContractNear _contractNear = new ContractNear(account, contractId, contractOptions);

        var args = new Dictionary<string, object>
        {
            { "token_id", tokenId },
            { "health_pill_token_id", healthPillTokenId }
        };
        
        UInt128 getMinimalFeeResult;
        if (value != default)
        {
            getMinimalFeeResult = UInt128.Parse(value);
        }
        else
        {
            getMinimalFeeResult = UInt128.One;
        }



        try
        {
            bool shouldExecute = await _confirmationMenu.ShowConfirmationMenu((double)getMinimalFeeResult);
            if (!shouldExecute)
            {
                Debug.Log("Declined");
                return "fail";
            }
            
            JObject addHealthResponse = JObject.FromObject(await _contractNear.Change("add_health_nft", args, 50000000000000, getMinimalFeeResult));
            string response;
            if (addHealthResponse.TryGetValue("status", StringComparison.OrdinalIgnoreCase, out JToken eventTypeJsonProperty)
                && addHealthResponse["status"].ToString().Contains("SuccessValue"))
            {
                response = "success";

            }
            else
            {
                response = "fail";
            }

            return response;
        }catch (Exception e) {
            Debug.LogException(e);
            return "fail";
        }
    }
    
    public static async Task<string> AddHealthMoney(string tokenId, string value = default)
    {
        var walletAccountId = NearPersistentManager.Instance.WalletAccount.GetAccountId();
        Account account = await NearPersistentManager.Instance.Near.AccountAsync(walletAccountId);
        string contractId = AvailableNetworks.CurrentNetworkData.contracts[0];
        ContractOptions contractOptions = new ContractOptions()
        {
            changeMethods = new [] {"add_health_money"},
            sender = walletAccountId
        };
        ContractNear _contractNear = new ContractNear(account, contractId, contractOptions);

        var args = new Dictionary<string, object>
        {
            { "token_id", tokenId }
        };

        if (value == default)
            value = await ValueInMainCoin(int.Parse(await GetTokenTypeNft(tokenId)));


        try
        {
            bool shouldExecute = await _confirmationMenu.ShowConfirmationMenu(double.Parse(value));
            if (!shouldExecute)
            {
                Debug.Log("Declined");
                return "fail";
            }

            JObject addHealthResponse = JObject.FromObject(await _contractNear.Change("add_health_money", args, 50000000000000, UInt128.Parse(value)));
            string response;
            if (addHealthResponse.TryGetValue("status", StringComparison.OrdinalIgnoreCase, out JToken eventTypeJsonProperty)
                && addHealthResponse["status"].ToString().Contains("SuccessValue"))
            {
                response = "success";

            }
            else
            {
                response = "fail";
            }

            return response;
        }catch (Exception e) {
            Debug.LogException(e);
            return "fail";
        }
    }
    
    public static async Task<string> BidFor(string trackTokenId, string motoTokenId, string value)
    {
        var walletAccountId = NearPersistentManager.Instance.WalletAccount.GetAccountId();
        Account account = await NearPersistentManager.Instance.Near.AccountAsync(walletAccountId);
        string contractId = AvailableNetworks.CurrentNetworkData.contracts[0];
        ContractOptions contractOptions = new ContractOptions()
        {
            changeMethods = new [] {"bid_for"},
            sender = walletAccountId
        };
        ContractNear _contractNear = new ContractNear(account, contractId, contractOptions);

        var args = new Dictionary<string, object>
        {
            { "track_token_id", trackTokenId },
            { "moto_token_id", motoTokenId }
        };
        
        UInt128 bidValue;
        if (value != default)
        {
            bidValue = UInt128.Parse(value);
        }
        else
        {
            bidValue = UInt128.One;
        }

        try
        {
            bool shouldExecute = await _confirmationMenu.ShowConfirmationMenu((double)bidValue);
            if (!shouldExecute)
            {
                Debug.Log("Declined");
                return "fail";
            }
            
            JObject buyResponse = JObject.FromObject(await _contractNear.Change("bid_for", args, 50000000000000, bidValue));
            string response;
            if (buyResponse.TryGetValue("status", StringComparison.OrdinalIgnoreCase, out JToken eventTypeJsonProperty)
                && buyResponse["status"].ToString().Contains("SuccessValue"))
            {
                response = "success";
            }
            else
            {
                response = "fail";
            }

            return response;
        }catch (Exception e) {
            Debug.LogException(e);
            return "fail";
        }
    }
}
#endif