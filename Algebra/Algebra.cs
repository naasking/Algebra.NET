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
    public static class Algebra
    {
        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Function<Func<double, double>> Function(Func<Variable, Function<Func<double, double>>> body)
        {
            var p = body.Method.GetParameters();
            var x = new Variable(p[0].Name, 0);
            return body(x);
        }

        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Function<Func<double, double, double>> Function(Func<Variable, Variable, Function<Func<double, double, double>>> body)
        {
            var p = body.Method.GetParameters();
            var x = new Variable(p[0].Name, 0);
            var y = new Variable(p[1].Name, 1);
            return body(x, y);
        }

        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Function<Func<double, double, double, double>> Function(Func<Variable, Variable, Variable, Function<Func<double, double, double, double>>> body)
        {
            var p = body.Method.GetParameters();
            var x = new Variable(p[0].Name, 0);
            var y = new Variable(p[1].Name, 1);
            var z = new Variable(p[2].Name, 2);
            return body(x, y, z);
        }

        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Function<Func<double, double, double, double, double>> Function(Func<Variable, Variable, Variable, Variable, Function<Func<double, double, double, double, double>>> body)
        {
            var p = body.Method.GetParameters();
            var x = new Variable(p[0].Name, 0);
            var y = new Variable(p[1].Name, 1);
            var z = new Variable(p[2].Name, 2);
            var h = new Variable(p[3].Name, 3);
            return body(x, y, z, h);
        }

        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Identity Identity(Func<Variable, Identity> body)
        {
            var p = body.Method.GetParameters();
            var x = new Variable(p[0].Name, 0);
            return body(x);
        }

        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Identity Identity(Func<Variable, Variable, Identity> body)
        {
            var p = body.Method.GetParameters();
            var x = new Variable(p[0].Name, 0);
            var y = new Variable(p[1].Name, 1);
            return body(x, y);
        }

        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Identity Identity(Func<Variable, Variable, Variable, Identity> body)
        {
            var p = body.Method.GetParameters();
            var x = new Variable(p[0].Name, 0);
            var y = new Variable(p[1].Name, 1);
            var z = new Variable(p[2].Name, 2);
            return body(x, y, z);
        }

        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Identity Equality(Func<Variable, Variable, Variable, Identity> body)
        {
            var p = body.Method.GetParameters();
            var x = new Variable(p[0].Name, 0);
            var y = new Variable(p[1].Name, 1);
            var z = new Variable(p[2].Name, 2);
            return body(x, y, z);
        }

        /// <summary>
        /// Define a function.
        /// </summary>
        /// <param name="body">The function body.</param>
        /// <returns>A function.</returns>
        public static Identity Equality(Func<Variable, Variable, Variable, Variable, Identity> body)
        {
            var p = body.Method.GetParameters();
            var x = new Variable(p[0].Name, 0);
            var y = new Variable(p[1].Name, 1);
            var z = new Variable(p[2].Name, 2);
            var h = new Variable(p[3].Name, 3);
            return body(x, y, z, h);
        }

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
            var signature = typeof(T).GetGenericArguments().Take(Body.nextVar).ToArray();
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
            // rewriting
            // continue looping until we reach a fixed point, ie. term of last loop == this loop's term
            Term last, current = Body;
            // allocate enough space for all possible variables
            var bindings = new Term[Math.Max(Body.nextVar, equalities.Max(x => (int?)x.left.nextVar) ?? 0)];
            do
            {
                // apply every equality to rewrite the term
                last = current;
                foreach (var e in equalities)
                {
                    Array.Clear(bindings, 0, bindings.Length);
                    current = current.Rewrite(e, bindings);
                }
            } while (--rounds > 0 && !ReferenceEquals(last, current));
            return current;
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
            if (left.nextVar != right.nextVar)
                throw new ArgumentException("The number of variables in each term must be equal.");
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
        internal string name;
        internal int index;

        //FIXME: 1 + index just checks the max variable used, it doesn't ensure that all variables are used
        //Perhaps use a 64-bit bitmap supporting a max of 64 variables?
        internal Variable(string name, int index)
            : base(TermType.Var, 1 + index)
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
        protected internal int nextVar;

        protected Term(TermType type, int nextVar)
        {
            this.type = type;
            this.nextVar = nextVar;
        }

        sealed class Binary : Term
        {
            internal Term left;
            internal Term right;
            static MethodInfo pow = typeof(Math).GetMethod("Pow", new[] { typeof(double), typeof(double) });

            public Binary(TermType type, Term left, Term right)
                : base(type, Math.Max(left.nextVar, right.nextVar))
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
                switch (type)
                {
                    case TermType.Add: return nleft + nright;
                    case TermType.Div: return nleft / nright;
                    case TermType.Mul: return nleft * nright;
                    case TermType.Pow: return nleft.Pow(nright);
                    case TermType.Sub: return nleft - nright;
                    default:
                        throw new NotSupportedException("Unknown binary operation: " + type);
                }
            }

            protected internal override bool TryUnify(Term e, Term[] bindings)
            {
                if (e.type != type) return false;
                var eb = e as Binary;
                return left.TryUnify(eb.left, bindings) && right.TryUnify(eb.right, bindings);
            }

            protected internal override Term Rewrite(Identity e, Term[] bindings)
            {
                var nleft = left.Rewrite(e, bindings);
                var nright = right.Rewrite(e, bindings);
                return ReferenceEquals(nleft, left) && ReferenceEquals(nright, right)
                     ? base.Rewrite(e, bindings)
                     : new Binary(type, nleft, nright).Rewrite(e, bindings);
                //Binary x = this, last;
                //do
                //{
                //    last = x;
                //    var nleft = left.Rewrite(e, bindings);
                //    var nright = right.Rewrite(e, bindings);
                //    if (ReferenceEquals(nleft, left) && ReferenceEquals(nright, right))
                //        return base.Rewrite(e, bindings);
                //    var tmp = new Binary(type, nleft, nright).Rewrite(e, bindings);
                //    x = tmp as Binary;
                //    if (ReferenceEquals(x, null)) return tmp;
                //} while (!ReferenceEquals(x, last));
                //return x;
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
            public Const(double value) : base(TermType.Const, 0)
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
        /// Rewrite the current term with the given equality.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="bindings"></param>
        /// <returns></returns>
        internal protected virtual Term Rewrite(Identity e, Term[] bindings)
        {
            return e.left.TryUnify(this, bindings) ?  e.right.Subsitute(bindings) : this;
        }
        #endregion

        /// <summary>
        /// Equality on terms.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public abstract bool Equals(Term other);

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
