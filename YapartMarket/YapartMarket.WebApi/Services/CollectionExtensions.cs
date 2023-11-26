using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace YapartMarket.WebApi.Services
{
    public static class CollectionExtensions
    { /// <summary>
      /// Indicates whether the specified collection is null or an empty collection.
      /// </summary>
      /// <typeparam name="T">The element type.</typeparam>
      /// <param name="collection">The collection to test.</param>
      /// <returns>true if the collection parameter is null or an empty collection; otherwise, false.</returns>
        public static bool IsNullOrEmpty<T>([NotNullWhen(false)] this IReadOnlyCollection<T>? collection) => collection == null || collection.Count == 0;

        /// <summary>
        /// Indicates whether the specified collection is null or an empty collection.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="collection">The collection to test.</param>
        /// <returns>true if the collection parameter is null or an empty collection; otherwise, false.</returns>
        public static bool IsNullOrEmpty<T>([NotNullWhen(false)] this IEnumerable<T>? collection) => collection == null || !collection.Any();

        /// <summary>
        /// Returns the empty list if the <paramref name="source"/> is null; otherwise, <paramref name="source"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns><paramref name="source"/> if is not null; otherwise, empty list.</returns>
        [return: NotNull]
        public static IReadOnlyList<T> EmptyIfNull<T>(this IReadOnlyList<T>? source) => source ?? Array.Empty<T>();

        /// <summary>
        /// Returns the empty collection if the <paramref name="source"/> is null; otherwise, <paramref name="source"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns><paramref name="source"/> if is not null; otherwise, empty collection.</returns>
        [return: NotNull]
        public static IReadOnlyCollection<T> EmptyIfNull<T>(this IReadOnlyCollection<T>? source) => source ?? Array.Empty<T>();

        /// <summary>
        /// Returns the empty enumeration if the <paramref name="source"/> is null; otherwise, <paramref name="source"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns><paramref name="source"/> if is not null; otherwise, empty enumeration.</returns>
        [return: NotNull]
        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T>? source) => source ?? Enumerable.Empty<T>();

    }
}
