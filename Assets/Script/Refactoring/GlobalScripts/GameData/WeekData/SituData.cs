using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
public class SituData : IIndexer<object>
    {
        public string mon { get; set; }
        public string tue { get; set; }
        public string wed { get; set; }
        public string thu { get; set; }
        public string fri { get; set; }

        private readonly PropertyInfo[] fields;

        public SituData()
        {
            fields = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.PropertyType == typeof(string))
                .ToArray();
        }

        public object this[int index]
        {
            get { return fields[index].GetValue(this); }
            set { fields[index].SetValue(this, value); }
        }

        public int Length
        {
            get { return fields.Length; }
        }
    }
