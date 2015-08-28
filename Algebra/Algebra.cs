using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;

namespace AlgebraDotNet
{
    /// <summary>
    /// Defines methods for constructing algebraic functions and algebraic equalities.
    /// </summary>
    public static partial class Algebra
    {
        /// <summary>
        /// The associative identity.
        /// </summary>
        public static readonly Identity Associative = Identity((x, y, z) => x + (y + z) == (x + y) + z);

        /// <summary>
        /// The commutative identity.
        /// </summary>
        public static readonly Identity Commutative = Identity((x, y) => x + y == y + x);

        /// <summary>
        /// Exponentiation.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Term Pow(this double x, double exponent)
        {
            return Term.Constant(x).Pow(exponent);
        }
    }

    /// <summary>
    /// A function describing a numerical expression.
    /// </summary>
    /// <typeparam name="T">The type of the function.</typeparam>
    public struct Function<T> : IEquatable<Function<T>>
        where T : class
    {
        Term body;
        string[] parameters;

        public Function(Term body, ParameterInfo[] param)
            : this(body, param.Select(x => x.Name).ToArray())
        {
        }

        public Function(Term body, params string[] param)
        {
            this.body = body;
            this.parameters = param;
        }

        /// <summary>
        /// Compile the numerical function into a delegate.
        /// </summary>
        /// <param name="name">The internal delegate name.</param>
        /// <returns>A compiled delegate for the expression.</returns>
        public T Compile(string name)
        {
            // assumes that # generic arguments = # parameters + 1, ie. Func<arg0, arg1, returnType>
            var signature = typeof(T).GetGenericArguments().Take((int)HighestBit(body.VMask)).ToArray();
            var method = new DynamicMethod(name, typeof(double), signature);
            for (int i = 0; i < signature.Length; ++i)
                method.DefineParameter(i, ParameterAttributes.In, "arg" + i);
            var il = method.GetILGenerator();
            body.Compile(il);
            il.Emit(OpCodes.Ret);
            return method.CreateDelegate(typeof(T)) as T;
        }

        /// <summary>
        /// Test two functions for reference equality.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Function<T> other)
        {
            return body.Equals(other.body);
        }

        /// <summary>
        /// Rewrite a function.
        /// </summary>
        /// <param name="equalities">The equalities defining the substitutions to perform.</param>
        /// <returns>A new function with the given equalities applied.</returns>
        public Function<T> Rewrite(params Identity[] equalities)
        {
            return Rewrite(int.MaxValue, equalities);
        }

        /// <summary>
        /// Rewrite a function for a maximum number of iterations.
        /// </summary>
        /// <param name="equalities">The equalities defining the substitutions to perform.</param>
        /// <param name="rounds">The maximum number of iterations to perform.</param>
        /// <returns>A new function with the given equalities applied.</returns>
        public Function<T> Rewrite(int rounds, params Identity[] equalities)
        {
            var bindings = new Term[Math.Max(HighestBit(body.VMask), equalities.Max(x => (int?)HighestBit(x.left.VMask)) ?? 0)];
            return new Function<T>(Term.Rewrite(body, rounds, equalities, bindings), parameters);
            //return Term.Reduce(Body, rounds, equalities, bindings);
        }

        /// <summary>
        /// Compare two functions for equality.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(Function<T> left, Function<T> right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compare two functions for inequality.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Function<T> left, Function<T> right)
        {
            return !(left == right);
        }

        internal Function<T> With(ParameterInfo[] parameters)
        {
            return new Function<T>(body, parameters);
        }

        /// <summary>
        /// Generate a string representation of the function.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            //return "f[" + Body.nextVar + "] = " + Body.ToString();
            return body.Print(new StringBuilder(), parameters).ToString();
        }

        /// <summary>
        /// Implicit convert a term to the expected function type.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public static implicit operator Function<T>(Term body)
        {
            return new Function<T>(body);
        }

        #region Bit-twiddling functions
        /// <summary>
        /// Fold the high order bits into the low order bits.
        /// </summary>
        /// <param name="value">The value to operate on.</param>
        /// <returns>The folded value.</returns>
        static uint Fold(uint value)
        {
            value |= (value >> 1);
            value |= (value >> 2);
            value |= (value >> 4);
            value |= (value >> 8);
            value |= (value >> 16);
            return value;
        }

        /// <summary>
        /// Extract the highest bit.
        /// </summary>
        /// <param name="value">The bit pattern to use.</param>
        /// <returns>The bit pattern with only the highest bit set.</returns>
        /// <remarks>
        /// Implementation taken from:
        /// http://aggregate.org/MAGIC/#Most%20Significant%201%20Bit
        /// </remarks>
        static int HighestBit(int value)
        {
            var x = Fold(unchecked((uint)value));
            return unchecked((int)(x ^ (x >> 1)));
        }
        #endregion
    }

    /// <summary>
    /// Defines an equality between two terms.
    /// </summary>
    public struct Identity
    {
        internal Term left;
        internal Term right;
        public Identity(Term left, Term right)
        {
            if ((left.VMask | right.VMask) != left.VMask)
                throw new ArgumentException("The right hand side contains variables not appearing in the left hand side.");
            this.left = left;
            this.right = right;
        }

        /// <summary>
        /// Generate a string representation of this equality.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return left.ToString() + " == " + right.ToString();
        }
    }

    /// <summary>
    /// The term node type.
    /// </summary>
    public enum TermType
    {
        Const = 0, Var, Add, Sub, Mul, Div, Pow,
    }
}
