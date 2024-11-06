using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public void customerGreet(string line)
    {
        // todo
        // button will call customerRequestOrder
    }

    public void customerRequestOrder(string line)
    {
        // todo
        // button will throw event onPreparationStart (caught in customer manager)
    }

    public void customerServe(string line)
    {
        // todo
        // button will close dialogue and throw event onCustomerLeave
    }
}
