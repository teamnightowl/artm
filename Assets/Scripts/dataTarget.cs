using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Vuforia
{
	public class dataTarget : MonoBehaviour {

		public Transform Amount;
		public Transform BalanceInquiry;
		public Transform Withdraw;
		public Transform Deposit;
		public Transform Back;
		public Transform Numbers;
		public Coroutine co;

		private bool click;
		private bool state;

		// Use this for initialization
		void Start()
		{
			click = true;
			//add Audio Source as new game object component
			//soundTarget = (AudioSource)gameObject.AddComponent<AudioSource>();
		}

		// Update is called once per frame
		void Update()
		{
			StateManager sm = TrackerManager.Instance.GetStateManager();
			IEnumerable<TrackableBehaviour> tbs = sm.GetActiveTrackableBehaviours();

			BalanceInquiry.GetComponent<Button> ().onClick.AddListener (delegate {
				if(click){
					click = false;
					BalanceInquiry.gameObject.SetActive (false);
					Withdraw.gameObject.SetActive (false);
					Deposit.gameObject.SetActive (false);
					co = StartCoroutine(showBalance());
				}
			});

			Back.GetComponent<Button> ().onClick.AddListener (delegate {
				hideAmount();
			});

			Withdraw.GetComponent<Button> ().onClick.AddListener (delegate {
				if(click){
					click = false;
					state = false;
					BalanceInquiry.gameObject.SetActive(false);
					Withdraw.gameObject.SetActive(false);
					Deposit.gameObject.SetActive(false);
					Numbers.gameObject.SetActive(true);
					click = true;
				}
			});

			Deposit.GetComponent<Button> ().onClick.AddListener (delegate {
				if(click){
					click = false;
					state = true;
					BalanceInquiry.gameObject.SetActive(false);
					Withdraw.gameObject.SetActive(false);
					Deposit.gameObject.SetActive(false);
					Numbers.gameObject.SetActive(true);
					click = true;
				}	
			});

			Numbers.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate {
				if(click){
					click = false;
					Numbers.gameObject.SetActive(false);
					co  = (state) ? StartCoroutine(deposit(500)) : StartCoroutine(withdraw(500));
				}	
			});

			Numbers.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate {
				if(click){
					click = false;
					Numbers.gameObject.SetActive(false);
					co = (state) ? StartCoroutine(deposit(1000)) : StartCoroutine(withdraw(1000));
				}	
			});

			Numbers.GetChild(2).GetComponent<Button>().onClick.AddListener(delegate {
				if(click){
					click = false;
					Numbers.gameObject.SetActive(false);
					co = (state) ? StartCoroutine(deposit(2000)) : StartCoroutine(withdraw(2000));
				}	
			});

			foreach (TrackableBehaviour tb in tbs)
			{
				string name = tb.TrackableName;
				ImageTarget it = tb.Trackable as ImageTarget;
				Vector2 size = it.GetSize();

				Debug.Log("Active image target:" + name + "  -size: " + size.x + ", " + size.y);

			}
		}

		IEnumerator getBalance(){
			WWW www = new WWW ("http://192.168.8.100/uplus-serve/balanceinquiry.php?id=100445808459");
			yield return www;
			if (www.error != null) {
				Debug.Log ("Error" + www.error);
			} else {
				Debug.Log (www.text);
			}
		}

		private IEnumerator showBalance(){
			WWW www = new WWW ("http://192.168.8.100/uplus-serve/balanceinquiry.php?id=100445808459");
			yield return www;
			if (www.error != null) {
				Debug.Log ("Error" + www.error);
			} else {
				Debug.Log (www.text);
				Amount.GetComponent<Text> ().text = www.text;
				Back.gameObject.SetActive (true);
				Amount.gameObject.SetActive (true);
				click = true;
				StopCoroutine (co);
			}

		}

		private IEnumerator withdraw(int amount){
			WWW www = new WWW ("http://192.168.8.100/uplus-serve/withdraw.php?acct1=100445808459&acct2=100370848420&amount=" + amount);
			yield return www;
			if (www.error != null) {
				Debug.Log ("Error" + www.error);
			} else {
				Debug.Log (www.text);
				if (www.text == "1") {
					Amount.GetComponent<Text> ().text = "Withdraw Successful";
				} else {
					Amount.GetComponent<Text> ().text = "Withdraw Failed!";
				}
				Numbers.gameObject.SetActive (false);
				Back.gameObject.SetActive (true);
				Amount.gameObject.SetActive (true);
				click = true;
				StopCoroutine (co);

			}
		}

		private IEnumerator deposit(int amount){
			WWW www = new WWW ("http://192.168.8.100/uplus-serve/deposit.php?acct1=100445808459&acct2=100370848420&amount=" + amount);
			yield return www;
			if (www.error != null) {
				Debug.Log ("Error" + www.error);
			} else {
				Debug.Log (www.text);
				if (www.text == "1") {
					Amount.GetComponent<Text> ().text = "Deposit Successful!";
				} else {
					Amount.GetComponent<Text> ().text = "Deposit Failed!";
				}
				Numbers.gameObject.SetActive (false);
				Back.gameObject.SetActive (true);
				Amount.gameObject.SetActive (true);
				click = true;
				StopCoroutine (co);
			}
		}
			

		private void hideAmount(){
			BalanceInquiry.gameObject.SetActive (true);
			Withdraw.gameObject.SetActive (true);
			Deposit.gameObject.SetActive (true);
			Back.gameObject.SetActive (false);
			Amount.gameObject.SetActive (false);			
		}


	}
}
