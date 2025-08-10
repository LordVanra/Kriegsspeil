using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class CombatHandler : MonoBehaviour
{
    private List<GameObject> selectedObjects = new List<GameObject>();
    private Slider enable;
    private Slider inRiver;
    private Slider againstRiver;
    private Slider flanking;
    private Slider topology;
    private string attacker;
    private GameObject defender;
    private int advantages;
    private bool disOrg = false;
    private struct Block
    {
        public int str;
        public double dist;
        public string type;
        public Vector2 pos;

        public Block(int st, double dis, string t, Vector2 p)
        {
            str = st;
            dist = dis;
            type = t;
            pos = p;
        }
    }

    private GameObject combat;
    private GameObject combatBG;
    private GameObject mainCamera;
    private Camera cameraObj;
    private List<Block> attackers = new List<Block>();
    private List<GameObject> fired = new List<GameObject>();

    void Start()
    {
        enable = GetComponent<Slider>();

        combat = GameObject.Find("CombatCanv");
        combatBG = GameObject.Find("CombatBG");
        mainCamera = GameObject.Find("Main Camera");
        cameraObj = mainCamera.GetComponent<Camera>();

        flanking = GameObject.Find("Flanking").GetComponent<Slider>();
        topology = GameObject.Find("Topology").GetComponent<Slider>();
        inRiver = GameObject.Find("InRiver").GetComponent<Slider>();
        againstRiver = GameObject.Find("AgainstRiver").GetComponent<Slider>();

        combat.SetActive(false);
        combatBG.SetActive(false);
    }

    void Update()
    {
        combatBG.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z + 2f);
        combatBG.transform.localScale = new Vector3(2.8f * cameraObj.orthographicSize, 1.4f * cameraObj.orthographicSize, 1f);

        if (enable.value == 1f && Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                GameObject clickedObject = hit.collider.gameObject;
                if (!(selectedObjects.Contains(clickedObject)))
                {
                    SelectObject(clickedObject);
                }
            }
        }
    }

    public int getAdvantages(int attackers){
        int adv = (int) (flanking.value + topology.value + againstRiver.value * attackers - inRiver.value);
        if(defender.GetComponent<Drag>().disOrgMult == 0.5f){
            adv += 1;
        }
        return adv;
    }

    public void formSubmit(){
        advantages = getAdvantages(attackers.Count);

        toggleCombat();

        int newCount = doCombat(attackers.ToArray(), int.Parse(defender.GetComponentInChildren<Canvas>().GetComponentInChildren<TextMeshProUGUI>(true).text), defender.name.Split(' ')[1], advantages);
        if(defender.name.Split(' ')[1] == "Off(Clone)"){
            int n = 0;
            int m = int.Parse(defender.GetComponentInChildren<Canvas>().GetComponentInChildren<TextMeshProUGUI>(true).text);
            for(int i = 0; i < m - newCount; i++){
                n = UnityEngine.Random.Range(1, m);
                m--;
                if(n == 1 || n == 2){
                    Debug.Log("AE");
                }
            }
        }
        defender.GetComponentInChildren<Canvas>().GetComponentInChildren<TextMeshProUGUI>(true).text = newCount.ToString();
        if(disOrg){
            defender.GetComponent<Drag>().disOrgMult = 0.5f;
        }
        else{
            defender.GetComponent<Drag>().disOrgMult = 1f;
        }

        selectedObjects.Clear();
        attackers.Clear();
        disOrg = false;
        advantages = 0;
    }

    public void clearCombat()
    {
        selectedObjects.Clear();
        fired.Clear();
    }

    void SelectObject(GameObject obj)
    {
        if (selectedObjects.Count == 0 && !fired.Contains(obj))
        {
            attacker = obj.name.Split(' ')[0];
            selectedObjects.Add(obj);
        }
        else if (attacker == obj.name.Split(' ')[0] && !fired.Contains(obj))
        {
            selectedObjects.Add(obj);
        }
        else if(selectedObjects.Count != 0)
        {
            for (int i = 0; i < selectedObjects.Count; i++)
            {
                GameObject currBlock = selectedObjects[i];
                int health = int.Parse(currBlock.GetComponentInChildren<Canvas>().GetComponentInChildren<TextMeshProUGUI>(true).text);
                double dist = Vector2.Distance(currBlock.transform.position, obj.transform.position);

                attackers.Add(new Block(health, dist, currBlock.name.Split(' ')[2], currBlock.transform.position));
                fired.Add(currBlock);
                //Debug.Log(currBlock);
            }

            defender = obj;
            //Debug.Log(fired.Contains(defender));
            toggleCombat();

            flanking.maxValue = topology.maxValue = inRiver.maxValue = selectedObjects.Count;

        }
    }

    int doCombat(Block[] attackers, int def, string defType, int adv)
    {

        adv += attackers.Length - 1;

        float mAttackers = 0;
        float rAttackers = 0;
        double distance = 0;
        GameObject[] generals = GameObject.FindGameObjectsWithTag("Officer");
        int c = 0;

        foreach (Block block in attackers)
        {
            switch (block.type)
            {
                case "Inf(Clone)":
                    if (block.dist < 2)
                    {
                        mAttackers += block.str * 1.5f;
                        disOrg = true;
                    }
                    else
                    {
                        rAttackers += block.str * 1.5f;
                        distance += block.dist;
                        c++;
                    }

                    for (int i = 0; i < generals.Length; i++)
                    {
                        if (Vector2.Distance(block.pos, generals[i].transform.position) < 3.5f)
                        {
                            adv++;
                        }
                    }
                    break;

                case "Art(Clone)":
                    rAttackers += block.str * 18f;
                    distance += block.dist;
                    if(block.dist < 17.5f){
                        disOrg = true;
                    }
                    c++;

                    for (int i = 0; i < generals.Length; i++)
                    {
                        if (Vector2.Distance(block.pos, generals[i].transform.position) < 3.5f)
                        {
                            adv++;
                        }
                    }
                    break;

                case "Cav(Clone)":
                    mAttackers += block.str * 4f;
                    disOrg = true;

                    for (int i = 0; i < generals.Length; i++)
                    {
                        if (Vector2.Distance(block.pos, generals[i].transform.position) < 3.5f)
                        {
                            adv++;
                        }
                    }
                    break;

                case "Off(Clone)":
                    mAttackers += block.str * 4f;
                    break;

                case "Skirm(Clone)":
                    rAttackers += block.str * 1.5f;
                    distance += block.dist;
                    c++;

                    for (int i = 0; i < generals.Length; i++)
                    {
                        if (Vector2.Distance(block.pos, generals[i].transform.position) < 7f)
                        {
                            adv++;
                        }
                    }
                    break;

                default:
                    break;
            }
        }
        distance /= c + 0.01f;
        rAttackers *= Mathf.Pow(1.1f, adv);
        mAttackers *= Mathf.Pow(1.1f, adv);
        double rangedCasualties = Mathf.Pow((rAttackers / def), 0.8f) * (0.1 * UnityEngine.Random.Range(1, 7) + 0.6f) * (Math.Exp(-distance / 15f) + 1f / ((4f * distance + 1f))) * 5f;
        double meleeCasualties = Mathf.Pow((mAttackers / def), 0.8f) * (0.1 * UnityEngine.Random.Range(1, 7) + 0.6f) * 10f;
        double survivors = def - def * (rangedCasualties + meleeCasualties) * 0.01f;

        float threshold = 0f;
        switch (defType)
        {
            case "Inf":
                threshold = 60f;
                break;
            case "Art":
                threshold = 7f;
                break;
            case "Cav":
                threshold = 30f;
                break;
            case "Off":
                threshold = 22f;
                break;
            case "Skirm":
                threshold = 40f;
                break;
        }

        if (survivors < threshold * Mathf.Pow(1.1f, -adv))
        {
            survivors = 0;
        }

        return (int)survivors;
    }

    public void toggleCombat()
    {
        combat.SetActive(!combat.activeSelf);
        combatBG.SetActive(!combatBG.activeSelf);

        CameraBehavior.combatClosed = !CameraBehavior.combatClosed;
    }
}