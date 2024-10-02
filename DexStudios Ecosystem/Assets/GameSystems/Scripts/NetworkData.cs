using System;
using UnityEngine;

[Serializable]
public class NetworkData
{
    public string chain;
    public string network;
    public string[] contracts;
    public string networkName;
    public string networkUrl;
    public string networkChainId;
    public double balanceToPlay;
    public string balanceCurrency;
    public string uriPathName;
    public bool isPricesInUSD;
    public bool isEVMNotCompatible;
    public double DecimalToHuman;
}

