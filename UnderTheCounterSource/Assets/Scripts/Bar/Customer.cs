using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using CocktailCreation;

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
        Gaston,
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
        
        public SerializedDictionary<string, List<string>> lines;
        
        public float tip;

        public override string ToString()
        {
            try
            {
                return $"Sprite: {sprite}, Order: {order}, Greet line: {lines["greet"][0]}, Tip: {tip}";
            }
            catch (Exception e)
            {
                return $"Error :( Exception in Customer.ToString(): {e}";
            }
        }
    }
}