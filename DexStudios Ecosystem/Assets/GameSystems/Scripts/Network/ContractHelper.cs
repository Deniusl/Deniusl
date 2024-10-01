using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using Cysharp.Threading.Tasks;
using DIContainerLib;
using GameSystems.Scripts;
using GameSystems.Scripts.Constants;
using GameSystems.Scripts.Services.Interfaces;
using Nethereum.ABI.FunctionEncoding;
using Newtonsoft.Json;
using UI;
using UnityEngine;
using Web3Unity.Scripts.Library.Ethers.Contracts;
using Web3Unity.Scripts.Library.Ethers.Contracts.Builders;
using Network = GameSystems.Scripts.Network;
using Types = GameSystems.Scripts.Types;

public static class ContractHelper
{
    [Serializable]
    private class ConcordiumContractsData
    {
        [JsonProperty("track_token_id")] public string trackTokenId;
        [JsonProperty("track_type")] public string trackType;
        [JsonProperty("track_health")] public string trackHealth;
        [JsonProperty("participants")] public ConcordiumMotoTokens[] concordiumMotoTokens;
        [JsonProperty("latest_update_time")] public string latestUpdateTime;

        public string attempts;

        [JsonProperty("game_bids_sum")] public string gameBidsSum;
    }

    [Serializable]
    private class ConcordiumMotoTokens
    {
        [JsonProperty("moto_token_id")] public string motoTokenId;
        [JsonProperty("moto_type")] public string motoType;
        [JsonProperty("moto_health")] public string motoHealth;
        [JsonProperty("best_time_result")] public string bestTimeResult;
    }

    [Serializable]
    private class ConcordiumEpochPaymentViewData
    {
        [JsonProperty("track_token_id")] public string trackTokenId;
        [JsonProperty("moto_token_id")] public string motoTokenId;
        public Double amount;
        public string to;
        [JsonProperty("receiver_type")] public string receiverType;
    }

    [Serializable]
    private class NearContractsData
    {
        [JsonProperty("track_token_id")] public string trackTokenId;
        [JsonProperty("track_type")] public NearTrackType trackType;
        [JsonProperty("track_health")] public string trackHealth;
        [JsonProperty("moto_tokens")] public NearMotoTokens[] nearMotoTokens;
        [JsonProperty("latest_update_time")] public string latestUpdateTime;

        public string attempts;

        [JsonProperty("game_bids_sum")] public string gameBidsSum;
    }

    [Serializable]
    private class NearGameBidsData
    {
        public string amount;
        public string track;
        public string moto;
        public string timestamp;
        public string bidder;
    }

    [Serializable]
    private class NearTrackType
    {
        [JsonProperty("Track")] public string trackTypeName;
    }

    [Serializable]
    private class NearMotoTokens
    {
        [JsonProperty("moto_token_id")] public string motoTokenId;
        [JsonProperty("moto_type")] public NearMotoType nearMotoType;
        [JsonProperty("moto_health")] public string motoHealth;
        [JsonProperty("best_time_result")] public string bestTimeResult;
    }

    [Serializable]
    private class NearMotoType
    {
        [JsonProperty("Moto")] public string motoTypeName;
    }

    [Serializable]
    private class NearEpochPaymentViewData
    {
        [JsonProperty("track_token_id")] public string trackTokenId;
        [JsonProperty("moto_token_id")] public string motoTokenId;
        public Double amount;
        public string to;
        [JsonProperty("receiver_type")] public string receiverType;
    }

    [Serializable]
    private struct RawContractsData
    {
        [JsonProperty("0")] public string[][] data;
        [JsonProperty("1")] public string count;
    }

    private const string DefaulSessions =
        "{\"0\":[[\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\"]],\"1\":\"0\"}";

    public static async UniTask<BidContract[]> GetGameSessions()
    {
        string currentChain = AvailableNetworks.CurrentNetworkData.chain;
        string gameSessions = await GetGameSessionsData(currentChain);

        gameSessions = CheckGameSessions(gameSessions);

        var gameSessionsData = JsonConvert.DeserializeObject<RawContractsData>(gameSessions);
        return ProcessGameSessionsData(gameSessionsData, currentChain);
    }

