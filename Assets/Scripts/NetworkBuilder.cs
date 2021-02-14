using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkBuilder : MonoBehaviour
{
    public Network Network;

    private void Start()
    {
        NetworkDrawer drawer = GetComponent<NetworkDrawer>();
        drawer.DrawNodes(Network);
        drawer.DrawConnections(Network);
    }
}
