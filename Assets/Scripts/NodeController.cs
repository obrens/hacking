using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeController : MonoBehaviour
{
    #region Logical state
    public Node StaticModel;
    public enum HackState {UNHACKED, HACKING, HACKED};
    public HackState NodePlayerState;
    public HackState NodeEnemyState;
    #endregion

    public List<ConnectionController> Connections;

    public bool Hackable => 
        NodePlayerState == HackState.UNHACKED 
        && Connections.Exists(
            con => con.OtherNode(this).NodePlayerState == HackState.HACKED
        );
}