    private static string CheckGameSessions(string gameSessions)
    {
        if (string.IsNullOrEmpty(gameSessions) || gameSessions == "[]" || gameSessions == "\"[]\"")
        {
            gameSessions = DefaulSessions;
        }

        return gameSessions;
    }

    private static async UniTask<string> GetGameSessionsData(string currentChain)
    {
        string gameSessions = await ContractHandler.MotodDexCallWithoutSerialize("getGameSessions", Abis.MotoDEXABI,
            Chain.InUse, Network.InUse, Contracts.MotoDEX, "[]");

        return currentChain switch
        {
            "near" => StructuredNearAllGameSessions(gameSessions),
            "concordium" => await StructuredConcordiumAllGameSessions(gameSessions),
            _ => gameSessions
        };
    }

    private static BidContract[] ProcessGameSessionsData(RawContractsData gameSessionsData, string currentChain)
    {
        int bidContractsCount = Convert.ToInt32(gameSessionsData.count);
        var contracts = new BidContract[bidContractsCount];

        for (int i = 0; i < bidContractsCount; i++)
        {
            // if(gameSessionsData.data[i][7] == "0" || gameSessionsData.data[i][5] == "0") continue;
            Debug.Log(
                $"Processing contract {i}: time {gameSessionsData.data[i][7]}, motoHealth {gameSessionsData.data[i][5]}");
            contracts[i] = new BidContract
            {
                trackId = gameSessionsData.data[i][0],
                trackType = gameSessionsData.data[i][1],
                motoId = gameSessionsData.data[i][3],
                motoType = gameSessionsData.data[i][4],
                timeValue = Convert.ToDouble(gameSessionsData.data[i][7]) / 1000,
                motoHealth = CalculateMotoHealth(gameSessionsData.data[i][5], currentChain),
                totalBidsValue = CalculateTotalBids(gameSessionsData.data[i][9], currentChain)
            };
            contracts[i].time = contracts[i].timeValue.ToString(CultureInfo.InvariantCulture);
            contracts[i].totalBids = contracts[i].totalBidsValue.ToString(CultureInfo.InvariantCulture);
        }

        return contracts;
    }

    private static string CalculateMotoHealth(string motoHealthData, string currentChain)
    {
        if (motoHealthData == "0") return "0";
        double tempMotoHealth = Convert.ToDouble(motoHealthData);
        tempMotoHealth /= currentChain == "near" ? Const.NearHealth.nearHpDiff :
            currentChain == "concordium" ? 1 / Const.ConcordiumHealth.concordiumHpDiff : 1;
        double oneHpInWei = currentChain is not "near" and not "concordium"
            ? Math.Pow(10, 17)
            : Const.Health.oneHpInWei;
        var calculateMotoHealth =
            Math.Ceiling(tempMotoHealth / oneHpInWei).ToString("F0", CultureInfo.InvariantCulture);
        Debug.Log($"MotoHealth: {calculateMotoHealth}");
        return calculateMotoHealth;
    }

    private static double CalculateTotalBids(string totalBidsData, string currentChain)
    {
        double totalBids = double.Parse(totalBidsData);
        totalBids /= currentChain == "near" ? Const.Wallet.NEARToHuman :
            currentChain == "concordium" || currentChain == "skale" ? Const.Wallet.CCDToHuman : Const.Wallet.BNBToHuman;
        return Math.Round(totalBids, 6);
    }

    [Serializable]
    public struct LatestPrice
    {
        public double value;
        public int digits;
    }

    [Serializable]
    private struct RawCurrencyObject
    {
        [JsonProperty("0")] public string amount;
        [JsonProperty("1")] public string digits;
    }

