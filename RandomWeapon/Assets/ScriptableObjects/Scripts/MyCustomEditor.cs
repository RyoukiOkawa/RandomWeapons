#if UNITY_EDITOR

namespace Myspace.Editor
{
    using System;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;
    using Myspace.Weapon;

    public abstract class MyCustomEditor : Editor
    {
        public static WeightLayerList<T> WeightLayerGUIObject<T>(string name, WeightLayerList<T> layer, WeightLayerChanger changer) where T : UnityEngine.Object
        {
            GUILayout.BeginVertical();
            changer.Open = EditorGUILayout.Foldout(changer.Open, name);

            if (changer.Open)
            {

                Dictionary<T, float> pairs = layer;
                var count = pairs.Count;

                T[] keys = new T[count];
                float[] values = new float[count];
                pairs.Keys.CopyTo(keys, 0);
                pairs.Values.CopyTo(values, 0);

                if (count == 0)
                {
                    GUILayout.Label("登録されているものがありません追加してください");
                }
                else
                {
                    #region 登録済みレイヤーの開示と要素の変更

                    if (count == 1)
                    {
                        pairs[keys[0]] = 100;
                    }

                    GUILayout.Label("レイヤー　：　要素　：　値（％）　：　削除ボタン");

                    for (int i = 0; i < count; i++)
                    {
                        if (keys[i] is T key)
                        {
                            GUILayout.BeginHorizontal();

                            var obj = EditorGUILayout.ObjectField("登録済レイヤー" + i, key, typeof(T), true) as T;

                            if (key != obj)
                            {
                                if (pairs.ContainsKey(obj))
                                {
                                    Debug.Log("この要素は既に登録されています");
                                }
                                else
                                {
                                    pairs.Remove(keys[i]);
                                    pairs.Add(obj, values[i]);
                                }
                            }
                            else
                            {
                                GUILayout.Label(" : ");

                                pairs[obj] = EditorGUILayout.Slider(values[i], 0, 100);

                                GUILayout.Label("（％）");
                            }

                            // 要素の削除
                            if (GUILayout.Button("-"))
                            {
                                pairs.Remove(keys[i]);
                            }

                            GUILayout.EndHorizontal();
                        }
                    }

                    #endregion
                }


                #region 追加レイヤーの操作

                for (int i = 0; i < changer.AddCnt; i++)
                {

                    var obj = EditorGUILayout.ObjectField("追加レイヤー" + i, null, typeof(T), true);

                    if (obj != null && obj is T t)
                    {
                        if (!pairs.ContainsKey(t))
                        {
                            pairs.Add(t, 0);
                            changer.AddCnt--;
                        }
                        else
                        {
                            Debug.Log("この要素は既に登録されています");
                        }
                    }


                }

                GUILayout.BeginHorizontal();

                if (GUILayout.Button("+"))
                {
                    changer.AddCnt++;
                }

                if (GUILayout.Button("-") && changer.AddCnt > 0)
                {
                    changer.AddCnt--;
                }

                GUILayout.EndHorizontal();
                #endregion

                #region 内容の並び替え

                if (count > 1)
                {
                    GUILayout.BeginVertical();
                    if (GUILayout.Button("値（％）の降順に並び変える"))
                    {
                        OrderValueDescending(ref pairs);
                    }
                    if (GUILayout.Button("値（％）の昇順に並び変える"))
                    {
                        OrderValueAscending(ref pairs);
                    }

                    if (GUILayout.Button("値（％）の合計が100になるように調整する\n（小数点第二位以下は切り捨て）"))
                    {
                        Adjustment(ref pairs);
                    }

                    GUILayout.EndVertical();
                }


                #endregion


                layer = pairs;
            }

            GUILayout.EndVertical();

            return layer;
        }


