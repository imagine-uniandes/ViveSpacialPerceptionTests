using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class FadeController : MonoBehaviour
{
    public float FadingSpeed = 1.0f; // The speed of the effect
    public float FadeValue = 0;
    public float Offset;


    public void Start()
    {
        this.transform.position = GameObject.FindObjectOfType<SteamVR_Camera>().transform.position + GameObject.FindObjectOfType<SteamVR_Camera>().transform.forward * Offset;
        this.transform.parent = GameObject.FindObjectOfType<SteamVR_Camera>().transform;
        this.transform.localScale = Vector3.zero;
    }

    
    public void RpcBeginFade()
    {
        this.transform.localScale = Vector3.one;
        //this.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 0, 1.0f);
        FadeValue = 1.0f;
        //this.StartCoroutine(OnBeginFade());
    }

    public void RpcEndFade()
    {
        this.transform.localScale = Vector3.zero;
        //this.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 0, 0.0f);
        FadeValue = 0.0f;
        //this.StartCoroutine(OnEndFade());
    }

    /*IEnumerator OnBeginFade()
    {
        m_FadeValue = 0.0f;
        Color c = this.GetComponent<MeshRenderer>().material.color;        
        while (m_FadeValue < 1.0)
        {
            Debug.Log("BeginFade!");
            m_FadeValue += FadingSpeed * Time.deltaTime;            
            this.GetComponent<MeshRenderer>().material.color = new Color(c.r, c.g, c.b, m_FadeValue);

            yield return new WaitForEndOfFrame();
        }

        this.GetComponent<MeshRenderer>().material.color = new Color(c.r, c.g, c.b, 1.0f);
    }

    IEnumerator OnEndFade()
    {        
        m_FadeValue = 1.0f;
        Color c = this.GetComponent<MeshRenderer>().material.color;
        Debug.Log("EndFade!");
        while (m_FadeValue > 0.0)
        {
            m_FadeValue += FadingSpeed * Time.deltaTime;
            this.GetComponent<MeshRenderer>().material.color = new Color(c.r, c.g, c.b, 0);

            yield return new WaitForEndOfFrame();
        }
    }*/
}