    public static async UniTask<LatestPrice> GetLatestCurrencyPrice()
    {
        Debug.Log($"#debug #motodexcallWithoutSerialize #contracts");
        var response = await ContractHandler.MotodDexCallWithoutSerialize("getLatestPrice", Abis.MotoDEXABI,
            Chain.InUse, Network.InUse, Contracts.MotoDEX, "[]");
        //Debug.Log(response);
        if (response == "[]" || response == "\"[]\"")
            response = "{\"0\":\"0\",\"1\":\"0\"}";
        var rawObject = JsonConvert.DeserializeObject<RawCurrencyObject>(response);

        return new LatestPrice()
        {
            value = Convert.ToDouble(rawObject.amount),
            digits = Convert.ToInt32(rawObject.digits),
        };
    }

    public enum ReceiverType
    {
        TRACK,
        MOTO,
        BIDDER,
        PLATFORM
    }

    [Serializable]
    public struct EpochPayment
    {
        public double amount;
        public string to;
        public string trackTokenId;
        public string motoTokenId;
        public string indexForDelete;
        public ReceiverType receiverType;
        public string amountPlatform;
    }

    [Serializable]
    private struct RawEpochResultData
    {
        [JsonProperty("0")] public string[][] value;
        [JsonProperty("1")] public int count;
    }

    public static async UniTask<EpochPayment[]> GetEpochResultBids()
    {
        Debug.Log($"#debug #motodexcallWithoutSerialize #contracts");
        var response = await ContractHandler.MotodDexCallWithoutSerialize("syncEpochResultsBidsFinal", Abis.MotoDEXABI,
            Chain.InUse, Network.InUse, Contracts.MotoDEX, "[]");
        Debug.Log(response);

        if (response == "[]" || response == "\"[]\"")
            response = "{\"0\":[[\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\"]],\"1\":\"0\"}";

        return InternalParseEpochPaymentValues(response);
    }

    public static async UniTask<EpochPayment[]> GetEpochResultMotos()
    {
        Debug.Log($"#debug #motodexcallWithoutSerialize #contracts");
        var response = await ContractHandler.MotodDexCallWithoutSerialize("syncEpochResultsMotosFinal", Abis.MotoDEXABI,
            Chain.InUse, Network.InUse, Contracts.MotoDEX, "[]");
        Debug.Log(response);

        if (response == "[]" || response == "\"[]\"")
            response = "{\"0\":[[\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\"]],\"1\":\"0\"}";

        return InternalParseEpochPaymentValues(response);
    }

    private static EpochPayment[] InternalParseEpochPaymentValues(string response)
    {
        if (string.IsNullOrEmpty(response))
            return null;

        if (AvailableNetworks.CurrentNetworkData.chain == "near" && !response.Contains("\"1\":\"0\""))
        {
            var nearResult = NearParseEpochPaymentValues(response);
            return nearResult;
        }

        if (AvailableNetworks.CurrentNetworkData.chain == "concordium" && !response.Contains("\"1\":\"0\""))
        {
            var concResult = ConcordiumParseEpochPaymentValues(response);
            return concResult;
        }

        var rawObject = JsonConvert.DeserializeObject<RawEpochResultData>(response);
        var result = new EpochPayment[rawObject.count];
        for (var i = 0; i < rawObject.count; i++)
        {
            result[i].amount = Convert.ToDouble(rawObject.value[i][0]);
            result[i].to = rawObject.value[i][1];
            result[i].trackTokenId = rawObject.value[i][2];
            result[i].motoTokenId = rawObject.value[i][3];
            result[i].indexForDelete = rawObject.value[i][4];
            result[i].receiverType = (ReceiverType)int.Parse(rawObject.value[i][5]);
            result[i].amountPlatform = rawObject.value[i][6];
        }

        return result;
    }

    private static EpochPayment[] NearParseEpochPaymentValues(string response)
    {
        var NearEpochPaymentViewData = JsonConvert.DeserializeObject<NearEpochPaymentViewData[]>(response);
        var result = new EpochPayment[NearEpochPaymentViewData.Length];
        for (var i = 0; i < NearEpochPaymentViewData.Length; i++)
        {
            result[i].trackTokenId = NearEpochPaymentViewData[i].trackTokenId;
            result[i].motoTokenId = NearEpochPaymentViewData[i].motoTokenId;
            result[i].amount = Convert.ToDouble(NearEpochPaymentViewData[i].amount);
            result[i].to = NearEpochPaymentViewData[i].to;
            result[i].receiverType =
                (ReceiverType)Enum.Parse(typeof(ReceiverType), NearEpochPaymentViewData[i].receiverType);
        }

        return result;
    }