        public static WeightLayerList<int> WeightLayerGUIInt
            (string name, WeightLayerList<int> layer, WeightLayerChanger changer,int? minSize = null, int? maxSize = null)
        {
            GUILayout.BeginVertical();

            changer.Open = EditorGUILayout.Foldout(changer.Open, name);

            if (changer.Open)
            {

                Dictionary<int, float> pairs = layer;
                var count = pairs.Count;

                int[] keys = new int[count];
                float[] values = new float[count];
                pairs.Keys.CopyTo(keys, 0);
                pairs.Values.CopyTo(values, 0);

                if (count == 0)
                {
                    GUILayout.Label("登録されているものがありません追加してください");
                }
                else
                {
                    #region 登録済みレイヤーの開示と要素の変更

                    if (count == 1)
                    {
                        pairs[keys[0]] = 100;
                    }

                    GUILayout.Label("レイヤー　：　要素　：　値（％）　：　削除ボタン");

                    for (int i = 0; i < count; i++)
                    {
                        if (keys[i] is int key)
                        {
                            GUILayout.BeginHorizontal();

                            var obj = EditorGUILayout.IntField("登録済レイヤー" + i, key);

                            if(maxSize != null && maxSize < obj)
                            {
                                obj = (int)maxSize;
                            }
                            if(minSize != null && minSize > obj)
                            {
                                obj = (int)minSize;
                            }

                            if (key != obj)
                            {
                                if (pairs.ContainsKey(obj))
                                {
                                    Debug.Log("この要素は既に登録されています");
                                }
                                else
                                {
                                    pairs.Remove(keys[i]);
                                    pairs.Add(obj, values[i]);

                                }
                            }
                            else
                            {
                                GUILayout.Label(" : ");

                                pairs[obj] = EditorGUILayout.Slider(values[i], 0, 100);

                                GUILayout.Label("（％）");
                            }
                            

                            // 要素の削除
                            if (GUILayout.Button("-"))
                            {
                                pairs.Remove(keys[i]);
                            }

                            GUILayout.EndHorizontal();
                        }
                    }

                    #endregion
                }


                #region 追加レイヤーの操作

                for (int i = 0; i < changer.AddCnt; i++)
                {

                    var obj = EditorGUILayout.IntField("追加レイヤー" + i, 0);

                    if (maxSize != null && maxSize < obj)
                    {
                        obj = (int)maxSize;
                    }
                    if (minSize != null && minSize > obj)
                    {
                        obj = (int)minSize;
                    }

                    if (!pairs.ContainsKey(obj))
                    {
                        pairs.Add(obj, 0);
                        changer.AddCnt--;
                    }
                    else
                    {
                        Debug.Log("この要素は既に登録されています");
                    }
                }

                GUILayout.BeginHorizontal();

                if (GUILayout.Button("+"))
                {
                    changer.AddCnt++;
                }

                if (GUILayout.Button("-") && changer.AddCnt > 0)
                {
                    changer.AddCnt--;
                }

                GUILayout.EndHorizontal();
                #endregion



                #region 内容の並び替え

                if (count > 1)
                {
                    if (GUILayout.Button("要素の降順に並び変える"))
                    {
                        OrderKeyDescending(ref pairs);
                    }
                    if (GUILayout.Button("要素の昇順に並び変える"))
                    {
                        OrderKeyAscending(ref pairs);
                    }
                    if (GUILayout.Button("値（％）の降順に並び変える"))
                    {
                        OrderValueDescending(ref pairs);
                    }
                    if (GUILayout.Button("値（％）の昇順に並び変える"))
                    {
                        OrderValueAscending(ref pairs);
                    }
                    if (GUILayout.Button("値（％）の合計が100になるように調整する\n（小数点第二位以下は切り捨て）"))
                    {
                        Adjustment(ref pairs);
                    }
                }

                #endregion

                layer = pairs;
            }

            GUILayout.EndVertical();

            return layer;
        }

        #region Oder Method and Adjustment Method

        public static void Adjustment<T>(ref Dictionary<T, float> pairs,float ratio = 0.01f)
        {
            var count = pairs.Count;

            T[] keys = new T[count];
            pairs.Keys.CopyTo(keys, 0);

            float sum = (pairs.Values.Sum() / 100);

            float sumResidue = 0;

            for (int i = 0; i < keys.Length; i++)
            {
                var key = keys[i];

                float result = pairs[key] / sum;

                float residue = result % ratio;
                sumResidue += residue;

                result -= residue;

                if (sumResidue >= ratio)
                {
                    sumResidue -= ratio;
                    result += ratio;
                }

                pairs[keys[i]] = result;
            }


            var resultSum = pairs.Values.Sum();

            if (resultSum != 100)
            {
                if (sumResidue > 0)
                {
                    pairs[keys[count - 1]] += ratio;
                }
                else
                {
                    pairs[keys[count - 1]] -= ratio;
                }
            }

        }

