using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Unity.XR.CoreUtils;
using UnityEngine.EventSystems;

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using SocketIOClient;
using SocketIOClient.Newtonsoft.Json;
using UnityEngine;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine.UI;

using System.Threading;
using Microsoft.MixedReality.GraphicsTools;

public class EventManager : MonoBehaviour
{
    private Queue<Action> m_queueAction = new Queue<Action>();
    private float timeActivated = float.MinValue;

    // public Transform head;
    // public Transform origin;
    // public Transform target;
    // public InputActionProperty recenterButton;

    // JArray algorithm = JArray.Parse("[\r\n    {\r\n     \"No\": 1,\r\n     \"ID\": 1,\r\n     \"CurrentStepAlgorithm\": \"TEP (signs of life ?)\",\r\n     \"GuidingPadContent\": \"Info : Signs of life ? \\nButtons :  YES\\/NO\\n\",\r\n     \"TeamScreen\": \"1) \\n2)\\n3)\",\r\n     \"NextSteps\": \"1) \\n2)\\n3)\",\r\n     \"CurrentStep2\": \"Assess ABCs\",\r\n     \"NextSteps2\": \"1) Check pulse\\n2)\\n3)\"\r\n    },\r\n    {\r\n     \"No\": 2,\r\n     \"ID\": 2,\r\n     \"CurrentStepAlgorithm\": \"Pulse ?\",\r\n     \"GuidingPadContent\": \"Info : Pulse ?\\nButtons : YES\\/NO\\n\",\r\n     \"TeamScreen\": \"1) \\n2)\\n3)\",\r\n     \"NextSteps\": \"1) \\n2)\\n3)\",\r\n     \"CurrentStep2\": \"Check pulse\",\r\n     \"NextSteps2\": \"1) Start CPR\\n2) Initiate BVM ventilation\\n3) Verify patient weight\"\r\n    },\r\n    {\r\n     \"No\": 3,\r\n     \"ID\": 3,\r\n     \"CurrentStepAlgorithm\": \"Start CPR\",\r\n     \"GuidingPadContent\": \"Info : Confirm this action\\nButton : CPR started \",\r\n     \"TeamScreen\": \"1) \\n2)\\n3)\",\r\n     \"NextSteps\": \"1) Verify patient weight\\n2) \\n3)\",\r\n     \"CurrentStep2\": \"1) Initiate CPR\\n2) Provide CPR Coaching\\n3) Initiate BVM ventilation\",\r\n     \"NextSteps2\": \"1) Verify patient weight\\n2) Check cardiac rhythm\\n3) Insert IV\\/IO\"\r\n    },\r\n    {\r\n     \"No\": 4,\r\n     \"ID\": 4,\r\n     \"CurrentStepAlgorithm\": \"Check for problem A and\\/or B\",\r\n     \"TeamScreen\": \"1) \\n2)\\n3)\",\r\n     \"NextSteps\": \"1) Verify patient weight\\n2) \\n3)\",\r\n     \"CurrentStep2\": \"1) Assess suitability for ECMO\\n2)\\n3)\",\r\n     \"NextSteps2\": \"1) Activate ECMO team\\n2)\\n3)\"\r\n    },\r\n    {\r\n     \"No\": 5,\r\n     \"CurrentStepAlgorithm\": \"First call to indicate weight or age\",\r\n     \"GuidingPadContent\": \"Info : Patient data (Optional)\\nAction : indicate weight and\\/or age and click on \\\"Confirm\\\" OR click on \\\"Skip\\\" to move forward\",\r\n     \"TeamScreen\": \"1) \\n2)\\n3)\",\r\n     \"CurrentStep\": \"1) Verify patient weight\\n2) \\n3)\",\r\n     \"NextSteps\": \"1) Prepare epinephrine doses x 3\\n2)\\n3)\",\r\n     \"CurrentStep2\": \"1) Verify patient weight\\n2) \\n3)\",\r\n     \"NextSteps2\": \"1) Check cardiac rhythm\\n2) Insert IV\\/IO\\n3) \"\r\n    },\r\n    {\r\n     \"No\": 6,\r\n     \"ID\": 5,\r\n     \"CurrentStepAlgorithm\": \"Check cardiac rythm \",\r\n     \"GuidingPadContent\": \"Info : Select rhythm\\nButtons : VF\\/pVT\\/Asystole\\/PEA    \\n\",\r\n     \"TeamScreen\": \"1) \\n2)\\n3)\",\r\n     \"CurrentStep\": \"1) Verify patient weight\\n2) \\n3)\",\r\n     \"NextSteps\": \"1) Prepare epinephrine doses x 3\\n2)\\n3)\",\r\n     \"CurrentStep2\": \"1) Check cardiac rhythm\\n2) Insert IV\\/IO\\n3) \",\r\n     \"NextSteps2\": \"1) Verbally prepare team for defibrillation\\n2) Prepare epinephrine doses x 3\\n3) \"\r\n    },\r\n    {\r\n     \"No\": 7,\r\n     \"CurrentStepAlgorithm\": \"Last call to indicate weight or age\",\r\n     \"GuidingPadContent\": \"Info : Indicate weight before defibrillation! OR Indicate weight before administering epinephrine!\",\r\n     \"TeamScreen\": \"1) \\n2)\\n3)\",\r\n     \"CurrentStep\": \"1) Verify patient weight\\n2) \\n3)\",\r\n     \"NextSteps\": \"1) Prepare epinephrine doses x 3\\n2) Prepare ketamine 1mg\\/kg x 2\\n3) \",\r\n     \"CurrentStep2\": \"1) Check cardiac rhythm\\n2) Insert IV\\/IO\\n3)\",\r\n     \"NextSteps2\": \"1) Verbally prepare team for defibrillation\\n2) Prepare epinephrine doses x 3\\n3)\"\r\n    },\r\n    {\r\n     \"No\": 8,\r\n     \"ID\": 6,\r\n     \"CurrentStepAlgorithm\": \"Shock \",\r\n     \"GuidingPadContent\": \"Info : Confirm this action\\nButton : Shock given \",\r\n     \"TeamScreen\": \"1) \\n2)\\n3)\",\r\n     \"CurrentStep\": \"1) Prepare epinephrine doses x 3\\n2) Prepare ketamine 1mg\\/kg x 2\\n3)\",\r\n     \"NextSteps\": \"1) Prepare amiodarone 5mg\\/kg\\n2) Prepare lidocaine 1mg\\/kg\\n3) \",\r\n     \"CurrentStep2\": \"1) Verbally prepare team for defibrillation\\n2) Defibrillate 2J\\/kg \\n3) Resume CPR - Minimize pauses\",\r\n     \"NextSteps2\": \"1) Prepare epinephrine doses x 3\\n2)\\n3)\"\r\n    },\r\n    {\r\n     \"No\": 9,\r\n     \"ID\": 7,\r\n     \"CurrentStepAlgorithm\": \"CPR \",\r\n     \"GuidingPadContent\": \"Info : Confirm this action\\nButton : CPR resumed \",\r\n     \"TeamScreen\": \"1) \\n2)\\n3)\",\r\n     \"CurrentStep\": \"1) Prepare epinephrine doses x 3\\n2) Prepare ketamine 1mg\\/kg x 2\\n3)\",\r\n     \"NextSteps\": \"1) Prepare amiodarone 5mg\\/kg\\n2) Prepare lidocaine 1mg\\/kg\\n3) Prepare rocuronium 1mg\\/kg \",\r\n     \"CurrentStep2\": \"1) Provide CPR\\n2) Provide CPR coaching\\n3) Prepare epinephrine doses x 3\",\r\n     \"NextSteps2\": \"1) Reassess airway\\n2) Reassess breathing\\n3)\"\r\n    },\r\n    {\r\n     \"No\": 10,\r\n     \"ID\": 8,\r\n     \"CurrentStepAlgorithm\": \"Check for problem A and\\/or B\",\r\n     \"GuidingPadContent\": \"Info : Confirm this action\\nToggle : a) airways PROBLEM\\/NORMAL\\nb) ventilation PROBLEM\\/NORMAL \",\r\n     \"TeamScreen\": \"1) \\n2)\\n3)\",\r\n     \"CurrentStep\": \"1) Prepare epinephrine doses x 3\\n2) Prepare ketamine 1mg\\/kg x 2\\n3) Prepare rocuronium 1mg\\/kg\",\r\n     \"NextSteps\": \"1) Prepare amiodarone 5mg\\/kg\\n2) Prepare lidocaine 1mg\\/kg\\n3) \",\r\n     \"CurrentStep2\": \"1) Reassess airway\\n2) Reassess breathing\\n3)\",\r\n     \"NextSteps2\": \"1) \\n2)\\n3)\"\r\n    },\r\n    {\r\n     \"No\": 11,\r\n     \"CurrentStepAlgorithm\": \"Problem A\",\r\n     \"GuidingPadContent\": \"Info : A- Obstructed airways\\nToggle : Repositioning\\/ Aspiration\\/ Foreign body extraction\\/ Wendel\\/ Guedel\",\r\n     \"TeamScreen\": \"1) \\n2)\\n3)\",\r\n     \"CurrentStep\": \"1) Prepare epinephrine doses x 3\\n2) Prepare ketamine 1mg\\/kg x 2\\n3) Prepare rocuronium 1mg\\/kg\",\r\n     \"NextSteps\": \"1) Prepare amiodarone 5mg\\/kg\\n2) Prepare lidocaine 1mg\\/kg\\n3) \",\r\n     \"CurrentStep2\": \"1) Reassess airway\\n2) Reassess breathing\\n3)\",\r\n     \"NextSteps2\": \"1) Reposition airway as needed\\n2) Clear airway as needed\\n3) Improve ventilation as needed\"\r\n    },\r\n    {\r\n     \"No\": 12,\r\n     \"CurrentStepAlgorithm\": \"Problem B\",\r\n     \"GuidingPadContent\": \"Info : B - Non-intubated patient\\nToggle : Exsufflation - if asymmetric ventilation\\nOR Info : B - Intubated patient\\nToggle : Displacement\\/Tube obstruction\\/ Tension pneumothorax\\/ Equipment\",\r\n     \"TeamScreen\": \"1) \\n2)\\n3)\",\r\n     \"CurrentStep\": \"1) Prepare epinephrine doses x 3\\n2) Prepare ketamine 1mg\\/kg x 2\\n3) Prepare rocuronium 1mg\\/kg\",\r\n     \"NextSteps\": \"1) Prepare amiodarone 5mg\\/kg\\n2) Prepare lidocaine 1mg\\/kg\\n3) \",\r\n     \"CurrentStep2\": \"1) Reassess airway\\n2) Reassess breathing\\n3)\",\r\n     \"NextSteps2\": \"1) Improve ventilation as needed\\n2) Check pulse\\n3) Check rhythm\"\r\n    },\r\n    {\r\n     \"No\": 13,\r\n     \"CurrentStepAlgorithm\": \"Waiting for the end of the 2 min timer\",\r\n     \"GuidingPadContent\": \"Info : Please wait until the two minutes are up before proceeding.\",\r\n     \"TeamScreen\": \"1) \\n2)\\n3)\",\r\n     \"CurrentStep\": \"1) Prepare epinephrine doses x 3\\n2)\\n3)\",\r\n     \"NextSteps\": \"1) Prepare amiodarone 5mg\\/kg\\n2) Prepare lidocaine 1mg\\/kg\\n3) \",\r\n     \"CurrentStep2\": \"1) Check pulse\\n2) Check rhythm\\n3)\",\r\n     \"NextSteps2\": \"1) \\n2) \\n3) \"\r\n    },\r\n    {\r\n     \"No\": 14,\r\n     \"ID\": 9,\r\n     \"CurrentStepAlgorithm\": \"Check Pulse\",\r\n     \"GuidingPadContent\": \"Info : Pulse ?\\nButtons : YES\\/NO\\n\",\r\n     \"TeamScreen\": \"1) \\n2)\\n3)\",\r\n     \"CurrentStep\": \"1) Prepare epinephrine doses x 3\\n2)\\n3)\",\r\n     \"NextSteps\": \"1) Prepare ketamine 1mg\\/kg\\n2) Prepare rocuronium 1mg\\/kg\\n3) \",\r\n     \"CurrentStep2\": \"1) Check pulse\\n2) Check rhythm\\n3)\",\r\n     \"NextSteps2\": \"1) Verbally prepare team for defibrillation\\n2) Prepare epinephrine doses x 3\\n3)\"\r\n    },\r\n    {\r\n     \"No\": 15,\r\n     \"ID\": 10,\r\n     \"CurrentStepAlgorithm\": \"Check cardiac rythm\",\r\n     \"GuidingPadContent\": \"Info : Select rhythm\\nButtons : VF\\/pVT\\/Asystole\\/PEA    \\n\",\r\n     \"TeamScreen\": \"1) \\n2)\\n3)\",\r\n     \"CurrentStep\": \"1) Prepare epinephrine doses x 3\\n2)\\n3)\",\r\n     \"NextSteps\": \"1) Prepare ketamine 1mg\\/kg\\n2) Prepare rocuronium 1mg\\/kg\\n3) \",\r\n     \"CurrentStep2\": \"1) Check pulse\\n2) Check rhythm\\n3)\",\r\n     \"NextSteps2\": \"1) Verbally prepare team for defibrillation\\n2) Prepare epinephrine doses x 3\\n3)\"\r\n    },\r\n    {\r\n     \"No\": 16,\r\n     \"ID\": 11,\r\n     \"CurrentStepAlgorithm\": \"Shock \",\r\n     \"GuidingPadContent\": \"Info : Confirm this action\\nButton : Shock given \",\r\n     \"TeamScreen\": \"1) \\n2)\\n3)\",\r\n     \"CurrentStep\": \"1) Prepare epinephrine doses x 3\\n2) Prepare ketamine 1mg\\/kg x 2\\n3) Prepare rocuronium 1mg\\/kg\",\r\n     \"NextSteps\": \"1) Prepare amiodarone 5mg\\/kg\\n2) Prepare lidocaine 1mg\\/kg\\n3) \",\r\n     \"CurrentStep2\": \"1) Verbally prepare team for defibrillation\\n2) Defibrillate 4J\\/kg \\n3) Resume CPR - Minimize pauses\",\r\n     \"NextSteps2\": \"1) Prepare epinephrine doses x 3\\n2)\\n3)\"\r\n    },\r\n    {\r\n     \"No\": 17,\r\n     \"ID\": 12,\r\n     \"CurrentStepAlgorithm\": \"CPR + Epinephrine \",\r\n     \"GuidingPadContent\": \"Info : Confirm this action\\nButton : CPR resumed \",\r\n     \"TeamScreen\": \"1) \\n2)\\n3)\",\r\n     \"CurrentStep\": \"1) Prepare epinephrine doses x 3\\n2)\\n3)\",\r\n     \"NextSteps\": \"1) Prepare amiodarone 5mg\\/kg\\n2) Prepare lidocaine 1mg\\/kg\\n3) \",\r\n     \"CurrentStep2\": \"1) Provide CPR\\n2) Provide CPR Coaching\\n3) Epinephrine 0.1 ml\\/kg  IV\\/IO\",\r\n     \"NextSteps2\": \"1) Prepare for intubation \\/ LMA insertion\\n2)\\n3)\"\r\n    },\r\n    {\r\n     \"No\": 18,\r\n     \"ID\": 13,\r\n     \"CurrentStepAlgorithm\": \"Check for problem A and\\/or B\",\r\n     \"GuidingPadContent\": \"Info : Confirm this action\\nToggle : a) airways PROBLEM\\/NORMAL\\nb) ventilation PROBLEM\\/NORMAL \",\r\n     \"TeamScreen\": \"1) \\n2)\\n3)\",\r\n     \"NextSteps\": \"1) \\n2)\\n3)\",\r\n     \"CurrentStep2\": \"1) Intubate or insert LMA\\n2)\\n3)\",\r\n     \"NextSteps2\": \"1) Check tube placement\\n2)\\n3)\"\r\n    },\r\n    {\r\n     \"No\": 19,\r\n     \"CurrentStepAlgorithm\": \"Problem A\",\r\n     \"GuidingPadContent\": \"Info : A- Obstructed airways\\nToggle : Repositioning\\/ Aspiration\\/ Foreign body extraction\\/ Wendel\\/ Guedel\",\r\n     \"TeamScreen\": \"1) \\n2)\\n3)\",\r\n     \"NextSteps\": \"1) \\n2)\\n3)\",\r\n     \"CurrentStep2\": \"1) Check tube \\/ LMA placement\\n2) Reassess airway\\n3) Clear \\/ suction tube as needed\",\r\n     \"NextSteps2\": \"1) Reintubate \\/ reinsert LMA as needed\\n2) \\n3)\"\r\n    },\r\n    {\r\n     \"No\": 20,\r\n     \"CurrentStepAlgorithm\": \"Problem B\",\r\n     \"GuidingPadContent\": \"Info : B - Non-intubated patient\\nToggle : Exsufflation - if asymmetric ventilation\\nOR Info : B - Intubated patient\\nToggle : Displacement\\/Tube obstruction\\/ Tension pneumothorax\\/ Equipment\",\r\n     \"TeamScreen\": \"1) \\n2)\\n3)\",\r\n     \"NextSteps\": \"1) \\n2)\\n3)\",\r\n     \"CurrentStep2\": \"1) Check tube \\/ LMA placement\\n2) Reassess airway\\n3) Clear \\/ suction tube as needed\",\r\n     \"NextSteps2\": \"1) Reintubate \\/ reinsert LMA as needed\\n2) Check pulse\\n3) Check rhythm\"\r\n    },\r\n    {\r\n     \"No\": 21,\r\n     \"ID\": 14,\r\n     \"CurrentStepAlgorithm\": \"Check Pulse\",\r\n     \"GuidingPadContent\": \"Info : Pulse ?\\nButtons : YES\\/NO\\n\",\r\n     \"TeamScreen\": \"1) \\n2)\\n3)\",\r\n     \"CurrentStep\": \"1) Prepare epinephrine doses x 3\\n2)\\n3)\",\r\n     \"NextSteps\": \"1) Prepare ketamine 1mg\\/kg\\n2) Prepare rocuronium 1mg\\/kg\\n3) \",\r\n     \"CurrentStep2\": \"1) Check pulse\\n2) Check rhythm\\n3)\",\r\n     \"NextSteps2\": \"1) Verbally prepare team for defibrillation\\n2) Prepare epinephrine doses x 3\\n3)\"\r\n    },\r\n    {\r\n     \"No\": 22,\r\n     \"ID\": 15,\r\n     \"CurrentStepAlgorithm\": \"Check cardiac rythm \",\r\n     \"GuidingPadContent\": \"Info : Select rhythm\\nButtons : VF\\/pVT\\/Asystole\\/PEA    \\n\",\r\n     \"TeamScreen\": \"1) \\n2)\\n3)\",\r\n     \"CurrentStep\": \"1) Prepare epinephrine doses x 3\\n2)\\n3)\",\r\n     \"NextSteps\": \"1) Prepare ketamine 1mg\\/kg\\n2) Prepare rocuronium 1mg\\/kg\\n3) \",\r\n     \"CurrentStep2\": \"1) Check pulse\\n2) Check rhythm\\n3\",\r\n     \"NextSteps2\": \"1) Verbally prepare team for defibrillation\\n2) Prepare epinephrine doses x 3\\n3)\"\r\n    },\r\n    {\r\n     \"No\": 23,\r\n     \"ID\": 16,\r\n     \"CurrentStepAlgorithm\": \"Shock \",\r\n     \"GuidingPadContent\": \"Info : Confirm this action\\nButton : Shock given \",\r\n     \"TeamScreen\": \"1) \\n2)\\n3)\",\r\n     \"CurrentStep\": \"1) Prepare epinephrine doses x 3\\n2) Prepare ketamine 1mg\\/kg x 2\\n3) Prepare rocuronium 1mg\\/kg\",\r\n     \"NextSteps\": \"1) Prepare amiodarone 5mg\\/kg\\n2) Prepare lidocaine 1mg\\/kg\\n3) \",\r\n     \"CurrentStep2\": \"1) Verbally prepare team for defibrillation\\n2) Defibrillate 4J\\/kg \\n3) Resume CPR - Minimize pauses\",\r\n     \"NextSteps2\": \"1) Prepare amiodarone and\\/or lidocaine\\n2) Consider reversible causes\\n3) \"\r\n    },\r\n    {\r\n     \"No\": 24,\r\n     \"ID\": 17,\r\n     \"CurrentStepAlgorithm\": \"CPR \",\r\n     \"GuidingPadContent\": \"Info : Confirm this action\\nButton : CPR resumed \",\r\n     \"TeamScreen\": \"1) \\n2)\\n3)\",\r\n     \"CurrentStep\": \"1) Prepare epinephrine doses x 3\\n2)\\n3)\",\r\n     \"NextSteps\": \"1) Prepare amiodarone 5mg\\/kg\\n2) Prepare lidocaine 1mg\\/kg\\n3) \",\r\n     \"CurrentStep2\": \"1) Provide CPR\\n2) Provide CPR Coaching\\n3) \",\r\n     \"NextSteps2\": \"1) Reassess airway\\n2) Reassess breathing \\/ ventilation\\n3)\"\r\n    },\r\n    {\r\n     \"No\": 25,\r\n     \"ID\": 18,\r\n     \"CurrentStepAlgorithm\": \"Check for problem A and\\/or B\",\r\n     \"GuidingPadContent\": \"Info : Confirm this action\\nToggle : a) airways PROBLEM\\/NORMAL\\nb) ventilation PROBLEM\\/NORMAL \",\r\n     \"TeamScreen\": \"1) \\n2)\\n3)\",\r\n     \"NextSteps\": \"1) \\n2)\\n3)\",\r\n     \"CurrentStep2\": \"1) Reassess airway\\n2) Reassess breathing \\/ ventilation\\n3)\",\r\n     \"NextSteps2\": \"1) Clear aiway as needed\\n2) Improve ventilation as needed\\n3)\"\r\n    },\r\n    {\r\n     \"No\": 26,\r\n     \"CurrentStepAlgorithm\": \"Problem A\",\r\n     \"GuidingPadContent\": \"Info : A- Obstructed airways\\nToggle : Repositioning\\/ Aspiration\\/ Foreign body extraction\\/ Wendel\\/ Guedel\",\r\n     \"TeamScreen\": \"1) \\n2)\\n3)\",\r\n     \"NextSteps\": \"1) \\n2)\\n3)\",\r\n     \"CurrentStep2\": \"1) Check tube \\/ LMA placement\\n2) Reassess airway\\n3) Clear \\/ suction tube as needed\",\r\n     \"NextSteps2\": \"1) Reintubate \\/ reinsert LMA as needed\\n2) \\n3)\"\r\n    },\r\n    {\r\n     \"No\": 27,\r\n     \"CurrentStepAlgorithm\": \"Problem B\",\r\n     \"GuidingPadContent\": \"Info : B - Non-intubated patient\\nToggle : Exsufflation - if asymmetric ventilation\\nOR Info : B - Intubated patient\\nToggle : Displacement\\/Tube obstruction\\/ Tension pneumothorax\\/ Equipment\",\r\n     \"TeamScreen\": \"1) \\n2)\\n3)\",\r\n     \"NextSteps\": \"1) \\n2)\\n3)\",\r\n     \"CurrentStep2\": \"1) Check tube \\/ LMA placement\\n2) Reassess airway\\n3) Clear \\/ suction tube as needed\",\r\n     \"NextSteps2\": \"1) Reintubate \\/ reinsert LMA as needed\\n2) Check pulse\\n3) Check rhythm\"\r\n    },\r\n    {\r\n     \"No\": 28,\r\n     \"ID\": 19,\r\n     \"CurrentStepAlgorithm\": \"Check Pulse\",\r\n     \"GuidingPadContent\": \"Info : Pulse ?\\nButtons : YES\\/NO\\n\",\r\n     \"TeamScreen\": \"1) \\n2)\\n3)\",\r\n     \"CurrentStep\": \"1) Prepare epinephrine doses x 3\\n2)\\n3)\",\r\n     \"NextSteps\": \"1) Prepare amiodarone 5mg\\/kg\\n2) Prepare lidocaine 1mg\\/kg\\n3) \",\r\n     \"CurrentStep2\": \"1) Check pulse\\n2) Check rhythm\\n3)\",\r\n     \"NextSteps2\": \"1) Verbally prepare team for defibrillation\\n2) Prepare epinephrine doses x 3\\n3) Prepare amiodarone and\\/or lidocaine\"\r\n    },\r\n    {\r\n     \"No\": 29,\r\n     \"ID\": 20,\r\n     \"CurrentStepAlgorithm\": \"Check cardiac rythm \",\r\n     \"GuidingPadContent\": \"Info : Select rhythm\\nButtons : VF\\/pVT\\/Asystole\\/PEA    \\n\",\r\n     \"TeamScreen\": \"1) \\n2)\\n3)\",\r\n     \"CurrentStep\": \"1) Prepare epinephrine doses x 3\\n2)\\n3)\",\r\n     \"NextSteps\": \"1) Prepare amiodarone 5mg\\/kg\\n2) Prepare lidocaine 1mg\\/kg\\n3) \",\r\n     \"CurrentStep2\": \"1) Check pulse\\n2) Check rhythm\\n3)\",\r\n     \"NextSteps2\": \"1) Verbally prepare team for defibrillation\\n2) Prepare epinephrine doses x 3\\n3) Prepare amiodarone and\\/or lidocaine\"\r\n    },\r\n    {\r\n     \"No\": 30,\r\n     \"ID\": 21,\r\n     \"CurrentStepAlgorithm\": \"CPR + Epinephrine \",\r\n     \"GuidingPadContent\": \"Info : Confirm this action\\nButton : CPR resumed \",\r\n     \"TeamScreen\": \"1) \\n2)\\n3)\",\r\n     \"CurrentStep\": \"1) Prepare epinephrine doses x 3\\n2)\\n3)\",\r\n     \"NextSteps\": \"1) Prepare amiodarone 5mg\\/kg\\n2) Prepare lidocaine 1mg\\/kg\\n3) \",\r\n     \"CurrentStep2\": \"1) Provide CPR\\n2) Provide CPR Coaching\\n3) Epinephrine 0.1 ml\\/kg  IV\\/IO\",\r\n     \"NextSteps2\": \"1) Prepare amiodarone and\\/or lidocaine\\n2)\\n3)\"\r\n    },\r\n    {\r\n     \"No\": 31,\r\n     \"ID\": 22,\r\n     \"CurrentStepAlgorithm\": \"Check for problem A and\\/or B\",\r\n     \"GuidingPadContent\": \"Info : Confirm this action\\nToggle : a) airways PROBLEM\\/NORMAL\\nb) ventilation PROBLEM\\/NORMAL \",\r\n     \"TeamScreen\": \"1) \\n2)\\n3)\",\r\n     \"NextSteps\": \"1) \\n2)\\n3)\",\r\n     \"CurrentStep2\": \"1) Reassess airway\\n2) Reassess breathing \\/ ventilation\\n3)\",\r\n     \"NextSteps2\": \"1) Clear aiway as needed\\n2) Improve ventilation as needed\\n3)\"\r\n    },\r\n    {\r\n     \"No\": 32,\r\n     \"CurrentStepAlgorithm\": \"Problem A\",\r\n     \"GuidingPadContent\": \"Info : A- Obstructed airways\\nToggle : Repositioning\\/ Aspiration\\/ Foreign body extraction\\/ Wendel\\/ Guedel\",\r\n     \"TeamScreen\": \"1) \\n2)\\n3)\",\r\n     \"NextSteps\": \"1) \\n2)\\n3)\",\r\n     \"CurrentStep2\": \"1) Check tube \\/ LMA placement\\n2) Reassess airway\\n3) Clear \\/ suction tube as needed\",\r\n     \"NextSteps2\": \"1) Reintubate \\/ reinsert LMA as needed\\n2) \\n3)\"\r\n    },\r\n    {\r\n     \"No\": 33,\r\n     \"CurrentStepAlgorithm\": \"Problem B\",\r\n     \"GuidingPadContent\": \"Info : B - Non-intubated patient\\nToggle : Exsufflation - if asymmetric ventilation\\nOR Info : B - Intubated patient\\nToggle : Displacement\\/Tube obstruction\\/ Tension pneumothorax\\/ Equipment\",\r\n     \"TeamScreen\": \"1) \\n2)\\n3)\",\r\n     \"NextSteps\": \"1) \\n2)\\n3)\",\r\n     \"CurrentStep2\": \"1) Check tube \\/ LMA placement\\n2) Reassess airway\\n3) Clear \\/ suction tube as needed\",\r\n     \"NextSteps2\": \"1) Reintubate \\/ reinsert LMA as needed\\n2) Check pulse\\n3) Check rhythm\"\r\n    },\r\n    {\r\n     \"No\": 34,\r\n     \"ID\": 23,\r\n     \"CurrentStepAlgorithm\": \"Check Pulse\",\r\n     \"GuidingPadContent\": \"Info : Pulse ?\\nButtons : YES\\/NO\\n\",\r\n     \"TeamScreen\": \"1) \\n2)\\n3)\",\r\n     \"CurrentStep\": \"1) Prepare epinephrine doses x 3\\n2)\\n3)\",\r\n     \"NextSteps\": \"1) Prepare amiodarone 5mg\\/kg\\n2) Prepare lidocaine 1mg\\/kg\\n3) \",\r\n     \"CurrentStep2\": \"1) Check pulse\\n2) Check rhythm\\n3)\",\r\n     \"NextSteps2\": \"1) Verbally prepare team for defibrillation\\n2) Prepare epinephrine doses x 3\\n3) Prepare amiodarone and\\/or lidocaine\"\r\n    },\r\n    {\r\n     \"No\": 35,\r\n     \"ID\": 24,\r\n     \"CurrentStepAlgorithm\": \"Check cardiac rythm \",\r\n     \"GuidingPadContent\": \"Info : Select rhythm\\nButtons : VF\\/pVT\\/Asystole\\/PEA    \\n\",\r\n     \"TeamScreen\": \"1) \\n2)\\n3)\",\r\n     \"CurrentStep\": \"1) Prepare epinephrine doses x 3\\n2)\\n3)\",\r\n     \"NextSteps\": \"1) Prepare amiodarone 5mg\\/kg\\n2) Prepare lidocaine 1mg\\/kg\\n3) \",\r\n     \"CurrentStep2\": \"1) Check pulse\\n2) Check rhythm\\n3)\",\r\n     \"NextSteps2\": \"1) Verbally prepare team for defibrillation\\n2) Prepare epinephrine doses x 3\\n3) Prepare amiodarone and\\/or lidocaine\"\r\n    },\r\n    {\r\n     \"No\": 36,\r\n     \"ID\": 25,\r\n     \"CurrentStepAlgorithm\": \"CPR + Amiodarone or Lidocaine\",\r\n     \"GuidingPadContent\": \"Info : Confirm this action\\nButton : CPR resumed \",\r\n     \"TeamScreen\": \"1) \\n2)\\n3)\",\r\n     \"CurrentStep\": \"1) Prepare amiodarone 5mg\\/kg\\n2) Prepare lidocaine 1mg\\/kg\\n3) \",\r\n     \"NextSteps\": \"1) \\n2)\\n3)\",\r\n     \"CurrentStep2\": \"1) Provide CPR\\n2) Provide CPR Coaching\\n3) Amiodarone or Lidocaine IV\\/IO\",\r\n     \"NextSteps2\": \"1) Consider reversible causes\\n2) Prepare epinephrine doses x 3\\n3)\"\r\n    },\r\n    {\r\n     \"No\": 37,\r\n     \"ID\": 26,\r\n     \"CurrentStepAlgorithm\": \"Check for problem A and\\/or B\",\r\n     \"GuidingPadContent\": \"Info : Confirm this action\\nToggle : a) airways PROBLEM\\/NORMAL\\nb) ventilation PROBLEM\\/NORMAL \",\r\n     \"TeamScreen\": \"1) \\n2)\\n3)\",\r\n     \"NextSteps\": \"1) \\n2)\\n3)\",\r\n     \"CurrentStep2\": \"1) Reassess airway\\n2) Reassess breathing \\/ ventilation\\n3)\",\r\n     \"NextSteps2\": \"1) Clear aiway as needed\\n2) Improve ventilation as needed\\n3)\"\r\n    },\r\n    {\r\n     \"No\": 38,\r\n     \"CurrentStepAlgorithm\": \"Problem A\",\r\n     \"GuidingPadContent\": \"Info : A- Obstructed airways\\nToggle : Repositioning\\/ Aspiration\\/ Foreign body extraction\\/ Wendel\\/ Guedel\",\r\n     \"TeamScreen\": \"1) \\n2)\\n3)\",\r\n     \"NextSteps\": \"1) \\n2)\\n3)\",\r\n     \"CurrentStep2\": \"1) Check tube \\/ LMA placement\\n2) Reassess airway\\n3) Clear \\/ suction tube as needed\",\r\n     \"NextSteps2\": \"1) Reintubate \\/ reinsert LMA as needed\\n2) \\n3)\"\r\n    },\r\n    {\r\n     \"No\": 39,\r\n     \"CurrentStepAlgorithm\": \"Problem B\",\r\n     \"GuidingPadContent\": \"Info : B - Non-intubated patient\\nToggle : Exsufflation - if asymmetric ventilation\\nOR Info : B - Intubated patient\\nToggle : Displacement\\/Tube obstruction\\/ Tension pneumothorax\\/ Equipment\",\r\n     \"TeamScreen\": \"1) \\n2)\\n3)\",\r\n     \"NextSteps\": \"1) \\n2)\\n3)\",\r\n     \"CurrentStep2\": \"1) Check tube \\/ LMA placement\\n2) Reassess airway\\n3) Clear \\/ suction tube as needed\",\r\n     \"NextSteps2\": \"1) Reintubate \\/ reinsert LMA as needed\\n2) Check pulse\\n3) Check rhythm\"\r\n    },\r\n    {\r\n     \"No\": 40,\r\n     \"ID\": 27,\r\n     \"CurrentStepAlgorithm\": \"Check Pulse\",\r\n     \"GuidingPadContent\": \"Info : Pulse ?\\nButtons : YES\\/NO\\n\",\r\n     \"TeamScreen\": \"1) \\n2)\\n3)\",\r\n     \"CurrentStep\": \"1) Prepare epinephrine doses x 3\\n2)\\n3)\",\r\n     \"NextSteps\": \"1) Prepare amiodarone 5mg\\/kg\\n2) Prepare lidocaine 1mg\\/kg\\n3) \",\r\n     \"CurrentStep2\": \"1) Check pulse\\n2) Check rhythm\\n3)\",\r\n     \"NextSteps2\": \"1) Verbally prepare team for defibrillation\\n2) Prepare epinephrine doses x 3\\n3) Consider reversible causes\"\r\n    },\r\n    {\r\n     \"No\": 41,\r\n     \"ID\": 28,\r\n     \"CurrentStepAlgorithm\": \"Check cardiac rythm \",\r\n     \"GuidingPadContent\": \"Info : Select rhythm\\nButtons : VF\\/pVT\\/Asystole\\/PEA    \\n\",\r\n     \"TeamScreen\": \"1) \\n2)\\n3)\",\r\n     \"CurrentStep\": \"1) Prepare epinephrine doses x 3\\n2)\\n3)\",\r\n     \"NextSteps\": \"1) Prepare amiodarone 5mg\\/kg\\n2) Prepare lidocaine 1mg\\/kg\\n3) \",\r\n     \"CurrentStep2\": \"1) Check pulse\\n2) Check rhythm\\n3)\",\r\n     \"NextSteps2\": \"1) Verbally prepare team for defibrillation\\n2) Prepare epinephrine doses x 3\\n3) Prepare amiodarone and\\/or lidocaine\"\r\n    },\r\n    {\r\n     \"No\": 42,\r\n     \"ID\": 29,\r\n     \"CurrentStepAlgorithm\": \"CPR + Epinephrine\",\r\n     \"GuidingPadContent\": \"Info : Confirm this action\\nButton : CPR resumed \",\r\n     \"TeamScreen\": \"1) \\n2)\\n3)\",\r\n     \"CurrentStep\": \"1) Prepare epinephrine doses x 3\\n2)\\n3)\",\r\n     \"NextSteps\": \"1) \\n2)\\n3)\",\r\n     \"CurrentStep2\": \"1) Provide CPR\\n2) Provide CPR Coaching\\n3) Epinephrine 0.1 ml\\/kg  IV\\/IO\",\r\n     \"NextSteps2\": \"1) Consider reversible causes \\n2)\\n3)\"\r\n    }\r\n   ]");
    JArray algorithm = JArray.Parse("[\r\n     {\r\n      \"ScenarioStage\": \"Stage 1: PEA\",\r\n      \"CurrentStatus\": \"PEA\",\r\n      \"No\": 1,\r\n      \"ID\": 1,\r\n      \"IntermediaryAction(onGuidingPad)\": \"TEP\",\r\n      \"GuidingPadContent\": \"Info : Signs of life ? \\nButtons :  YES\\/NO\\n\",\r\n      \"TeamScreenCurrentStep\": \"Assess PAT\\n\",\r\n      \"TeamScreenNextInevitableSteps\": \"Check pulse\\nAssess CAB\\nApply patient monitors\",\r\n      \"CurrentStep2\": \"Assess PAT\",\r\n      \"NextSteps2\": \"Check pulse\\nAssess CAB\",\r\n      \"CurrentStep\": \"Verify patient weight\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"PEA\",\r\n      \"No\": 2,\r\n      \"ID\": 2,\r\n      \"CurrentStepAlgorithm\": \"Pulse ?\",\r\n      \"IntermediaryAction(onGuidingPad)\": \"Check pulse (1)\",\r\n      \"GuidingPadContent\": \"Info : Pulse ?\\nButtons : YES\\/NO\\n\",\r\n      \"TeamScreenCurrentStep\": \"Check pulse\\nAssess CAB\\nApply patient monitors\",\r\n      \"TeamScreenNextInevitableSteps\": \"Obtain patient weight\\nCheck cardiac rhythm\\nInsert IO\",\r\n      \"CurrentStep2\": \"Check pulse\\nAssess CAB\",\r\n      \"NextSteps2\": \"Verify patient weight\\nCheck cardiac rhythm\\nInsert IO\",\r\n      \"CurrentStep\": \"Verify patient weight\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"PEA\",\r\n      \"No\": 3,\r\n      \"ID\": 3,\r\n      \"CurrentStepAlgorithm\": \"Start CPR (1)\",\r\n      \"GuidingPadContent\": \"Info: Confirm this action\\nButton : CPR resumed\",\r\n      \"TeamScreenCurrentStep\": \"Start CPR and BVM Ventilation\\nPlace backboard \\nAttach defibrillation pads\",\r\n      \"TeamScreenNextInevitableSteps\": \"Obtain patient weight\\nCheck cardiac rhythm\\nInsert IO \",\r\n      \"CurrentStep2\": \"Start CPR and BVM Ventilation\\nPlace backboard \\nAttach defibrillation pads\",\r\n      \"NextSteps2\": \"Verify patient weight\\nCheck cardiac rhythm\\nInsert IO\",\r\n      \"CurrentStep\": \"Verify patient weight\",\r\n      \"NextSteps\": \"Prepare Epinephrine 0.01 mg\\/kg (0.1 mL\\/kg) x 3\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"PEA\",\r\n      \"No\": 4,\r\n      \"ID\": 4,\r\n      \"CurrentStepAlgorithm\": \"Last call to indicate weight or age\",\r\n      \"GuidingPadContent\": \"Info : Indicate weight before defibrillation! OR Indicate weight before administering epinephrine!\",\r\n      \"TeamScreenCurrentStep\": \"Obtain patient weight\\nInsert IO \",\r\n      \"TeamScreenNextInevitableSteps\": \"Check cardiac rhythm\\nAssign CPR Coach\",\r\n      \"CurrentStep2\": \"Verify patient weight\\nInsert IO\\n\",\r\n      \"NextSteps2\": \"Check cardiac rhythm\\nAssign CPR Coach\",\r\n      \"CurrentStep\": \"Verify patient weight\",\r\n      \"NextSteps\": \"Prepare Epinephrine 0.01 mg\\/kg (0.1 mL\\/kg) x 3\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"PEA\",\r\n      \"No\": 5,\r\n      \"ID\": 5,\r\n      \"CurrentStepAlgorithm\": \"Check cardiac rhythm\",\r\n      \"GuidingPadContent\": \"Info : Select rhythm\\nButtons : VF\\/pVT\\/Asystole\\/PEA    \\n\",\r\n      \"TeamScreenCurrentStep\": \"Check cardiac rhythm\\nAssign CPR Coach\",\r\n      \"TeamScreenNextInevitableSteps\": \"Order epinephrine 0.01 mg\\/kg (0.1 mL\\/kg)\\n\",\r\n      \"CurrentStep2\": \"Check cardiac rhythm\\nAssign CPR Coach\",\r\n      \"NextSteps2\": \"Order epinephrine 0.01 mg\\/kg (0.1 mL\\/kg)\\n\",\r\n      \"CurrentStep\": \"Prepare Epinephrine 0.01 mg\\/kg (0.1 mL\\/kg) x 3\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"PEA\",\r\n      \"No\": 5,\r\n      \"ID\": 5,\r\n      \"CurrentStepAlgorithm\": \"Asystole\\/PEA - Epineprhine\",\r\n      \"IntermediaryAction(onGuidingPad)\": \"Deliver Epinephrine (1)\",\r\n      \"GuidingPadContent\": \"Medication button : \\na.Prescription list: select Epinephrine + select preparation dose\\nb.Ready button + dose number: ???\\nc.Administered button : Administered\",\r\n      \"TeamScreenCurrentStep\": \"Give epinephrine 0.1 mL\\/kg\\n\",\r\n      \"TeamScreenNextInevitableSteps\": \"Adjust bed height for optimal CPR\\nEnsure active CPR Coaching\\nInsert second IO \",\r\n      \"CurrentStep2\": \"Give epinephrine 0.01 mg\\/kg (0.1 mL\\/kg)\\n\",\r\n      \"NextSteps2\": \"Adjust bed height for optimal CPR\\nEnsure active CPR Coaching\\nInsert second IO \",\r\n      \"CurrentStep\": \"Prepare Epinephrine 0.01 mg\\/kg (0.1 mL\\/kg) x 3\",\r\n      \"NextSteps\": \"Prepare Amiodarone 5 mg\\/kg\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"PEA\",\r\n      \"No\": 21,\r\n      \"ID\": 21,\r\n      \"CurrentStepAlgorithm\": \"Resume CPR for 2 min\",\r\n      \"TeamScreenCurrentStep\": \"Adjust bed height for optimal CPR\\nResume CPR & ensure active CPR Coaching\\nInsert second IO \",\r\n      \"TeamScreenNextInevitableSteps\": \"Reassess A and B\\nIf in hospital, page ECMO team\\nGive NS fluid bolus 20 cc\\/kg\",\r\n      \"CurrentStep2\": \"Adjust bed height for optimal CPR\\nResume CPR & ensure active CPR Coaching\\nInsert second IO \",\r\n      \"NextSteps2\": \"Reassess A and B\\nIf in hospital, page ECMO team\\nGive NS fluid bolus 20 cc\\/kg\",\r\n      \"CurrentStep\": \"Prepare Amiodarone 5 mg\\/kg\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"PEA\",\r\n      \"No\": 22,\r\n      \"ID\": 22,\r\n      \"IntermediaryAction(onGuidingPad)\": \"Reassess A and B\",\r\n      \"GuidingPadContent\": \"Info: Confirm this action\\nToggle : a) airways PROBLEM\\/NORMAL\\nb) ventilation PROBLEM\\/NORMAL\",\r\n      \"TeamScreenCurrentStep\": \"Reassess A and B\\nIf in hospital, page ECMO team\\n\",\r\n      \"TeamScreenNextInevitableSteps\": \"Prepare equipment for intubation\",\r\n      \"CurrentStep2\": \"Reassess A and B\\nIf in hospital, page ECMO team\\n\",\r\n      \"NextSteps2\": \"Prepare equipment for intubation\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"PEA\",\r\n      \"No\": 22,\r\n      \"ID\": 22,\r\n      \"IntermediaryAction(onGuidingPad)\": \"Problem A\",\r\n      \"GuidingPadContent\": \"Info: A – Obstructed airways\\nToggle : Repositioning\\/ Aspiration\\/ Foreign body extraction\\/ Wendel\\/ Guedel\",\r\n      \"TeamScreenCurrentStep\": \"Continue CPR for 2 minutes\\nAction A according to selection\",\r\n      \"TeamScreenNextInevitableSteps\": \"Prepare equipment for intubation\",\r\n      \"CurrentStep2\": \"Continue CPR for 2 minutes\\nAction A according to selection\",\r\n      \"NextSteps2\": \"Prepare equipment for intubation\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"PEA\",\r\n      \"No\": 22,\r\n      \"ID\": 22,\r\n      \"IntermediaryAction(onGuidingPad)\": \"Problem B\",\r\n      \"GuidingPadContent\": \"Info: B – Non-intubated patient\\nToggle : Exsufflation - if asymmetric ventilation\\nOR \\nInfo : B - Intubated patient\\nToggle : Displacement\\/Tube obstruction\\/ Tension pneumothorax\\/ Equipment\",\r\n      \"TeamScreenCurrentStep\": \"Continue CPR for 2 minutes\\nAction B according to selection\",\r\n      \"TeamScreenNextInevitableSteps\": \"Prepare equipment for intubation\",\r\n      \"CurrentStep2\": \"Continue CPR for 2 minutes\\nAction A according to selection\",\r\n      \"NextSteps2\": \"Prepare equipment for intubation\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"PEA\",\r\n      \"No\": 23,\r\n      \"ID\": 23,\r\n      \"IntermediaryAction(onGuidingPad)\": \"Waiting for the end of the 2 min timer\",\r\n      \"GuidingPadContent\": \"Info : Please wait until the two minutes are up before proceeding.\",\r\n      \"TeamScreenCurrentStep\": \"Prepare equipment for intubation\\nPrepare 2nd CPR provider\\nObtain bloodwork \\/ capillary blood gas\",\r\n      \"TeamScreenNextInevitableSteps\": \"Check pulse\\nCheck cardiac rhythm\\nSwitch CPR providers \",\r\n      \"CurrentStep2\": \"Prepare equipment for intubation\\nPrepare 2nd CPR provider & verbal countdown\\nObtain bloodwork \\/ capillary blood gas\",\r\n      \"NextSteps2\": \"Check pulse\\nCheck cardiac rhythm\\nSwitch CPR providers \",\r\n      \"CurrentStep\": \"Prepare NS fluid bolus 20 cc\\/kg\",\r\n      \"NextSteps\": \"Prepare Epinephrine 0.01 mg\\/kg (0.1 mL\\/kg)\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"PEA\",\r\n      \"No\": 24,\r\n      \"ID\": 24,\r\n      \"CurrentStepAlgorithm\": \"Shockable rhythm?\",\r\n      \"GuidingPadContent\": \"Info: Select rhythm\\nButtons : VF\\/pVT\\/Asystole\\/PEA \",\r\n      \"TeamScreenCurrentStep\": \"Check pulse\\nCheck cardiac rhythm\\nSwitch CPR providers \",\r\n      \"TeamScreenNextInevitableSteps\": \"Resume CPR\\nEnsure active CPR Coaching\\nIntubate patient\",\r\n      \"CurrentStep2\": \"Check pulse\\nCheck cardiac rhythm\\nSwitch CPR providers \",\r\n      \"NextSteps2\": \"Resume CPR\\nEnsure active CPR Coaching\\nIntubate patient\",\r\n      \"CurrentStep\": \"Prepare NS fluid bolus 20 cc\\/kg\",\r\n      \"NextSteps\": \"Prepare Epinephrine 0.01 mg\\/kg (0.1 mL\\/kg)\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"PEA\",\r\n      \"No\": 25,\r\n      \"ID\": 25,\r\n      \"CurrentStepAlgorithm\": \"Resume CPR for 2 min\",\r\n      \"GuidingPadContent\": \"Info: Confirm this action\\nButton : CPR resumed\",\r\n      \"TeamScreenCurrentStep\": \"Resume CPR & ensure active CPR Coaching\\nIntubate patient\\nGive IV NS Fluid bolus 20cc\\/kg\",\r\n      \"TeamScreenNextInevitableSteps\": \"Assess tube placement\\nOrder epinephrine 0.01 mg\\/kg (0.1 ml\\/kg)\",\r\n      \"CurrentStep2\": \"Resume CPR & ensure active CPR Coaching\\nIntubate patient\\nGive IV NS Fluid bolus 20cc\\/kg\",\r\n      \"NextSteps2\": \"Assess tube placement\\nOrder epinephrine 0.01 mg\\/kg (0.1 ml\\/kg)\",\r\n      \"CurrentStep\": \"Prepare NS fluid bolus 20 cc\\/kg\",\r\n      \"NextSteps\": \"Prepare Epinephrine 0.01 mg\\/kg (0.1 mL\\/kg)\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"PEA\",\r\n      \"No\": 26,\r\n      \"ID\": 26,\r\n      \"IntermediaryAction(onGuidingPad)\": \"Reassess A and B\",\r\n      \"GuidingPadContent\": \"Info: Confirm this action\\nToggle : a) airways PROBLEM\\/NORMAL\\nb) ventilation PROBLEM\\/NORMAL\",\r\n      \"TeamScreenCurrentStep\": \"Assess tube placement\",\r\n      \"TeamScreenNextInevitableSteps\": \"Adjust tube or suction as needed\",\r\n      \"CurrentStep2\": \"Assess tube placement\",\r\n      \"NextSteps2\": \"Adjust tube or suction as needed\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"PEA\",\r\n      \"No\": 26,\r\n      \"ID\": 26,\r\n      \"IntermediaryAction(onGuidingPad)\": \"Problem A\",\r\n      \"GuidingPadContent\": \"Info: A – Obstructed airways\\nToggle : Repositioning\\/ Aspiration\\/ Foreign body extraction\\/ Wendel\\/ Guedel\",\r\n      \"TeamScreenCurrentStep\": \"Continue CPR for 2 minutes\\nAdjust tube or suction as needed\",\r\n      \"TeamScreenNextInevitableSteps\": \"Adjust tube or suction as needed\",\r\n      \"CurrentStep2\": \"Continue CPR for 2 minutes\\nAdjust tube or suction as needed\",\r\n      \"NextSteps2\": \"Adjust tube or suction as needed\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"PEA\",\r\n      \"No\": 26,\r\n      \"ID\": 26,\r\n      \"IntermediaryAction(onGuidingPad)\": \"Problem B\",\r\n      \"GuidingPadContent\": \"Info: B – Non-intubated patient\\nToggle : Exsufflation - if asymmetric ventilation\\nOR \\nInfo : B - Intubated patient\\nToggle : Displacement\\/Tube obstruction\\/ Tension pneumothorax\\/ Equipment\",\r\n      \"TeamScreenCurrentStep\": \"Continue CPR for 2 minutes\\nAction B according to selection\",\r\n      \"CurrentStep2\": \"Continue CPR for 2 minutes\\nAction B according to selection\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"PEA\",\r\n      \"No\": 27,\r\n      \"ID\": 27,\r\n      \"IntermediaryAction(onGuidingPad)\": \"Waiting for the end of the 2 min timer\",\r\n      \"TeamScreenCurrentStep\": \"Ensure active CPR Coaching\\nGive epinephrine 0.01 mg\\/kg (0.1 ml\\/kg)\\nPrepare 2nd CPR provider\",\r\n      \"TeamScreenNextInevitableSteps\": \"Check pulse\\nCheck cardiac rhythm\\nSwitch CPR providers \",\r\n      \"CurrentStep2\": \"Give epinephrine 0.01 mg\\/kg (0.1 ml\\/kg)\\nPrepare 2nd CPR provider\\nVerbal countdown to end of 2 min CPR cycle\",\r\n      \"NextSteps2\": \"Check pulse\\nCheck cardiac rhythm\\nSwitch CPR providers \",\r\n      \"CurrentStep\": \"Prepare Epinephrine 0.01 mg\\/kg (0.1 mL\\/kg)\",\r\n      \"NextSteps\": \"Prepare Amiodarone 5 mg\\/kg\"\r\n     },\r\n     {\r\n      \"ScenarioStage\": \"Stage 2: Pulseless VT\",\r\n      \"CurrentStatus\": \"Pulseless VT\",\r\n      \"No\": 28,\r\n      \"ID\": 90,\r\n      \"IntermediaryAction(onGuidingPad)\": \"Check pulse (2)\",\r\n      \"GuidingPadContent\": \"Info : Pulse ?\\nButtons : YES\\/NO\",\r\n      \"TeamScreenCurrentStep\": \"Check pulse\",\r\n      \"CurrentStep2\": \"Check pulse\",\r\n      \"CurrentStep\": \"Prepare Epinephrine 0.01 mg\\/kg (0.1 mL\\/kg)\",\r\n      \"NextSteps\": \"Prepare Amiodarone 5 mg\\/kg\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"Pulseless VT\",\r\n      \"No\": 28,\r\n      \"ID\": 90,\r\n      \"CurrentStepAlgorithm\": \"Check cardiac rhythm\",\r\n      \"GuidingPadContent\": \"Info: Select rhythm\\nButtons : VF\\/pVT\\/Asystole\\/PEA \",\r\n      \"TeamScreenCurrentStep\": \"Check cardiac rhtyhm\\nSwitch CPR providers\\nResume CPR \",\r\n      \"TeamScreenNextInevitableSteps\": \"\\nPrepare for defibrillation 2J\\/kg\\n\",\r\n      \"CurrentStep2\": \"Check cardiac rhtyhm\\nSwitch CPR providers\\nResume CPR \",\r\n      \"NextSteps2\": \"Verbally preview steps for defibrillation\\nPrepare for defibrillation 2J\\/kg\\n\",\r\n      \"CurrentStep\": \"Prepare Amiodarone 5 mg\\/kg\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"Pulseless VT\",\r\n      \"No\": 29,\r\n      \"ID\": 6,\r\n      \"CurrentStepAlgorithm\": \"Shock\",\r\n      \"GuidingPadContent\": \"Info : Confirm this action\\nButton : Shock given\",\r\n      \"TeamScreenCurrentStep\": \"Review steps for defibrillation\\nDefibrillation 2J\\/kg\\nResume CPR & Ensure active CPR Coaching\",\r\n      \"TeamScreenNextInevitableSteps\": \"Resume CPR\\nEnsure active CPR Coaching\",\r\n      \"CurrentStep2\": \"Verbally preview steps for defibrillation\\nDefibrillate 2J\\/kg\\nResume CPR & ensure active CPR Coaching\",\r\n      \"NextSteps2\": \"Resume CPR\\nEnsure active CPR Coaching\",\r\n      \"CurrentStep\": \"Prepare Amiodarone 5 mg\\/kg\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"Pulseless VT\",\r\n      \"No\": 17,\r\n      \"ID\": 7,\r\n      \"CurrentStepAlgorithm\": \"Resume CPR for 2 min\",\r\n      \"GuidingPadContent\": \"Info: Confirm this action\\nButton : CPR resumed\",\r\n      \"TeamScreenCurrentStep\": \"Resume CPR\\nEnsure active CPR Coaching\",\r\n      \"TeamScreenNextInevitableSteps\": \"Order amiodarone 5mg\\/kg\",\r\n      \"CurrentStep2\": \"\\nResume CPR\\nEnsure active CPR Coaching\",\r\n      \"NextSteps2\": \"Order amiodarone 5mg\\/kg\",\r\n      \"CurrentStep\": \"Prepare Amiodarone 5 mg\\/kg\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"Pulseless VT\",\r\n      \"No\": 18,\r\n      \"ID\": 8,\r\n      \"IntermediaryAction(onGuidingPad)\": \"Deliver Amiodarone\",\r\n      \"GuidingPadContent\": \"Medication button : \\na.Prescription list: select Amiodarone + select preparation dose\\nb.Ready button + dose number: ???\\nc.Administered button : Administered\",\r\n      \"TeamScreenCurrentStep\": \"Give amiodarone 5 mg\\/kg\\n\",\r\n      \"CurrentStep2\": \"Give amiodarone 5mg\\/kg\",\r\n      \"CurrentStep\": \"Prepare Amiodarone 5 mg\\/kg\",\r\n      \"NextSteps\": \"Prepare Epinephrine 0.01 mg\\/kg (0.1 mL\\/kg)\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"Pulseless VT\",\r\n      \"No\": 19,\r\n      \"ID\": 8,\r\n      \"IntermediaryAction(onGuidingPad)\": \"Reassess A and B\",\r\n      \"GuidingPadContent\": \"Info: Confirm this action\\nToggle : a) airways PROBLEM\\/NORMAL\\nb) ventilation PROBLEM\\/NORMAL\",\r\n      \"TeamScreenCurrentStep\": \"Reassess A and B\\nContinue CPR for 2 minutes\\n\",\r\n      \"TeamScreenNextInevitableSteps\": \"Ensure active CPR Coaching\\nPrepare 2nd CPR provider\\nReview Hs and Ts\",\r\n      \"CurrentStep2\": \"Reassess A and B\\nContinue CPR for 2 minutes\\n\",\r\n      \"NextSteps2\": \"Prepare 2nd CPR provider                          Review Hs and Ts\\n\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"Pulseless VT\",\r\n      \"No\": 19,\r\n      \"ID\": 8,\r\n      \"IntermediaryAction(onGuidingPad)\": \"Problem A\",\r\n      \"GuidingPadContent\": \"Info: A – Obstructed airways\\nToggle : Repositioning\\/ Aspiration\\/ Foreign body extraction\\/ Wendel\\/ Guedel\",\r\n      \"TeamScreenCurrentStep\": \"Continue CPR for 2 minutes\\nAction A according to selection\",\r\n      \"CurrentStep2\": \"Continue CPR for 2 minutes\\nAction A according to selection\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"Pulseless VT\",\r\n      \"No\": 19,\r\n      \"ID\": 8,\r\n      \"IntermediaryAction(onGuidingPad)\": \"Problem B\",\r\n      \"GuidingPadContent\": \"Info: B – Non-intubated patient\\nToggle : Exsufflation - if asymmetric ventilation\\nOR \\nInfo : B - Intubated patient\\nToggle : Displacement\\/Tube obstruction\\/ Tension pneumothorax\\/ Equipment\",\r\n      \"TeamScreenCurrentStep\": \"Continue CPR for 2 minutes\\nAction B according to selection\",\r\n      \"CurrentStep2\": \"Continue CPR for 2 minutes\\nAction B according to selection\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"Pulseless VT\",\r\n      \"No\": 19,\r\n      \"ID\": 8,\r\n      \"IntermediaryAction(onGuidingPad)\": \"Waiting for the end of the 2 min timer\",\r\n      \"GuidingPadContent\": \"Info : Please wait until the two minutes are up before proceeding.\",\r\n      \"TeamScreenCurrentStep\": \"Ensure active CPR Coaching\\nPrepare 2nd CPR provider\\nReview Hs and Ts\",\r\n      \"TeamScreenNextInevitableSteps\": \"Check cardiac rhythm\\nSwitch CPR providers\",\r\n      \"CurrentStep2\": \"Prepare 2nd CPR provider                          Review Hs and Ts\\nVerbal countdown to end of CPR cycle\",\r\n      \"NextSteps2\": \"Check cardiac rhythm\\nSwitch CPR providers\",\r\n      \"CurrentStep\": \"Prepare Epinephrine 0.01 mg\\/kg (0.1 mL\\/kg)\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"Pulseless VT\",\r\n      \"No\": 19,\r\n      \"ID\": 8,\r\n      \"IntermediaryAction(onGuidingPad)\": \"Check pulse (3)\",\r\n      \"GuidingPadContent\": \"Info : Pulse ?\\nButtons : YES\\/NO\",\r\n      \"TeamScreenCurrentStep\": \"Check pulse\",\r\n      \"CurrentStep2\": \"Check pulse\",\r\n      \"CurrentStep\": \"Prepare Epinephrine 0.01 mg\\/kg (0.1 mL\\/kg)\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"Pulseless VT\",\r\n      \"No\": 10,\r\n      \"ID\": 10,\r\n      \"CurrentStepAlgorithm\": \"Check cardiac rhythm\",\r\n      \"GuidingPadContent\": \"Info: Select rhythm\\nButtons : VF\\/pVT\\/Asystole\\/PEA \",\r\n      \"TeamScreenCurrentStep\": \"Check cardiac rhtyhm\\nSwitch CPR providers\\nResume CPR \",\r\n      \"TeamScreenNextInevitableSteps\": \"\\nPrepare for defibrillation 4J\\/kg\\n\",\r\n      \"CurrentStep2\": \"Check cardiac rhtyhm\\nSwitch CPR providers\\nResume CPR \",\r\n      \"NextSteps2\": \"Verbally preview steps for defibrillation\\nPrepare for defibrillation 4J\\/kg\\n\",\r\n      \"CurrentStep\": \"Prepare Epinephrine 0.01 mg\\/kg (0.1 mL\\/kg)\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"Pulseless VT\",\r\n      \"No\": 11,\r\n      \"ID\": 11,\r\n      \"CurrentStepAlgorithm\": \"Give shock (2)\",\r\n      \"GuidingPadContent\": \"Info : Confirm this action\\nButton : Shock given\",\r\n      \"TeamScreenCurrentStep\": \"Review steps for defibrillation\\nDefibrillation 4J\\/kg\\nResume CPR & Ensure active CPR Coaching\",\r\n      \"TeamScreenNextInevitableSteps\": \"Resume CPR\\nEnsure active CPR Coaching\",\r\n      \"CurrentStep2\": \"Verbally preview steps for defibrillation\\nDefibrillate 4J\\/kg\\nResume CPR & ensure active CPR Coaching\",\r\n      \"NextSteps2\": \"Resume CPR\\nEnsure active CPR Coaching\",\r\n      \"CurrentStep\": \"Prepare Epinephrine 0.01 mg\\/kg (0.1 mL\\/kg)\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"Pulseless VT\",\r\n      \"No\": 12,\r\n      \"ID\": 12,\r\n      \"CurrentStepAlgorithm\": \"Resume CPR (4)\",\r\n      \"GuidingPadContent\": \"Info: Confirm this action\\nButton : CPR resumed\",\r\n      \"TeamScreenCurrentStep\": \"\\nResume CPR\\nEnsure active CPR Coaching\",\r\n      \"TeamScreenNextInevitableSteps\": \"Order epinephrine 0.1 ml\\/kg\",\r\n      \"CurrentStep2\": \"\\nResume CPR\\nEnsure active CPR Coaching\",\r\n      \"NextSteps2\": \"Order epinephrine 0.1 ml\\/kg\",\r\n      \"CurrentStep\": \"Prepare Epinephrine 0.01 mg\\/kg (0.1 mL\\/kg)\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"Pulseless VT\",\r\n      \"No\": 12,\r\n      \"ID\": 12,\r\n      \"IntermediaryAction(onGuidingPad)\": \"Deliver Epinephrine (3)\",\r\n      \"GuidingPadContent\": \"Medication button : \\na.Prescription list: select Epinephrine + select preparation dose\\nb.Ready button + dose number: ???\\nc.Administered button : Administered\",\r\n      \"TeamScreenCurrentStep\": \"Ensure active CPR Coaching\\nGive epinephrine 0.01 mg\\/kg (0.1 ml\\/kg)\\n\",\r\n      \"TeamScreenNextInevitableSteps\": \"Prepare 2nd CPR provider\",\r\n      \"CurrentStep2\": \"Give epinephrine 0..01 mg\\/kg (0.1 ml\\/kg)\\n\",\r\n      \"NextSteps2\": \"Prepare 2nd CPR provider\\nVerbal countdown to end of 2 min CPR cycle\",\r\n      \"CurrentStep\": \"Prepare Epinephrine 0.01 mg\\/kg (0.1 mL\\/kg)\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"Pulseless VT\",\r\n      \"No\": 13,\r\n      \"ID\": 13,\r\n      \"IntermediaryAction(onGuidingPad)\": \"Reassess A and B\",\r\n      \"GuidingPadContent\": \"Info : Confirm this action\\nToggle : a) airways PROBLEM\\/NORMAL\\nb) ventilation PROBLEM\\/NORMAL \",\r\n      \"TeamScreenCurrentStep\": \"Reassess A and B\\nContinue CPR for 2 minutes\\n\",\r\n      \"CurrentStep2\": \"Reassess A and B\\nContinue CPR for 2 minutes\\n\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"Pulseless VT\",\r\n      \"No\": 13,\r\n      \"ID\": 13,\r\n      \"IntermediaryAction(onGuidingPad)\": \"Problem A\",\r\n      \"GuidingPadContent\": \"Info : A- Obstructed airways\\nToggle : Repositioning\\/ Aspiration\\/ Foreign body extraction\\/ Wendel\\/ Guedel\",\r\n      \"TeamScreenCurrentStep\": \"Continue CPR for 2 minutes\\nAction A according to selection\",\r\n      \"CurrentStep2\": \"Continue CPR for 2 minutes\\nAction A according to selection\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"Pulseless VT\",\r\n      \"No\": 13,\r\n      \"ID\": 13,\r\n      \"IntermediaryAction(onGuidingPad)\": \"Problem B\",\r\n      \"GuidingPadContent\": \"Info : B - Non-intubated patient\\nToggle : Exsufflation - if asymmetric ventilation\\nOR Info : B - Intubated patient\\nToggle : Displacement\\/Tube obstruction\\/ Tension pneumothorax\\/ Equipment\",\r\n      \"TeamScreenCurrentStep\": \"Continue CPR for 2 minutes\\nAction B according to selection\",\r\n      \"CurrentStep2\": \"Continue CPR for 2 minutes\\nAction B according to selection\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"Pulseless VT\",\r\n      \"No\": 14,\r\n      \"ID\": 14,\r\n      \"IntermediaryAction(onGuidingPad)\": \"Waiting for the end of the 2 min timer\",\r\n      \"GuidingPadContent\": \"Info : Please wait until the two minutes are up before proceeding.\",\r\n      \"TeamScreenCurrentStep\": \"Ensure active CPR Coaching\\nPrepare 2nd CPR provider\\nReview Hs and Ts\",\r\n      \"TeamScreenNextInevitableSteps\": \"Check pulse\\nCheck cardiac rhythm\\nSwitch CPR providers \",\r\n      \"CurrentStep2\": \"Prepare 2nd CPR provider                          Review Hs and Ts\\nVerbal countdown to end of CPR cycle\",\r\n      \"NextSteps2\": \"Check pulse\\nCheck cardiac rhythm\\nSwitch CPR providers \",\r\n      \"CurrentStep\": \"Prepare Epinephrine 0.01 mg\\/kg (0.1 mL\\/kg)\"\r\n     },\r\n     {\r\n      \"ScenarioStage\": \"Stage 3: VF\",\r\n      \"CurrentStatus\": \"VF\",\r\n      \"No\": 15,\r\n      \"ID\": 15,\r\n      \"IntermediaryAction(onGuidingPad)\": \"Check pulse (4)\",\r\n      \"GuidingPadContent\": \"Info : Pulse ?\\nButtons : YES\\/NO\",\r\n      \"TeamScreenCurrentStep\": \"Check pulse\",\r\n      \"CurrentStep2\": \"Check pulse\",\r\n      \"CurrentStep\": \"Prepare Epinephrine 0.01 mg\\/kg (0.1 mL\\/kg)\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"VF\",\r\n      \"No\": 15,\r\n      \"ID\": 15,\r\n      \"CurrentStepAlgorithm\": \"Check cardiac rhythm\",\r\n      \"GuidingPadContent\": \"Info: Select rhythm\\nButtons : VF\\/pVT\\/Asystole\\/PEA \",\r\n      \"TeamScreenCurrentStep\": \"Check cardiac rhtyhm\\nSwitch CPR providers\\nResume CPR \",\r\n      \"TeamScreenNextInevitableSteps\": \"\\nPrepare for defibrillation 4J\\/kg\\n\",\r\n      \"CurrentStep2\": \"Check cardiac rhtyhm\\nSwitch CPR providers\\nResume CPR \",\r\n      \"NextSteps2\": \"Verbally preview steps for defibrillation\\nPrepare for defibrillation 4J\\/kg\\n\",\r\n      \"CurrentStep\": \"Prepare Epinephrine 0.01 mg\\/kg (0.1 mL\\/kg)\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"VF\",\r\n      \"No\": 16,\r\n      \"ID\": 16,\r\n      \"CurrentStepAlgorithm\": \"Give shock (3)\",\r\n      \"GuidingPadContent\": \"Info : Confirm this action\\nButton : Shock given\",\r\n      \"TeamScreenCurrentStep\": \"Review steps for defibrillation\\nDefibrillation 4J\\/kg\\nResume CPR & Ensure active CPR Coaching\",\r\n      \"TeamScreenNextInevitableSteps\": \"Resume CPR\\nEnsure active CPR Coaching\",\r\n      \"CurrentStep2\": \"Verbally preview steps for defibrillation\\nDefibrillate 4J\\/kg\\nResume CPR & ensure active CPR Coaching\",\r\n      \"NextSteps2\": \"Resume CPR\\nEnsure active CPR Coaching\",\r\n      \"CurrentStep\": \"Prepare Epinephrine 0.01 mg\\/kg (0.1 mL\\/kg)\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"VF\",\r\n      \"No\": 17,\r\n      \"ID\": 17,\r\n      \"CurrentStepAlgorithm\": \"Resume CPR (5)\",\r\n      \"GuidingPadContent\": \"Info: Confirm this action\\nButton : CPR resumed\",\r\n      \"TeamScreenCurrentStep\": \"\\nResume CPR\\nEnsure active CPR Coaching\",\r\n      \"CurrentStep2\": \"\\nResume CPR\\nEnsure active CPR Coaching\",\r\n      \"CurrentStep\": \"Prepare Epinephrine 0.01 mg\\/kg (0.1 mL\\/kg)\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"VF\",\r\n      \"No\": 18,\r\n      \"ID\": 18,\r\n      \"IntermediaryAction(onGuidingPad)\": \"Reassess A and B\",\r\n      \"GuidingPadContent\": \"Info : Confirm this action\\nToggle : a) airways PROBLEM\\/NORMAL\\nb) ventilation PROBLEM\\/NORMAL \",\r\n      \"TeamScreenCurrentStep\": \"Reassess A and B\\nContinue CPR for 2 minutes\\n\",\r\n      \"CurrentStep2\": \"Reassess A and B\\nContinue CPR for 2 minutes\\n\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"VF\",\r\n      \"No\": 18,\r\n      \"ID\": 18,\r\n      \"IntermediaryAction(onGuidingPad)\": \"Problem A\",\r\n      \"GuidingPadContent\": \"Info : A- Obstructed airways\\nToggle : Repositioning\\/ Aspiration\\/ Foreign body extraction\\/ Wendel\\/ Guedel\",\r\n      \"TeamScreenCurrentStep\": \"Continue CPR for 2 minutes\\nAction A according to selection\",\r\n      \"CurrentStep2\": \"Continue CPR for 2 minutes\\nAction A according to selection\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"VF\",\r\n      \"No\": 18,\r\n      \"ID\": 18,\r\n      \"IntermediaryAction(onGuidingPad)\": \"Problem B\",\r\n      \"GuidingPadContent\": \"Info : B - Non-intubated patient\\nToggle : Exsufflation - if asymmetric ventilation\\nOR Info : B - Intubated patient\\nToggle : Displacement\\/Tube obstruction\\/ Tension pneumothorax\\/ Equipment\",\r\n      \"TeamScreenCurrentStep\": \"Continue CPR for 2 minutes\\nAction B according to selection\",\r\n      \"CurrentStep2\": \"Continue CPR for 2 minutes\\nAction B according to selection\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"VF\",\r\n      \"No\": 18,\r\n      \"ID\": 18,\r\n      \"IntermediaryAction(onGuidingPad)\": \"Waiting for the end of the 2 min timer\",\r\n      \"GuidingPadContent\": \"Info : Please wait until the two minutes are up before proceeding.\",\r\n      \"TeamScreenCurrentStep\": \"Ensure active CPR Coaching\\nPrepare 2nd CPR provider\",\r\n      \"TeamScreenNextInevitableSteps\": \"Check pulse\\nCheck cardiac rhythm\\nSwitch CPR providers \",\r\n      \"CurrentStep2\": \"Prepare 2nd CPR provider\\nVerbal countdown to end of 2 min CPR cycle\",\r\n      \"NextSteps2\": \"Check pulse\\nCheck cardiac rhythm\\nSwitch CPR providers \",\r\n      \"CurrentStep\": \"Prepare Epinephrine 0.01 mg\\/kg (0.1 mL\\/kg)\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"VF\",\r\n      \"No\": 19,\r\n      \"ID\": 19,\r\n      \"IntermediaryAction(onGuidingPad)\": \"Check pulse (3)\",\r\n      \"GuidingPadContent\": \"Info : Pulse ?\\nButtons : YES\\/NO\",\r\n      \"TeamScreenCurrentStep\": \"Check pulse\",\r\n      \"CurrentStep2\": \"Check pulse\",\r\n      \"CurrentStep\": \"Prepare Epinephrine 0.01 mg\\/kg (0.1 mL\\/kg)\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"VF\",\r\n      \"No\": 10,\r\n      \"ID\": 10,\r\n      \"CurrentStepAlgorithm\": \"Check cardiac rhythm\",\r\n      \"GuidingPadContent\": \"Info: Select rhythm\\nButtons : VF\\/pVT\\/Asystole\\/PEA \",\r\n      \"TeamScreenCurrentStep\": \"Check cardiac rhtyhm\\nSwitch CPR providers\\nResume CPR \",\r\n      \"TeamScreenNextInevitableSteps\": \"\\nPrepare for defibrillation 4J\\/kg\\n\",\r\n      \"CurrentStep2\": \"Check cardiac rhtyhm\\nSwitch CPR providers\\nResume CPR \",\r\n      \"NextSteps2\": \"Verbally preview steps for defibrillation\\nPrepare for defibrillation 4J\\/kg\\n\",\r\n      \"CurrentStep\": \"Prepare Epinephrine 0.01 mg\\/kg (0.1 mL\\/kg)\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"VF\",\r\n      \"No\": 11,\r\n      \"ID\": 11,\r\n      \"CurrentStepAlgorithm\": \"Give shock (2)\",\r\n      \"GuidingPadContent\": \"Info : Confirm this action\\nButton : Shock given\",\r\n      \"TeamScreenCurrentStep\": \"Review steps for defibrillation\\nDefibrillation 4J\\/kg\\nResume CPR & Ensure active CPR Coaching\",\r\n      \"TeamScreenNextInevitableSteps\": \"Resume CPR\\nEnsure active CPR Coaching\",\r\n      \"CurrentStep2\": \"Verbally preview steps for defibrillation\\nDefibrillate 4J\\/kg\\nResume CPR & ensure active CPR Coaching\",\r\n      \"NextSteps2\": \"Resume CPR\\nEnsure active CPR Coaching\",\r\n      \"CurrentStep\": \"Prepare Epinephrine 0.01 mg\\/kg (0.1 mL\\/kg)\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"VF\",\r\n      \"No\": 12,\r\n      \"ID\": 12,\r\n      \"CurrentStepAlgorithm\": \"Resume CPR (4)\",\r\n      \"GuidingPadContent\": \"Info: Confirm this action\\nButton : CPR resumed\",\r\n      \"TeamScreenCurrentStep\": \"\\nResume CPR\\nEnsure active CPR Coaching\",\r\n      \"TeamScreenNextInevitableSteps\": \"Order epinephrine 0.1 ml\\/kg\",\r\n      \"CurrentStep2\": \"\\nResume CPR\\nEnsure active CPR Coaching\",\r\n      \"NextSteps2\": \"Order epinephrine 0.1 ml\\/kg\",\r\n      \"CurrentStep\": \"Prepare Epinephrine 0.01 mg\\/kg (0.1 mL\\/kg)\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"VF\",\r\n      \"No\": 12,\r\n      \"ID\": 12,\r\n      \"IntermediaryAction(onGuidingPad)\": \"Deliver Epinephrine (3)\",\r\n      \"GuidingPadContent\": \"Medication button : \\na.Prescription list: select Epinephrine + select preparation dose\\nb.Ready button + dose number: ???\\nc.Administered button : Administered\",\r\n      \"TeamScreenCurrentStep\": \"Ensure active CPR Coaching\\nGive epinephrine 0.01 mg\\/kg (0.1 ml\\/kg)\\n\",\r\n      \"TeamScreenNextInevitableSteps\": \"Prepare 2nd CPR provider\",\r\n      \"CurrentStep2\": \"Give epinephrine 0.01 mg\\/kg (0.1 ml\\/kg)\\n\",\r\n      \"NextSteps2\": \"Prepare 2nd CPR provider\\nVerbal countdown to end of 2 min CPR cycle\",\r\n      \"CurrentStep\": \"Prepare Epinephrine 0.01 mg\\/kg (0.1 mL\\/kg)\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"VF\",\r\n      \"No\": 18,\r\n      \"ID\": 18,\r\n      \"IntermediaryAction(onGuidingPad)\": \"Reassess A and B\",\r\n      \"GuidingPadContent\": \"Info : Confirm this action\\nToggle : a) airways PROBLEM\\/NORMAL\\nb) ventilation PROBLEM\\/NORMAL \",\r\n      \"TeamScreenCurrentStep\": \"Reassess A and B\\nContinue CPR for 2 minutes\\n\",\r\n      \"CurrentStep2\": \"Reassess A and B\\nContinue CPR for 2 minutes\\n\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"VF\",\r\n      \"No\": 18,\r\n      \"ID\": 18,\r\n      \"IntermediaryAction(onGuidingPad)\": \"Problem A\",\r\n      \"GuidingPadContent\": \"Info : A- Obstructed airways\\nToggle : Repositioning\\/ Aspiration\\/ Foreign body extraction\\/ Wendel\\/ Guedel\",\r\n      \"TeamScreenCurrentStep\": \"Continue CPR for 2 minutes\\nAction A according to selection\",\r\n      \"CurrentStep2\": \"Continue CPR for 2 minutes\\nAction A according to selection\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"VF\",\r\n      \"No\": 18,\r\n      \"ID\": 18,\r\n      \"IntermediaryAction(onGuidingPad)\": \"Problem B\",\r\n      \"GuidingPadContent\": \"Info : B - Non-intubated patient\\nToggle : Exsufflation - if asymmetric ventilation\\nOR Info : B - Intubated patient\\nToggle : Displacement\\/Tube obstruction\\/ Tension pneumothorax\\/ Equipment\",\r\n      \"TeamScreenCurrentStep\": \"Continue CPR for 2 minutes\\nAction B according to selection\",\r\n      \"CurrentStep2\": \"Continue CPR for 2 minutes\\nAction B according to selection\"\r\n     },\r\n     {\r\n      \"CurrentStatus\": \"VF\",\r\n      \"No\": 18,\r\n      \"ID\": 18,\r\n      \"IntermediaryAction(onGuidingPad)\": \"Waiting for the end of the 2 min timer\",\r\n      \"GuidingPadContent\": \"Info : Please wait until the two minutes are up before proceeding.\",\r\n      \"TeamScreenCurrentStep\": \"Ensure active CPR Coaching\\nPrepare 2nd CPR provider\",\r\n      \"TeamScreenNextInevitableSteps\": \"Check pulse\\nCheck cardiac rhythm\\nSwitch CPR providers \",\r\n      \"CurrentStep2\": \"Prepare 2nd CPR provider\\nVerbal countdown to end of 2 min CPR cycle\",\r\n      \"NextSteps2\": \"Check pulse\\nCheck cardiac rhythm\\nSwitch CPR providers \"\r\n     }\r\n    ]");
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
    TextMeshProUGUI CardiacRhythm;

