using System.Collections.Generic;
using UnityEngine;

public class AgentControlZone : MonoBehaviour
{
    [SerializeField] private List<NavGridAgent> _agents = new List<NavGridAgent>();
    
    [SerializeField] private float _maxDistanceFromControlZone =15;
    [SerializeField] private int _fallBackZoneSize =6;
    
    private NavGridManager _gridManager;
    void Start()
    { 
        _gridManager = NavGridManager.Instance;
        if (_gridManager == null) {
            Debug.LogWarning("No Grid Manager Found", this);
        }

        foreach (var agent in _agents) {
            AddAgent(agent);
        }
        
    }

   
    void Update() {
        ManageDistanceToAgent();
    }

    private void AddAgent(NavGridAgent agent) {
        agent.SetAgentControlZone(this);
        if (!_agents.Contains(agent))_agents.Add(agent);
    }

    private void ManageDistanceToAgent() {
        foreach (var agent in _agents) {
            if (agent == null) continue;
            if (agent.IsFallBack) continue;
            if (Vector2.Distance(agent.transform.position, transform.position) >= _maxDistanceFromControlZone)
            {
                Debug.Log("Give Order To Fallback");
                agent.OrderToFallBack(GetReturnPos());
            }
        }
    }

    public Vector2 GetReturnPos()
    {
        NavGridCell[] cells =_gridManager.GetBFS(_gridManager.GetCell(transform.position), _fallBackZoneSize);
        return _gridManager.GetCenterWorldPos(cells[Random.Range(0, cells.Length)]);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _maxDistanceFromControlZone);
        Gizmos.DrawWireSphere(transform.position, _fallBackZoneSize);

        foreach (var agent in _agents) {
            if (agent == null) continue;
            Gizmos.DrawLine(transform.position, agent.transform.position);
            Gizmos.DrawWireCube(agent.transform.position, Vector3.one);
        }
    }
}
