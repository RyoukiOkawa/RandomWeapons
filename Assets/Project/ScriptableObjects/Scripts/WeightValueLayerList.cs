using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomWeapons.Weapon
{

    [Serializable]
    public class WeightValueLayerList<T>
    {
        [SerializeField] WeightValueLayer<T>[] m_weightLayers = new WeightValueLayer<T>[0];

        public float GetRate(T parameter)
        {
            for (int i = 0; i < WeightLayers.Length; i++)
            {
                if (WeightLayers[i].Parameter.Equals(parameter))
                {
                    return WeightLayers[i].Rate;
                }

            }

            Debug.Log("parameter is not found");
            return 0;
        }

        public WeightValueLayer<T>[] WeightLayers { get => m_weightLayers; set => m_weightLayers = value; }

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

        public static bool operator ==(WeightValueLayerList<T> list, Dictionary<T, float> pairs)
        {
            var layers = list.WeightLayers;

            var layersCount = layers.Length;
            var pairsCount = pairs.Count;

            if (layersCount != pairsCount)
                return false;

            var ts = new T[pairsCount];
            var rates = new float[pairsCount];

            for (int i = 0; i < pairsCount; i++)
            {
                var layer = layers[i];
                if (!layer.Parameter.Equals(ts[i]) || layer.Rate != rates[i])
                {
                    return false;
                }
            }

            return true;
        }

        public static bool operator !=(WeightValueLayerList<T> list, Dictionary<T, float> pairs)
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


        public static implicit operator Dictionary<T, float>(WeightValueLayerList<T> list)
        {
            var layers = list.WeightLayers;
            var lenght = layers.Length;

            var result = new Dictionary<T, float>(lenght);

            for (int i = 0; i < lenght; i++)
            {
                var layer = layers[i];
                result.Add(layer.Parameter, layer.Rate);
            }

            return result;
        }

        public static implicit operator WeightValueLayerList<T>(Dictionary<T, float> pairs)
        {
            var result = new WeightValueLayerList<T>();
            result.WeightLayers = new WeightValueLayer<T>[pairs.Count];


            T[] ts = new T[pairs.Count];
            pairs.Keys.CopyTo(ts, 0);

            for (int i = 0; i < ts.Length; i++)
            {
                var t = ts[i];
                float rate = pairs[t];
                var layer = new WeightValueLayer<T>(t, rate);

                result.WeightLayers[i] = layer;
            }


            return result;
        }

        #endregion
    }

    [Serializable]
    public class WeightValueLayer<T>
    {
        [SerializeField] T m_parameter;
        [SerializeField] float m_rate = 0;

        public WeightValueLayer()
        {

        }

        public WeightValueLayer(T t, float v)
        {
            m_parameter = t;
            Rate = v;
        }

        public T Parameter { get => m_parameter; internal set => m_parameter = value; }
        public float Rate { get => m_rate; internal set => m_rate = value; }
    }

}
