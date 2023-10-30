using UnityEngine;

[CreateAssetMenu(fileName = "IntVariable", menuName = "ScriptableObjects/PositionVariable", order = 4)]
public class PositionVariable : Variable<Vector2> {

    public override void SetValue(Vector2 value) {
        _value = value;
    }

    // overload
    public void SetValue(PositionVariable value) {
        SetValue(value.Value);
    }


}