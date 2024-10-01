import {
	SigningStargateClient, StargateClient, GasPrice, coin
} from 'https://cdn.jsdelivr.net/npm/@cosmjs/stargate@0.32.3/+esm'
import { SigningCosmWasmClient } from "https://cdn.jsdelivr.net/npm/@cosmjs/cosmwasm-stargate@0.32.3/+esm";

window.SigningCosmWasmClient = SigningCosmWasmClient

window.SigningStargateClient = SigningStargateClient
window.StargateClient = StargateClient
window.GasPrice = GasPrice
window.coin = coin
// import {
//   TxRaw
// } from 'https://cdn.jsdelivr.net/npm/cosmjs-types@0.9.0/+esm'

const nibiruChainInfoTestnet = {
	chainId: "nibiru-testnet-1", // Replace with "nibiru-testnet-1" for testnet
	chainName: "nibiru-testnet-1", // Replace with "nibiru-testnet-1" for testnet
	rpc: "https://rpc.testnet-1.nibiru.fi:443", // Replace with testnet URL if needed
	rest: "https://lcd.testnet-1.nibiru.fi:443", // Replace with testnet URL if needed
	stakeCurrency: {
		coinDenom: "NIBI",
		coinMinimalDenom: "unibi",
		coinDecimals: 6,
	},
	bip44: {
		coinType: 118,
	},
	bech32Config: {
		bech32PrefixAccAddr: "nibi",
		bech32PrefixAccPub: "nibipub",
		bech32PrefixValAddr: "nibivaloper",
		bech32PrefixValPub: "nibivaloperpub",
		bech32PrefixConsAddr: "nibivalcons",
		bech32PrefixConsPub: "nibivalconspub",
	},
	currencies: [
		{
			coinDenom: "NIBI",
			coinMinimalDenom: "unibi",
			coinDecimals: 6,
		},
	],
	feeCurrencies: [
		{
			coinDenom: "NIBI",
			coinMinimalDenom: "unibi",
			coinDecimals: 6,
			gasPriceStep: {
				low: 0.025,
				average: 0.05,
				high: 0.1,
			},
		},
	],
};

window.nibiruChainInfoTestnet = nibiruChainInfoTestnet;


const nibiruChainInfoMainnet = {
	chainId: "cataclysm-1", // Replace with "nibiru-testnet-1" for testnet
	chainName: "cataclysm-1", // Replace with "nibiru-testnet-1" for testnet
	rpc: "https://rpc.nibiru.fi", // Replace with testnet URL if needed
	rest: "https://lcd.nibiru.fi", // Replace with testnet URL if needed
	stakeCurrency: {
		coinDenom: "NIBI",
		coinMinimalDenom: "unibi",
		coinDecimals: 6,
	},
	bip44: {
		coinType: 118,
	},
	bech32Config: {
		bech32PrefixAccAddr: "nibi",
		bech32PrefixAccPub: "nibipub",
		bech32PrefixValAddr: "nibivaloper",
		bech32PrefixValPub: "nibivaloperpub",
		bech32PrefixConsAddr: "nibivalcons",
		bech32PrefixConsPub: "nibivalconspub",
	},
	currencies: [
		{
			coinDenom: "NIBI",
			coinMinimalDenom: "unibi",
			coinDecimals: 6,
		},
	],
	feeCurrencies: [
		{
			coinDenom: "NIBI",
			coinMinimalDenom: "unibi",
			coinDecimals: 6,
			gasPriceStep: {
				low: 0.025,
				average: 0.05,
				high: 0.1,
			},
		},
	],
};

window.nibiruChainInfoMainnet = nibiruChainInfoMainnet;











// // Dynamically load CosmJS Stargate script
// const script = document.createElement("script");
// script.src = "https://cdn.jsdelivr.net/npm/@nibiruchain/nibijs";
// script.onload = async () => {
// 	// const contractAddress = "nibi1tnce5ynlucdml9h8nl7aefa04097qtl8nejhuexz90t6pxp3xwwsuu2ud4"
//
// 	// Use CosmJS once it's loaded
// 	eval(script.textContent); // Evaluate the loaded script
// 	// const client = await StargateClient.connect(chainInfo.rpc);
//
// 	const signingClient =
// 		await SigningStargateClient.connectWithSigner(
// 			chainInfo.rpc,
// 			offlineSigner,
// 		);
//
// 	const wasmClient = await SigningCosmWasmClient.connectWithSigner(
// 		chainInfo.rpc,
// 		offlineSigner,
// 		{gasPrice: GasPrice.fromString("25000unibi")}
// 	);
//
// 	console.log(
// 		"With client, chain id:",
// 		await signingClient.getChainId(),
// 		", height:",
// 		await signingClient.getHeight()
// 	);
// 	console.log(
// 		accounts[0].address + " balances:",
// 		await signingClient.getAllBalances(accounts[0].address)
// 	); // <-- replace with your generated address
//
// 	console.log("wasm client", wasmClient);
// 	console.log("Wallet connected:", offlineSigner);
// 	window.nibiruWasmClient = wasmClient;
//
// 	// let counter =
// 	// 	await wasmClient.queryClient.wasm.queryContractSmart(
// 	// 		contractAddress,
// 	// 		{
// 	// 			get_counter: {},
// 	// 		}
// 	// 	);
// 	// console.log("1 get_counter", counter);
// 	//
// 	// counter =
// 	// 	await wasmClient.queryClient.wasm.queryContractSmart(
// 	// 		contractAddress,
// 	// 		{
// 	// 			get_counter: {},
// 	// 		}
// 	// 	);
// 	// console.log("2 get_counter", counter);
// 	// const value_in_main_coin =
// 	// 	await wasmClient.queryClient.wasm.queryContractSmart(
// 	// 		contractAddress,
// 	// 		{
// 	// 			value_in_main_coin: { "type_nft":0 },
// 	// 		}
// 	// 	);
// 	// console.log(" value_in_main_coin", value_in_main_coin);
// 	//
// 	// let coinResult = coin(value_in_main_coin.price + '', chainInfo.currencies[0].coinMinimalDenom);
// 	// console.log(" coinResult", coinResult);
// 	//
// 	// let funds = [coinResult];
// 	//
// 	// const contractExecutePurchase =
// 	// 	await wasmClient.execute(
// 	// 		accounts[0].address,
// 	// 		contractAddress,
// 	// 		{
// 	// 			//execution message
// 	// 			"purchase": { "type_nft":0, "extension":{} },
// 	// 		},
// 	// 		"auto",
// 	// 		"",
// 	// 		funds
// 	// 	);
// 	// console.log(contractExecutePurchase);
// 	// const events = contractExecutePurchase.events
// 	// const execute = events[12]
// 	// const token_id = execute.attributes[2]
// 	// console.log("token_id",token_id.token_id);
// };
// document.head.appendChild(script);
