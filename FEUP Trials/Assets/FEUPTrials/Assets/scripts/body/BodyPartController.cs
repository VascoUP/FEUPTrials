using UnityEngine;

public class BodyPartController : MonoBehaviour
{
    private bool _isBroken = false;
    private BikeController _bikeController;

    private void Start()
    {
        GameObject playerManagerGameObject = Utils.FilterTaggedObjectByParent("PlayerManager", transform.parent.parent.name);
        if (playerManagerGameObject == null)
            Debug.LogError("Null player manager game object");

        PlayerManager playerManager = playerManagerGameObject.GetComponent<PlayerManager>();
        if (playerManager == null)
            Debug.LogError("Null player manager");

        GameObject bike = playerManager.activeBike;
        if(bike == null)
        {
            return;
        }

        _bikeController = bike.GetComponent<BikeController>();
    }

    private void OnJointBreak2D(Joint2D joint)
    {
        _isBroken = true;
        _bikeController.StopMotion();
    }

    public bool IsBroken()
    {
        return _isBroken;
    }
}
