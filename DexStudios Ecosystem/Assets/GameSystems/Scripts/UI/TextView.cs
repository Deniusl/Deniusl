using UnityEngine;
using UnityEngine.UI;

namespace GameSystems.Scripts.UI
{
    public class TextView : MonoBehaviour
    {
        [SerializeField] private Text _text;

        public string Text
        {
            set => _text.text = value;
        }
    }
}
