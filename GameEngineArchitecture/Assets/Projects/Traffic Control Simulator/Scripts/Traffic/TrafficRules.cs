public struct TrafficRules
{
    public float SpeedLimit;
    public float MaxAcceleration;
    public float MinimumObstacleDistance;
    public float SecondsBehindObstacle;

    public TrafficRules(float speedLimit, float maxAcceleration, float minimumObstacleDistance, float secondsBehindObstacle)
    {
        SpeedLimit = speedLimit;
        MaxAcceleration = maxAcceleration;
        MinimumObstacleDistance = minimumObstacleDistance;
        SecondsBehindObstacle = secondsBehindObstacle;
    }
}
