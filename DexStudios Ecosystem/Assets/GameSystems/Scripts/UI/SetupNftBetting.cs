using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameSystems.Scripts;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using Network = GameSystems.Scripts.Network;

namespace UI
{
	public class SetupNftBetting : MonoBehaviour
	{
		[SerializeField] private GameObject[] _nftItemComponents;
		[SerializeField] private GameObject _NftBid;
		[SerializeField] private GameObject _scrollbar;
		[SerializeField] private GameObject _loadingSpinner;
		[SerializeField] private GameObject _epoch;
		[SerializeField] private GameObject _connectWallet;
			
		private BidContract[] _bidContracts;

		public void Setup(BidContract[] items, Action onSetupComplete = null)
		{
			if (items == null) return;
			_nftItemComponents = new GameObject[items.Length];
			for (int i = 0; i < items.Length; i++)
			{
				_nftItemComponents[i] = Instantiate(_NftBid, transform);
				_nftItemComponents[i].GetComponent<NFTBid>().Setup(items[i].trackId, items[i].trackType,
					items[i].motoId, items[i].motoType, items[i].time.ToString(CultureInfo.InvariantCulture), 
					items[i].motoHealth, items[i].totalBids, items[i].playerBidsValue);
				_nftItemComponents[i].SetActive(true);
			}
			onSetupComplete?.Invoke();
		}

		private async void OnEnable()
		{
			await SetBids();
	
			_epoch.SetActive(true);
			if (gameObject.activeInHierarchy)
				_epoch.transform.GetChild(0).gameObject.SetActive(true);
		}

		private void OnDisable()
		{
			_epoch.transform.GetChild(0).gameObject.SetActive(false);
		}

		private async UniTask SetBids()
		{
			if (!_connectWallet.activeSelf)
			{
				_loadingSpinner.SetActive(true);
				foreach (Transform child in transform)
					Destroy(child.gameObject);
				_bidContracts = await ContractHelper.GetGameSessions();
				//SortByLess(ref _bidContracts);
				Array.Sort(_bidContracts, ContractHelper.CompareContractByTime);
				if (Invites.bidCombination.Length > 1)
				{
					for (int i = 0; i < _bidContracts.Length; i++)
					{
						if (_bidContracts[i].motoId == Invites.bidCombination[0] &&
						    _bidContracts[i].trackId == Invites.bidCombination[1])
						{
							List<BidContract> tempBidContracts = _bidContracts.ToList();
							BidContract tempBidContract = _bidContracts[i];
							tempBidContracts.RemoveAt(i);
							tempBidContracts.Insert(0, tempBidContract);
							_bidContracts = tempBidContracts.ToArray();
							break;
						}
					}
				}
			}

			Setup(_bidContracts, () =>
			{
				_loadingSpinner.SetActive(false);
				StartCoroutine("InitializeScrollbar");
			});
		}
		
		private void SortByLess(ref BidContract[] bidContracts)
		{
			for (int i = 0; i < bidContracts.Length; i++)
			{
				if (bidContracts.Length == 1)
				{
					continue;
				}
		
				for (int j = i + 1; j < bidContracts.Length; j++)
				{
					if (bidContracts[j].timeValue < bidContracts[i].timeValue)
					{
						// Swap
						(bidContracts[i], bidContracts[j]) = (bidContracts[j], bidContracts[i]);
					}
				}
			}
		}

		public void SetInteractable(bool isInteractable)
        {
            foreach(var nftItemComponent in _nftItemComponents)
            {
                nftItemComponent.GetComponent<NFTBid>().SetInteractable(isInteractable);
            }
        }
		

		public void ItemsAddListener()
		{
			foreach (var nftItemComponent in _nftItemComponents)
			{
				nftItemComponent.GetComponent<NFTBid>().AddListeners();
			}
		}
		public void ItemsRemoveListener()
		{
			foreach (var nftItemComponent in _nftItemComponents)
			{
				nftItemComponent.GetComponent<NFTBid>().RemoveListeners();
			}
		}
		
		private IEnumerator InitializeScrollbar()
		{
			yield return new WaitForEndOfFrame();
			
			_scrollbar.GetComponent<Scrollbar>().value = 1;
		}
		
		public void Show()
		{
			gameObject.SetActive(true);
		}

		public void Hide()
		{
			gameObject.SetActive(false);
		}
	}
}