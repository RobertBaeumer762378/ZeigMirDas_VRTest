using ReadyPlayerMe.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Output : MonoBehaviour
{

    private string path = "";
    private string path2;
    private string path3;
    private string path4;

    public RecordReplayManager replayManager;

    public GameObject[] buttons; //Arrays mit Buttons und Lamps
    public GameObject[] lamps;

    public GameObject yellowB; //Buttons
    public GameObject greenB;
    public GameObject blueB;
    public GameObject pinkB;
    public GameObject greyB;
    public GameObject redB;

    public GameObject yellowL; //Lampen
    public GameObject greenL;
    public GameObject blueL;
    public GameObject pinkL;
    public GameObject greyL;
    public GameObject redL;

    public Material[] materials;
    public Material[] materialsA;

    public Material yellow; //Materialien
    public Material green;
    public Material blue;
    public Material pink;
    public Material grey;
    public Material red;

    public Material yellowA; //Active-Materialien
    public Material greenA;
    public Material blueA;
    public Material pinkA;
    public Material greyA;
    public Material redA;


    public bool yellowBPressed = false;

    private DateTime dateAndTime = DateTime.Now;
    private string dateTimeStr;
    private Stopwatch sw;

    // Start is called before the first frame update
    void Start()
    {
        sw = Stopwatch.StartNew();
        path = "C:\\Ergebnisse Unity\\Tatsächliche Tests\\";
        DateTime dayDate = dateAndTime.Date;
        string dayDateStr = dayDate.ToString();
        dayDateStr.Remove(10);
        dayDateStr = dayDateStr.Replace(".", "-");
        dayDateStr = dayDateStr.Replace(":", "-");
        dayDateStr = dayDateStr.Replace(" ", "_");
        dayDateStr = dayDateStr.Substring(0, 10);

        path2 = Path.Combine(path, dayDateStr);  //"C:\\Ergebnisse Unity\\DATUM"
        bool exists = System.IO.Directory.Exists(path2);

        if (!exists)
        {
            System.IO.Directory.CreateDirectory(path2);
        }
        dateTimeStr = dateAndTime.ToString();
        dateTimeStr = dateTimeStr.Replace(".", "-");
        dateTimeStr = dateTimeStr.Replace(":", "-");
        dateTimeStr = dateTimeStr.Replace(" ", "_");
        dateTimeStr = dateTimeStr.Substring(11);

        path3 = Path.Combine(path2, dateTimeStr);
        path4 = path3 + "_mitAvatarUndPult.txt";  //"C:\\Ergebnisse Unity\\DATUM\\DATUMZEIT.txt"

        var stream = File.CreateText(path4);
        stream.Dispose();

        using (StreamWriter writer = new StreamWriter(path4, true))
        {
            writer.Write("Der Test wurde gestartet!\n");
            writer.Dispose();
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    //public void WaitForYellow()
    //{
    //    yellowBPressed = false;
    //}

    public void ButtonPressed(GameObject button)
    {
        if (button.name == yellowB.name)
        {
            replayManager.yellowBPressed = true;
        }

        DeactivateLampIn4(button);

        TimeSpan timePassed = sw.Elapsed;
        //TimeSpan startTime = dateAndTime.TimeOfDay;
        //TimeSpan currentDateTime = DateTime.Now.TimeOfDay;
        using (StreamWriter writer = new StreamWriter(path4, true))
        {
            writer.Write(button.name + " was pressed after " + timePassed.ToString("mm':'ss") + "\n");
            writer.Dispose();
        }
    }

    public void WriteDownWhichSoundWasPlayed(int soundNumber)
    {
        TimeSpan timePassed = sw.Elapsed;
        using (StreamWriter writer = new StreamWriter(path4, true))
        {
            writer.Write("The Sound ");
            switch (soundNumber)
            {
                case 0:
                    writer.Write("Gong");
                    break;
                case 1:
                    writer.Write("Bird Whistle");
                    break;
                case 2:
                    writer.Write("Cash");
                    break;
                case 3:
                    writer.Write("Thunder");
                    break;
                case 4:
                    writer.Write("Train");
                    break;
                case 5:
                    writer.Write("Drumset");
                    break;
            }
            writer.Write(" was played after " + timePassed.ToString("mm':'ss") + ", ");
            writer.Write("the Button to be pressed should be " + buttons[soundNumber].name + ".\n");
            writer.Dispose();
        }
    }

    public void ButtonPressed(int button) //für Aufrufe aus RecordReplayManager
    {
        lamps[button].GetComponent<Renderer>().material = materialsA[button];

        DeactivateLampIn4(buttons[button]);

    }

    //private IEnumerator WaitFor(int seconds)
    //{
    //    yield return new WaitForSeconds(seconds);
    //}

    public void DeactivateLampIn4(GameObject button)
    {
        int val = -1;
        for (int i = 0; i <= 5; i++)
        {
            if (buttons[i] == button)
            {
                val = i;
                break;
            }
        }

        if (val != -1)
        {
            StartCoroutine(Turnoff(lamps[val]));
        }

    }


    private IEnumerator Turnoff(GameObject lamp)
    {
        yield return new WaitForSeconds(4);

        if (lamp == yellowL)
        {
            Console.Out.WriteLine("yellowLamp is supposed to turn off");
            lamps[0].GetComponent<Renderer>().material = yellow;
        }
        if (lamp == greenL)
        {
            lamps[1].GetComponent<Renderer>().material = green;
        }
        if (lamp == blueL)
        {
            lamps[2].GetComponent<Renderer>().material = blue;
        }
        if (lamp == pinkL)
        {
            lamps[3].GetComponent<Renderer>().material = pink;
        }
        if (lamp == greyL)
        {
            lamps[4].GetComponent<Renderer>().material = grey;
        }
        if (lamp == redL)
        {
            lamps[5].GetComponent<Renderer>().material = red;
        }
    }
}
