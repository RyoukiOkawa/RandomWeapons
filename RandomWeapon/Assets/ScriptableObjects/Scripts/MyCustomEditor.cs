#if UNITY_EDITOR

namespace Myspace.Editor
{
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

                            GUILayout.Label(" : " + values[i]);

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
                    if (GUILayout.Button("値（％）の昇順に並び変える"))
                    {
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
                    GUILayout.EndVertical();
                }


                #endregion


                layer = pairs;
            }

            GUILayout.EndVertical();

            return layer;
        }

        public static WeightLayerList<int> WeightLayerGUIInt(string name, WeightLayerList<int> layer, WeightLayerChanger changer)
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

                            GUILayout.Label(" : " + values[i]);

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
                        for(int i = 0;i < count - 1; i++)
                        {
                            for (int j = i + 1; j < count; j++)
                            {
                                if (keys[i] < keys[j])
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

                        for(int i = 0;i < count; i++)
                        {
                            pairs.Add(keys[i], values[i]);
                        }
                    }
                    if (GUILayout.Button("要素の昇順に並び変える"))
                    {
                        for (int i = 0; i < count - 1; i++)
                        {
                            for (int j = i + 1; j < count; j++)
                            {
                                if (keys[i] > keys[j])
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
                    if (GUILayout.Button("値（％）の降順に並び変える"))
                    {
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
                    if (GUILayout.Button("値（％）の昇順に並び変える"))
                    {
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
                }

                #endregion

                layer = pairs;
            }

            GUILayout.EndVertical();

            return layer;
        }

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