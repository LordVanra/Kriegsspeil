using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class CombatHandler : MonoBehaviour
{
  private List<GameObject> selectedObjects = new List<GameObject>();
  private Slider enable;
  private string attacker;

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


  private List<Block> attackers = new List<Block>();

  void Start()
  {
    enable = GetComponent<Slider>();
  }

  void Update()
  {

    if (enable.value == 1f && Input.GetMouseButtonDown(0))
    {
      RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
      if (hit.collider != null)
      {
        GameObject clickedObject = hit.collider.gameObject;
        if (!selectedObjects.Contains(clickedObject))
        {
          SelectObject(clickedObject);
        }
      }
    }
  }

  void SelectObject(GameObject obj)
  {
    if (selectedObjects.Count == 0)
    {
      attacker = obj.name.Split(' ')[0];
      selectedObjects.Add(obj);
    }
    else if (attacker == obj.name.Split(' ')[0])
    {
      selectedObjects.Add(obj);
    }
    else
    {
      for (int i = 0; i < selectedObjects.Count; i++)
      {
        GameObject currBlock = selectedObjects[i];
        int health = int.Parse(currBlock.GetComponentInChildren<Canvas>().GetComponentInChildren<TextMeshProUGUI>(true).text);
        double dist = Vector2.Distance(currBlock.transform.position, obj.transform.position);

        attackers.Add(new Block(health, dist, currBlock.name.Split(' ')[2], currBlock.transform.position));
      }
      obj.GetComponentInChildren<Canvas>().GetComponentInChildren<TextMeshProUGUI>(true).text = doCombat(attackers.ToArray(), int.Parse(obj.GetComponentInChildren<Canvas>().GetComponentInChildren<TextMeshProUGUI>(true).text), 0).ToString();
      selectedObjects.Clear();
      attackers.Clear();
    }
  }

  int doCombat(Block[] attackers, int def, int adv)
  {

    adv += attackers.Length-1;

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
          }
          else
          {
            rAttackers += block.str * 1.5f;
            distance += block.dist;
            c++;
          }

          for(int i = 0; i < generals.Length; i++){
            if(Vector2.Distance(block.pos, generals[i].transform.position)<3.5f){
              adv++;
            }
          }
          break;

        case "Art(Clone)":
          rAttackers += block.str * 18f;
          distance += block.dist;
          c++;

          for(int i = 0; i < generals.Length; i++){
            if(Vector2.Distance(block.pos, generals[i].transform.position)<3.5f){
              adv++;
            }
          }
          break;

        case "Cav(Clone)":
          mAttackers += block.str * 4f;

          for(int i = 0; i < generals.Length; i++){
            if(Vector2.Distance(block.pos, generals[i].transform.position)<3.5f){
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

          for(int i = 0; i < generals.Length; i++){
            if(Vector2.Distance(block.pos, generals[i].transform.position)<7f){
              adv++;
            }
          }
          break;

        default:
          break;
      }
    }
    
    distance /= c+0.01f;
    rAttackers *= Mathf.Pow(1.1f, adv);
    mAttackers *= Mathf.Pow(1.1f, adv);
    Debug.Log(adv);
    double rangedCasualties = Mathf.Pow((rAttackers / def), 0.8f) * (0.1 * UnityEngine.Random.Range(1, 7) + 0.6f) * (Math.Exp(-distance / 15f) + 1f / ((4f*distance + 1f))) * 5f;
    double meleeCasualties = Mathf.Pow((mAttackers / def), 0.8f) * (0.1 * UnityEngine.Random.Range(1, 7) + 0.6f) * 10f;
    double survivors = def - def * (rangedCasualties + meleeCasualties) * 0.01f;
    if (survivors < 0)
    {
      survivors = 0;
    }

    return (int)survivors;
  }
}

/**

    
def checkInput(phrase, valids):
  answer = input(phrase)
  while answer not in valids:
    print("Invalid input")
    answer = input(phrase)
  return answer
  
def calcDamage(attackers, defender, attackerType, distance):
  baseCasualties = ((attackers/defender)**0.8)*15*(0.1*randint(1, 6)+0.6)*typeToMult(attackerType)
  totalCasualties = baseCasualties*((e**(-distance/200))+1/(5*(distance+5)))

  defender -= defender*totalCasualties*0.01
  if defender<0:
    defender = 0
  return round(defender)

def doBattle():
  
  side = int(checkInput("Attacking Side: ", ["0","1"]))
  typeStr = checkInput("Attacking Type: ", ["i", "c", "a", "o", "s"])
  type = typeToIndex(typeStr)
  
  attackers = []
  attackers.append(input("Add a battalion: "))
  while attackers[-1] != "q":
    try:
      int(attackers[-1])
    except ValueError:
      attackers.pop()
      print("Invalid entry")
    attackers.append(input("Add a battalion: "))
    if(attackers[-1] in attackers[:-1]):
      attackers.pop()
      print("Already added battalion")
  attackers.pop()
  targetType = typeToIndex(checkInput("Defending Type: ", ["i", "c", "a", "o", "s"]))
  target = int(input("Target: "))-1
  totalAttackers = 0
  for i in attackers:
    totalAttackers += troops[side][type][int(i)-1]
    
  defenders = troops[1-side][targetType][target]
    
  while True:
    try:
      distance = int(input("Distance: "))
    except ValueError:
      print("Invalid input")
    finally:
      break

  advantages = 1.3**int(input("Number of Advantages: "))

  if typeStr == "i":
    mult = 1
    dMult = 1
  if typeStr == "a":
    mult = 3.5
    totalAttackers = 3*(totalAttackers//3)
    dMult = 0.5
  if typeStr == "c" or typeStr == "o":
    mult = 2
    dMult = 15
  if typeStr == "s":
    mult = 1
    dMult = 0.6
      
  endDefenders = calcDamage(totalAttackers*mult*advantages, defenders, typeStr, distance*dMult)
  print(f"{totalAttackers} troops fired at {defenders} troops at a distance of {distance} meters and caused {defenders-endDefenders} deaths, resulting in them having {endDefenders} troops")
  
  troops[1-side][targetType][target] = endDefenders
  print(troops)
  return endDefenders

while True:
  doBattle()
**/