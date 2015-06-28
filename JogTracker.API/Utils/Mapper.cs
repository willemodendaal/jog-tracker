using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JogTracker.Api.Utils
{
/// <summary>
    /// Interface to be implemented by classes supporting simple mapping.
    /// </summary>
    public interface IMappable<TSource, TTarget>
    {
        TTarget Map(TSource source);
    }


    /// <summary>
    /// Class that can map domain models to json models.
    /// </summary>
    internal class Mapper<TSource, TTarget> where TTarget: IMappable<TSource, TTarget>, new()
    {
        public ICollection<TTarget> Map(ICollection<TSource> sourceCollection)
        {
            List<TTarget> result = new List<TTarget>();

            foreach (var item in sourceCollection)
            {
                result.Add(new TTarget().Map(item));
            }

            return result;
        }
    }

   
}