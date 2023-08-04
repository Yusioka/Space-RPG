using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    // если на объекте есть этот скрипт, то Health добавится автоматически
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour
    {

    }
}
