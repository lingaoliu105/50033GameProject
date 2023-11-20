using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Assets.Scripts.UI {
    public enum TubeColor {
        Red,
        Blue,
        Green,
        Yellow,
        Empty
    }
    public class Tube : MonoBehaviour {
        public Image TubeImage;
        Color red = new Color(1f, 0.2f, 0.2f);
        Color blue = new Color(0.2f, 0.2f, 1f);
        Color green = new Color(0.2f, 1f, 0.2f);
        Color yellow = new Color(1f, 1f, 0.2f);
        Color empty = new Color(0.2f, 0.2f, 0.2f, 0f);
        public void SetColor(TubeColor tubeColor) {
            if (tubeColor == TubeColor.Red) {
                TubeImage.color = red;
            } else if (tubeColor == TubeColor.Blue) {
                TubeImage.color = blue;
            } else if (tubeColor == TubeColor.Green) {
                TubeImage.color = green;
            } else if (tubeColor == TubeColor.Yellow) {
                TubeImage.color = yellow;
            } else if (tubeColor == TubeColor.Empty) {
                TubeImage.color = empty;
            }
        }
    }
}

