using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Unity.XR.CoreUtils;

using System;
using System.Collections;
using System.Collections.Generic;
using SocketIOClient;
using SocketIOClient.Newtonsoft.Json;
using UnityEngine;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine.UI;

using System.Threading;

public class EventManager : MonoBehaviour
{
    private Queue<Action> m_queueAction = new Queue<Action>();

    // public Transform head;
    // public Transform origin;
    // public Transform target;
    // public InputActionProperty recenterButton;

    JArray algorithm = JArray.Parse("[{\"id\":1,\"instruction\":[\"Begin bag-mask ventilation and given oxygen\",\"Attach monitor/defibrillator\"],\"epinephrine\":false,\"cpr\":true,\"question\":{\"title\":\"Rythm shockable?\",\"y\":{\"goto\":2,\"contents\":[\"VF/pVT\",\"Shock\",\"CPR 2 min\"]},\"n\":{\"goto\":9,\"contents\":[\"Asystole/PEA\",\"Epinephrine ASAP\",\"CPR 2 min\"]}}},{\"id\":2,\"instruction\":[\"VP/pVT\",\"Shock\",\"CPR 2 min\",\"IV/IO Access\"],\"epinephrine\":false,\"cpr\":true,\"question\":{\"title\":\"Rythm shockable?\",\"y\":{\"goto\":5,\"contents\":[\"Shock\",\"CPR 2 min\"]},\"n\":{\"goto\":12,\"contents\":[\"Return of spontaneous circulation?\"]}}},{\"id\":5,\"instruction\":[\"Shock\",\"CPR 2 min\",\"Epinephrine every 3-5 min\",\"Consider advanced airway\"],\"epinephrine\":true,\"cpr\":true,\"question\":{\"title\":\"Rythm shockable?\",\"y\":{\"goto\":7,\"contents\":[\"Shock\",\"CPR 2 min\"]},\"n\":{\"goto\":12,\"contents\":[\"Return of spontaneous circulation?\"]}}},{\"id\":7,\"instruction\":[\"Shock\",\"CPR 2 min\",\"Amiodarone or lidocaine\",\"Treat reversible causes\"],\"epinephrine\":true,\"cpr\":true,\"question\":{\"title\":\"Rythm shockable?\",\"y\":{\"goto\":5,\"contents\":[\"Shock\",\"CPR 2 min\"]},\"n\":{\"goto\":12,\"contents\":[\"Return of spontaneous circulation?\"]}}},{\"id\":12,\"instruction\":[],\"epinephrine\":false,\"cpr\":false,\"question\":{\"title\":\"Check Return of spontaneous circulation (ROSC)\",\"y\":{\"goto\":999,\"contents\":[\"Post-Cardiac Arrest Care checklist\"]},\"n\":{\"goto\":10,\"contents\":[\"CPR 2 min\"]}}},{\"id\":9,\"instruction\":[\"Asytole/PEA\",\"Epinephrine ASAP\",\"CPR 2 min\",\"IV/IO access\",\"Epinephrine every 3-5 min\",\"Consider advanced airway and capnography\"],\"epinephrine\":true,\"cpr\":true,\"question\":{\"title\":\"Rythm shockable?\",\"y\":{\"goto\":7,\"contents\":[\"Shock\",\"CPR 2 min\"]},\"n\":{\"goto\":11,\"contents\":[\"CPR 2 min\"]}}},{\"id\":10,\"instruction\":[\"CPR 2 min\",\"IV/IO access\",\"Epinephrine every 3-5 min\",\"Consider advanced airway and capnography\"],\"epinephrine\":true,\"cpr\":true,\"question\":{\"title\":\"Rythm shockable?\",\"y\":{\"goto\":7,\"contents\":[\"Shock\",\"CPR 2 min\"]},\"n\":{\"goto\":11,\"contents\":[\"CPR 2 min\"]}}},{\"id\":11,\"instruction\":[\"CPR 2 min\",\"Treat reversible causes\"],\"epinephrine\":true,\"cpr\":true,\"question\":{\"title\":\"Rythm shockable?\",\"y\":{\"goto\":7,\"contents\":[\"Shock\",\"CPR 2 min\"]},\"n\":{\"goto\":12,\"contents\":[\"Return of spontaneous circulation?\"]}}}]");

