using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersectionSimple : MonoBehaviour
{
    [Header("GameObjects and Materials of intersection")]
    [Tooltip("The northern light of the intersection.\nThe directions of these lights are arbitrary, I set them like this so as to make it easier for me to visualize it.")]
    public GameObject northLight;
    public Material _northLightMaterial;
    [Tooltip("The eastern light of the intersection.\nThe directions of these lights are arbitrary, I set them like this so as to make it easier for me to visualize it.")]
    public GameObject eastLight;
    public Material _eastLightMaterial;
    [Tooltip("The southern light of the intersection.\nThe directions of these lights are arbitrary, I set them like this so as to make it easier for me to visualize it.")]
    public GameObject southLight;
    public Material _southLightMaterial;
    [Tooltip("The western light of the intersection.\nThe directions of these lights are arbitrary, I set them like this so as to make it easier for me to visualize it.")]
    public GameObject westLight;
    public Material _westLightMaterial;

    [Header("Colors of the stoplight")]
    public Color stop;
    public Color slow;
    public Color go;

    private int globalTrafficTimer = 0;
    private int nsSlowTimer = 0;
    private int ewSlowTimer = 0;
    [HideInInspector]
    public bool nsGoing;
    [HideInInspector]
    public bool ewGoing;

    [HideInInspector]
    public enum State
    {
        Stop,
        Go,
        Slow
    }
    [HideInInspector]
    public State northSouthState;
    [HideInInspector]
    public State eastWestState;
    private void Start()
    {
        northSouthState = State.Stop;
        eastWestState = State.Go;
        //ugh this start function is ugly. But this is how it was made. Enjoy
        _northLightMaterial = new Material(northLight.GetComponent<Renderer>().material);
        _eastLightMaterial = new Material(eastLight.GetComponent<Renderer>().material);
        _southLightMaterial = new Material(southLight.GetComponent<Renderer>().material);
        _westLightMaterial = new Material(westLight.GetComponent<Renderer>().material);

        _northLightMaterial.color = stop;
        _eastLightMaterial.color = go;
        _westLightMaterial.color = go;
        _southLightMaterial.color = stop;

        CreateNewMats(northLight, _northLightMaterial);
        CreateNewMats(southLight, _southLightMaterial);
        CreateNewMats(eastLight, _eastLightMaterial);
        CreateNewMats(westLight, _westLightMaterial);
    }
    // Update is called once per frame
    void Update()
    {
        UpdateTrafficSignals();
    }

    void UpdateTrafficSignals()
    {
        switch (northSouthState)
        {
            case State.Go:
                if (globalTrafficTimer >= 2000)
                {
                    SetMats(slow, _northLightMaterial, _southLightMaterial);
                    northSouthState = State.Slow;
                }
                break;
            case State.Stop:
                nsSlowTimer = 0;
                if (globalTrafficTimer >= 3000)
                {
                    SetMats(go, _northLightMaterial, _southLightMaterial);
                    nsGoing = true;
                    northSouthState = State.Go;
                }
                break;
            case State.Slow:
                if(nsSlowTimer >= 1000)
                {
                    nsGoing = false;
                    SetMats(stop, _northLightMaterial, _southLightMaterial);
                    northSouthState = State.Stop;
                }
                nsSlowTimer++;
                break;
        }
        switch (eastWestState)
        {
            case State.Go:
                if (globalTrafficTimer >= 2000)
                {
                    SetMats(slow, _eastLightMaterial, _westLightMaterial);
                    eastWestState = State.Slow;
                }
                break;
            case State.Stop:
                ewSlowTimer = 0;
                if (globalTrafficTimer >= 3000)
                {
                    ewGoing = true;
                    SetMats(go, _eastLightMaterial, _westLightMaterial);
                    eastWestState = State.Go;
                }
                break;
            case State.Slow:
                if (ewSlowTimer >= 1000)
                {
                    ewGoing = false;
                    SetMats(stop, _eastLightMaterial, _westLightMaterial);
                    eastWestState = State.Stop;
                }
                ewSlowTimer++;
                break;
        }
        if (globalTrafficTimer >= 3000) globalTrafficTimer = 0;
        globalTrafficTimer++;
    }
    void SetMats(Color c, Material m1, Material m2)
    {
        m1.color = c;
        m2.color = c;
        m1.SetColor("_EmissionColor", c);
        m2.SetColor("_EmissionColor", c);
    }
    void CreateNewMats(GameObject g, Material m)
    {
        g.GetComponent<Renderer>().material = m;
    }
}
