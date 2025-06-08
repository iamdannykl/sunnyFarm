using System.Collections.Generic;
using System.Linq;
using Godot;

namespace SunnyFarm.code.Utils;

public static class UsefulTools
{
    public static (Area2D target, float distance)? GetNearest(
        List<Area2D> targets,
        Vector2 from,
        float minDistance = 0f,
        float maxRange = 9999f)
    {
        var nearest = targets
            .Where(t => t != null && IsInstanceValid(t))
            .Select(t => new
            {
                Target = t,
                Distance = t.GlobalPosition.DistanceTo(from)
            })
            .Where(x => IsBetween(x.Distance, minDistance, maxRange))
            .ToList(); // 必须要物化才能安全使用 MinBy

        if (nearest.Count == 0)
            return null;

        var closest = nearest.MinBy(x => x.Distance);
        return (closest.Target, closest.Distance);
    }

    public static bool IsBetween(float value, float min, float max)
    {
        return value >= min && value <= max;
    }

    public static bool IsInstanceValid(Node node)
    {
        return GodotObject.IsInstanceValid(node);
    }
}