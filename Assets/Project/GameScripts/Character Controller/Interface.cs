using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomWeapons.Character
{
    public interface ICharacter
    {
        public Transform Transform { get; }
        public GameObject GameObject { get; }
    }
    public interface IMoveCharacter : ICharacter
    {
        public Rigidbody Rigidbody { get; }
        public void MoveCharcter();

        // public  GetCurrentArea();
    }

    public interface IAIMoveCharacter : IMoveCharacter
    {
        AnimationCurve XRootCurve { get; set; }
        AnimationCurve ZRootCurve { get; set; }
    }

    public interface IFightCharacter : ICharacter
    {
        public void Reborn(Vector3 position);
        public void Damage(int AttackPoint);
        public bool DeadChack();
        public void Dead();
    }
}