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
        internal Term Body { get; set; }

        /// <summary>
        /// Compile the numerical function into a delegate.
        /// </summary>
        /// <param name="name">The internal delegate name.</param>
        /// <returns>A compiled delegate for the expression.</returns>
        public T Compile(string name)
        {
            // assumes that # generic arguments = # parameters + 1, ie. Func<arg0, arg1, returnType>
            var signature = typeof(T).GetGenericArguments().Take((int)HighestBit(Body.varMask)).ToArray();
            var method = new DynamicMethod(name, typeof(double), signature);
            for (int i = 0; i < signature.Length; ++i)
                method.DefineParameter(i, ParameterAttributes.In, "arg" + i);
            var il = method.GetILGenerator();
            Body.Compile(il);
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
            return ReferenceEquals(Body, other.Body);
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
            var bindings = new Term[Math.Max(HighestBit(Body.varMask), equalities.Max(x => (int?)HighestBit(x.left.varMask)) ?? 0)];
            return Term.Rewrite(Body, rounds, equalities, bindings);
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
        /// <summary>
        /// Implicit convert a term to the expected function type.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public static implicit operator Function<T>(Term body)
        {
            return new Function<T> { Body = body };
        }

        /// <summary>
        /// Generate a string representation of the function.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            //return "f[" + Body.nextVar + "] = " + Body.ToString();
            return Body.ToString();
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
        static int HighestBit(uint value)
        {
            value = Fold(value);
            return unchecked((int)(value ^ (value >> 1)));
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
            if ((left.varMask | right.varMask) != left.varMask)
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
        Add, Sub, Mul, Div, Pow, Const, Var
    }

    /// <summary>
    /// A variable used to define an term.
    /// </summary>
    public class Variable : Term
    {
        internal short index;
        internal string name;

        //FIXME: 1 + index just checks the max variable used, it doesn't ensure that all variables are used
        //Perhaps use a 64-bit bitmap supporting a max of 64 variables?

        internal Variable(string name, short index)
            : base(TermType.Var, 1 << index, 1)
        {
            this.name = name;
            this.index = index;
        }

        internal protected override void Compile(ILGenerator il)
        {
            il.Emit(OpCodes.Ldarg, index);
        }
        protected internal override Term Subsitute(Term[] subs)
        {
            return subs[index];
        }
        protected internal override bool TryUnify(Term e, Term[] bindings)
        {
            if (ReferenceEquals(bindings[index], e) || ReferenceEquals(bindings[index], null) || bindings[index].Equals(e))
            {
                bindings[index] = e;
                return true;
            }
            else
            {
                return false;
            }
        }
        public override bool Equals(Term other)
        {
            return other.type == TermType.Var && (other as Variable).index == index;
        }
        public override string ToString()
        {
            return name;
        }
    }

    /// <summary>
    /// A numerical term.
    /// </summary>
    public abstract class Term : IEquatable<Term>
    {
        protected internal TermType type;
        protected internal ushort varMask;  // a bitmask listing the variables in this term
        protected internal short nodeCount; // the total number of nodes in this term
        
        protected Term(TermType type, int varMask, short nodeCount)
        {
            this.type = type;
            this.varMask = (ushort)varMask;
            this.nodeCount = nodeCount;
        }

        sealed class Binary : Term
        {
            internal Term left;
            internal Term right;
            static MethodInfo pow = typeof(Math).GetMethod("Pow", new[] { typeof(double), typeof(double) });

            public Binary(TermType type, Term left, Term right)
                : base(type, left.varMask | right.varMask, (short)(1 + left.nodeCount + right.nodeCount))
            {
                this.left = left;
                this.right = right;
            }

            internal protected override void Compile(ILGenerator il)
            {
                left.Compile(il);
                right.Compile(il);
                switch (type)
                {
                    case TermType.Add: il.Emit(OpCodes.Add); break;
                    case TermType.Div: il.Emit(OpCodes.Div); break;
                    case TermType.Pow: il.Emit(OpCodes.Call, pow); break;
                    case TermType.Mul: il.Emit(OpCodes.Mul); break;
                    case TermType.Sub: il.Emit(OpCodes.Sub); break;
                    default:
                        throw new NotSupportedException("Unknown operator: " + type);
                }
            }

            protected internal override Term Subsitute(Term[] subs)
            {
                var nleft = left.Subsitute(subs);
                var nright = right.Subsitute(subs);
                return nleft.Operation(type, nright);
            }

            protected internal override bool TryUnify(Term e, Term[] bindings)
            {
                if (e.type != type) return false;
                var eb = e as Binary;
                return left.TryUnify(eb.left, bindings) && right.TryUnify(eb.right, bindings);
            }

            public override bool Equals(Term other)
            {
                if (type != other.type) return false;
                var x = other as Binary;
                return left.Equals(x.left) && right.Equals(x.right);
            }

            public override string ToString()
            {
                char op;
                switch (type)
                {
                    case TermType.Add: op = '+'; break;
                    case TermType.Div: op = '/'; break;
                    case TermType.Mul: op = '*'; break;
                    case TermType.Pow: op = '^'; break;
                    case TermType.Sub: op = '-'; break;
                    default:
                        throw new NotSupportedException("Unknown type: " + type);
                }
                return '(' + left.ToString() + ' ' + op + (type == TermType.Pow ? " (" + right.ToString() + "))" : ' ' + right.ToString() + ')');
            }
        }

        sealed class Const : Term
        {
            internal double value;
            public Const(double value) : base(TermType.Const, 0, 1)
            {
                this.value = value;
            }
            internal protected override void Compile(ILGenerator il)
            {
                il.Emit(OpCodes.Ldc_R8, value);
            }
            protected internal override Term Subsitute(Term[] subs)
            {
                return this;
            }
            protected internal override bool TryUnify(Term e, Term[] bindings)
            {
                return e.type == TermType.Const && value == (e as Const).value;
            }
            public override bool Equals(Term other)
            {
                return other.type == TermType.Const && (other as Const).value == value;
            }
            public override string ToString()
            {
                return value.ToString();
            }
        }

        #region Internal operations
        /// <summary>
        /// Compile the instructions to CIL.
        /// </summary>
        /// <param name="il"></param>
        internal protected abstract void Compile(ILGenerator il);

        /// <summary>
        /// Replace the existing term with the given bindings.
        /// </summary>
        /// <param name="bindings"></param>
        /// <returns></returns>
        internal protected abstract Term Subsitute(Term[] bindings);

        /// <summary>
        /// Attempt to unify this term with the given term.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="bindings"></param>
        /// <returns></returns>
        internal protected abstract bool TryUnify(Term e, Term[] bindings);

        /// <summary>
        /// Rewrite a term given a set of identities and a maximum number of rounds per-node.
        /// </summary>
        internal protected static Term Rewrite(Term current, int rounds, Identity[] equalities, Term[] bindings)
        {
            Term last;
            do
            {
                last = current;
                switch (current.type)
                {
                    case TermType.Add:
                    case TermType.Div:
                    case TermType.Mul:
                    case TermType.Pow:
                    case TermType.Sub:
                        var bin = current as Binary;
                        var nleft = Rewrite(bin.left, rounds, equalities, bindings);
                        var nright = Rewrite(bin.right, rounds, equalities, bindings);
                        current = ReferenceEquals(nleft, bin.left) && ReferenceEquals(nright, bin.right)
                                 ? current
                                 : nleft.Operation(bin.type, nright);
                        // now that sub-terms have been rewritten, try rewriting this term
                        goto case TermType.Var;
                    case TermType.Var:
                    case TermType.Const:
                        foreach (var e in equalities)
                        {
                            Array.Clear(bindings, 0, bindings.Length);
                            if (e.left.TryUnify(current, bindings))
                                current = e.right.Subsitute(bindings);
                        }
                        break;
                    default:
                        throw new NotSupportedException("Unknown term type: " + current.type);
                }
            } while (--rounds > 0 && !ReferenceEquals(last, current));
            return current;
        }
        #endregion

        /// <summary>
        /// Equality on terms.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public abstract bool Equals(Term other);

        /// <summary>
        /// Create a binary operation for two terms.
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public Term Operation(TermType operation, Term right)
        {
            switch (operation)
            {
                case TermType.Add: return this + right;
                case TermType.Div: return this / right;
                case TermType.Mul: return this * right;
                case TermType.Pow: return this.Pow(right);
                case TermType.Sub: return this - right;
                default:
                    throw new NotSupportedException("Unknown binary operation: " + type);
            }
        }

        /// <summary>
        /// Add two terms.
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public Term Add(Term right)
        {
            return type == TermType.Const && right.type == TermType.Const
                 ? (this as Const).value + (right as Const).value
                 : new Binary(TermType.Add, this, right) as Term;
        }

        /// <summary>
        /// Subtract two terms.
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public Term Subtract(Term right)
        {
            return type == TermType.Const && right.type == TermType.Const
                 ? (this as Const).value - (right as Const).value
                 : new Binary(TermType.Sub, this, right) as Term;
        }

        /// <summary>
        /// Multiply two terms.
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public Term Multiply(Term right)
        {
            return type == TermType.Const && right.type == TermType.Const
                 ? (this as Const).value * (right as Const).value
                 : new Binary(TermType.Mul, this, right) as Term;
        }
        
        /// <summary>
        /// Divide two terms.
        /// </summary>
        /// <param name="denominator"></param>
        /// <returns></returns>
        public Term Divide(Term denominator)
        {
            return type == TermType.Const && denominator.type == TermType.Const
                 ? (this as Const).value / (denominator as Const).value
                 : new Binary(TermType.Div, this, denominator) as Term;
        }

        /// <summary>
        /// Raise the current term to the power of <paramref name="exponent"/>.
        /// </summary>
        /// <param name="exponent"></param>
        /// <returns></returns>
        public Term Pow(Term exponent)
        {
            return type == TermType.Const && exponent.type == TermType.Const
                 ? Math.Pow((this as Const).value, (exponent as Const).value)
                 : new Binary(TermType.Pow, this, exponent) as Term;
        }
        
        /// <summary>
        /// Negate the current term.
        /// </summary>
        /// <returns></returns>
        public Term Negate()
        {
            return type == TermType.Const ? -(this as Const).value : new Const(0) - this;
        }
        
        /// <summary>
        /// Construct an term encapsulating a constant.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Term Constant(double value)
        {
            return new Const(value);
        }

        /// <summary>
        /// Generate a string representation of the term.
        /// </summary>
        /// <returns></returns>
        public abstract override string ToString();

        #region Operators
        /// <summary>
        /// Create an term encapsulating a double.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator Term(double value)
        {
            return Constant(value);
        }
        
        /// <summary>
        /// Add two terms.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Term operator +(Term left, Term right)
        {
            return left.Add(right);
        }

        /// <summary>
        /// Subtract two terms.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Term operator -(Term left, Term right)
        {
            return left.Subtract(right);
        }

        /// <summary>
        /// Negate an term.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Term operator -(Term x)
        {
            return x.Negate();
        }

        /// <summary>
        /// Multiply two terms.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Term operator *(Term left, Term right)
        {
            return left.Multiply(right);
        }

        /// <summary>
        /// Divide two terms.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Term operator /(Term left, Term right)
        {
            return left.Divide(right);
        }

        /// <summary>
        /// Generate an identity between two terms.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Identity operator ==(Term left, Term right)
        {
            return new Identity(left, right);
        }

        /// <summary>
        /// ERROR: inequality is not supported.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Identity operator !=(Term left, Term right)
        {
            throw new NotSupportedException("Inequalities not supported.");
        }
        #endregion
    }
}
