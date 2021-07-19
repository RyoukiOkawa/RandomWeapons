namespace Myspace.Weapon
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using UnityEngine;


    [CreateAssetMenu(menuName = "ScriptableObject/Weapon", fileName = "newWeapon")]
    public class Weapon : ScriptableObject
    {
        [SerializeField] private string m_name = "";
        [SerializeField] private Texture2D m_image = null;
        [SerializeField] private WeightLayerList<Element> m_elementWeight = new WeightLayerList<Element>();
        [SerializeField] private WeightLayerList<int> m_killWeight = new WeightLayerList<int>();
        [SerializeField] private WeaponType m_type = WeaponType.Sword;

        public string Name { get => m_name; internal set=> m_name = value; }
        public Texture2D Image { get => m_image; internal set => m_image = value; }
        public WeightLayerList<Element> ElementWeight { get => m_elementWeight; internal set => m_elementWeight = value; }
        public WeightLayerList<int> SkillWeight { get => m_killWeight; internal set => m_killWeight = value; }
        public WeaponType Type { get => m_type;internal set => m_type = value; }
    }

    public class WeaponParametor
    {

        WeaponParametor(Weapon weapon)
        {
            Weapon = weapon;
        }

        public Weapon Weapon { get ; private set ; }

        public int Attack { get; private set; }
        public Element Element { get; private set; }
    }


    public enum WeaponType
    {
        Hand,
        Sword
    }

    #region WeightLayerList<T> and WeightLayer class

    [Serializable]
    public class WeightLayerList<T>
    {
        [SerializeField] WeightLayer<T>[] m_weightLayers = new WeightLayer<T>[0];

        public float GetRate(T parameter)
        {
            for(int i = 0;i < WeightLayers.Length; i++)
            {
                if (WeightLayers[i].Parameter.Equals(parameter))
                {
                    return WeightLayers[i].Rate;
                }

            }

            Debug.Log("parameter is not found");
            return 0;
        }

        public WeightLayer<T>[] WeightLayers { get => m_weightLayers; set => m_weightLayers = value; }

        public bool TryGetNameAndWeight(out string[] names, out float[] weights)
        {
            var count = m_weightLayers.Length;

            names = new string[count];
            weights = new float[count];

            if (count != 0)
            {
                names = m_weightLayers
                    .Select(name => name.Parameter.ToString())
                    .ToArray();

                weights = m_weightLayers
                    .Select(rate => rate.Rate)
                    .ToArray();

                return true;
            }
            return false;
        }

        #region Same operators

        public static bool operator ==(WeightLayerList<T> list, Dictionary<T, float> pairs)
        {
            var layers = list.WeightLayers;

            var layersCount = layers.Length;
            var pairsCount = pairs.Count;

            if (layersCount != pairsCount)
                return false;

            var ts = new T[pairsCount];
            var rates = new float[pairsCount];

            for(int i = 0; i < pairsCount; i++)
            {
                var layer = layers[i]; 
                if(!layer.Parameter.Equals(ts[i]) || layer.Rate != rates[i])
                {
                    return false;
                }
            }

            return true;
        }

        public static bool operator !=(WeightLayerList<T> list, Dictionary<T, float> pairs)
        {
            var layers = list.WeightLayers;

            var layersCount = layers.Length;
            var pairsCount = pairs.Count;

            if (layersCount != pairsCount)
                return true;

            var ts = new T[pairsCount];
            var rates = new float[pairsCount];

            for (int i = 0; i < pairsCount; i++)
            {
                var layer = layers[i];
                if (!layer.Parameter.Equals(ts[i]) || layer.Rate != rates[i])
                {
                    return true;
                }
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


        public static implicit operator Dictionary<T,float> (WeightLayerList<T> list)
        {
            var layers = list.WeightLayers;
            var lenght = layers.Length;

            var result = new Dictionary<T, float>(lenght);

            for(int i = 0; i < lenght; i++)
            {
                var layer = layers[i];
                result.Add(layer.Parameter, layer.Rate);
            }

            return result;
        }

        public static implicit operator WeightLayerList<T>(Dictionary<T,float> pairs)
        {
            var result = new WeightLayerList<T>();
            result.WeightLayers = new WeightLayer<T>[pairs.Count];


            T[] ts = new T[pairs.Count];
            pairs.Keys.CopyTo(ts, 0);

            for(int i = 0;i < ts.Length; i++)
            {
                var t = ts[i];
                float rate = pairs[t];
                var layer = new WeightLayer<T>(t, rate);

                result.WeightLayers[i] = layer;
            }


            return result;
        }

        #endregion

    }

    [Serializable]
    public class WeightLayer<T>
    {
        [SerializeField] T m_parameter;
        [SerializeField] float m_rate = 0;

        public WeightLayer()
        {

        }

        public WeightLayer(T t,float v)
        {
            m_parameter = t;
            Rate = v;
        }

        public T Parameter { get => m_parameter;internal set => m_parameter = value; }
        public float Rate { get => m_rate;internal set => m_rate = value; }
    }
}


#endregion


//namespace Myspace
//{
//    [Serializable]
//    public struct MaxAndMin<T> where T : struct, IComparable<T>
//    {
//        [Header("最小値")] [SerializeField] T m_min;
//        [Header("最大値")] [SerializeField] T m_max;

//        #region Editor Only
//#if UNITY_EDITOR
//        private T m_maxToMaX;
//        private T m_minToMin;

//        public MaxAndMin(T MinToMin, T MaxToMax)
//        {
//            if (MinToMin.CompareTo(MaxToMax) > 0)
//            {
//                m_maxToMaX = MinToMin;
//                m_minToMin = MaxToMax;
//            }
//            else
//            {
//                m_maxToMaX = MaxToMax;
//                m_minToMin = MinToMin;
//            }
//            m_min = m_minToMin;
//            m_max = m_maxToMaX;
//        }
//#endif
//        #endregion


//        public T Min { get => m_min; }
//        public T Max { get => m_max; }

//        /// <summary>
//        /// 最小値を0以上、最大値以下
//        /// <para>最大値を0以上最大値の最大値以下</para>
//        /// </summary>
//        public void Format()
//        {
//            var @default = new T();
//            if (@default.CompareTo(m_min) > 0)
//            {
//                m_min = @default;
//            }
//            if (@default.CompareTo(m_max) > 0)
//            {
//                m_max = @default;
//            }
//            if (m_maxToMaX.CompareTo(m_max) < 0)
//            {
//                m_max = m_maxToMaX;
//            }
//            if (m_min.CompareTo(m_max) > 0)
//            {
//                m_min = m_max;
//            }
//        }
//    }
//}

#region Editor Only WeaponEditor

#if UNITY_EDITOR

namespace Myspace.Editor
{



    using UnityEditor;
    using UnityEngine;
    using Myspace.Weapon;

    [CustomEditor(typeof(Weapon))]
    internal class WeaponEditor : MyCustomEditor
    {
        WeightLayerChanger elementChager = new WeightLayerChanger();
        WeightLayerChanger SkillChager = new WeightLayerChanger();

        public override void OnInspectorGUI()
        {
            serializedObject.Update();


            var instance = target as Weapon;

            GUILayout.BeginHorizontal();


            GUILayout.BeginVertical();

            GUILayout.Label("武器の名前");
            instance.Name = EditorGUILayout.TextField(instance.Name);

            instance.Type = (WeaponType)EditorGUILayout.EnumPopup("装備モード",instance.Type);

            
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            
            instance.Image = EditorGUILayout.ObjectField("武器画像", instance.Image, typeof(Texture2D), true, GUILayout.Height(100),GUILayout.ExpandWidth(true)) as Texture2D;
            
            GUILayout.EndVertical();


            GUILayout.EndHorizontal();

            //EditorGUILayout.PropertyField(serializedObject.FindProperty("m_killWeight"));


            //var s = EditorGUILayout.ObjectField("説明文", new Object(), typeof(ScriptableObject), true) as ScriptableObject;



            instance.ElementWeight = WeightLayerGUIObject("属性のウェイト", instance.ElementWeight, elementChager);

            instance.SkillWeight = WeightLayerGUIInt("スキル数", instance.SkillWeight, SkillChager, 0, 5);


            var ele =serializedObject.FindProperty("m_elementWeight");

            EditorGUILayout.PropertyField(ele);



            GUILayout.Space(20);

            //if (GUILayout.Button("Save Asset"))
            //{
            //    serializedObject.ApplyModifiedProperties();
            //    EditorUtility.SetDirty(instance);
            //    AssetDatabase.SaveAssets();
            //}
        }
    }

}


#endif

#endregion