        public static void OrderValueAscending<T>(ref Dictionary<T, float> pairs)
        {
            var count = pairs.Count;

            T[] keys = new T[count];
            float[] values = new float[count];
            pairs.Keys.CopyTo(keys, 0);
            pairs.Values.CopyTo(values, 0);

            for (int i = 0; i < count - 1; i++)
            {
                for (int j = i + 1; j < count; j++)
                {
                    if (values[i] > values[j])
                    {
                        var keyTemp = keys[j];
                        var valueTemp = values[j];
                        keys[j] = keys[i];
                        values[j] = values[i];
                        keys[i] = keyTemp;
                        values[i] = valueTemp;
                    }
                }
            }

            pairs.Clear();

            for (int i = 0; i < count; i++)
            {
                pairs.Add(keys[i], values[i]);
            }
        }

        public static void OrderValueDescending<T>(ref Dictionary<T, float> pairs)
        {
            var count = pairs.Count;

            T[] keys = new T[count];
            float[] values = new float[count];
            pairs.Keys.CopyTo(keys, 0);
            pairs.Values.CopyTo(values, 0);

            for (int i = 0; i < count - 1; i++)
            {
                for (int j = i + 1; j < count; j++)
                {
                    if (values[i] < values[j])
                    {
                        var keyTemp = keys[j];
                        var valueTemp = values[j];
                        keys[j] = keys[i];
                        values[j] = values[i];
                        keys[i] = keyTemp;
                        values[i] = valueTemp;
                    }
                }
            }

            pairs.Clear();

            for (int i = 0; i < count; i++)
            {
                pairs.Add(keys[i], values[i]);
            }
        }

        public static void OrderKeyAscending<T>(ref Dictionary<T, float> pairs) where T : IComparable<T>
        {
            var count = pairs.Count;

            T[] keys = new T[count];
            float[] values = new float[count];
            pairs.Keys.CopyTo(keys, 0);
            pairs.Values.CopyTo(values, 0);

            for (int i = 0; i < count - 1; i++)
            {
                for (int j = i + 1; j < count; j++)
                {
                    if (keys[i].CompareTo(keys[j]) > 0)
                    {
                        var keyTemp = keys[j];
                        var valueTemp = values[j];
                        keys[j] = keys[i];
                        values[j] = values[i];
                        keys[i] = keyTemp;
                        values[i] = valueTemp;
                    }
                }
            }

            pairs.Clear();

            for (int i = 0; i < count; i++)
            {
                pairs.Add(keys[i], values[i]);
            }
        }

        
        public static void OrderKeyDescending<T>(ref Dictionary<T, float> pairs) where T : IComparable<T>
        {
            var count = pairs.Count;

            T[] keys = new T[count];
            float[] values = new float[count];
            pairs.Keys.CopyTo(keys, 0);
            pairs.Values.CopyTo(values, 0);

            for (int i = 0; i < count - 1; i++)
            {
                for (int j = i + 1; j < count; j++)
                {
                    if (keys[i].CompareTo(keys[j]) < 0)
                    {
                        var keyTemp = keys[j];
                        var valueTemp = values[j];
                        keys[j] = keys[i];
                        values[j] = values[i];
                        keys[i] = keyTemp;
                        values[i] = valueTemp;
                    }
                }
            }

            pairs.Clear();

            for (int i = 0; i < count; i++)
            {
                pairs.Add(keys[i], values[i]);
            }
        }

        #endregion

        public class WeightLayerChanger
        {
            public int AddCnt = 0;
            public bool Active { get; set; } = false;
            public bool Open { get; set; } = true;
            public AnimationCurve Curve { get; set; } = AnimationCurve.Linear(0, 1, 1, 0);
        }
    }
}
#endif