    CanvasElementRoundedRect CPR_Plate;
    CanvasElementRoundedRect Epi_Plate;
    GameObject medUI;
    GameObject noti;
    Transform notiTransform;

    RawImage AlgoImg;

    public Material[] mat = new Material[9];
    public GameObject notiCprPref;
    public GameObject notiEpiPref;
    public GameObject notiMedPref;
    /*
    *CanvasBackplate
    *CPRBorderCanvasBackplate //CPR less 10 sec
    *CPROriginCanvasBackplate //CPR Original
    *EpiBorderCanvasBackplate //Epi less 10 sec
    *EpiOriginCanvasBackplate //Epi Original
    *RedBorderCanvasBackplate //0 sec left flash for 5 secs
    *RedBorderCanvasBackplate //0 sec left flash for 5 secs without border
    */

    double time1 = 0;
    double time2 = 0;
    float effectTime = 1f;

    // Update is called once per frame

    public SocketIOUnity socket;
    int idx = 1;

    double startTimestamp = 0;
    double cprStartTimestamp = 0;
    double epiStartTimestamp = 0;
    double prev_cprStartTimestamp = 0;
    double prev_epiStartTimestamp = 0;

    bool cpr_5sec = false;
    bool epi_5sec = false;

    bool cpr_5sec_coroutine = false;
    bool epi_5sec_coroutine = false;