    private static EpochPayment[] ConcordiumParseEpochPaymentValues(string response)
    {
        var ConcordiumEpochPaymentViewData = JsonConvert.DeserializeObject<ConcordiumEpochPaymentViewData[]>(response);
        var result = new EpochPayment[ConcordiumEpochPaymentViewData.Length];
        for (var i = 0; i < ConcordiumEpochPaymentViewData.Length; i++)
        {
            result[i].trackTokenId = ConcordiumEpochPaymentViewData[i].trackTokenId;
            result[i].motoTokenId = ConcordiumEpochPaymentViewData[i].motoTokenId;
            result[i].amount = Convert.ToDouble(ConcordiumEpochPaymentViewData[i].amount);
            result[i].to = ConcordiumEpochPaymentViewData[i].to;
            result[i].receiverType =
                (ReceiverType)Enum.Parse(typeof(ReceiverType), ConcordiumEpochPaymentViewData[i].receiverType);
        }

        return result;
    }

    public static int CompareContractByTime(BidContract left, BidContract right)
    {
        return left.timeValue.CompareTo(right.timeValue);
    }

    private static string StructuredNearGameBidsData(string data)
    {
        if (data == "[]" || data == "\"[]\"" || data == "") return data;
        NearGameBidsData[] nearGameBidsData = JsonConvert.DeserializeObject<NearGameBidsData[]>(data);
        List<string[]> fullBidsData = new List<string[]>();
        foreach (var bidData in nearGameBidsData)
        {
            List<string> bidsData = new List<string>(5);
            bidsData.Add(bidData.amount);
            bidsData.Add(bidData.track);
            bidsData.Add(bidData.moto);
            bidsData.Add(bidData.timestamp);
            bidsData.Add(bidData.bidder);

            fullBidsData.Add(bidsData.ToArray());
        }

        var rawContractsData = new RawContractsData
        {
            data = fullBidsData.ToArray(),
            count = fullBidsData.Count.ToString()
        };
        string requiredResponseString = JsonConvert.SerializeObject(rawContractsData);
        return requiredResponseString;
    }

    private static string StructuredNearAllGameSessions(string data)
    {
        if (data == "[]" || data == "\"[]\"" || data == "") return data;
        NearContractsData[] nearContractsData = JsonConvert.DeserializeObject<NearContractsData[]>(data);
        List<string[]> motoDatas = new List<string[]>();
        foreach (var nearContract in nearContractsData)
        {
            foreach (var nearMoto in nearContract.nearMotoTokens)
            {
                List<string> motoData = new List<string>(10);
                motoData.Add(nearContract.trackTokenId);
                motoData.Add(((int)Enum.Parse(typeof(Types), nearContract.trackType.trackTypeName)).ToString());
                motoData.Add(nearContract.trackHealth);
                motoData.Add(nearMoto.motoTokenId);
                motoData.Add(((int)Enum.Parse(typeof(Types), nearMoto.nearMotoType.motoTypeName)).ToString());
                motoData.Add(nearMoto.motoHealth);
                motoData.Add(nearContract.latestUpdateTime);
                motoData.Add(nearMoto.bestTimeResult);
                motoData.Add(nearContract.attempts);
                motoData.Add(nearContract.gameBidsSum);

                motoDatas.Add(motoData.ToArray());
            }
        }

        var rawContractsData = new RawContractsData
        {
            data = motoDatas.ToArray(),
            count = motoDatas.Count.ToString()
        };
        string requiredResponseString = JsonConvert.SerializeObject(rawContractsData);
        return requiredResponseString;
    }