    TextMeshProUGUI timer1;
    TextMeshProUGUI timer2;

    TextMeshProUGUI Doc_Cur_1;
    TextMeshProUGUI Doc_Cur_2;
    TextMeshProUGUI Doc_Cur_3;
    TextMeshProUGUI Doc_Next_1;
    TextMeshProUGUI Doc_Next_2;
    TextMeshProUGUI Doc_Next_3;
    TextMeshProUGUI Nurse_Cur_1;
    TextMeshProUGUI Nurse_Cur_2;
    TextMeshProUGUI Nurse_Cur_3;
    TextMeshProUGUI Nurse_Next_1;
    TextMeshProUGUI Nurse_Next_2;
    TextMeshProUGUI Nurse_Next_3;

    double time1 = 0;
    double time2 = 0;

    // Update is called once per frame

    public SocketIOUnity socket;
    int idx = 1;
    double startTimestamp = 0;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(algorithm);
        
        if (GameObject.FindWithTag("CPRTimer") != null) {
            timer1 = GameObject.FindWithTag("CPRTimer").GetComponent<TextMeshProUGUI>();
        }

        if (GameObject.FindWithTag("EpiTimer") != null) {
           timer2 = GameObject.FindWithTag("EpiTimer").GetComponent<TextMeshProUGUI>();
        }

        if (GameObject.FindWithTag("Doc_Cur_1") != null) {
           Doc_Cur_1 = GameObject.FindWithTag("Doc_Cur_1").GetComponent<TextMeshProUGUI>();
        }

        if (GameObject.FindWithTag("Doc_Cur_2") != null) {
           Doc_Cur_2 = GameObject.FindWithTag("Doc_Cur_2").GetComponent<TextMeshProUGUI>();
        }

        if (GameObject.FindWithTag("Doc_Cur_3") != null) {
           Doc_Cur_3 = GameObject.FindWithTag("Doc_Cur_3").GetComponent<TextMeshProUGUI>();
        }

        if (GameObject.FindWithTag("Doc_Next_1") != null) {
           Doc_Next_1 = GameObject.FindWithTag("Doc_Next_1").GetComponent<TextMeshProUGUI>();
        }

        if (GameObject.FindWithTag("Doc_Next_2") != null) {
           Doc_Next_2 = GameObject.FindWithTag("Doc_Next_2").GetComponent<TextMeshProUGUI>();
        }

        if (GameObject.FindWithTag("Doc_Next_3") != null) {
           Doc_Next_3 = GameObject.FindWithTag("Doc_Next_3").GetComponent<TextMeshProUGUI>();
        }

        if (GameObject.FindWithTag("Nurse_Cur_1") != null) {
           Nurse_Cur_1 = GameObject.FindWithTag("Nurse_Cur_1").GetComponent<TextMeshProUGUI>();
        }

        if (GameObject.FindWithTag("Nurse_Cur_2") != null) {
           Nurse_Cur_2 = GameObject.FindWithTag("Nurse_Cur_2").GetComponent<TextMeshProUGUI>();
        }

        if (GameObject.FindWithTag("Nurse_Cur_3") != null) {
           Nurse_Cur_3 = GameObject.FindWithTag("Nurse_Cur_3").GetComponent<TextMeshProUGUI>();
        }

        if (GameObject.FindWithTag("Nurse_Next_1") != null) {
           Nurse_Next_1 = GameObject.FindWithTag("Nurse_Next_1").GetComponent<TextMeshProUGUI>();
        }

