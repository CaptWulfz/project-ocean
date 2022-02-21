using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parameters
{
    Dictionary<string, object> param;

    public Parameters()
    {
        param = new Dictionary<string, object>();
    }

    /// <summary>
    /// Adds / Replaces a parameter of the desired type with a given key
    /// </summary>
    /// <typeparam name="T">Type of the object parameter</typeparam>
    /// <param name="key">Key of the object parameter</param>
    /// <param name="value">The Object to be set with a key</param>
    public void AddParameter<T>(string key, T value)
    {
        if (this.param.ContainsKey(key))
            this.param[key] = value;
        else
            this.param.Add(key, value);
    }

    /// <summary>
    /// Gets a parameter of the desired type with a given key
    /// </summary>
    /// <typeparam name="T">The type of the object parameter</typeparam>
    /// <param name="key">Key of the object parameter</param>
    /// <param name="defaultValue">Default value to be returned when desired object does not exist</param>
    /// <returns></returns>
    public T GetParameter<T>(string key, T defaultValue)
    {
        if (this.param.ContainsKey(key))
            return (T)this.param[key];
        else
            return defaultValue;
    }
}
