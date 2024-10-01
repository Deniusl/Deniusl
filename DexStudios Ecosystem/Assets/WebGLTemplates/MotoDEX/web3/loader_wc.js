window.process = {}
window.process.env= {}
window.process.env.NODE_ENV= 'production'

// import {
// 	writeContract
// } from "https://unpkg.com/viem";

import {
    EthereumClient,
    w3mConnectors,
    w3mProvider,
    WagmiCore,
    WagmiCoreChains,
    WagmiCoreConnectors
} from 'https://unpkg.com/@web3modal/ethereum'

import { Web3Modal } from 'https://unpkg.com/@web3modal/html'

// Equivalent to importing from @wagmi/core
const { configureChains, createConfig, getAccount, getNetwork, fetchBalance, readContracts, readContract, writeContract, switchNetwork, fetchTransaction, waitForTransaction, getWebSocketPublicClient } = WagmiCore

// Equivalent to importing from @wagmi/core/chains
const { bscTestnet, aurora, auroraTestnet, polygon, flare } = WagmiCoreChains

// Equivalent to importing from @wagmi/core/providers
const eos = {
    id: 17777,
    name: "EOS EVM",
    network: "eos",
    nativeCurrency: {
        decimals: 18,
        name: "EOS",
        symbol: "EOS"
    },
    rpcUrls: {
        default: {
            http: ["https://api.evm.eosnetwork.com/"],
        },
        public: {
            http: ["https://api.evm.eosnetwork.com/"]
        }
    },
    blockExplorers: {
        etherscan: {
            name: "eos Scan",
            url: "https://explorer.evm.eosnetwork.com/"
        },
        default: {
            name: "eos Scan",
            url: "https://explorer.evm.eosnetwork.com/"
        }
    }

};

const mantleTestnet = {
    id: 5001,
    name: "Mantle Testnet",
    network: "mantleTestnet",
    nativeCurrency: {
        decimals: 18,
        name: "tMNT",
        symbol: "tMNT"
    },
    rpcUrls: {
        default: {
            http: ["https://rpc.testnet.mantle.xyz"],
        },
        public: {
            http: ["https://rpc.testnet.mantle.xyz"]
        }
    },
    blockExplorers: {
        etherscan: {
            name: "mantle Scan",
            url: "https://explorer.testnet.mantle.xyz/"
        },
        default: {
            name: "eos Scan",
            url: "https://explorer.testnet.mantle.xyz/"
        }
    }

};

const mantle = {
    id: 5000,
    name: "Mantle",
    network: "mantle",
    nativeCurrency: {
        decimals: 18,
        name: "MNT",
        symbol: "MNT"
    },
    rpcUrls: {
        default: {
            http: ["https://rpc.mantle.xyz"],
        },
        public: {
            http: ["https://rpc.mantle.xyz"]
        }
    },
    blockExplorers: {
        etherscan: {
            name: "mantle Scan",
            url: "http://explorer.mantle.xyz/"
        },
        default: {
            name: "eos Scan",
            url: "https://explorer.mantle.xyz/"
        }
    }

};

const zetachainTestnet = {
    id: 7001,
    name: "ZetaChain Athens Testnet",
    network: "zetachainTestnet",
    nativeCurrency: {
        decimals: 18,
        name: "aZETA",
        symbol: "aZETA"
    },
    rpcUrls: {
        default: {
            http: ["https://api.athens2.zetachain.com/evm"],
        },
        public: {
            http: ["https://api.athens2.zetachain.com/evm"]
        }
    },
    blockExplorers: {
        etherscan: {
            name: "eos Scan",
            url: "https://explorer.athens.zetachain.com/"
        },
        default: {
            name: "eos Scan",
            url: "https://explorer.athens.zetachain.com/"
        }
    }

};

const skaleTestnet = {
    id: 503129905,
    name: "Skale Testnet",
    network: "skaleTestnet",
    nativeCurrency: {
        decimals: 18,
        name: "sFuel",
        symbol: "sFuel"
    },
    rpcUrls: {
        default: {
            http: ["https://staging-v3.skalenodes.com/v1/staging-faint-slimy-achird"],
            webSocket: ['wss://staging-v3.skalenodes.com/v1/ws/staging-faint-slimy-achird'],
        },
        public: {
            http: ["https://staging-v3.skalenodes.com/v1/staging-faint-slimy-achird"],
            webSocket: ['wss://staging-v3.skalenodes.com/v1/ws/staging-faint-slimy-achird']
        }
    },
    blockExplorers: {
        etherscan: {
            name: "Skale Scan",
            url: "https://staging-faint-slimy-achird.explorer.staging-v3.skalenodes.com/"
        },
        default: {
            name: "Skale Scan",
            url: "https://staging-faint-slimy-achird.explorer.staging-v3.skalenodes.com/"
        }
    }

};

