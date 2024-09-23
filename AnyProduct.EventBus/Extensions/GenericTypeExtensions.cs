using System.Diagnostics.CodeAnalysis;

namespace eShop.EventBus.Extensions;

public static class GenericTypeExtensions
{
    public static string GetGenericTypeName([NotNull] this Type type)
    {
        string typeName;

        if (type.IsGenericType)
        {
            var genericTypes = string.Join(",", type.GetGenericArguments().Select(t => t.Name).ToArray());
            typeName = $"{type.Name.Remove(type.Name.IndexOf('`', StringComparison.OrdinalIgnoreCase))}<{genericTypes}>";
        }
        else
        {
            typeName = type.Name;
        }

        return typeName;
    }

}
