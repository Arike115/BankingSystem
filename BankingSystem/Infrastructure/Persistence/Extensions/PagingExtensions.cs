namespace BankingSystem.Infrastructure.Persistence.Extensions
{
    public static class PagingExtensions
    {
        //used by LINQ to SQL
        public static IQueryable<TSource> Page<TSource>(this IQueryable<TSource> source, int page, int pageSize)
        {
            return source.Skip((page - 1) * pageSize).Take(pageSize);
        }

        //used by LINQ
        public static IEnumerable<TSource> Page<TSource>(this IEnumerable<TSource> source, int page, int pageSize)
        {
            return source.Skip((page - 1) * pageSize).Take(pageSize);
        }
        public static pageResponse PagedResult<T>(IEnumerable<T> results, int pagesize) where T : class
        {
            if (pagesize != 0)
            {
                // Perform the division operation
                var pages = (results.Count() / pagesize);
                if (results.Count() % pagesize != 0)
                    pages = pages + 1;
                pageResponse result = new()
                {
                    TotalCount = results.Count(),
                    NumbersOfPages = pages == 0 ? 1 : pages,
                };
                return result;
            }
            else
            {
                // Handle the division by zero scenario
                // You can throw an exception, return a default value, or take other appropriate action
                // For example:
                throw new DivideByZeroException("pagesize cannot be zero.");
            }

        }
        public class pageResponse
        {
            public Guid Id { get; set; }
            public int NumbersOfPages { get; set; }
            public int TotalCount { get; set; }
        }
    }
}