const skaleMainnet = {
    id: 1482601649,
    name: "Skale Mainnet",
    network: "skaleMainnet",
    nativeCurrency: {
        decimals: 18,
        name: "sFuel",
        symbol: "sFuel"
    },
    rpcUrls: {
        default: {
            http: ["https://mainnet.skalenodes.com/v1/green-giddy-denebola"],
            webSocket: ['wss://mainnet.skalenodes.com/v1/ws/green-giddy-denebola'],
        },
        public: {
            http: ["https://mainnet.skalenodes.com/v1/green-giddy-denebola"],
            webSocket: ['wss://mainnet.skalenodes.com/v1/ws/green-giddy-denebola']
        }
    },
    blockExplorers: {
        etherscan: {
            name: "Skale Scan",
            url: "https://staging-faint-slimy-achird.explorer.staging-v3.skalenodes.com/"
        },
        default: {
            name: "Skale Scan",
            url: "https://staging-faint-slimy-achird.explorer.staging-v3.skalenodes.com/"
        }
    }

};

const klaytnTestnet = {
    id: 1001,
    name: "Klaytn Testnet Baobab",
    network: "klaytnTestnet",
    nativeCurrency: {
        decimals: 18,
        name: "KLAY",
        symbol: "KLAY"
    },
    rpcUrls: {
        default: {
            http: ["https://api.baobab.klaytn.net:8651"],
        },
        public: {
            http: ["https://api.baobab.klaytn.net:8651"]
        }
    },
    blockExplorers: {
        etherscan: {
            name: "eos Scan",
            url: "https://baobab.scope.klaytn.com/"
        },
        default: {
            name: "eos Scan",
            url: "https://baobab.scope.klaytn.com/"
        }
    }

};

const baseMainnet = {
    id: 8453,
    name: "Base Mainnet",
    network: "baseMainnet",
    nativeCurrency: {
        decimals: 18,
        name: "ETH",
        symbol: "ETH"
    },
    rpcUrls: {
        default: {
            http: ["https://mainnet.base.org"],
        },
        public: {
            http: ["https://mainnet.base.org"]
        }
    },
    blockExplorers: {
        etherscan: {
            name: "eos Scan",
            url: "https://basescan.org"
        },
        default: {
            name: "eos Scan",
            url: "https://basescan.org"
        }
    }

};

const mantaMainnet = {
    id: 169,
    name: "Manta Mainnet",
    network: "mantaMainnet",
    nativeCurrency: {
        decimals: 18,
        name: "ETH",
        symbol: "ETH"
    },
    rpcUrls: {
        default: {
            http: ["https://pacific-rpc.manta.network/http"],
        },
        public: {
            http: ["https://pacific-rpc.manta.network/http"]
        }
    },
    blockExplorers: {
        etherscan: {
            name: "manta Scan",
            url: "https://pacific-explorer.manta.network/"
        },
        default: {
            name: "manta Scan",
            url: "https://pacific-explorer.manta.network/"
        }
    }

};

const motodexTestnet = {
    id: 1698905757615031,
    name: "Motodex Testnet",
    network: "motodex_t",
    nativeCurrency: {
        decimals: 18,
        name: "OBS",
        symbol: "OBS"
    },
    rpcUrls: {
        default: {
            http: ["https://motodex-1698905757615031-1.jsonrpc.testnet-sp1.sagarpc.io/"],
        },
        public: {
            http: ["https://motodex-1698905757615031-1.jsonrpc.testnet-sp1.sagarpc.io/"]
        }
    },
    blockExplorers: {
        etherscan: {
            name: "Motodex Scan",
            url: "https://motodex-1698905757615031-1.testnet-sp1.sagaexplorer.io/"
        },
        default: {
            name: "Motodex Scan",
            url: "https://motodex-1698905757615031-1.testnet-sp1.sagaexplorer.io/"
        }
    }

};

const toposTestnet = {
    id: 2359,
    name: "Topos Testnet",
    network: "topos_t",
    nativeCurrency: {
        decimals: 18,
        name: "TOPOS",
        symbol: "TOPOS"
    },
    rpcUrls: {
        default: {
            http: ["https://rpc.topos-subnet.testnet-1.topos.technology"],
        },
        public: {
            http: ["https://rpc.topos-subnet.testnet-1.topos.technology"]
        }
    },
    blockExplorers: {
        etherscan: {
            name: "Topos Scan",
            url: "https://explorer.testnet-1.topos.technology/"
        },
        default: {
            name: "Topos Scan",
            url: "https://explorer.testnet-1.topos.technology/"
        }
    }

};

const qMainnet = {
    id: 35441,
    name: "Q Mainnet",
    network: "q",
    nativeCurrency: {
        decimals: 18,
        name: "Q",
        symbol: "Q"
    },
    rpcUrls: {
        default: {
            http: ["https://rpc.q.org"],
        },
        public: {
            http: ["https://rpc.q.org"]
        }
    },
    blockExplorers: {
        etherscan: {
            name: "Q Scan",
            url: "https://explorer.q.org/"
        },
        default: {
            name: "Q Scan",
            url: "https://explorer.q.org/"
        }
    }

};

