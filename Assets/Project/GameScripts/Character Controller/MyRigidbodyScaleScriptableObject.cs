using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObject/MyRigidbodyScale", fileName = "newMyRigidbodyScale")] 
public class MyRigidbodyScaleScriptableObject : ScriptableObject 
{
    [ SerializeField] MyRigidbodyScale rigidbodyScale = MyRigidbodyScale.Default();
    public MyRigidbodyScale RigidbodyScale { get => rigidbodyScale; } 
}