using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBotton : MonoBehaviour
{
    public GameObject toggle1;
    public GameObject toggle2;
    public GameObject toggle3;
    private static bool onOff1 = false;
    private static bool onOff2 = false;
    private static bool onOff3 = false;

    public void onToggle1()
    {
        onOff1 = !onOff1;
        onOff2 = false;
        onOff3 = false;
        toggle1.SetActive(onOff1);
        toggle2.SetActive(onOff2);
        toggle3.SetActive(onOff3);

    }

    public void OnGrassLandButton()
    {
        Debug.Log("GrassLand Random Map Generation");;
        MapGenerator.instance.GenerateMap(true, MapGenerator.Land.grass);
    }

    public void OnSnowLandButton()
    {
        Debug.Log("SnowLand Random Map Generation");
        MapGenerator.instance.GenerateMap(true, MapGenerator.Land.snow);
    }

    public void OnGraveLandButton()
    {
        Debug.Log("GraveLand Random Map Generation");
        MapGenerator.instance.GenerateMap(true, MapGenerator.Land.grave);
    }

    public void OnMineLandButton()
    {
        Debug.Log("MineLand Random Map Generation");
        MapGenerator.instance.GenerateMap(true, MapGenerator.Land.mine);
    }

    public void OnJungleLandButton()
    {
        Debug.Log("JungleLand Random Map Generation");
        MapGenerator.instance.GenerateMap(true, MapGenerator.Land.jungle);
    }

    //*******************************************************************************************************************//

    public void onToggle2()
    {
        onOff2 = !onOff2;
        onOff1 = false;
        onOff3 = false;
        toggle1.SetActive(onOff1);
        toggle2.SetActive(onOff2);
        toggle3.SetActive(onOff3);
    }

    public void OnGrassLandButton2()
    {
        Debug.Log("GrassLand Random Map Generation");
        MapGenerator.instance.GenerateMap(false, MapGenerator.Land.grass);
    }

    public void OnSnowLandButton2()
    {
        Debug.Log("SnowLand Random Map Generation");
        MapGenerator.instance.GenerateMap(false, MapGenerator.Land.snow);
    }

    public void OnGraveLandButton2()
    {
        Debug.Log("GraveLand Random Map Generation");
        MapGenerator.instance.GenerateMap(false, MapGenerator.Land.grave);
    }

    public void OnMineLandButton2()
    {
        Debug.Log("MineLand Random Map Generation");
        MapGenerator.instance.GenerateMap(false, MapGenerator.Land.mine);
    }

    public void OnJungleLandButton2()
    {
        Debug.Log("JungleLand Random Map Generation");
        MapGenerator.instance.GenerateMap(false, MapGenerator.Land.jungle);
    }

    //*******************************************************************************************************************//

    public void onToggle3()
    {
        onOff3 = !onOff3;
        onOff1 = false;
        onOff2 = false;
        toggle1.SetActive(onOff1);
        toggle2.SetActive(onOff2);
        toggle3.SetActive(onOff3);
    }

    public void OnGrassLandButton3()
    {
        Debug.Log("GrassLand Random Map Generation");
        MapGenerator.instance.GenerateMap(true, MapGenerator.Land.grass, true);
    }

    public void OnSnowLandButton3()
    {
        Debug.Log("SnowLand Random Map Generation");
        MapGenerator.instance.GenerateMap(true, MapGenerator.Land.snow, true);
    }

    public void OnGraveLandButton3()
    {
        Debug.Log("GraveLand Random Map Generation");
        MapGenerator.instance.GenerateMap(true, MapGenerator.Land.grave, true);
    }

    public void OnMineLandButton3()
    {
        Debug.Log("MineLand Random Map Generation");
        MapGenerator.instance.GenerateMap(true, MapGenerator.Land.mine, true);
    }

    public void OnJungleLandButton3()
    {
        Debug.Log("JungleLand Random Map Generation");
        MapGenerator.instance.GenerateMap(true, MapGenerator.Land.jungle, true);
    }
}