    ArrayList notiArr = new ArrayList();
    ArrayList notiCprArr = new ArrayList();
    ArrayList notiEpiArr = new ArrayList();

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

        if (GameObject.FindWithTag("CardiacRhythm") != null) {
           CardiacRhythm = GameObject.FindWithTag("CardiacRhythm").GetComponent<TextMeshProUGUI>();
        }

        if (GameObject.FindWithTag("algoImg") != null) {
           AlgoImg = GameObject.FindWithTag("algoImg").GetComponent<RawImage>();
        }

        if (GameObject.FindWithTag("CPRTimerPlate") != null) {
           CPR_Plate = GameObject.FindWithTag("CPRTimerPlate").GetComponent<CanvasElementRoundedRect>();
        }

        if (GameObject.FindWithTag("EpiTimerPlate") != null) {
           Epi_Plate = GameObject.FindWithTag("EpiTimerPlate").GetComponent<CanvasElementRoundedRect>();
        }

        medUI = GameObject.FindWithTag("Medication_UI");
        noti = GameObject.FindWithTag("Notifications");

        if (noti != null) {
            notiTransform = noti.transform;
        }

        var uri = new Uri("http://136.159.140.66");

        socket = new SocketIOUnity(uri, new SocketIOOptions
        {
            Path = "/cpr/socket.io"
        });

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
            long unixTime = ((DateTimeOffset)currentTime).ToUnixTimeMilliseconds();

