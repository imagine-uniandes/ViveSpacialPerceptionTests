using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public float Offset;

    // Use this for initialization
    void Start () {
        this.GetComponentInChildren<Image>().enabled = false;
        this.GetComponentInChildren<Text>().enabled = false;

        this.transform.position = GameObject.FindObjectOfType<SteamVR_Camera>().transform.position + GameObject.FindObjectOfType<SteamVR_Camera>().transform.forward * Offset;
        this.transform.parent = GameObject.FindObjectOfType<SteamVR_Camera>().transform;
    }

    // Update is called once per frame
    void Update () {

        this.transform.position = GameObject.FindObjectOfType<SteamVR_Camera>().transform.position + GameObject.FindObjectOfType<SteamVR_Camera>().transform.forward * Offset;
    }
    public void RpcShowMessage(string text)
    {
        this.StartCoroutine(ShowMessageCoroutine(text));
    }

    private IEnumerator ShowMessageCoroutine(string text)
    {
        this.GetComponentInChildren<Image>().enabled = true;
        this.GetComponentInChildren<Text>().enabled = true;
        this.GetComponentInChildren<Text>().text = text;

        yield return new WaitForSeconds(2.0f);

        this.GetComponentInChildren<Image>().enabled = false;
        this.GetComponentInChildren<Text>().enabled = false;

    }
}

