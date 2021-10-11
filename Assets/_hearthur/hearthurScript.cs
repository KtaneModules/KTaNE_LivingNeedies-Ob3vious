// You wanted to explore him inside I see?

using System.Collections;
using UnityEngine;
using Rnd = UnityEngine.Random;

public class hearthurScript : MonoBehaviour
{

	public KMAudio Audio;
	public KMNeedyModule Module;
	public KMSelectable Module2;
	public Transform Heart;

	private bool positioned = false;
	private bool holding = false;
	private bool active = false;

	void Awake()
	{
		Module.OnNeedyActivation += OnNeedyActivation;
		Module2.OnFocus += delegate { holding = true; };
		Module2.OnDefocus += delegate { holding = false; };
	}

	// Just used to get rid of normal neediness
	void Update()
	{
		if (!positioned)
		{
			Module.transform.localScale /= 2f;
			positioned = true;
			StartCoroutine(HeartBeat());
		}
		Module.SetNeedyTimeRemaining(999999f);
	}

	protected void OnNeedyActivation()
	{
		active = true;
		StartCoroutine(PanicTime());
	}

	private IEnumerator PanicTime()
	{
		while (active)
		{
			if (holding)
			{
				holding = false;
				active = false;
				Module.HandleStrike();
				Module.HandlePass();
			}
			yield return null;
		}
		yield return null;
	}

	private IEnumerator HeartBeat()
	{
		while (true)
		{
            if (active)
            {
				Audio.PlaySoundAtTransform("HeartBeat1", Heart);
				float x = Rnd.Range(0.1f, 0.25f);
				for (float t = 0; t < 1; t += Time.deltaTime / x)
				{
					Heart.localScale = Vector3.Lerp(new Vector3(7f, 7f, 8f), new Vector3(8f, 8f, 8f), t);
					yield return null;
				}
				Audio.PlaySoundAtTransform("HeartBeat2", Heart);
				x = Rnd.Range(0.25f, 0.75f);
				for (float t = 0; t < 1; t += Time.deltaTime / x)
				{
					Heart.localScale = Vector3.Lerp(new Vector3(7f, 7f, 8f), new Vector3(8f, 8f, 8f), t);
					yield return null;
				}
			}
			yield return null;
		}
	}

#pragma warning disable 414
	private string TwitchHelpMessage = "Please don't send anything. You'll only cause problems.";
#pragma warning restore 414
	IEnumerator ProcessTwitchCommand(string command)
	{
		holding = true;
		yield return null;
		holding = false;
	}
	IEnumerator TwitchHandleForcedSolve()
	{
		while (true)
		{
			holding = false;
			yield return true;
		}
	}
}
