	import {
		Aptos, AptosConfig, Network
	} from 'https://cdn.jsdelivr.net/npm/@aptos-labs/ts-sdk@latest/+esm'
	const aptosConfig = new AptosConfig({ network: Network.MAINNET });
	const aptos = new Aptos(aptosConfig);

	import {
		WalletCore
	} from 'https://cdn.jsdelivr.net/npm/@aptos-labs/wallet-adapter-core@latest/+esm' // https://github.com/aptos-labs/aptos-wallet-adapter/blob/main/packages/wallet-adapter-core/src/WalletCore.ts

	import {
		PetraWallet
	} from 'https://cdn.jsdelivr.net/npm/petra-plugin-wallet-adapter@latest/+esm' // https://github.com/aptos-labs/petra-plugin-wallet-adapter/blob/main/src/index.ts


	const petraWallet = new PetraWallet();
	const walletCore = new WalletCore([petraWallet],[]);
	// const account = await petraWallet.account();

	window.petraWallet = petraWallet
	window.walletCore = walletCore
	window.aptos = aptos
