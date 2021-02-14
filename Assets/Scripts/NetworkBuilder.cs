using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkBuilder : MonoBehaviour
{
    public Network Network;

    private void Start()
    {
        GetComponent<NetworkDrawer>().DrawConnections(Network);
    }
}
