using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;

public class UseData : MonoBehaviour
{/**
  * Tutorial link
  * https://github.com/tikonen/blog/tree/master/csvreader
  * */

    /**
     *  Data pulled from: 
     *  
     *  https://daac.ornl.gov/cgi-bin/dsviewer.pl?ds_id=1831
     *  
Ground-based Observations of XCO2, XCH4, and XCO, Fairbanks, AK, 2016-2019

Description
This dataset provides ground-based column-averaged dry mole fractions (DMFs) of CO2 (xco2), CO (xco), CH4 (xch4), and N2O (xn2o) to supplement satellite-based observations of carbon dynamics of northern boreal ecosystems. Measurements were conducted with Bruker EM27/SUN Fourier transform spectrometers (FTS) at the University of Alaska Fairbanks (UAF) and two sites on the edges of the Tanana Flats wetlands to the south from 2016-08-04 to 2019-10-31. Single detectors were used during the first campaign at UAF in 2017, then two instruments were updated to dual detectors in early 2018 to allow retrieval of xco and xn2o. Data from additional FTS instruments, operated by Los Alamos National Laboratories (LANL), Karlsruhe Institute of Technology (KIT), and Jet Propulsion Laboratory (JPL), employed in these campaigns are included.


Data Use and Citation
Download citation from Datacite
RISBibTexOther
Crosscite Citation Formatter
Jacobs, N., W.R. Simpson, F. Hase, T. Blumenstock, Q. Tu, M. Frey, M.K. Dubey, and H.A. Parker. 2021. Ground-based Observations of XCO2, XCH4, and XCO, Fairbanks, AK, 2016-2019. ORNL DAAC, Oak Ridge, Tennessee, USA. https://doi.org/10.3334/ORNLDAAC/1831
This dataset is openly shared, without restriction, in accordance with the EOSDIS Data Use Policy. 

     */

    public object dataObject;
    List<Dictionary<string, object>> data; 
    public GameObject myCube;
    public Material material;

    int currentEntry;
    private float startDelay = 2.0f;
    private float timeInterval = 0.05f;

    void Awake()
    {

        data = CSVReader.Read("OnlyCO2");//udata is the name of the csv file 

        for (var i = 0; i < data.Count; i++)
        {
            //name, age, speed, description, is the headers of the database
            print("xco2 " + data[i]["xco2"]);
        }
        currentEntry = 0;
    }

    // Use this for initialization
    void Start()
    {
        InvokeRepeating("SpawnObject", startDelay, timeInterval);
    }

    void SpawnObject()
    {
        Debug.Log("Spawn Object called");
        dataObject = data[currentEntry]["xco2"];
        float co2Data = map(System.Convert.ToSingle(dataObject), 388.09f, 426.59f, 1, 3);
        float scaledData = Mathf.Exp(co2Data);

        dataObject = data[currentEntry]["year"];
        float yearData = map(System.Convert.ToSingle(dataObject), 2018, 2018, 0, 1);

        dataObject = data[currentEntry]["day"];
        float dayData = map(System.Convert.ToSingle(dataObject), 138, 159, 0, 1);

        dataObject = data[currentEntry]["hour"];
        float hourData = map(System.Convert.ToSingle(dataObject), 15.279f, 27.417f, 0, 1);

        currentEntry += 1;

        transform.localScale = new Vector3(scaledData, scaledData, scaledData);
        Debug.Log("co2 count: " + currentEntry + "\nco2 data: " + co2Data);
        material.color = new Color(yearData, dayData, hourData);
    }

    float map(float value, float domainMin, float domainMax, float newDomainMin, float newDomainMax)
    {
        return newDomainMin + ((newDomainMax - newDomainMin) / (domainMax - domainMin)) * (value - domainMin);
    }
}