using System.Linq.Expressions;
using System.Reflection;

namespace IF.ContactManagement.Infrastructure.Persistence.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> OrderByProperty<T>(this IQueryable<T> source, string propertyName, bool ascending = true)
        {
            // Obtener el tipo de la entidad
            var entityType = typeof(T);

            // Crear el parámetro de la expresión lambda (p => ...)
            var parameter = Expression.Parameter(entityType, "p");

            // Crear el cuerpo de la expresión lambda (p.PropertyName)
            var property = entityType.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (property == null)
            {
                throw new ArgumentException($"Cannot find '{propertyName}' on type '{entityType.Name}'.", nameof(propertyName));
            }
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);

            // Crear la expresión lambda
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);

            // Llamar al método OrderBy/OrderByDescending apropiado
            var methodName = ascending ? "OrderBy" : "OrderByDescending";
            var method = typeof(Queryable).GetMethods()
                                           .Where(m => m.Name == methodName && m.GetParameters().Length == 2)
                                           .Single()
                                           .MakeGenericMethod(entityType, property.PropertyType);

            // Aplicar la ordenación a la consulta
            var resultExpression = Expression.Call(method, source.Expression, Expression.Quote(orderByExpression));

            // Retornar la consulta ordenada
            return source.Provider.CreateQuery<T>(resultExpression);
        }
    }
}
