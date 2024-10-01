mergeInto(LibraryManager.library, {
  Web3Connect: function () {
    console.log("connect jslib", window.web3gl);
    window.web3gl.connect();
  },

  ConnectAccount: function () {
    var bufferSize = lengthBytesUTF8(window.web3gl.connectAccount) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(window.web3gl.connectAccount, buffer, bufferSize);
    return buffer;
  },

  SetConnectAccount: function (value) {
    window.web3gl.connectAccount = value;
  },

  SendContractJs: function (method, abi, contract, args, value, gasLimit, gasPrice) {
    window.web3gl.sendContract(
      UTF8ToString(method),
      UTF8ToString(abi),
      UTF8ToString(contract),
      UTF8ToString(args),
      UTF8ToString(value),
      UTF8ToString(gasLimit),
      UTF8ToString(gasPrice)
    );
  },

  SendContractResponse: function () {
    var bufferSize = lengthBytesUTF8(window.web3gl.sendContractResponse) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(window.web3gl.sendContractResponse, buffer, bufferSize);
    return buffer;
  },

  SetContractResponse: function (value) {
    window.web3gl.sendContractResponse = value;
  },

  SendTransactionJs: function (to, value, gasLimit, gasPrice) {
    window.web3gl.sendTransaction(
      UTF8ToString(to),
      UTF8ToString(value),
      UTF8ToString(gasLimit),
      UTF8ToString(gasPrice)
    );
  },

  SendTransactionResponse: function () {
    var bufferSize = lengthBytesUTF8(window.web3gl.sendTransactionResponse) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(window.web3gl.sendTransactionResponse, buffer, bufferSize);
    return buffer;
  },

  SetTransactionResponse: function (value) {
    window.web3gl.sendTransactionResponse = value;
  },

  SignMessage: function (message) {
    window.web3gl.signMessage(UTF8ToString(message));
  },

  SignMessageResponse: function () {
    var bufferSize = lengthBytesUTF8(window.web3gl.signMessageResponse) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(window.web3gl.signMessageResponse, buffer, bufferSize);
    return buffer; 
  },

  SetSignMessageResponse: function (value) {
    window.web3gl.signMessageResponse = value;
  },

  GetNetwork: function () {
    return window.web3gl.networkId;
  },

  AddNetworkJs: function (message) {
    window.web3gl.addNetwork(UTF8ToString(message));
  },

  AddNetworkResponse: function () {
    var bufferSize = lengthBytesUTF8(window.web3gl.addNetworkResponse) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(window.web3gl.addNetworkResponse, buffer, bufferSize);
    return buffer; 
  },

  SetAddNetworkResponse: function (value) {
    window.web3gl.addNetworkResponse = value;
  },

  ChangeChainIdJs: function (message) {
    console.log("Change chain id jslib", window.web3gl);
    window.web3gl.changeChainId(UTF8ToString(message));
  },
  
  GetAllErc721Js: function (abi, contract) {
    window.web3gl.getAllErc721(
      UTF8ToString(abi),
      UTF8ToString(contract)
    );
  },
  
  GetAllErc721Response: function () {
    var bufferSize = lengthBytesUTF8(window.web3gl.getAllErc721Response) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(window.web3gl.getAllErc721Response, buffer, bufferSize);
    return buffer;
  },
  
  SetAllErc721Response: function (value) {
    window.web3gl.getAllErc721Response = value;
  },
  
  GetLatestEpochJs: function (abi, contract) {
    window.web3gl.getLatestEpoch(
      UTF8ToString(abi),
      UTF8ToString(contract)
    );
  },
    
  GetLatestEpochResponse: function () {
    var bufferSize = lengthBytesUTF8(window.web3gl.getLatestEpochResponse) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(window.web3gl.getLatestEpochResponse, buffer, bufferSize);
    return buffer;
  },
    
  SetLatestEpochResponse: function (value) {
    window.web3gl.getLatestEpochResponse = value;
  },
  
  MethodCallJs: function (abi, contract, method, args, value) {
    window.web3gl.methodCall(
      UTF8ToString(abi),
      UTF8ToString(contract),
      UTF8ToString(method),
      UTF8ToString(args),
      UTF8ToString(value)   
    );
  },
  
  MethodCallResponse: function () {
    console.log("methodCallResponse from js: ", window.web3gl.methodCallResponse);
    var methodCallResponse = window.web3gl.methodCallResponse;
    var bufferSize = lengthBytesUTF8(methodCallResponse) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(methodCallResponse, buffer, bufferSize);
    console.log("Buffer: ", buffer);
    return buffer;
  },
  
  SetMethodCallResponse: function (value) {
    console.log("MethodCallResponse value: ", value);
    window.web3gl.methodCallResponse = value;
  },

  GetTxStatusJs: function (transactionHash) {
    window.web3gl.getTxStatus(
      UTF8ToString(transactionHash) 
    );
  },
  
  GetTxStatusResponse: function () {
    var bufferSize = lengthBytesUTF8(window.web3gl.getTxStatusResponse) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(window.web3gl.getTxStatusResponse, buffer, bufferSize);
    return buffer;
  },
  
  SetTxStatusResponse: function (value) {
    window.web3gl.getTxStatusResponse = value;
  },

  GetBalanceJs: function () {
    window.web3gl.getBalance();
  },
  
  GetBalanceResponse: function () {
    var bufferSize = lengthBytesUTF8(window.web3gl.getBalanceResponse) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(window.web3gl.getBalanceResponse, buffer, bufferSize);
    return buffer;
  },
  
  SetBalanceResponse: function (value) {
    window.web3gl.getBalanceResponse = value;
  },
  
  Web3NearConnect: function (mainnet) {
    window.web3gl.connectNearWallet(
      UTF8ToString(mainnet)
    );
  },
  
  NearConnectAccount: function () {
    var bufferSize = lengthBytesUTF8(window.web3gl.connectNearWalletAccount) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(window.web3gl.connectNearWalletAccount, buffer, bufferSize);
    return buffer;
  },
  
  SetNearConnectAccount: function (value) {
    window.web3gl.connectNearWalletAccount = value;
  },
  
  NearSendContractJs: function (mainnet, motoDexContract, method, args, value) {
    window.web3gl.nearSendContract(
      UTF8ToString(mainnet),
      UTF8ToString(motoDexContract),
      UTF8ToString(method),
      UTF8ToString(args),
      UTF8ToString(value)
    );
  },
  
  NearSendContractResponse: function () {
    var bufferSize = lengthBytesUTF8(window.web3gl.nearSendContractResponse) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(window.web3gl.nearSendContractResponse, buffer, bufferSize);
    return buffer;
  },
  
  NearSetContractResponse: function (value) {
    window.web3gl.nearSendContractResponse = value;
  },
  
  NearMethodCallJs: function (mainnet, motoDexContract, method, args, value) {
    window.web3gl.nearMethodCall(
      UTF8ToString(mainnet),
      UTF8ToString(motoDexContract),
      UTF8ToString(method),
      UTF8ToString(args),
      UTF8ToString(value)
    );
  },
  
  NearMethodCallResponse: function () {
    var bufferSize = lengthBytesUTF8(window.web3gl.nearMethodCallResponse) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(window.web3gl.nearMethodCallResponse, buffer, bufferSize);
    return buffer;
  },
  
  NearSetMethodCallResponse: function (value) {
    window.web3gl.nearMethodCallResponse = value;
  },  
    
  GetListNearNFTsWebJs: function (mainnet, contractAddress, selectedAccount) {
    window.web3gl.listNearNFTsWeb(
      UTF8ToString(mainnet),
      UTF8ToString(contractAddress),
      UTF8ToString(selectedAccount)
    );
  },
  
  GetListNearNFTsWebResponse: function () {
    var bufferSize = lengthBytesUTF8(window.web3gl.listNearNFTsWebResponse) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(window.web3gl.listNearNFTsWebResponse, buffer, bufferSize);
    return buffer;
  },
  
  SetListNearNFTsWebResponse: function (value) {
    window.web3gl.listNearNFTsWebResponse = value;
  },
  
  
  GetNearLatestEpochJs: function (abi, contract) {
    window.web3gl.nearLatestEpoch(
      UTF8ToString(abi),
      UTF8ToString(contract)
    );
  },
    
  GetNearLatestEpochResponse: function () {
    var bufferSize = lengthBytesUTF8(window.web3gl.nearLatestEpochResponse) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(window.web3gl.nearLatestEpochResponse, buffer, bufferSize);
    return buffer;
  },
    
  SetNearLatestEpochResponse: function (value) {
    window.web3gl.nearLatestEpochResponse = value;
  },
  
  WebGLReloadJs: function (value) {
        window.web3gl.webGLReload(
          UTF8ToString(value)
        );
  },
  
  GoogleAnalyticsSendEventJs: function (message) {
      window.web3gl.googleAnalyticsSendEvent(UTF8ToString(message));
  }

});
