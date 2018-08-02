using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FlipperManager : MonoBehaviour
{
	//Okaun P/T Calc
	[Header("Okaun P/T Calc")]
	[SerializeField] private InputField okaunPower;
	[SerializeField] private InputField okaunToughness;
	[SerializeField] private TextMeshProUGUI okaunOutputText;
	private int OP;
	private int OT;
	[SerializeField] private Button quickSetOkaunButton;
	//Krark's Thumb
	[Header("Krark's Thumb")]
	[SerializeField] private Switch thumbSwitch;
	[SerializeField] private TextMeshProUGUI thumbQuantityText;
	[SerializeField] private Button addThumbButton;
	[SerializeField] private Button removeThumbButton;
	private int numThumbs;
	//Output
	[Header("Output")]
	[SerializeField] private TextMeshProUGUI outputText;
	[SerializeField] private Button stopButton;
	//NumFlip
	[Header("Specific Number of Flips")]
	[SerializeField] private InputField numFlips;
	[SerializeField] private Button flipButton;
	[SerializeField] private Button pressYourLuckButton;
	private bool flipRunning = false;
	//ChainFlip
	[Header("Chain Flips")]
	[SerializeField] private Button chainFlipButton;
	[SerializeField] private Button twoChainFlipButton;
	//QuickFlip
	[Header("Quick Flip")]
	[SerializeField] private Button quickFlipButton;

	void Awake() {
		ResetALL();
	}

	public void ResetALL() {
		outputText.text = "";
		numFlips.text = "";
		numThumbs = 1;
		thumbQuantityText.text = "1";
		StopAllCoroutines();
		flipRunning = false;
		SetButtonInteractions(true);
		thumbSwitch.isOn = false;
		ResetOkaun();
	}


	private void SetButtonInteractions(bool interactable) {
		flipButton.interactable = interactable;
		pressYourLuckButton.interactable = interactable;
		chainFlipButton.interactable = interactable;
		twoChainFlipButton.interactable = interactable;
		quickFlipButton.interactable = interactable;
		addThumbButton.interactable = interactable;
		removeThumbButton.interactable = interactable;
		quickSetOkaunButton.interactable = interactable;
		stopButton.gameObject.SetActive(!interactable);
	}

	public bool Flip(int playerCall = 0) {
		bool call = false;
		bool flip = false;
		if (playerCall < 0) {
			call = false;
		} else if (playerCall > 0) {
			call = true;
		} else {
			if (Random.Range(0, 2) > 0.5f) {
				call = true;
			} else {
				call = false;
			}
		}
		if (Random.Range(0, 2) > 0.5f) {
			flip = true;
		}
		if (flip == call) {
			return true;
		} else {
			return false;
		}
	}

	private bool KrarkFlip() {
		if (numThumbs < 1) {
			numThumbs = 1;
		}
		for (int i = 0; i < Mathf.Pow(2, numThumbs); i++) {
			if (Flip()) {
				return true;
			}
		}

		return false;
	}

	public void ClickFlip() {
		if (!flipRunning) {
			int flips = 0;
			if (numFlips.text != "") {
				flips = int.Parse(numFlips.text);
			}
			if (flips > 99999) {
				flips = 99999;
				numFlips.text = flips.ToString();
			}
			if (flips > 0) {
				SetButtonInteractions(false);
				StartCoroutine(FlipCoins(flips));
			}
		}
	}

	IEnumerator FlipCoins(int num) {
		flipRunning = true;
		outputText.text = "-";
		yield return null;
		int runningTotal = 0;
		bool success = false;
		for (int i = 0; i < num; i++) {
			success = false;
			if (thumbSwitch.isOn) {
				success = KrarkFlip();
			} else if (Flip()) {
				success = true;
			}

			if (success) {
				runningTotal++;
			}

			if (i % 10 == 0) {
				outputText.text = runningTotal.ToString();
				yield return null;
			}
		}
		CalcOkaunStats(runningTotal);
		outputText.text = runningTotal.ToString();
		flipRunning = false;
		SetButtonInteractions(true);
		yield return null;
	}

	public void ClickPressYourLuck() {
		if (!flipRunning) {
			int flips = 0;
			if (numFlips.text != "") {
				flips = int.Parse(numFlips.text);
			}
			if (flips > 99999) {
				flips = 99999;
				numFlips.text = flips.ToString();
			}
			if (flips > 0) {
				SetButtonInteractions(false);
				StartCoroutine(PressYourLuck(flips));
			}
		}
	}

	IEnumerator PressYourLuck(int num) {
		flipRunning = true;
		outputText.text = "-";
		yield return null;
		int runningTotal = 0;
		bool success = false;
		for (int i = 0; i < num; i++) {
			success = false;
			if (thumbSwitch.isOn) {
				success = KrarkFlip();
			} else if (Flip()) {
				success = true;
			}

			if (success) {
				runningTotal++;
			} else {
				break;
			}

			if (i % 10 == 0) {
				outputText.text = runningTotal.ToString();
				yield return null;
			}
		}


		CalcOkaunStats(runningTotal);
		outputText.text = runningTotal.ToString() + " of your " + num.ToString() + " flip goal.";
		flipRunning = false;
		SetButtonInteractions(true);
		yield return null;
	}

	public void ClickChainFlip() {
		if (!flipRunning) {
			SetButtonInteractions(false);
			StartCoroutine(ChainFlip());
		}
	}

	IEnumerator ChainFlip() {
		flipRunning = true;
		outputText.text = "-";
		yield return null;
		bool success = false;
		int runningTotal = 0;
		while (true) {
			success = false;
			if (thumbSwitch.isOn) {
				success = KrarkFlip();
			} else if (Flip()) {
				success = true;
			}

			if (success) {
				runningTotal++;
			} else {
				break;
			}
			if (runningTotal % 10 == 0) {
				outputText.text = runningTotal.ToString();
				yield return null;
			}
		}
		CalcOkaunStats(runningTotal);
		outputText.text = runningTotal.ToString();
		flipRunning = false;
		SetButtonInteractions(true);
		yield return null;
	}

	public void ClickDoubleChain() {
		if (!flipRunning) {
			SetButtonInteractions(false);
			StartCoroutine(DoubleChain());
		}
	}

	IEnumerator DoubleChain() {
		flipRunning = true;
		outputText.text = "-";
		yield return null;
		bool success = false;
		int runningTotal = 0;
		for (int i = 0; i < 2; i++) {
			while (true) {
				success = false;
				if (thumbSwitch.isOn) {
					success = KrarkFlip();
				} else if (Flip()) {
					success = true;
				}

				if (success) {
					runningTotal++;
				} else {
					break;
				}
				if (runningTotal % 10 == 0) {
					outputText.text = runningTotal.ToString();
					yield return null;
				}
			}
		}
		CalcOkaunStats(runningTotal);
		outputText.text = runningTotal.ToString();
		flipRunning = false;
		SetButtonInteractions(true);
		yield return null;
	}

	public void ClickQuickFlip() {
		if (!flipRunning) {
			SetButtonInteractions(false);
			StartCoroutine(QuickFlip());
		}
	}

	IEnumerator QuickFlip() {
		flipRunning = true;
		outputText.text = "-";
		yield return null;
		bool success = false;
		if (thumbSwitch.isOn) {
			success = KrarkFlip();
		} else {
			success = Flip();
		}

		if (success) { 
			outputText.text = "<color=green>WIN</color>";
			CalcOkaunStats(1);
		} else {
			outputText.text = "<color=red>LOSE</color>";
			CalcOkaunStats(0);
		}

		flipRunning = false;
		SetButtonInteractions(true);
		yield return null;
	}

	public void AddThumb() {
		numThumbs++;
		thumbQuantityText.text = numThumbs.ToString();
	}

	public void RemoveThumb() {
		numThumbs--;
		if (numThumbs < 1) {
			numThumbs = 1;
		}
		thumbQuantityText.text = numThumbs.ToString();
	}

	private void ResetOkaun() {
		OP = 3;
		OT = 3;
		okaunPower.text = "3";
		okaunToughness.text = "3";
		okaunOutputText.text = "3/3";
	}

	private void CalcOkaunStats(int wins) {
		if (okaunPower.text == "") {
			OP = 0;
			okaunPower.text = "0";
		} else {
			OP = int.Parse(okaunPower.text);
		}
		if (okaunToughness.text == "") {
			OT = 0;
			okaunToughness.text = "0";
		} else {
			OT = int.Parse(okaunToughness.text);
		}

		OP = OP * Mathf.RoundToInt(Mathf.Pow(2, wins));
		OT = OT * Mathf.RoundToInt(Mathf.Pow(2, wins));
		if (OP > 10000000 || OP < 0) {
			okaunOutputText.text = "big / big";
		} else {
			okaunOutputText.text = OP.ToString() + "/" + OT.ToString();
		}
	}

	public void QuickSetOkaun() {
		okaunPower.text = OP.ToString();
		okaunToughness.text = OT.ToString();
	}

	public void StopButton() {
		StopAllCoroutines();
		flipRunning = false;
		SetButtonInteractions(true);
	}
}