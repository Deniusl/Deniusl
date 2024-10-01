export const idlFactory = ({ IDL }) => {
  const Account = IDL.Record({
    'owner' : IDL.Principal,
    'subaccount' : IDL.Opt(IDL.Vec(IDL.Nat8)),
  });
  const InitArg = IDL.Record({
    'icrc7_supply_cap' : IDL.Opt(IDL.Nat),
    'icrc7_description' : IDL.Opt(IDL.Text),
    'tx_window' : IDL.Opt(IDL.Nat64),
    'minting_account' : IDL.Opt(Account),
    'icrc7_max_query_batch_size' : IDL.Opt(IDL.Nat),
    'permitted_drift' : IDL.Opt(IDL.Nat64),
    'icrc7_max_take_value' : IDL.Opt(IDL.Nat),
    'icrc7_max_memo_size' : IDL.Opt(IDL.Nat),
    'icrc7_symbol' : IDL.Text,
    'icrc7_max_update_batch_size' : IDL.Opt(IDL.Nat),
    'icrc7_atomic_batch_transfers' : IDL.Opt(IDL.Bool),
    'icrc7_default_take_value' : IDL.Opt(IDL.Nat),
    'icrc7_logo' : IDL.Opt(IDL.Text),
    'icrc7_name' : IDL.Text,
  });
  const Result = IDL.Variant({ 'Ok' : IDL.Nat, 'Err' : IDL.Text });
  const BurnArg = IDL.Record({
    'token_id' : IDL.Nat,
    'memo' : IDL.Opt(IDL.Vec(IDL.Nat8)),
    'from_subaccount' : IDL.Opt(IDL.Vec(IDL.Nat8)),
  });
  const BurnError = IDL.Variant({
    'GenericError' : IDL.Record({
      'message' : IDL.Text,
      'error_code' : IDL.Nat,
    }),
    'NonExistingTokenId' : IDL.Null,
    'Unauthorized' : IDL.Null,
    'GenericBatchError' : IDL.Record({
      'message' : IDL.Text,
      'error_code' : IDL.Nat,
    }),
  });
  const Result_1 = IDL.Variant({ 'Ok' : IDL.Nat, 'Err' : BurnError });
  const Standard = IDL.Record({ 'url' : IDL.Text, 'name' : IDL.Text });
  const MetadataValue = IDL.Variant({
    'Int' : IDL.Int,
    'Nat' : IDL.Nat,
    'Blob' : IDL.Vec(IDL.Nat8),
    'Text' : IDL.Text,
  });
  const TransferArg = IDL.Record({
    'to' : Account,
    'token_id' : IDL.Nat,
    'memo' : IDL.Opt(IDL.Vec(IDL.Nat8)),
    'from_subaccount' : IDL.Opt(IDL.Vec(IDL.Nat8)),
    'created_at_time' : IDL.Opt(IDL.Nat64),
  });
  const TransferError = IDL.Variant({
    'GenericError' : IDL.Record({
      'message' : IDL.Text,
      'error_code' : IDL.Nat,
    }),
    'Duplicate' : IDL.Record({ 'duplicate_of' : IDL.Nat }),
    'NonExistingTokenId' : IDL.Null,
    'Unauthorized' : IDL.Null,
    'CreatedInFuture' : IDL.Record({ 'ledger_time' : IDL.Nat64 }),
    'InvalidRecipient' : IDL.Null,
    'GenericBatchError' : IDL.Record({
      'message' : IDL.Text,
      'error_code' : IDL.Nat,
    }),
    'TooOld' : IDL.Null,
  });
  const Result_2 = IDL.Variant({ 'Ok' : IDL.Nat, 'Err' : TransferError });
  const MintArg = IDL.Record({
    'to' : Account,
    'token_id' : IDL.Nat,
    'memo' : IDL.Opt(IDL.Vec(IDL.Nat8)),
    'from_subaccount' : IDL.Opt(IDL.Vec(IDL.Nat8)),
    'token_description' : IDL.Opt(IDL.Text),
    'token_logo' : IDL.Opt(IDL.Text),
    'token_name' : IDL.Opt(IDL.Text),
  });
  const MintError = IDL.Variant({
    'GenericError' : IDL.Record({
      'message' : IDL.Text,
      'error_code' : IDL.Nat,
    }),
    'SupplyCapReached' : IDL.Null,
    'Unauthorized' : IDL.Null,
    'GenericBatchError' : IDL.Record({
      'message' : IDL.Text,
      'error_code' : IDL.Nat,
    }),
    'TokenIdAlreadyExist' : IDL.Null,
  });
  const Result_3 = IDL.Variant({ 'Ok' : IDL.Nat, 'Err' : MintError });
  return IDL.Service({
    'add_health_money' : IDL.Func([IDL.Nat, IDL.Nat64], [], []),
    'add_health_nft' : IDL.Func([IDL.Nat, IDL.Nat], [], []),
    'add_moto' : IDL.Func([IDL.Nat], [Result], []),
    'add_track' : IDL.Func([IDL.Nat], [Result], []),
    'burn' : IDL.Func([IDL.Vec(BurnArg)], [IDL.Vec(IDL.Opt(Result_1))], []),
    'create_or_update_game_session_for' : IDL.Func(
        [IDL.Nat, IDL.Nat, IDL.Nat64],
        [],
        [],
      ),
    'get_counter' : IDL.Func([], [IDL.Nat], ['query']),
    'get_current_supply' : IDL.Func([], [IDL.Nat], ['query']),
    'get_epoch_min_interval' : IDL.Func([], [IDL.Nat], ['query']),
    'get_game_server' : IDL.Func([], [Account], ['query']),
    'get_latest_price' : IDL.Func([], [IDL.Nat64], ['query']),
    'get_minimal_fee' : IDL.Func([], [IDL.Nat64], ['query']),
    'get_minimal_fee_rate' : IDL.Func([], [IDL.Nat32], ['query']),
    'get_minimal_fee_usd' : IDL.Func([], [IDL.Nat], ['query']),
    'get_owner' : IDL.Func([], [Account], ['query']),
    'get_price_for_type' : IDL.Func([IDL.Nat8], [IDL.Nat64], ['query']),
    'get_price_main_usd' : IDL.Func([], [IDL.Opt(IDL.Nat64)], ['query']),
    'get_token_health' : IDL.Func([IDL.Nat], [IDL.Nat64], ['query']),
    'get_token_type_nft' : IDL.Func([IDL.Nat], [IDL.Nat64], ['query']),
    'icrc7_atomic_batch_transfers' : IDL.Func(
        [],
        [IDL.Opt(IDL.Bool)],
        ['query'],
      ),
    'icrc7_balance_of' : IDL.Func(
        [IDL.Vec(Account)],
        [IDL.Vec(IDL.Nat)],
        ['query'],
      ),
    'icrc7_default_take_value' : IDL.Func([], [IDL.Opt(IDL.Nat)], ['query']),
    'icrc7_description' : IDL.Func([], [IDL.Opt(IDL.Text)], ['query']),
    'icrc7_logo' : IDL.Func([], [IDL.Opt(IDL.Text)], ['query']),
    'icrc7_max_memo_size' : IDL.Func([], [IDL.Opt(IDL.Nat)], ['query']),
    'icrc7_max_query_batch_size' : IDL.Func([], [IDL.Opt(IDL.Nat)], ['query']),
    'icrc7_max_take_value' : IDL.Func([], [IDL.Opt(IDL.Nat)], ['query']),
    'icrc7_max_update_batch_size' : IDL.Func([], [IDL.Opt(IDL.Nat)], ['query']),
    'icrc7_minting_authority' : IDL.Func([], [Account], ['query']),
    'icrc7_name' : IDL.Func([], [IDL.Text], ['query']),
    'icrc7_owner_of' : IDL.Func(
        [IDL.Vec(IDL.Nat)],
        [IDL.Vec(IDL.Opt(Account))],
        ['query'],
      ),
    'icrc7_supply_cap' : IDL.Func([], [IDL.Opt(IDL.Nat)], ['query']),
    'icrc7_supported_standards' : IDL.Func([], [IDL.Vec(Standard)], ['query']),
    'icrc7_symbol' : IDL.Func([], [IDL.Text], ['query']),
    'icrc7_token_metadata' : IDL.Func(
        [IDL.Vec(IDL.Nat)],
        [IDL.Vec(IDL.Opt(IDL.Vec(IDL.Tuple(IDL.Text, MetadataValue))))],
        ['query'],
      ),
    'icrc7_tokens' : IDL.Func(
        [IDL.Opt(IDL.Nat), IDL.Opt(IDL.Nat)],
        [IDL.Vec(IDL.Nat)],
        ['query'],
      ),
    'icrc7_tokens_of' : IDL.Func(
        [Account, IDL.Opt(IDL.Nat), IDL.Opt(IDL.Nat)],
        [IDL.Vec(IDL.Nat)],
        ['query'],
      ),
    'icrc7_total_supply' : IDL.Func([], [IDL.Nat], ['query']),
    'icrc7_transfer' : IDL.Func(
        [IDL.Vec(TransferArg)],
        [IDL.Vec(IDL.Opt(Result_2))],
        [],
      ),
    'mint' : IDL.Func([MintArg], [Result_3], []),
    'motodex' : IDL.Func([IDL.Text], [], ['query']),
    'nft_mint_batch' : IDL.Func([IDL.Vec(Account), IDL.Vec(IDL.Nat8)], [], []),
    'purchase' : IDL.Func([IDL.Nat8], [Result], []),
    'purchase_batch' : IDL.Func(
        [IDL.Vec(IDL.Nat8), IDL.Opt(Account), IDL.Nat64],
        [],
        [],
      ),
    'purchase_batch_preview' : IDL.Func(
        [IDL.Vec(IDL.Nat8)],
        [IDL.Nat],
        ['query'],
      ),
    'remove_game_session' : IDL.Func([IDL.Nat], [], []),
    'return_moto' : IDL.Func([IDL.Nat], [Result], []),
    'return_track' : IDL.Func([IDL.Nat], [Result], []),
    'set_counter' : IDL.Func([IDL.Nat], [], []),
    'set_game_server' : IDL.Func([Account], [], []),
    'set_health_for_token' : IDL.Func([IDL.Nat, IDL.Nat64], [], []),
    'set_health_for_token_admin' : IDL.Func([IDL.Nat, IDL.Nat64], [], []),
    'set_main_price' : IDL.Func([IDL.Nat64], [], []),
    'set_max_moto_per_game' : IDL.Func([IDL.Nat64], [], []),
    'set_minimal_fee' : IDL.Func([IDL.Nat32], [], []),
    'set_motodex_health' : IDL.Func([IDL.Text], [], ['query']),
    'set_owner' : IDL.Func([Account], [], []),
    'set_percent_for_track_owner' : IDL.Func([IDL.Nat, IDL.Nat8], [], []),
    'set_price_for_type_admin' : IDL.Func([IDL.Nat8, IDL.Nat64], [], []),
    'set_types_admin' : IDL.Func([], [], []),
    'sync_epoch_game_session' : IDL.Func([IDL.Nat], [], []),
    'token_ids_and_owners' : IDL.Func(
        [IDL.Opt(IDL.Nat), IDL.Opt(IDL.Nat64)],
        [IDL.Text],
        ['query'],
      ),
    'update_counter' : IDL.Func([], [], []),
    'value_in_main_coin' : IDL.Func([IDL.Nat8], [IDL.Nat64], ['query']),
  });
};
export const init = ({ IDL }) => {
  const Account = IDL.Record({
    'owner' : IDL.Principal,
    'subaccount' : IDL.Opt(IDL.Vec(IDL.Nat8)),
  });
  const InitArg = IDL.Record({
    'icrc7_supply_cap' : IDL.Opt(IDL.Nat),
    'icrc7_description' : IDL.Opt(IDL.Text),
    'tx_window' : IDL.Opt(IDL.Nat64),
    'minting_account' : IDL.Opt(Account),
    'icrc7_max_query_batch_size' : IDL.Opt(IDL.Nat),
    'permitted_drift' : IDL.Opt(IDL.Nat64),
    'icrc7_max_take_value' : IDL.Opt(IDL.Nat),
    'icrc7_max_memo_size' : IDL.Opt(IDL.Nat),
    'icrc7_symbol' : IDL.Text,
    'icrc7_max_update_batch_size' : IDL.Opt(IDL.Nat),
    'icrc7_atomic_batch_transfers' : IDL.Opt(IDL.Bool),
    'icrc7_default_take_value' : IDL.Opt(IDL.Nat),
    'icrc7_logo' : IDL.Opt(IDL.Text),
    'icrc7_name' : IDL.Text,
  });
  return [InitArg];
};
