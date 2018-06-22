﻿namespace AdHr.Repository.Models
{
    /// <summary>
    /// Erre az osztályra azért van szükség, hogy az
    /// automapper tudjon működni.
    /// 
    /// A dictionary-ből akkor tudok jól
    /// </summary>
    public class AdhrValue
    {
        public AdhrValue(string value)
        {
            Value = value;
        }

        public string Value { get; set; }
    }
}
