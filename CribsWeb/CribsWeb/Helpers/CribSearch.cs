using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Cribs.Web.Models;
using Cribs.Web.ViewModels;

namespace Cribs.Web.Helpers
{
    public class CribSearch
    {
        private static readonly IdentityDb Db = new IdentityDb();

        public static IQueryable<RentCrib> Search(SearchModel queryParams)
        {
            Stack<Expression<Func<RentCrib, bool>>> filters = BuildFilters(queryParams);

            IQueryable<RentCrib> list = Db.RentCribs.Where(x => x.Active).OrderBy(x => x.DatePost);
            var filtered = ApplyFilters(list, filters);
            return filtered;
        }

        private static IQueryable<RentCrib> ApplyFilters(IQueryable<RentCrib> list, Stack<Expression<Func<RentCrib, bool>>> filters)
        {
            var filtered = list;
            if (filters.Count > 0)
            {
                filtered = list.Where(filters.Pop());
            }

            while (filters.Count != 0)
            {
                filtered = filtered.Union(list.Where(filters.Pop()));
            }
            return filtered;
        }

        public static Stack<Expression<Func<RentCrib, bool>>> BuildFilters(SearchModel queryParams)
        {
            Stack<Expression<Func<RentCrib, bool>>> filters = new Stack<Expression<Func<RentCrib, bool>>>();
            Expression<Func<RentCrib, bool>> inPriceRange;


            if (queryParams.MinPrice != null && queryParams.MaxPrice == null)
            {
                inPriceRange = c => c.MonthlyPrice >= queryParams.MinPrice;
                filters.Push(inPriceRange);
            }
            else if (queryParams.MinPrice == null && queryParams.MaxPrice != null)
            {
                inPriceRange = c => c.MonthlyPrice <= queryParams.MaxPrice;
                filters.Push(inPriceRange);
            }
            else if (queryParams.MinPrice != null && queryParams.MaxPrice != null)
            {
                inPriceRange = c => c.MonthlyPrice >= queryParams.MinPrice && c.MonthlyPrice <= queryParams.MaxPrice;
                filters.Push(inPriceRange);
            }


            if (queryParams.NumberOfRooms != null)
            {
                Expression<Func<RentCrib, bool>> numberOfRoomsFilter = c => c.NumberOfRooms == queryParams.NumberOfRooms;
                filters.Push(numberOfRoomsFilter);
            }

            if (queryParams.Address != null)
            {
                Expression<Func<RentCrib, bool>> addressFilter = c => c.Location.ToUpper().Contains(queryParams.Address.ToUpper());
                filters.Push(addressFilter);
            }

            if (queryParams.SearchPhrase != null)
            {
                var phrase = queryParams.SearchPhrase;
                var tokens = phrase.Split(' ');
                foreach (var token in tokens)
                {
                    var token1 = token;
                    Expression<Func<RentCrib, bool>> phraseFilter = c =>
                        (c.Title.ToUpper().Contains(token1.ToUpper())) ||
                        (c.Description.ToUpper().Contains(token1.ToUpper()));
                    filters.Push(phraseFilter);

                }


            }

            return filters;
        }
    }
}