        if (GameObject.FindWithTag("Nurse_Next_2") != null) {
           Nurse_Next_2 = GameObject.FindWithTag("Nurse_Next_2").GetComponent<TextMeshProUGUI>();
        }

        if (GameObject.FindWithTag("Nurse_Next_3") != null) {
           Nurse_Next_3 = GameObject.FindWithTag("Nurse_Next_3").GetComponent<TextMeshProUGUI>();
        }

        var uri = new Uri("http://localhost:3000");

        socket = new SocketIOUnity(uri);

        socket.JsonSerializer = new NewtonsoftJsonSerializer();

        socket.OnConnected += (sender, e) =>
        {
            Debug.Log("socket.OnConnected");
        };

        socket.On("currentStatus", (response) => {
            Debug.Log("currentStatus");
            Debug.Log(response);

            idx = response.GetValue<int>();
            Debug.Log("idx: " + idx);

            DateTime currentTime = DateTime.UtcNow;
            long unixTime = ((DateTimeOffset)currentTime).ToUnixTimeSeconds();

            startTimestamp = response.GetValue<double>(1);
            Debug.Log("startTimestamp:" + startTimestamp);

            if (unixTime * 1000 - startTimestamp < 120) {

            }

            time1 = (startTimestamp - unixTime * 1000) / 1000 + 120;
            time2 = (startTimestamp - unixTime * 1000) / 1000 + 300;
            m_queueAction.Enqueue(() => UpdateUI(idx));
        });