            startTimestamp = response.GetValue<double>(1);
            cprStartTimestamp = response.GetValue<double>(2);
            epiStartTimestamp = response.GetValue<double>(3);

            Debug.Log("startTimestamp:" + startTimestamp);

            time1 = (cprStartTimestamp - unixTime) / 1000 + 120;
            time2 = (epiStartTimestamp - unixTime) / 1000 + 240;

            m_queueAction.Enqueue(() => UpdateUI(idx));
        });

        socket.On("medication", (response) => {
            try {
                Debug.Log("medication");
                Debug.Log(response);
                string name = response.GetValue<string>(0);
                string dose = response.GetValue<string>(1);

                m_queueAction.Enqueue(() => UpdateNoti(name, dose, 0));
            } catch (Exception e) {
                Debug.Log(e);
            }
        });

        Debug.Log("Connecting...");
        socket.Connect();
    }

/*
*    type: 0 medication
*    type: 1 cpr
*    type: 2 epi
*/
    void UpdateNoti (string name, string dose, int type) {
        if (noti != null && notiTransform != null) {
            if (type == 0 && notiMedPref != null) {
                GameObject myInstance = Instantiate(notiMedPref, notiTransform);

                notiArr.Add(myInstance);
                StartCoroutine(Remove_Noti(myInstance, 0));
                TextMeshProUGUI txt = myInstance.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
                txt.text = name + "\n" + dose + " given";
            } else if (type == 1 && notiCprPref != null) {
                GameObject myInstance = Instantiate(notiCprPref, notiTransform);

                notiCprArr.Add(myInstance);

                StartCoroutine(Remove_Noti(myInstance, 1));
            } else if (type == 2 && notiEpiPref != null) {
                GameObject myInstance = Instantiate(notiEpiPref, notiTransform);

                notiEpiArr.Add(myInstance);

                StartCoroutine(Remove_Noti(myInstance, 2));
            }
        }

        Debug.Log("name: " + name + ", dose: " + dose);
    }

    void UpdateUI(int idx)
    {
        Debug.Log("UpdateUI");
        Debug.Log("UpdateUI: " + idx);
        if (prev_cprStartTimestamp != cprStartTimestamp) {
            StartCoroutine(SetCPR_5Sec(false));
            cpr_5sec = false;
            cpr_5sec_coroutine = false;
            prev_cprStartTimestamp = cprStartTimestamp;
        }
        
        if (prev_epiStartTimestamp != epiStartTimestamp) {
            StartCoroutine(SetEpi_5Sec(false));
            epi_5sec = false;
            epi_5sec_coroutine = false;
            prev_epiStartTimestamp = epiStartTimestamp;
        }
        // TextMeshProUGUI temp = GameObject.FindWithTag("Amiodarone").GetComponent<TextMeshProUGUI>();
        var lastID = 1;
        var index = 0;
        foreach(JObject obj in algorithm)
        {
            try
            {
                if (obj["ID"] != null) {
                    lastID = int.Parse(obj["ID"].ToString());
                }
                // if (int.Parse(obj["No"].ToString()) == idx) {
                if (index == idx) {

                    if (AlgoImg != null)
                    {
                        if (obj["ID"] != null) {
                            var texture = Resources.Load<Texture>("Texture/" + obj["ID"]);
                            AlgoImg.texture = texture;
                        } else {
                            var texture = Resources.Load<Texture>("Texture/" + lastID);
                            AlgoImg.texture = texture;
                        }
                    }

                    if (CardiacRhythm != null)
                    {
                        if (obj["CurrentStatus"] != null) {
                            CardiacRhythm.text = "Cardiac Rhythm: " + obj["CurrentStatus"].ToString();
                        } else {
                            CardiacRhythm.text = "Cardiac Rhythm: ";
                        }
                    }
                    // Debug.Log(index);
                    // Debug.Log(obj);
                    Init_Tasks();
                    if (obj["CurrentStep"] == null) {
                        
                    } else {
                        string[] instrunctions = obj["CurrentStep"].ToString().Split('\n');
                        instrunctions = instrunctions.Where(ins => ins != "" && ins != null).ToArray();

                        for(int i = 0; i < instrunctions.Length; i++)
                        {
                            string instruction = instrunctions[i];

                            if (i == 0) {
                                if (Nurse_Cur_1 != null) {
                                    Nurse_Cur_1.text = instruction.Replace("1)", "•");
                                    if (String.IsNullOrWhiteSpace(Nurse_Cur_1.text.Replace("•", ""))) {
                                        Nurse_Cur_1.text = "";
                                    }
                                }
                            } else if (i == 1) {
                                if (Nurse_Cur_2 != null) {
                                    Nurse_Cur_2.text = instruction.Replace("2)", "•");
                                    if (String.IsNullOrWhiteSpace(Nurse_Cur_2.text.Replace("•", ""))) {
                                        Nurse_Cur_2.text = "";
                                    }
                                }
                            } else if (i == 2) {
                                if (Nurse_Cur_3 != null) {
                                    Nurse_Cur_3.text = instruction.Replace("3)", "•");
                                    if (String.IsNullOrWhiteSpace(Nurse_Cur_3.text.Replace("•", ""))) {
                                        Nurse_Cur_3.text = "";
                                    }
                                }
                            }
                        }
                    }

                    if (obj["NextSteps"] == null) {
                        
                    } else {
                        string[] instrunctions = obj["NextSteps"].ToString().Split('\n');
                        instrunctions = instrunctions.Where(ins => ins != "" && ins != null).ToArray();

                        for(int i = 0; i < instrunctions.Length; i++)
                        {
                            string instruction = instrunctions[i];
                            if (i == 0) {
                                if (Nurse_Next_1 != null) {
                                    Nurse_Next_1.text = instruction.Replace("1)", "•");
                                    if (String.IsNullOrWhiteSpace(Nurse_Next_1.text.Replace("•", ""))) {
                                        Nurse_Next_1.text = "";
                                    }
                                }
                            } else if (i == 1) {
                                if (Nurse_Next_2 != null) {
                                    Nurse_Next_2.text = instruction.Replace("2)", "•");
                                    if (String.IsNullOrWhiteSpace(Nurse_Next_2.text.Replace("•", ""))) {
                                        Nurse_Next_2.text = "";
                                    }
                                }
                            } else if (i == 2) {
                                if (Nurse_Next_3 != null) {
                                    Nurse_Next_3.text = instruction.Replace("3)", "•");
                                    if (String.IsNullOrWhiteSpace(Nurse_Next_3.text.Replace("•", ""))) {
                                        Nurse_Next_3.text = "";
                                    }
                                }
                            }
                        }
                    }

                    if (obj["CurrentStep2"] == null) {
                        
                    } else {
                        string[] instrunctions = obj["CurrentStep2"].ToString().Split('\n');
                        instrunctions = instrunctions.Where(ins => ins != "" && ins != null).ToArray();

                        for(int i = 0; i < instrunctions.Length; i++)
                        {
                            string instruction = instrunctions[i];

                            if (i == 0) {
                                if (Doc_Cur_1 != null) {
                                    Doc_Cur_1.text = instruction.Replace("1)", "•");
                                    if (String.IsNullOrWhiteSpace(Doc_Cur_1.text.Replace("•", ""))) {
                                        Doc_Cur_1.text = "";
                                    }
                                }
                            } else if (i == 1) {
                                if (Doc_Cur_2 != null) {
                                    Doc_Cur_2.text = instruction.Replace("2)", "•");
                                    if (String.IsNullOrWhiteSpace(Doc_Cur_2.text.Replace("•", ""))) {
                                        Doc_Cur_2.text = "";
                                    }
                                }
                            } else if (i == 2) {
                                if (Doc_Cur_3 != null) {
                                    Doc_Cur_3.text = instruction.Replace("3)", "•");
                                    if (String.IsNullOrWhiteSpace(Doc_Cur_3.text.Replace("•", ""))) {
                                        Doc_Cur_3.text = "";
                                    }
                                }
                            }
                        }
                    }

                    if (obj["NextSteps2"] == null) {
                        
                    } else {
                        string[] instrunctions = obj["NextSteps2"].ToString().Split('\n');
                        instrunctions = instrunctions.Where(ins => ins != "" && ins != null).ToArray();

                        for(int i = 0; i < instrunctions.Length; i++)
                        {
                            string instruction = instrunctions[i];
                            if (i == 0) {
                                if (Doc_Next_1 != null) {
                                    Doc_Next_1.text = instruction.Replace("1)", "•");
                                    if (String.IsNullOrWhiteSpace(Doc_Next_1.text.Replace("•", ""))) {
                                        Doc_Next_1.text = "";
                                    }
                                }
                            } else if (i == 1) {
                                if (Doc_Next_2 != null) {
                                    Doc_Next_2.text = instruction.Replace("2)", "•");
                                    if (String.IsNullOrWhiteSpace(Doc_Next_2.text.Replace("•", ""))) {
                                        Doc_Next_2.text = "";
                                    }
                                }
                            } else if (i == 2) {
                                if (Doc_Next_3 != null) {
                                    Doc_Next_3.text = instruction.Replace("3)", "•");
                                    if (String.IsNullOrWhiteSpace(Doc_Next_3.text.Replace("•", ""))) {
                                        Doc_Next_3.text = "";
                                    }
                                }
                            }
                        }
                    }
                }
            } catch (Exception e) {
                Debug.Log(e);
            } finally {
                index++;
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
        DateTime currentTime = DateTime.UtcNow;
        long unixTime = ((DateTimeOffset)currentTime).ToUnixTimeMilliseconds();
        bool onOff = ((int) (Time.time * 10)) % 6 == 0 || ((int) (Time.time * 10)) % 6 == 1 || ((int) (Time.time * 10)) % 6 == 2;

        if (timer1 != null && time1 > 0) {
            //time1 -= Time.deltaTime;
            time1 = (cprStartTimestamp - unixTime) / 1000 + 120;
            string min = ((int)time1 / 60 % 60 ).ToString();
            if (min.Length == 1) {
                min = "0" + min;
            }
            string sec = ((int)time1 % 60 ).ToString();
            if (sec.Length == 1) {
                sec = "0" + sec;
            }
            timer1.text = min + ":" + sec;
        } else if (timer1 != null && time1 <= 0 && timer1.text != "00:00"){
            timer1.text = "00:00";
        }

        if (timer1 != null) {
            if ((int)time1 <= 0) {
                if (onOff) {
                    if (cpr_5sec == false) {
                        if (cpr_5sec_coroutine == false)
                        {
                            StartCoroutine(SetCPR_5Sec(true));
                            //Initial reach to 0 sec
                            if (notiCprArr.Count == 0) {
                                UpdateNoti("", "", 1);
                            }
                        }
                        CPR_Plate.material = mat[5];
                    }
                } else {
                    CPR_Plate.material = mat[6];
                }
            } else if ((int)time1 <= 10) {
                if (onOff) {
                    CPR_Plate.material = mat[1];
                } else {
                    CPR_Plate.material = mat[2];
                }
            } else if ((int)time1 > 10) {
                CPR_Plate.material = mat[2];
            }
        }

        if (timer2 != null && time2 > 0) {
            //time2 -= Time.deltaTime;
            time2 = (epiStartTimestamp - unixTime) / 1000 + 240;
            string min = ((int)time2 / 60 % 60 ).ToString();
            if (min.Length == 1) {
                min = "0" + min;
            }
            string sec = ((int)time2 % 60 ).ToString();
            if (sec.Length == 1) {
                sec = "0" + sec;
            }
            timer2.text = min + ":" + sec;
        } else if (timer2 != null && time2 <= 0 && timer2.text != "00:00"){
            timer2.text = "00:00";
        }
        if (timer2 != null) {
            if ((int)time2 <= 0) {
                if (onOff) {
                    if (epi_5sec == false) {
                        if (epi_5sec_coroutine == false)
                        {
                            StartCoroutine(SetEpi_5Sec(true));
                            if (notiEpiArr.Count == 0) {
                                UpdateNoti("", "", 2);
                            }
                        }
                        Epi_Plate.material = mat[5];
                    }
                } else {
                    Epi_Plate.material = mat[6];
                }
            } else if ((int)time2 <= 10) {
                if (onOff) {
                    Epi_Plate.material = mat[3];
                } else {
                    Epi_Plate.material = mat[4];
                }
            } else if ((int)time2 > 10) {
                Epi_Plate.material = mat[4];
            }
        }
    }

    void FlashNoti() {

        bool onOff = ((int) (Time.time * 10)) % 6 == 0 || ((int) (Time.time * 10)) % 6 == 1 || ((int) (Time.time * 10)) % 6 == 2;

        for (int i = 0; i < notiArr.Count; i++) {
            GameObject temp = (GameObject)notiArr[i];
            if (onOff) {
                temp.transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<CanvasElementRoundedRect>().material = mat[7];
            } else {
                temp.transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<CanvasElementRoundedRect>().material = mat[8];
            }
        }

        for (int i = 0; i < notiCprArr.Count; i++) {
            GameObject temp = (GameObject)notiCprArr[i];
            if (onOff) {
                temp.transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<CanvasElementRoundedRect>().material = mat[5];
            } else {
                temp.transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<CanvasElementRoundedRect>().material = mat[6];
            }
        }

        for (int i = 0; i < notiEpiArr.Count; i++) {
            GameObject temp = (GameObject)notiEpiArr[i];
            if (onOff) {
                temp.transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<CanvasElementRoundedRect>().material = mat[5];
            } else {
                temp.transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<CanvasElementRoundedRect>().material = mat[6];
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateClock();
        FlashNoti();
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

    public void StartGazeHover(GameObject gameObject)
    { 
        // Debug.Log("Started GazeHover");
        // Debug.Log(gameObject.name);
        timeActivated = Time.time;
    }

    public void EndGazeHover(GameObject gameObject)
    { 
        Debug.Log("Started GazeHover");
        DateTime currentTime = DateTime.UtcNow;
        long unixTime = ((DateTimeOffset)currentTime).ToUnixTimeSeconds();
        Debug.Log($"{gameObject.name}, {Time.time - timeActivated}, {unixTime}, {DateTime.Now.ToLocalTime()}");
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

    IEnumerator SetCPR_5Sec(bool val)
    {
        cpr_5sec_coroutine = val;
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(10);

        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
        cpr_5sec = val;
        Debug.Log("cpr_5sec: " + cpr_5sec);
    }

    IEnumerator SetEpi_5Sec(bool val)
    {
        epi_5sec_coroutine = val;
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(10);

        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
        epi_5sec = val;
        Debug.Log("epi_5sec: " + epi_5sec);
    }

    public void minimizeMed()
    {
        if (medUI != null) {
            // medUI.SetActive(true);
            // yield return new WaitForSeconds(0.2f);
            // Vector3 origScale = medUI.transform.localScale;
            // Vector3 largerScale = origScale + new Vector3(0.2f, 0.2f, 0f);
            // for(float t = 0f; t < 6f; t += 6f * Time.deltaTime / effectTime)
            // {
            //     float v = Mathf.PingPong(t, 1f);
            //     medUI.transform.localScale = Vector3.Lerp(origScale, largerScale, v);
            //     yield return null;
            // }
            // medUI.transform.localScale = origScale;
            // yield return new WaitForSeconds(0.2f);
            medUI.SetActive(false);
        }
    }

    public void maximizeMed()
    {
        if (medUI != null) {
            // medUI.SetActive(false);
            // yield return new WaitForSeconds(0.2f);
            // Vector3 origScale = medUI.transform.localScale;
            // Vector3 largerScale = origScale + new Vector3(0.2f, 0.2f, 0f);
            // for(float t = 0f; t < 6f; t += 6f * Time.deltaTime / effectTime)
            // {
            //     float v = Mathf.PingPong(t, 1f);
            //     medUI.transform.localScale = Vector3.Lerp(largerScale, origScale, v);
            //     yield return null;
            // }
            // medUI.transform.localScale = largerScale;
            // yield return new WaitForSeconds(0.2f);
            medUI.SetActive(true);
        }
    }

    IEnumerator Remove_Noti(GameObject go, int type)
    {
        Debug.Log("Started Coroutine at timestamp : " + Time.time);
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(10);
        go.SetActive(false);
        if (type == 0) {
            notiArr.Remove(go);
            Destroy(go, 0.0f);
        } else if (type == 1) {
            notiCprArr.Remove(go);
            Destroy(go, 0.0f);
        } else if (type == 2) {
            notiEpiArr.Remove(go);
            Destroy(go, 0.0f);
        }
        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }
}
