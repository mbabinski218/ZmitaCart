namespace ZmitaCart.Application.Common;

public static class MappingExtensions
{
	public static Task<PaginatedList<TDestination>> ToPaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, 
		int? pageNumber, int? pageSize)
		where TDestination : class
		=> PaginatedList<TDestination>.CreateAsync(queryable, pageNumber, pageSize);
	
	public static PaginatedList<TDestination> ToPaginatedList<TDestination>(this IEnumerable<TDestination> enumerable, 
		int? pageNumber, int? pageSize)
		where TDestination : class
		=> PaginatedList<TDestination>.Create(enumerable, pageNumber, pageSize);
}