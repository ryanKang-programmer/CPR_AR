using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class MedicationEvent : MonoBehaviour
{
    // JArray algorithm = JArray.Parse("[{\"id\":1,\"instruction\":[\"Begin bag-mask ventilation and given oxygen\",\"Attach monitor/defibrillator\"],\"epinephrine\":false,\"cpr\":true,\"question\":{\"title\":\"Rythm shockable?\",\"y\":{\"goto\":2,\"contents\":[\"VF/pVT\",\"Shock\",\"CPR 2 min\"]},\"n\":{\"goto\":9,\"contents\":[\"Asystole/PEA\",\"Epinephrine ASAP\",\"CPR 2 min\"]}}},{\"id\":2,\"instruction\":[\"VP/pVT\",\"Shock\",\"CPR 2 min\",\"IV/IO Access\"],\"epinephrine\":false,\"cpr\":true,\"question\":{\"title\":\"Rythm shockable?\",\"y\":{\"goto\":5,\"contents\":[\"Shock\",\"CPR 2 min\"]},\"n\":{\"goto\":12,\"contents\":[\"Return of spontaneous circulation?\"]}}},{\"id\":5,\"instruction\":[\"Shock\",\"CPR 2 min\",\"Epinephrine every 3-5 min\",\"Consider advanced airway\"],\"epinephrine\":true,\"cpr\":true,\"question\":{\"title\":\"Rythm shockable?\",\"y\":{\"goto\":7,\"contents\":[\"Shock\",\"CPR 2 min\"]},\"n\":{\"goto\":12,\"contents\":[\"Return of spontaneous circulation?\"]}}},{\"id\":7,\"instruction\":[\"Shock\",\"CPR 2 min\",\"Amiodarone or lidocaine\",\"Treat reversible causes\"],\"epinephrine\":true,\"cpr\":true,\"question\":{\"title\":\"Rythm shockable?\",\"y\":{\"goto\":5,\"contents\":[\"Shock\",\"CPR 2 min\"]},\"n\":{\"goto\":12,\"contents\":[\"Return of spontaneous circulation?\"]}}},{\"id\":12,\"instruction\":[],\"epinephrine\":false,\"cpr\":false,\"question\":{\"title\":\"Check Return of spontaneous circulation (ROSC)\",\"y\":{\"goto\":999,\"contents\":[\"Post-Cardiac Arrest Care checklist\"]},\"n\":{\"goto\":10,\"contents\":[\"CPR 2 min\"]}}},{\"id\":9,\"instruction\":[\"Asytole/PEA\",\"Epinephrine ASAP\",\"CPR 2 min\",\"IV/IO access\",\"Epinephrine every 3-5 min\",\"Consider advanced airway and capnography\"],\"epinephrine\":true,\"cpr\":true,\"question\":{\"title\":\"Rythm shockable?\",\"y\":{\"goto\":7,\"contents\":[\"Shock\",\"CPR 2 min\"]},\"n\":{\"goto\":11,\"contents\":[\"CPR 2 min\"]}}},{\"id\":10,\"instruction\":[\"CPR 2 min\",\"IV/IO access\",\"Epinephrine every 3-5 min\",\"Consider advanced airway and capnography\"],\"epinephrine\":true,\"cpr\":true,\"question\":{\"title\":\"Rythm shockable?\",\"y\":{\"goto\":7,\"contents\":[\"Shock\",\"CPR 2 min\"]},\"n\":{\"goto\":11,\"contents\":[\"CPR 2 min\"]}}},{\"id\":11,\"instruction\":[\"CPR 2 min\",\"Treat reversible causes\"],\"epinephrine\":true,\"cpr\":true,\"question\":{\"title\":\"Rythm shockable?\",\"y\":{\"goto\":7,\"contents\":[\"Shock\",\"CPR 2 min\"]},\"n\":{\"goto\":12,\"contents\":[\"Return of spontaneous circulation?\"]}}}]");
    // Start is called before the first frame update
    TextMeshProUGUI AmiCount;
    TextMeshProUGUI AtroCount;
    TextMeshProUGUI EpiCount;
    TextMeshProUGUI LidoCount;
    TextMeshProUGUI FenCount;
    TextMeshProUGUI KenCount;
    TextMeshProUGUI MidCount;
    TextMeshProUGUI MorCount;
    TextMeshProUGUI RocCount;
    TextMeshProUGUI SucCount;
    TextMeshProUGUI CalGCount;
    TextMeshProUGUI CalCCount;
    TextMeshProUGUI SalCount;
    TextMeshProUGUI SodCount;
    TextMeshProUGUI InsCount;
    TextMeshProUGUI GluCount;

    int max_count = 9999;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void plusAmi() {
        if (GameObject.FindWithTag("AmiCount") != null) {
            AmiCount = GameObject.FindWithTag("AmiCount").GetComponent<TextMeshProUGUI>();
        }
        int val = int.Parse(AmiCount.text);

        if (val > max_count) {
            return;
        }

        val++;

        AmiCount.text = val.ToString();
    }

    public void minusAmi() {
        if (GameObject.FindWithTag("AmiCount") != null) {
            AmiCount = GameObject.FindWithTag("AmiCount").GetComponent<TextMeshProUGUI>();
        }
        int val = int.Parse(AmiCount.text);

        if (val == 0) return;

        val--;

        AmiCount.text = val.ToString();
    }

    public void plusAtro() {
        if (GameObject.FindWithTag("AtroCount") != null) {
            AtroCount = GameObject.FindWithTag("AtroCount").GetComponent<TextMeshProUGUI>();
        }
        int val = int.Parse(AtroCount.text);

        if (val > max_count) {
            return;
        }

        val++;

        AtroCount.text = val.ToString();
    }

    public void minusAtro() {
        if (GameObject.FindWithTag("AtroCount") != null) {
            AtroCount = GameObject.FindWithTag("AtroCount").GetComponent<TextMeshProUGUI>();
        }
        int val = int.Parse(AtroCount.text);

        if (val == 0) return;

        val--;

        AtroCount.text = val.ToString();
    }

    public void plusEpi() {
        if (GameObject.FindWithTag("EpiCount") != null) {
            EpiCount = GameObject.FindWithTag("EpiCount").GetComponent<TextMeshProUGUI>();
        }
        int val = int.Parse(EpiCount.text);

        if (val > max_count) {
            return;
        }

        val++;

        EpiCount.text = val.ToString();
    }

    public void minusEpi() {
        if (GameObject.FindWithTag("EpiCount") != null) {
            EpiCount = GameObject.FindWithTag("EpiCount").GetComponent<TextMeshProUGUI>();
        }
        int val = int.Parse(EpiCount.text);

        if (val == 0) return;

        val--;

        EpiCount.text = val.ToString();
    }

    public void plusLido() {
        if (GameObject.FindWithTag("LidoCount") != null) {
            LidoCount = GameObject.FindWithTag("LidoCount").GetComponent<TextMeshProUGUI>();
        }
        int val = int.Parse(LidoCount.text);

        if (val > max_count) {
            return;
        }

        val++;

        LidoCount.text = val.ToString();
    }

    public void minusLido() {
        if (GameObject.FindWithTag("LidoCount") != null) {
            LidoCount = GameObject.FindWithTag("LidoCount").GetComponent<TextMeshProUGUI>();
        }
        int val = int.Parse(LidoCount.text);

        if (val == 0) return;

        val--;

        LidoCount.text = val.ToString();
    }

    public void plusFen() {
        if (GameObject.FindWithTag("FenCount") != null) {
            FenCount = GameObject.FindWithTag("FenCount").GetComponent<TextMeshProUGUI>();
        }
        int val = int.Parse(FenCount.text);

        if (val > max_count) {
            return;
        }

        val++;

        FenCount.text = val.ToString();
    }

    public void minusFen() {
        if (GameObject.FindWithTag("FenCount") != null) {
            FenCount = GameObject.FindWithTag("FenCount").GetComponent<TextMeshProUGUI>();
        }
        int val = int.Parse(FenCount.text);

        if (val == 0) return;

        val--;

        FenCount.text = val.ToString();
    }

    public void plusKen() {
        if (GameObject.FindWithTag("KenCount") != null) {
            KenCount = GameObject.FindWithTag("KenCount").GetComponent<TextMeshProUGUI>();
        }
        int val = int.Parse(KenCount.text);

        if (val > max_count) {
            return;
        }

        val++;

        KenCount.text = val.ToString();
    }

    public void minusKen() {
        if (GameObject.FindWithTag("KenCount") != null) {
            KenCount = GameObject.FindWithTag("KenCount").GetComponent<TextMeshProUGUI>();
        }
        int val = int.Parse(KenCount.text);

        if (val == 0) return;

        val--;

        KenCount.text = val.ToString();
    }

    public void plusMid() {
        if (GameObject.FindWithTag("MidCount") != null) {
            MidCount = GameObject.FindWithTag("MidCount").GetComponent<TextMeshProUGUI>();
        }
        int val = int.Parse(MidCount.text);

        if (val > max_count) {
            return;
        }

        val++;

        MidCount.text = val.ToString();
    }

    public void minusMid() {
        if (GameObject.FindWithTag("MidCount") != null) {
            MidCount = GameObject.FindWithTag("MidCount").GetComponent<TextMeshProUGUI>();
        }
        int val = int.Parse(MidCount.text);

        if (val == 0) return;

        val--;

        MidCount.text = val.ToString();
    }

    public void plusMor() {
        if (GameObject.FindWithTag("MorCount") != null) {
            MorCount = GameObject.FindWithTag("MorCount").GetComponent<TextMeshProUGUI>();
        }
        int val = int.Parse(MorCount.text);

        if (val > max_count) {
            return;
        }

        val++;

        MorCount.text = val.ToString();
    }

    public void minusMor() {
        if (GameObject.FindWithTag("MorCount") != null) {
            MorCount = GameObject.FindWithTag("MorCount").GetComponent<TextMeshProUGUI>();
        }
        int val = int.Parse(MorCount.text);

        if (val == 0) return;

        val--;

        MorCount.text = val.ToString();
    }

    public void plusRoc() {
        if (GameObject.FindWithTag("RocCount") != null) {
            RocCount = GameObject.FindWithTag("RocCount").GetComponent<TextMeshProUGUI>();
        }
        int val = int.Parse(RocCount.text);

        if (val > max_count) {
            return;
        }

        val++;

        RocCount.text = val.ToString();
    }

    public void minusRoc() {
        if (GameObject.FindWithTag("RocCount") != null) {
            RocCount = GameObject.FindWithTag("RocCount").GetComponent<TextMeshProUGUI>();
        }
        int val = int.Parse(RocCount.text);

        if (val == 0) return;

        val--;

        RocCount.text = val.ToString();
    }

    public void plusSuc() {
        if (GameObject.FindWithTag("SucCount") != null) {
            SucCount = GameObject.FindWithTag("SucCount").GetComponent<TextMeshProUGUI>();
        }
        int val = int.Parse(SucCount.text);

        if (val > max_count) {
            return;
        }

        val++;

        SucCount.text = val.ToString();
    }

    public void minusSuc() {
        if (GameObject.FindWithTag("SucCount") != null) {
            SucCount = GameObject.FindWithTag("SucCount").GetComponent<TextMeshProUGUI>();
        }
        int val = int.Parse(SucCount.text);

        if (val == 0) return;

        val--;

        SucCount.text = val.ToString();
    }

    public void plusCalG() {
        if (GameObject.FindWithTag("CalGCount") != null) {
            CalGCount = GameObject.FindWithTag("CalGCount").GetComponent<TextMeshProUGUI>();
        }
        int val = int.Parse(CalGCount.text);

        if (val > max_count) {
            return;
        }

        val++;

        CalGCount.text = val.ToString();
    }

    public void minusCalG() {
        if (GameObject.FindWithTag("CalGCount") != null) {
            CalGCount = GameObject.FindWithTag("CalGCount").GetComponent<TextMeshProUGUI>();
        }
        int val = int.Parse(CalGCount.text);

        if (val == 0) return;

        val--;

        CalGCount.text = val.ToString();
    }

    public void plusCalC() {
        if (GameObject.FindWithTag("CalCCount") != null) {
            CalCCount = GameObject.FindWithTag("CalCCount").GetComponent<TextMeshProUGUI>();
        }
        int val = int.Parse(CalCCount.text);

        if (val > max_count) {
            return;
        }

        val++;

        CalCCount.text = val.ToString();
    }

    public void minusCalC() {
        if (GameObject.FindWithTag("CalCCount") != null) {
            CalCCount = GameObject.FindWithTag("CalCCount").GetComponent<TextMeshProUGUI>();
        }
        int val = int.Parse(CalCCount.text);

        if (val == 0) return;

        val--;

        CalCCount.text = val.ToString();
    }

    public void plusSal() {
        if (GameObject.FindWithTag("SalCount") != null) {
            SalCount = GameObject.FindWithTag("SalCount").GetComponent<TextMeshProUGUI>();
        }
        int val = int.Parse(SalCount.text);

        if (val > max_count) {
            return;
        }

        val++;

        SalCount.text = val.ToString();
    }

    public void minusSal() {
        if (GameObject.FindWithTag("SalCount") != null) {
            SalCount = GameObject.FindWithTag("SalCount").GetComponent<TextMeshProUGUI>();
        }
        int val = int.Parse(SalCount.text);

        if (val == 0) return;

        val--;

        SalCount.text = val.ToString();
    }

    public void plusSod() {
        if (GameObject.FindWithTag("SodCount") != null) {
            SodCount = GameObject.FindWithTag("SodCount").GetComponent<TextMeshProUGUI>();
        }
        int val = int.Parse(SodCount.text);

        if (val > max_count) {
            return;
        }

        val++;

        SodCount.text = val.ToString();
    }

    public void minusSod() {
        if (GameObject.FindWithTag("SodCount") != null) {
            SodCount = GameObject.FindWithTag("SodCount").GetComponent<TextMeshProUGUI>();
        }
        int val = int.Parse(SodCount.text);

        if (val == 0) return;

        val--;

        SodCount.text = val.ToString();
    }

    public void plusIns() {
        if (GameObject.FindWithTag("InsCount") != null) {
            InsCount = GameObject.FindWithTag("InsCount").GetComponent<TextMeshProUGUI>();
        }
        int val = int.Parse(InsCount.text);

        if (val > max_count) {
            return;
        }

        val++;

        InsCount.text = val.ToString();
    }

    public void minusIns() {
        if (GameObject.FindWithTag("InsCount") != null) {
            InsCount = GameObject.FindWithTag("InsCount").GetComponent<TextMeshProUGUI>();
        }
        int val = int.Parse(InsCount.text);

        if (val == 0) return;

        val--;

        InsCount.text = val.ToString();
    }

    public void plusGlu() {
        if (GameObject.FindWithTag("GluCount") != null) {
            GluCount = GameObject.FindWithTag("GluCount").GetComponent<TextMeshProUGUI>();
        }
        int val = int.Parse(GluCount.text);

        if (val > max_count) {
            return;
        }

        val++;

        GluCount.text = val.ToString();
    }

    public void minusGlu() {
        if (GameObject.FindWithTag("GluCount") != null) {
            GluCount = GameObject.FindWithTag("GluCount").GetComponent<TextMeshProUGUI>();
        }
        int val = int.Parse(GluCount.text);

        if (val == 0) return;

        val--;

        GluCount.text = val.ToString();
    }
}