        Debug.Log("Connecting...");
        socket.Connect();
    }

    void UpdateUI(int idx)
    {
        Debug.Log("UpdateUI");
        Debug.Log("UpdateUI: " + idx);
        // TextMeshProUGUI temp = GameObject.FindWithTag("Amiodarone").GetComponent<TextMeshProUGUI>();

        foreach(JObject obj in algorithm)
        {
            if (int.Parse(obj["id"].ToString()) == idx) {
                if (obj["instruction"] == null) {
                    continue;
                }
                var instructions = (JArray) (obj["instruction"]);
                
                Init_Tasks();

                for(int i = 0; i < instructions.Count; i++) {
                    string instruction = instructions[i].Value<string>();
                    try {
                        if (i == 0) {
                            if (Doc_Cur_1 != null) {
                                Doc_Cur_1.text = instruction;
                            }
                            if (Nurse_Cur_1 != null) {
                                Nurse_Cur_1.text = instruction;
                            }
                        } else if (i == 1) {
                            if (Doc_Cur_2 != null) {
                                Doc_Cur_2.text = instruction;
                            }
                            if (Nurse_Cur_2 != null) {
                                Nurse_Cur_2.text = instruction;
                            }
                        } else if (i == 2) {
                            if (Doc_Cur_3 != null) {
                                Doc_Cur_3.text = instruction;
                            }
                            if (Nurse_Cur_3 != null) {
                                Nurse_Cur_3.text = instruction;
                            }
                        } else if (i == 3) {
                            if (Doc_Next_1 != null) {
                                Doc_Next_1.text = instruction;
                            }
                            if (Nurse_Next_1 != null) {
                                Nurse_Next_1.text = instruction;
                            }
                        } else if (i == 4) {
                            if (Doc_Next_2 != null) {
                                Doc_Next_2.text = instruction;
                            }
                            if (Nurse_Next_2 != null) {
                                Nurse_Next_2.text = instruction;
                            }
                        } else if (i == 5) {
                            if (Doc_Next_3 != null) {
                                Doc_Next_3.text = instruction;
                            }
                            if (Nurse_Next_3 != null) {
                                Nurse_Next_3.text = instruction;
                            }
                        }
                    } catch (Exception e) {
                        Debug.Log(e);
                    }
                }
                
            }
        }
    }

    public void Init_Tasks()
    {
        if (Doc_Cur_1 != null) {
            Doc_Cur_1.text = "";
        }
        if (Doc_Cur_2 != null) {
            Doc_Cur_2.text = "";
        }
        if (Doc_Cur_3 != null) {
            Doc_Cur_3.text = "";
        }
        if (Doc_Next_1 != null) {
            Doc_Next_1.text = "";
        }
        if (Doc_Next_2 != null) {
            Doc_Next_2.text = "";
        }
        if (Doc_Next_3 != null) {
            Doc_Next_3.text = "";
        }
        if (Nurse_Cur_1 != null) {
            Nurse_Cur_1.text = "";
        }
        if (Nurse_Cur_2 != null) {
            Nurse_Cur_2.text = "";
        }
        if (Nurse_Cur_3 != null) {
            Nurse_Cur_3.text = "";
        }
        if (Nurse_Next_1 != null) {
            Nurse_Next_1.text = "";
        }
        if (Nurse_Next_2 != null) {
            Nurse_Next_2.text = "";
        }
        if (Nurse_Next_3 != null) {
            Nurse_Next_3.text = "";
        }
    }

    public void UpdateClock()
    {
        if (timer1 != null && time1 > 0) {
            time1 -= Time.deltaTime;
            string min = ((int)time1 / 60 % 60 ).ToString();
            if (min.Length == 1) {
                min = "0" + min;
            }
            string sec = ((int)time1 % 60 ).ToString();
            if (sec.Length == 1) {
                sec = "0" + sec;
            }
            timer1.text = min + ":" + sec;
        }

        if (timer2 != null && time2 > 0) {
            time2 -= Time.deltaTime;
            string min = ((int)time2 / 60 % 60 ).ToString();
            if (min.Length == 1) {
                min = "0" + min;
            }
            string sec = ((int)time2 % 60 ).ToString();
            if (sec.Length == 1) {
                sec = "0" + sec;
            }
            timer2.text = min + ":" + sec;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateClock();
        while (m_queueAction.Count > 0)
        {
            m_queueAction.Dequeue().Invoke();
        }
    }

    public void ToMain()
    { 
        SceneManager.LoadScene("main_scene");
    }

    public void Doctor1()
    { 
        SceneManager.LoadScene("hmd_doctor_1");
    }

    public void Doctor2()
    { 
        SceneManager.LoadScene("hmd_doctor_2");
    }

    public void Doctor3()
    { 
        SceneManager.LoadScene("hmd_doctor_3");
    }

    public void Doctor4()
    { 
        SceneManager.LoadScene("hmd_doctor_4");
    }

    public void Doctor5()
    { 
        SceneManager.LoadScene("hmd_doctor_5");
    }

    public void Doctor6()
    { 
        SceneManager.LoadScene("hmd_doctor_6");
    }

    public void Doctor7()
    { 
        SceneManager.LoadScene("hmd_doctor_7");
    }

    public void Doctor8()
    { 
        SceneManager.LoadScene("hmd_doctor_8");
    }

    public void Doctor9()
    { 
        SceneManager.LoadScene("hmd_doctor_9");
    }


    public void Nurse1()
    { 
        SceneManager.LoadScene("hmd_nurse_1");
    }

    public void Nurse2()
    { 
        SceneManager.LoadScene("hmd_nurse_2");
    }

    public void Nurse3()
    { 
        SceneManager.LoadScene("hmd_nurse_3");
    }

    public void Nurse4()
    { 
        SceneManager.LoadScene("hmd_nurse_4");
    }

    public void Nurse5()
    { 
        SceneManager.LoadScene("hmd_nurse_5");
    }

    // public void ResetCenter()
    // { 
    //     Vector3 offset = head.position - origin.position;
    //     offset.y = 0;
    //     origin.position = target.position - offset;

    //     Vector3 targetForward = target.forward;
    //     targetForward.y = 0;
    //     Vector3 cameraForward = head.forward;
    //     cameraForward.y = 0;

    //     float angle = Vector3.SignedAngle(cameraForward, targetForward, Vector3.up);

    //     origin.RotateAround(head.position, Vector3.up, angle);
    // }
}