    private static async UniTask<string> StructuredConcordiumAllGameSessions(string data)
    {
        if (data == "[]" || data == "\"[]\"" || data == "") return data;
        ConcordiumContractsData[] concordiumContractsData =
            JsonConvert.DeserializeObject<ConcordiumContractsData[]>(data);
        List<string[]> motoDatas = new List<string[]>();
        Debug.Log("ConcordiumGameSessions deserialized");
        foreach (var concordiumContract in concordiumContractsData)
        {
            foreach (var concordiumMoto in concordiumContract.concordiumMotoTokens)
            {
                List<string> motoData = new List<string>(10);
                motoData.Add(concordiumContract.trackTokenId);
                Debug.Log("TrackTokenId: " + motoData[0]);
                string trackTokenType = concordiumContract.trackType;
                motoData.Add(trackTokenType);
                motoData.Add("0");
                motoData.Add(concordiumMoto.motoTokenId);
                string motoTokenType = concordiumMoto.motoType;
                string motoTokenHealth = concordiumMoto.motoHealth;
                motoData.Add(motoTokenType);
                motoData.Add(motoTokenHealth);
                motoData.Add(concordiumContract.latestUpdateTime);
                string bestTime = concordiumMoto.bestTimeResult;
                motoData.Add(bestTime);
                motoData.Add(concordiumContract.attempts);
                motoData.Add(concordiumContract.gameBidsSum);

                motoDatas.Add(motoData.ToArray());
            }
        }

        var rawContractsData = new RawContractsData
        {
            data = motoDatas.ToArray(),
            count = motoDatas.Count.ToString()
        };
        string requiredResponseString = JsonConvert.SerializeObject(rawContractsData);
        return requiredResponseString;
    }

    public static string ProcessCalldata(object[] calldata, string methodName)
    {
        if (calldata.Length == 1)
        {
            var processedData = ProcessData(calldata[0]);
            string jsonString = SerializeProcessedData(processedData);
            Debug.Log(methodName + " response: " + jsonString);
            return jsonString;
        }
        else
        {
            var resultDict = new Dictionary<string, object>();

            for (int i = 0; i < calldata.Length; i++)
            {
                var processedData = ProcessData(calldata[i]);
                resultDict[i.ToString()] = processedData;
            }

            string jsonString = JsonConvert.SerializeObject(resultDict);
            Debug.Log(methodName + " response: " + jsonString);
            return jsonString;
        }
    }

    public static object ProcessData(object data)
    {
        if (data is IEnumerable enumerable && !(data is string))
        {
            var processedData = new List<object>();

            foreach (var item in enumerable)
            {
                var itemData = ProcessData(item);
                processedData.Add(itemData);
            }

            return processedData;
        }
        else if (data is ParameterOutput parameterOutput)
        {
            return parameterOutput.Result.ToString();
        }
        else
        {
            return data.ToString();
        }
    }

    public static string SerializeProcessedData(object processedData)
    {
        if (processedData is string)
        {
            return (string)processedData;
        }
        else
        {
            return JsonConvert.SerializeObject(processedData);
        }
    }

    public static object[] ParseAndValidateParameters(string _args, string _abi, string _contract, string _method)
    {
        var args = MyUtils.Empty<object>();
        if (!string.IsNullOrEmpty(_args) && _args != "[]")
        {
            args = JsonConvert.DeserializeObject<object[]>(_args);

            var contractBuilder = new ContractBuilder(_abi, _contract);
            var function = contractBuilder.GetFunctionAbi(_method);

            for (int i = 0; i < args.Length; i++)
            {
                var parameter = function.InputParameters.ElementAtOrDefault(i);

                if (parameter != null && parameter.Type.Contains("int") && !parameter.Type.Contains("[]"))
                {
                    if (args[i] is string argString && BigInteger.TryParse(argString, out BigInteger argBigInt))
                    {
                        args[i] = argBigInt;
                    }
                    else
                    {
                        throw new ArgumentException("Invalid input argument at index " + i);
                    }
                }
            }
        }

        return args;
    }
}