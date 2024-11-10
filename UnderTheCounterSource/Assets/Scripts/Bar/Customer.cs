using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Bar
{
    [Serializable]
    public enum CustomerType
    {
        Margaret,
        Helene,
        Charles,
        Betty,
        Eugene,
        Doris,
        Kenneth,
        Kathryn,
        Willie,
        Louis,
        Ernest, // ex-bar owner
        Howard, // inspector
        Unspecified
        
    }

    [Serializable]
    public class CustomerList
    {
        public List<Customer> customers;
    }
    
    [Serializable]
    public class Customer
    {
        public CustomerType sprite;
        public CocktailType order;
        
        public Dictionary<string, string> lines;
        
        public float tip;

        public override string ToString()
        {
            try
            {
                return $"Sprite: {sprite}, Order: {order}, Greet line: {lines["greet"]}, Tip: {tip}";
            }
            catch (Exception e)
            {
                return $"Error :( Exception in Customer.ToString(): {e}";
            }
        }
    }
}