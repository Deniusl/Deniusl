using UnityEngine;


namespace GameSystems.Scripts
{
    public class AvailableNetworks : MonoBehaviour
    {
        public static readonly NetworkData[] allNetworksData = {
            new NetworkData{
                chain = "kroma",
                network = "mainnet",
                contracts = new[] { "0x1c050ea3673fad2Ac2f8559c81f2CD23a9Ec195e", "0xABa81bdbdBF45Edca8498D90ab1dEb9d85186930"},
                networkName = "Kroma Mainnet",
                networkUrl = "https://api.kroma.network/",
                networkChainId = "255",
                balanceToPlay = 0.5,
                balanceCurrency = "ETH",
                uriPathName = "kroma",
                isPricesInUSD = false,
                isEVMNotCompatible = false,
                DecimalToHuman = Const.Wallet.BNBToHuman
            },
            new NetworkData{
                chain = "nibiru",
                network = "mainnet",
                contracts = new[] { "nibi1ejj4gwtaraaf7mxuplsfxxf5kfdtcs6htm8sk38u6yy34asyar7sfagyf9", "nibi1ejj4gwtaraaf7mxuplsfxxf5kfdtcs6htm8sk38u6yy34asyar7sfagyf9"}, // 
                networkName = "Nibiru",
                networkUrl = "",
                networkChainId = "1111111456327830", // "1111111456327830" - mainnet
                balanceToPlay = 0.0,
                balanceCurrency = "NIBI",
                uriPathName = "nibiru",
                isPricesInUSD = false,
                isEVMNotCompatible = true,
                DecimalToHuman = Const.Wallet.CCDToHuman
            },
            //new NetworkData{
            //    chain = "nibiru_t",
            //    network = "testnet",
            //    contracts = new[] { "nibi1am400aj267rvch9evmmz2m6zyzgs6tc7cwxhmm2usc4aq5warlpse8tu6x", "nibi1am400aj267rvch9evmmz2m6zyzgs6tc7cwxhmm2usc4aq5warlpse8tu6x"}, // 
            //    networkName = "Nibiru Testnet",
            //    networkUrl = "",
            //    networkChainId = "1111111456327825", // "1111111456327830" - mainnet
            //    balanceToPlay = 0.0,
            //    balanceCurrency = "NIBI",
            //    uriPathName = "nibiru_t",
            //    isPricesInUSD = false,
            //    isEVMNotCompatible = true,
            //    DecimalToHuman = Const.Wallet.CCDToHuman
            //},

            //new NetworkData{
            //    chain = "icp_t",
            //    network = "testnet",
            //    contracts = new[] { "", ""}, // 
            //    networkName = "ICP Testnet",
            //    networkUrl = "",
            //    networkChainId = "111111456327825", // "111111456327830" - mainnet
            //    balanceToPlay = 0.0,
            //    balanceCurrency = "ICP",
            //    uriPathName = "icp_t",
            //    isPricesInUSD = false,
            //    isEVMNotCompatible = true,
            //    DecimalToHuman = Const.Wallet.AptosToHuman
            //},
            new NetworkData{
                chain = "icp",
                network = "mainnet",
                contracts = new[] { "", ""}, // 
                networkName = "ICP",
                networkUrl = "",
                networkChainId = "111111456327830", 
                balanceToPlay = 0.0,
                balanceCurrency = "ICP",
                uriPathName = "icp",
                isPricesInUSD = false,
                isEVMNotCompatible = true,
                DecimalToHuman = Const.Wallet.AptosToHuman
            },
            //new NetworkData{
            //    chain = "sui_t",
            //    network = "testnet",
            //    contracts = new[] { "0x7d5fd303232d7ffa80cc00d63bdbf636cb6544dfbc2429cefa4ce7b14755841e", "0x7d5fd303232d7ffa80cc00d63bdbf636cb6544dfbc2429cefa4ce7b14755841e"}, // PACKAGE_ID
            //    networkName = "SUI Testnet",
            //    networkUrl = "",
            //    networkChainId = "11111456327825", // "11111456327830" - mainnet
            //    balanceToPlay = 0.0,
            //    balanceCurrency = "SUI",
            //    uriPathName = "sui_t",
            //    isPricesInUSD = false,
            //    isEVMNotCompatible = true,
            //    DecimalToHuman = Const.Wallet.AptosToHuman                
            //},

            new NetworkData{
                chain = "sui",
                network = "mainnet",
                contracts = new[] { "0x7f8ad597e589e66606c4f5f484cb5a3e808701483dd43afe61cec10b97961745", "0x7f8ad597e589e66606c4f5f484cb5a3e808701483dd43afe61cec10b97961745"}, // PACKAGE_ID
                networkName = "SUI",
                networkUrl = "",
                networkChainId = "11111456327830", // "11111456327830" - mainnet
                balanceToPlay = 0.0,
                balanceCurrency = "SUI",
                uriPathName = "sui",
                isPricesInUSD = false,
                isEVMNotCompatible = true,
                DecimalToHuman = Const.Wallet.AptosToHuman
            },

            new NetworkData{
                chain = "aptos",
                network = "mainnnet",
                contracts = new[] { "0x22b1a894755ad4493c0219586e41f69e95a77d5630bbc0e1cf510946606d5cab", "0x22b1a894755ad4493c0219586e41f69e95a77d5630bbc0e1cf510946606d5cab"},
                networkName = "Aptos Mainnet",
                networkUrl = "https://fullnode.mainnet.aptoslabs.com/",

                networkChainId = "111456327825",
                balanceToPlay = 0.0,
                balanceCurrency = "APT",
                uriPathName = "aptos",
                isPricesInUSD = false,
                isEVMNotCompatible = true,
                DecimalToHuman = Const.Wallet.AptosToHuman
            },
            new NetworkData{
                chain = "flare",
                network = "mainnet",
                contracts = new[] { "0x4EAC987ccBb935348570fD9Fe963Fb4d17C6571a", "0x0888D4a5ff5C1B27EccD04420050294560C70376"}, // MotoDexNFT - [0], MotoDex - [1]
                networkName = "Flare",
                networkUrl = "https://flare-api.flare.network/ext/bc/C/rpc/",
                networkChainId = "14",
                balanceToPlay = 0.0,
                balanceCurrency = "FLR",
                uriPathName = "flare",
                isPricesInUSD = false,
                isEVMNotCompatible = false,
                DecimalToHuman = Const.Wallet.BNBToHuman
            },
            //new NetworkData{
            //    chain = "aurora",
            //    network = "testnet",
            //    contracts = new[] {"0xd87cA4cAC733E5450586183e2e020c6704F6E7Df", "0xAbAc0435A9Bf066b73aF77e0acbCa58F4774F615"},
            //    networkName = "Aurora Testnet",
            //    networkUrl = "https://testnet.aurora.dev",
            //    networkChainId = "1313161555",
            //    balanceToPlay = 0.003,
            //    balanceCurrency = "ETH",
            //    uriPathName = "aurora_t"
            //},
            new NetworkData{
                chain = "aurora",
                network = "mainnet",
                contracts = new[] {"0x9A8Ac91f60ede2E168337dBf85355DE427B29bD4", "0x578d0C462387715D28D05a0a1ba22a3A65aCE57A"},
                networkName = "Aurora Mainnet",
                networkUrl = "https://endpoints.omniatech.io/v1/aurora/mainnet/public", // "https://mainnet.aurora.dev",
                networkChainId = "1313161554",
                balanceToPlay = 0.003,
                balanceCurrency = "ETH",
                uriPathName = "aurora",
                isPricesInUSD = false,
                isEVMNotCompatible = false,
                DecimalToHuman = Const.Wallet.BNBToHuman
            },
            //new NetworkData{
            //    chain = "near",
            //    network = "testnet",
            //    contracts = new[] { "dev-1678015185015-40254331682784", "dev-1678015185015-40254331682784"},
            //    networkName = "Near Testnet",
            //    networkUrl = "https://rpc.testnet.near.org",
            //    networkChainId = "",
            //    balanceToPlay = 0.5,
            //    balanceCurrency = "NEAR",
            //    uriPathName = "near_t"
            //},
            new NetworkData{ 
                chain = "near", 
                network = "mainnet", 
                contracts = new[] { "motodex.near", "motodex.near"}, 
                networkName = "Near Mainnet", 
                networkUrl = "https://rpc.mainnet.near.org", 
                networkChainId = "", 
                balanceToPlay = 0.5,
                balanceCurrency = "NEAR", 
                uriPathName = "near",
                isPricesInUSD = false,
                isEVMNotCompatible = true,
                DecimalToHuman = Const.Wallet.NEARToHuman
            },
            //new NetworkData{
            //    chain = "concordium",
            //    network = "testnet",
            //    contracts = new[] {"MOTODEX:4349", "MOTODEX:4349"},
            //    networkName = "Concordium Testnet",
            //    networkUrl = "",
            //    networkChainId = "1456327825",
            //    balanceToPlay = 0.5,
            //    balanceCurrency = "CCD",
            //    uriPathName = "conc_t",
            //    isPricesInUSD = false,
            //    isEVMNotCompatible = false,
            //    DecimalToHuman = Const.Wallet.BNBToHuman
            //},
            new NetworkData{
                chain = "concordium",
                network = "mainnet",
                contracts = new[] {"MOTODEX:9333", "MOTODEX:9333"},
                networkName = "Concordium Mainnet",
                networkUrl = "",
                networkChainId = "1456327830",
                balanceToPlay = 0.5,
                balanceCurrency = "CCD",
                uriPathName = "conc",
                isPricesInUSD = false,
                isEVMNotCompatible = false,
                DecimalToHuman = Const.Wallet.BNBToHuman
            },
            new NetworkData{
                chain = "polygon",
                network = "mainnet",
                contracts = new[] {"0x54DB918B7eA57ab1D79F9874B1f5A7A2EAd4e2B9", "0x6e17bEF6871b5C0fd8a07eF4A1222aEaBd7b2f6E"},
                networkName = "Polygon Mainnet",
                networkUrl = "https://polygon.llamarpc.com", // "https://polygon-rpc.com",
                networkChainId = "137",
                balanceToPlay = 0.5,
                balanceCurrency = "MATIC",
                uriPathName = "polygon",
                isPricesInUSD = false,
                isEVMNotCompatible = false,
                DecimalToHuman = Const.Wallet.BNBToHuman
            },
            //new NetworkData{
            //    chain = "mantle",
            //    network = "testnet",
            //    contracts = new[] {"0xfA3f09b9870bD4D3374bAcb682fCB2bCC70714BE", "0x3db648A8173abAf27a5705c079a0bF951ff28432"},
            //    networkName = "Mantle Testnet",
            //    networkUrl = "https://rpc.testnet.mantle.xyz",
            //    networkChainId = "5001",
            //    balanceToPlay = 0.5,
            //    balanceCurrency = "MNT",
            //    uriPathName = "mantle_t"
            //},
            // new NetworkData{
            //     chain = "zetachain",
            //     network = "testnet",
            //     contracts = new[] {"0xfA3f09b9870bD4D3374bAcb682fCB2bCC70714BE", "0x3db648A8173abAf27a5705c079a0bF951ff28432"},
            //     networkName = "ZetaChain Testnet",
            //     networkUrl = "https://api.athens2.zetachain.com/evm",
            //     networkChainId = "7001",
            //     balanceToPlay = 0.5,
            //     balanceCurrency = "aZETA",
            //     uriPathName = "zeta_t",
            //     isPricesInUSD = false,
            //     isEVMNotCompatible = false,
            //     DecimalToHuman = Const.Wallet.BNBToHuman
            // },
                new NetworkData{
                chain = "zeta",
                network = "mainnet",
                contracts = new[] { "0x1c050ea3673fad2Ac2f8559c81f2CD23a9Ec195e", "0xABa81bdbdBF45Edca8498D90ab1dEb9d85186930"},
                networkName = "Zeta Mainnet",
                networkUrl = "https://zetachain-evm.blockpi.network/v1/rpc/public",
                networkChainId = "7000",
                balanceToPlay = 0.5,
                balanceCurrency = "ZETA",
                uriPathName = "zeta",
                isPricesInUSD = false,
                isEVMNotCompatible = false,
                DecimalToHuman = Const.Wallet.BNBToHuman
            },
            new NetworkData{
                chain = "eosevm",
                network = "mainnet",
                contracts = new[] {"0x6CD6978C794ee76d8440CB0a81CF3B200a39c7E3", "0x1c050ea3673fad2Ac2f8559c81f2CD23a9Ec195e"},
                networkName = "EOS EVM Mainnet",
                networkUrl = "https://api.evm.eosnetwork.com/",
                networkChainId = "17777",
                balanceToPlay = 0.5,
                balanceCurrency = "EOS",
                uriPathName = "eosevm",
                isPricesInUSD = false,
                isEVMNotCompatible = false,
                DecimalToHuman = Const.Wallet.BNBToHuman
            },
            new NetworkData{
                chain = "mantle",
                network = "mainnet",
                contracts = new[] {"0x6CD6978C794ee76d8440CB0a81CF3B200a39c7E3", "0x1c050ea3673fad2Ac2f8559c81f2CD23a9Ec195e"},
                networkName = "Mantle Mainnet",
                networkUrl = "https://rpc.mantle.xyz",
                networkChainId = "5000",
                balanceToPlay = 0.5,
                balanceCurrency = "MNT",
                uriPathName = "mantle",
                isPricesInUSD = false,
                isEVMNotCompatible = false,
                DecimalToHuman = Const.Wallet.BNBToHuman
            },
            new NetworkData{
                chain = "mint",
                network = "mainnet",
                contracts = new[] {"0x6CD6978C794ee76d8440CB0a81CF3B200a39c7E3", "0x1c050ea3673fad2Ac2f8559c81f2CD23a9Ec195e"},
                networkName = "Mint Mainnet",
                networkUrl = "https://rpc.mintchain.io/",
                networkChainId = "185",
                balanceToPlay = 0.5,
                balanceCurrency = "ETH",
                uriPathName = "mantle",
                isPricesInUSD = false,
                isEVMNotCompatible = false,
                DecimalToHuman = Const.Wallet.BNBToHuman
            },
            new NetworkData{
                chain = "dchain",
                network = "mainnet",
                contracts = new[] { "0x6CD6978C794ee76d8440CB0a81CF3B200a39c7E3", "0x1c050ea3673fad2Ac2f8559c81f2CD23a9Ec195e"},// MotoDexNFT - [0], MotoDex - [1]
                networkName = "Dchain",
                networkUrl = "https://dchain-2716446429837000-1.sagaexplorer.io",
                networkChainId = "2716446429837000",
                balanceToPlay = 0.0,
                balanceCurrency = "ETH",
                uriPathName = "dchain",
                isPricesInUSD = false,
                isEVMNotCompatible = false,
                DecimalToHuman = Const.Wallet.BNBToHuman
            },
                      
            //new NetworkData{
            //    chain = "skale",
            //    network = "testnet",
            //    contracts = new[] {"0x1c050ea3673fad2Ac2f8559c81f2CD23a9Ec195e", "0xABa81bdbdBF45Edca8498D90ab1dEb9d85186930"},
            //    networkName = "Skale Testnet",
            //    networkUrl = "https://staging-v3.skalenodes.com/v1/staging-faint-slimy-achird",
            //    networkChainId = "503129905",
            //    balanceToPlay = 0.5,
            //    balanceCurrency = "USDC",
            //    uriPathName = "skale_t"
            //},
            new NetworkData{
                chain = "skale",
                network = "mainnet",
                contracts = new[]{ "0x9ace5e62A42F251b73daac20a6633CBD103b3668", "0x54bD7f566e05258AcE8eB979e4dbCE5F6376Ff38"}, //{"0x56F57fa49DF41aE11610d362A00819755183733a", "0x8E88b33A6d4f31C59677eA55229451b1dc86Fcd9"},
                networkName = "Skale Mainnet",
                networkUrl = "https://mainnet.skalenodes.com/v1/green-giddy-denebola",
                networkChainId = "1482601649",
                balanceToPlay = 0.5,
                balanceCurrency = "USDC",
                uriPathName = "skale",
                isPricesInUSD = true,
                isEVMNotCompatible = true,
                DecimalToHuman = Const.Wallet.CCDToHuman
            },

            // new NetworkData{
            //    chain = "skale",
            //    network = "mainnet",
            //    contracts = new[] { "0x9ace5e62A42F251b73daac20a6633CBD103b3668", "0x54bD7f566e05258AcE8eB979e4dbCE5F6376Ff38"},
            //    networkName = "Skale Mainnet",
            //    networkUrl = "https://mainnet.skalenodes.com/v1/green-giddy-denebola",
            //    networkChainId = "1482601649",
            //    balanceToPlay = 0.5,
            //    balanceCurrency = "USDC",
            //    uriPathName = "skale"
            //},

            //new NetworkData{
            //    chain = "klaytn",
            //    network = "testnet",
            //    contracts = new[] {"0xABa81bdbdBF45Edca8498D90ab1dEb9d85186930", "0xA5CA66874Ca008D7bcD3f3F817a87F98bEA75Fd2"},
            //    networkName = "Klaytn Testnet",
            //    networkUrl = "https://api.baobab.klaytn.net:8651",
            //    networkChainId = "1001",
            //    balanceToPlay = 0.5,
            //    balanceCurrency = "KLAY",
            //    uriPathName = "klaytn_t"
            //},
            new NetworkData{
                chain = "base",
                network = "mainnet",
                contracts = new[] {"0x1c050ea3673fad2Ac2f8559c81f2CD23a9Ec195e", "0xABa81bdbdBF45Edca8498D90ab1dEb9d85186930"},
                networkName = "Base Mainnet",
                networkUrl = "https://mainnet.base.org",
                networkChainId = "8453",
                balanceToPlay = 0.5,
                balanceCurrency = "ETH",
                uriPathName = "base",
                isPricesInUSD = false,
                isEVMNotCompatible = false,
                DecimalToHuman = Const.Wallet.BNBToHuman
            },
            new NetworkData{
                chain = "manta",
                network = "mainnet",
                contracts = new[] { "0x6CD6978C794ee76d8440CB0a81CF3B200a39c7E3", "0x1c050ea3673fad2Ac2f8559c81f2CD23a9Ec195e"},
                networkName = "Manta Mainnet",
                networkUrl = "https://pacific-rpc.manta.network/http",
                networkChainId = "169",
                balanceToPlay = 0.05,
                balanceCurrency = "ETH",
                uriPathName = "manta",
                isPricesInUSD = false,
                isEVMNotCompatible = false,
                DecimalToHuman = Const.Wallet.BNBToHuman
            },
            new NetworkData{
                chain = "alephzero",
                network = "mainnet",
                contracts = new[] { "5EqN6wjxoF8ZfS1NQunkLCFYrG7cwMrgmhrBjCLssfSEri2V", "5EqN6wjxoF8ZfS1NQunkLCFYrG7cwMrgmhrBjCLssfSEri2V"},
                networkName = "Alephzero Mainnet",
                networkUrl = "wss://ws.azero.dev",
                networkChainId = "11456327825",
                balanceToPlay = 0.1,
                balanceCurrency = "AZERO",
                uriPathName = "alephzero",
                isPricesInUSD = false,
                isEVMNotCompatible = false,
                DecimalToHuman = Const.Wallet.BNBToHuman
            },
            //new NetworkData{
            //    chain = "motodex",
            //    network = "testnet",
            //    contracts = new[] { "0x6CD6978C794ee76d8440CB0a81CF3B200a39c7E3", "0x1c050ea3673fad2Ac2f8559c81f2CD23a9Ec195e"},
            //    networkName = "Motodex Testnet",
            //    networkUrl = "https://motodex-1698905757615031-1.jsonrpc.testnet-sp1.sagarpc.io",
            //    networkChainId = "1698905757615031",
            //    balanceToPlay = 0.5,
            //    balanceCurrency = "OBS",
            //    uriPathName = "motodex_t"
            //},
            //new NetworkData{
            //    chain = "topos",
            //    network = "testnet",
            //    contracts = new[] { "0x6CD6978C794ee76d8440CB0a81CF3B200a39c7E3", "0x1c050ea3673fad2Ac2f8559c81f2CD23a9Ec195e"},
            //    networkName = "Topos Testnet",
            //    networkUrl = "https://rpc.topos-subnet.testnet-1.topos.technology",
            //    networkChainId = "2359",
            //    balanceToPlay = 0.5,
            //    balanceCurrency = "TOPOS",
            //    uriPathName = "topos_t"
            //},
            //new NetworkData{
            //    chain = "q",
            //    network = "mainnet",
            //    contracts = new[] { "0x6CD6978C794ee76d8440CB0a81CF3B200a39c7E3", "0x1c050ea3673fad2Ac2f8559c81f2CD23a9Ec195e"},
            //    networkName = "Q Mainnet",
            //    networkUrl = "https://rpc.q.org",
            //    networkChainId = "35441",
            //    balanceToPlay = 0.5,
            //    balanceCurrency = "Q",
            //    uriPathName = "q"
            //},
            //new NetworkData{
            //    chain = "viction",
            //    network = "testnet",
            //    contracts = new[] { "0x6CD6978C794ee76d8440CB0a81CF3B200a39c7E3", "0x1c050ea3673fad2Ac2f8559c81f2CD23a9Ec195e"},
            //    networkName = "Viction Testnet",
            //    networkUrl = "https://rpc.testnet.tomochain.com",
            //    networkChainId = "39",
            //    balanceToPlay = 0.5,
            //    balanceCurrency = "TOMO",
            //    uriPathName = "viction_t"
            //},


            //// testnets to disable for prod
            //new NetworkData{
            //    chain = "binance",
            //    network = "testnet",
            //    contracts = new[] {"0x7eA522c22C8B0CFEaBbC9f5ABE5CAFf63821dd37", "0xf5B60702A524CA580f6B0e7b61Cc78d504979a93"}, // MotoDexNFT - [0], MotoDex - [1]
            //    networkName = "BSC Testnet",
            //    networkUrl = "https://data-seed-prebsc-1-s1.binance.org:8545/",
            //    networkChainId = "97",
            //    balanceToPlay = 0.5,
            //    balanceCurrency = "BNB",
            //    uriPathName = "bsc_t",
            //    isPricesInUSD = false
            //},
            //new NetworkData{
            //    chain = "vara_t",
            //    network = "testnet",
            //    contracts = new[] { "0x636443c4f676a256b317b82b6b13e2e0f64c4e72311db5d4318df0325d8eb62d", "0x636443c4f676a256b317b82b6b13e2e0f64c4e72311db5d4318df0325d8eb62d"},
            //    networkName = "VARA Testnet",
            //    networkUrl = "wss://testnet.vara.network",

            //    networkChainId = "1111456327825",
            //    balanceToPlay = 0.0,
            //    balanceCurrency = "VARA",
            //    uriPathName = "vara_t",
            //    isEVMcompatible = false,
            //    DecimalToHuman = Const.Wallet.VARAToHuman
            //},
            new NetworkData{
                chain = "vara",
                network = "mainnet",
                contracts = new[] { "0x31d6a69355cf72733a494d5e6a52203bdb1ed64829d2780e7304b56015091d0d", "0x31d6a69355cf72733a494d5e6a52203bdb1ed64829d2780e7304b56015091d0d"},
                networkName = "VARA Mainnet",
                networkUrl = "wss://rpc.vara-network.io",

                networkChainId = "1111456327830",
                balanceToPlay = 0.0,
                balanceCurrency = "VARA",
                uriPathName = "vara",
                isPricesInUSD = false,
                isEVMNotCompatible = false,
                DecimalToHuman = Const.Wallet.VARAToHuman
            },
            new NetworkData{
                chain = "binance",
                network = "testnet",
                contracts = new[] {"0x7eA522c22C8B0CFEaBbC9f5ABE5CAFf63821dd37", "0xf5B60702A524CA580f6B0e7b61Cc78d504979a93"}, // MotoDexNFT - [0], MotoDex - [1]
                networkName = "BSC Testnet",
                networkUrl = "https://data-seed-prebsc-1-s1.binance.org:8545/",
                networkChainId = "97",
                balanceToPlay = 0.0,
                balanceCurrency = "BNB",
                uriPathName = "bsc_t",
                isPricesInUSD = false,
                isEVMNotCompatible = false,
                DecimalToHuman = Const.Wallet.BNBToHuman
            },
            new NetworkData{
                chain = "mint",
                network = "mainnet",
                contracts = new[] {"0x6CD6978C794ee76d8440CB0a81CF3B200a39c7E3", "0x1c050ea3673fad2Ac2f8559c81f2CD23a9Ec195e"}, // MotoDexNFT - [0], MotoDex - [1]
                networkName = "Mint Mainnet",
                networkUrl = "https://rpc.mintchain.io/",
                networkChainId = "185",
                balanceToPlay = 0.0,
                balanceCurrency = "ETH",
                uriPathName = "mint",
                isPricesInUSD = false,
                isEVMNotCompatible = false,
                DecimalToHuman = Const.Wallet.BNBToHuman
            },

        };

        public static readonly NetworkData[] networksData = allNetworksData;

        private static NetworkData _networkData;

        public static NetworkData CurrentNetworkData
        {
            get => _networkData;
            set
            {
                Debug.Log($"NetworkDataSet, chain: {value.chain}");
                _networkData = value;

            }
        }
    }
}