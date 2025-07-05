using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class RecordReplayManager : MonoBehaviour
{
    public animatorStateController controller;
    public bool yellowBPressed = false;
    private bool spitzeOben = false;

    public GameObject spitze;
    private XRGrabInteractable interactable;

    public Transform[] transforms;
    public SkinnedMeshRenderer meshRenderer;
    public AudioSource audioSource;
    public AudioSource loudSpeaker1;
    public AudioSource loudSpeaker2;

    public int trackingRate = 30;
    public string recordFileName = "recording.csv";

    public string[] replayFileNames;
    public string replayFileName = "recording.csv";
    public bool isRecording = false;
    public bool isReplaying = false;

    public Output buttons;

    public AudioClip[] audioFiles;
    public AudioClip audioFile;

    public AudioClip[] geraeusche;
    public AudioClip aktGeraeusch;

    private string filePath;

    private float startTime = 0;
    private float nextTime = 0f;
    private float logInterval = 1.0f;

    private string[] lines;
    private int lineIndex = 0;

    private int next = 0;
    public void Start()
    {
        interactable = spitze.GetComponent<XRGrabInteractable>();
        interactable.enabled = true;

        NextReplay(); //StartReplay so umschreiben, dass er die entsprechenden replayFileName und audioFile setzt bevor StartStopRecprding aufgerufen wird
    }

    public void NextReplay()
    {
        if (next <= 5)
        {
            switch (next)
            {
                case 0:
                    StartReplay(next);
                    next += 1;
                    break;
                case 1:
                    StartCoroutine(WaitForCase1());//Warten, bis die Antenne bedient wird
                    break;
                case 2:
                    StartCoroutine(TestSound1());
                    StartCoroutine(WaitForCase2());//1 Minute warten
                    break;
                case 3:
                    yellowBPressed = false;
                    StartCoroutine(PlaySound());
                    StartCoroutine(WaitForCase3()); //Warten, bis der Proband den Knopf drückt
                    break;
                case 4:
                    StartCoroutine(WaitForCase4()); //Warten, bis die Geräusche vorbei sind
                    break;
                case 5:
                    StartCoroutine(WaitForCase5()); //Warten, während die Aufgabe läuft
                    break;
            }
            
        }
        
    }

    private IEnumerator TestSound1()
    {
        yield return new WaitForSeconds(61);
        aktGeraeusch = geraeusche[0];
        loudSpeaker1.clip = aktGeraeusch;
        loudSpeaker2.clip = aktGeraeusch;
        loudSpeaker1.Play();
        loudSpeaker2.Play();
        yield return new WaitForSeconds(3);
        loudSpeaker1.Stop();
        loudSpeaker2.Stop();
    }

    private IEnumerator PlaySound()
    {
        loudSpeaker1.Play();
        loudSpeaker2.Play();
        yield return new WaitForSeconds(3);
        loudSpeaker1.Stop();
        loudSpeaker2.Stop();
    }

    private IEnumerator WaitFor(int seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    private IEnumerator WaitForCase1()
    {
        yield return new WaitUntil(() => spitzeOben);
        StartReplay(next);
        next += 1;
    }

    private IEnumerator WaitForCase2()
    {
        yield return new WaitForSeconds(40);
        StartReplay(next);
        next += 1;
    }

    private IEnumerator WaitForCase3()
    {
        yield return new WaitUntil(() => yellowBPressed == true);
        StartReplay(next);
        next += 1;
    }

    private IEnumerator WaitForCase4()
    {
        for (int i = 0; i<=5; i++)
        {
            
            aktGeraeusch = geraeusche[i];
            loudSpeaker1.clip = aktGeraeusch;
            loudSpeaker2.clip = aktGeraeusch;
            StartCoroutine(PlaySound());
            buttons.ButtonPressed(i);
            yield return new WaitForSeconds(4);
        }
        StartReplay(next);
        next += 1;
    }

    private IEnumerator WaitForCase5()
    {
        for (int i = 0; i <= 20; i++)
        {
            int akt = UnityEngine.Random.Range(0, 6);
            aktGeraeusch = geraeusche[akt];
            loudSpeaker1.clip = aktGeraeusch;
            loudSpeaker2.clip = aktGeraeusch;
            StartCoroutine(PlaySound());
            buttons.WriteDownWhichSoundWasPlayed(akt);
            //Jetzt muss der Nutzer drücken
            yield return new WaitForSeconds(5);
        }
        StartReplay(next);
        next += 1;
    }

    private void Update()
    {
        if (spitze.transform.position.y >= 1)
        {
            interactable.enabled = false;
            spitzeOben = true;
        }

        if (isRecording)
        {
            if (Time.time >= nextTime)
            {
                nextTime += logInterval;

                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine(RecordString());
                }
            }
        }

        if (isReplaying)
        {
            if(lineIndex < lines.Length)
            {
                string line = lines[lineIndex];
                string[] lineParts = line.Split("|");

                if (Time.time - startTime >= float.Parse(lineParts[0]))
                {
                    StringToTransforms(transforms, lineParts[1]);
                    StringToBlendshapes(lineParts[2], meshRenderer);
                    lineIndex++;
                }
            }
            else
            {
                isReplaying = false;
                lineIndex = 0;
                NextReplay();
            }
        }
    }

    public void StartReplay(int i)
    {
        replayFileName = replayFileNames[i];
        audioFile = audioFiles[i];
        audioSource.clip = audioFile;

        
        StartStopReplaying();
    }

    public void StartStopRecording()
    {
        if (isRecording)
        {
            isRecording = false;
            lineIndex = 0;
        }
        else
        {
            logInterval = 1f / trackingRate;
            filePath = Path.Combine(Application.persistentDataPath, recordFileName);

            isRecording = true;
            File.Delete(filePath);
            nextTime = Time.time;
            startTime = Time.time;
        }
    }
    public void StartStopReplaying()
    {
        if (isReplaying)
        {
            audioSource.Stop();
            isReplaying = false;
        }
        else
        {
            isReplaying = true;

            lines = File.ReadAllLines(Path.Combine(Application.persistentDataPath, replayFileName));
            lineIndex = 0;
            startTime = Time.time;
            audioSource.Play();

        }
    }


    public string RecordString()
    {
        string recordingLine = TimeToString() + "|" + TransformsToString(transforms) + "|" + BlendshapesToString(meshRenderer);
        return recordingLine;
    }

    public string TimeToString()
    {
        return (Time.time - startTime).ToString();
    }

    public static string BlendshapesToString(SkinnedMeshRenderer renderer)
    {
        if (renderer == null)
        {
            return "";
        }

        int blendShapeCount = renderer.sharedMesh.blendShapeCount;
        string[] weights = new string[blendShapeCount];

        for (int i = 0; i < blendShapeCount; i++)
        {
            float weight = renderer.GetBlendShapeWeight(i);
            weights[i] = weight.ToString("F3", CultureInfo.InvariantCulture);
        }

        return string.Join(",", weights);
    }

    public static void StringToBlendshapes(string blendShapeString, SkinnedMeshRenderer renderer)
    {
        if (renderer == null || string.IsNullOrEmpty(blendShapeString))
        {
            return;
        }

        string[] weightStrings = blendShapeString.Split(',');
        int blendShapeCount = renderer.sharedMesh.blendShapeCount;

        for (int i = 0; i < blendShapeCount && i < weightStrings.Length; i++)
        {
            if (float.TryParse(weightStrings[i], NumberStyles.Float, CultureInfo.InvariantCulture, out float weight))
            {
                renderer.SetBlendShapeWeight(i, weight);
            }
        }
    }

    public static string TransformsToString(Transform[] transforms)
    {
        string[] transformStrings = transforms.Select(TransformToString).ToArray();

        return string.Join(";", transformStrings);
    }

    public static string TransformToString(Transform transform)
    {
        CultureInfo cultureInfo = CultureInfo.InvariantCulture;

        Vector3 position = transform.position;
        Vector3 rotation = transform.eulerAngles;

        string positionString = string.Format(cultureInfo, "{0:F3},{1:F3},{2:F3}",
            position.x, position.y, position.z);
        string rotationString = string.Format(cultureInfo, "{0:F3},{1:F3},{2:F3}",
            rotation.x, rotation.y, rotation.z);

        string result = positionString + "," + rotationString;

        return result;
    }

    public static void StringToTransforms(Transform[] transforms, string transformsString)
    {
        string[] values = transformsString.Split(';');
        if (values.Length != transforms.Length)
        {
            Debug.LogError("Invalid transforms string. Expected length of values and transform to be the same.");
            return;
        }
        for(int i = 0; i < transforms.Length; i++)
        {
            StringToTransform(transforms[i], values[i]);
        }
    }
    public static void StringToTransform(Transform transform, string transformString)
    {
        CultureInfo cultureInfo = CultureInfo.InvariantCulture;

        string[] values = transformString.Split(',');

        if (values.Length != 6)
        {
            Debug.LogError("Invalid transform string. Expected 6 values.");
            return;
        }

        float posX = float.Parse(values[0], cultureInfo);
        float posY = float.Parse(values[1], cultureInfo);
        float posZ = float.Parse(values[2], cultureInfo);
        float rotX = float.Parse(values[3], cultureInfo);
        float rotY = float.Parse(values[4], cultureInfo);
        float rotZ = float.Parse(values[5], cultureInfo);

        transform.position = new Vector3(posX, posY, posZ);
        transform.eulerAngles = new Vector3(rotX, rotY, rotZ);
    }

}
