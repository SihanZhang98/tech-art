using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;

public class ChangeWeather : MonoBehaviour
{


    [SerializeField] VolumeProfile volumeProfile;
    [SerializeField] GameObject rainParticle;
    VolumetricClouds vClouds;
    Fog fog;




    public void HandleDropdownInputData(int val)
    {
        if (val == 0)
        {
            Clear();
        }

        if (val == 1)
        {
            Cloudy();
        }

        if (val == 2) { 
            Rainy();
        }

        if (val == 3)
        {
            Foggy();
        }

    }

    public void HandleCurrentWeatherData(string val)
    {
        if (val == "Clear")
        {
            Clear();
        }

        if (val == "Clouds")
        {
            Cloudy();
        }

        if (val == "Rain")
        {
            Rainy();
        }

        if (val == "Smoke"|| val == "Fog" || val == "Mist")
        {
            Rainy();
        }

    }



    void Clear()
    {
        rainParticle.SetActive(false);

        if (volumeProfile.TryGet<VolumetricClouds>(out vClouds))
        {
            vClouds.cloudPreset.value = VolumetricClouds.CloudPresets.Sparse;
            Debug.Log(vClouds.cloudPreset);
        }

        if (volumeProfile.TryGet<Fog>(out fog))
        {
            fog.albedo.value = new Color32(6,122,149,255);
            fog.meanFreePath.value = 100;

        }

    }
    void Cloudy()
    {
        rainParticle.SetActive(false);
        if (volumeProfile.TryGet<VolumetricClouds>(out vClouds))
        {
            vClouds.cloudPreset.value = VolumetricClouds.CloudPresets.Custom;
            Debug.Log(vClouds.cloudPreset);
        }

        if (volumeProfile.TryGet<Fog>(out fog))
        {
            fog.albedo.value = new Color32(40, 86, 97, 255);
            fog.meanFreePath.value = 56;

        }

    }

    void Rainy()
    {
        //rain
        rainParticle.SetActive(true);

        //cloud
        if (volumeProfile.TryGet<VolumetricClouds>(out vClouds))
        {
            vClouds.cloudPreset.value = VolumetricClouds.CloudPresets.Stormy;
            Debug.Log(vClouds.cloudPreset);
        }

        //fog
        if (volumeProfile.TryGet<Fog>(out fog))
        {
            fog.albedo.value = new Color32(124,164,173,255);
            fog.meanFreePath.value = 20;
          
        }

    }

    void Foggy()
    {
   
        rainParticle.SetActive(false);

        //cloud
        if (volumeProfile.TryGet<VolumetricClouds>(out vClouds))
        {
            vClouds.cloudPreset.value = VolumetricClouds.CloudPresets.Custom;
            Debug.Log(vClouds.cloudPreset);
        }

        //fog
        if (volumeProfile.TryGet<Fog>(out fog))
        {
            fog.albedo.value = new Color32(142, 223, 241, 255);
            fog.meanFreePath.value = 14;

        }

    }
}