const victionTestnet = {
    id: 89,
    name: "Viction Mainnet",
    network: "viction_t",
    nativeCurrency: {
        decimals: 18,
        name: "TOMO",
        symbol: "TOMO"
    },
    rpcUrls: {
        default: {
            http: ["https://rpc.testnet.tomochain.com"],
        },
        public: {
            http: ["https://rpc.testnet.tomochain.com"]
        }
    },
    blockExplorers: {
        etherscan: {
            name: "Viction Scan",
            url: "https://testnet.tomoscan.io/"
        },
        default: {
            name: "Viction Scan",
            url: "https://testnet.tomoscan.io/"
        }
    }

};

const kromaMainnet = {
    id: 255,
    name: "Kroma Mainnet",
    network: "kroma",
    nativeCurrency: {
        decimals: 18,
        name: "ETH",
        symbol: "ETH"
    },
    rpcUrls: {
        default: {
            http: ["https://api.kroma.network"],
        },
        public: {
            http: ["https://api.kroma.network"]
        }
    },
    blockExplorers: {
        etherscan: {
            name: "Kroma Scan",
            url: "https://blockscout.kroma.network/"
        },
        default: {
            name: "Q Scan",
            url: "https://blockscout.kroma.network/"
        }
    }

};

const zetachainMainnet = {
    id: 7000,
    name: "ZetaChain Mainnet",
    network: "zetachainMainnet",
    nativeCurrency: {
        decimals: 18,
        name: "ZETA",
        symbol: "ZETA"
    },
    rpcUrls: {
        default: {
            http: ["https://zetachain-mainnet.blastapi.io/45b6edd3-4e18-4cdb-ab68-0ae1cde51648"],
        },
        public: {
            http: ["https://zetachain-mainnet.blastapi.io/45b6edd3-4e18-4cdb-ab68-0ae1cde51648"]
        }
    },
    blockExplorers: {
        etherscan: {
            name: "zeta Scan",
            url: "https://explorer.zetachain.com"
        },
        default: {
            name: "zeta Scan",
            url: "https://explorer.zetachain.com"
        }
    }

};

const dchainMainnet = {
    id: 2716446429837000,
    name: "Dchain Mainnet",
    network: "dchainMainnet",
    nativeCurrency: {
        decimals: 18,
        name: "ETH",
        symbol: "ETH"
    },
    rpcUrls: {
        default: {
            http: ["https://dchain-2716446429837000-1.jsonrpc.sagarpc.io"],
        },
        public: {
            http: ["https://dchain-2716446429837000-1.jsonrpc.sagarpc.io"]
        }
    },
    blockExplorers: {
        etherscan: {
            name: "dchain Scan",
            url: "https://dchain-2716446429837000-1.sagaexplorer.io"
        },
        default: {
            name: "dchain Scan",
            url: "https://dchain-2716446429837000-1.sagaexplorer.io"
        }
    }
};

const mintMainnet = {
    id: 185,
    name: "Mint Mainnet",
    network: "mint",
    nativeCurrency: {
        decimals: 18,
        name: "ETH",
        symbol: "ETH"
    },
    rpcUrls: {
        default: {
            http: ["https://rpc.mintchain.io/"],
        },
        public: {
            http: ["https://rpc.mintchain.io/"]
        }
    },
    blockExplorers: {
        etherscan: {
            name: "mint Scan",
            url: "https://explorer.mintchain.io/"
        },
        default: {
            name: "mint Scan",
            url: "https://explorer.mintchain.io/"
        }
    }
};


window.chains = [mintMainnet, dchainMainnet, flare, aurora, bscTestnet, auroraTestnet, polygon, eos, mantleTestnet, mantle, zetachainTestnet, skaleTestnet, skaleMainnet, klaytnTestnet, baseMainnet, mantaMainnet, motodexTestnet, toposTestnet, qMainnet, victionTestnet, kromaMainnet, zetachainMainnet]

const projectId = 'd3d0c489a0d7d980f51628258bf880bd'

const { publicClient, webSocketPublicClient } = configureChains(chains, [w3mProvider({ projectId })])
const wagmiConfig = createConfig({
    autoConnect: true,
    connectors: w3mConnectors({ projectId, chains }),
    publicClient,
    webSocketPublicClient
})
window.ethereumClient = new EthereumClient(wagmiConfig, chains)
window.projectId = projectId;

window.waitForTransaction = waitForTransaction
window.fetchTransaction = fetchTransaction
window.switchNetwork = switchNetwork
window.getAccount = getAccount
window.getNetwork = getNetwork
window.fetchBalance = fetchBalance
window.readContracts = readContracts
window.readContract = readContract
window.getWebSocketPublicClient = getWebSocketPublicClient
window.writeContract = writeContract