using UnityEngine;

[CreateAssetMenu(fileName = "BoolVariable", menuName = "ScriptableObjects/BoolVariable", order = 3)]
public class BoolVariable : Variable<bool> {

    public int previousHighestValue;
    public override void SetValue(bool value) {

        _value = value;
    }

    // overload
    public void SetValue(BoolVariable value) {
        SetValue(value.Value);
    }

    public void Toggle() {
        this.Value = !this.Value;
    }

}