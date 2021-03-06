﻿namespace EasySharp.NHelpers.Common.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        ///     Converts the variable of type <see cref="object" /> (which underlying type is anonymous) to the same anonymous
        ///     type.
        /// </summary>
        /// <example>
        ///     <code>
        ///         static void Enter(object wrapper)
        ///         {
        ///             var anonymousTypeHolder = wrapper.CastToAnonymous(new { x = 0, y = string.Empty });
        ///             Console.WriteLine($"{anonymousTypeHolder.x} : {anonymousTypeHolder.y}");
        ///         }
        ///     </code>
        /// </example>
        /// <typeparam name="TTarget">Inferred anonymous type</typeparam>
        /// <param name="value">Wrapper for an anonymous object</param>
        /// <param name="target">Variable provided for type inference</param>
        /// <returns>Wrapper casted to the inferred anonymous type</returns>
        //public static TTarget CastToAnonymous<TTarget>(this object value, TTarget target = default)
        //{
        //    return (TTarget)value;
        //}

        /// <summary>
        ///     SwapElementsAt two values
        /// </summary>
        /// <typeparam name="T">Type of elements to be swapped</typeparam>
        /// <param name="lhs">Left-hand side value</param>
        /// <param name="rhs">Right-hand side value</param>
        public static void Swap<T>(ref T lhs, ref T rhs)
        {
            T aux = lhs;
            lhs = rhs;
            rhs = aux;
        }
    }
}