using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace ZmitaCart.Application.Common;

public static class Extensions
{
	public static Task<PaginatedList<TDestination>> ToPaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable,
		int? pageNumber, int? pageSize)
		where TDestination : class
		=> PaginatedList<TDestination>.CreateAsync(queryable, pageNumber, pageSize);

	public static PaginatedList<TDestination> ToPaginatedList<TDestination>(this IEnumerable<TDestination> enumerable,
		int? pageNumber, int? pageSize)
		where TDestination : class
		=> PaginatedList<TDestination>.Create(enumerable, pageNumber, pageSize);
	
	public static IQueryable<TSource> OrderByIf<TSource, TKey>(this IQueryable<TSource> queryable, 
		Expression<Func<TSource, TKey>> keySelector, bool? condition)
		=> condition is true ? queryable.OrderBy(keySelector) : queryable;

	public static async Task<List<NamedList<TKey, TElement>>> ToUniqueListAsync<TSource, TKey, TElement>(
		this IQueryable<TSource> queryable,
		Func<TSource, TKey> keySelector,
		Func<TSource, TElement> elementSelector)
		where TKey : IEquatable<TKey>
	{
		var list = new List<NamedList<TKey, TElement>>();
		
		await foreach (var element in queryable.AsAsyncEnumerable())
		{
			if (list.Any(x => x.Name.Equals(keySelector(element))))
			{
				list.First(x => x.Name.Equals(keySelector(element))).Data.Add(elementSelector(element));
			}
			else
			{
				list.Add(new NamedList<TKey, TElement>
				{
					Name = keySelector(element),
					Data = new List<TElement> { elementSelector(element) }
				});
			}
		}

		return list;
	}
	
	public static async Task<Dictionary<TKey, List<TElement>>> ToDictionaryWithList<TSource, TKey, TElement>(
		this IQueryable<TSource> queryable,
		Func<TSource, TKey> keySelector,
		Func<TSource, TElement> elementSelector)
		where TKey : notnull
	{
		var dictionary = new Dictionary<TKey, List<TElement>>();
		
		await foreach (var element in queryable.AsAsyncEnumerable())
		{
			if (!dictionary.TryAdd(keySelector(element), new List<TElement>{ elementSelector(element) }))
			{
				dictionary[keySelector(element)].Add(elementSelector(element));
			}
		}

		return dictionary;
	}
}
