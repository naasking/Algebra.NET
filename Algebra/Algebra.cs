using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;

namespace Newton
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
        public static Expression Pow(this double x, double exponent)
        {
            return Expression.Constant(x).Pow(exponent);
        }
    }

    /// <summary>
    /// A function describing a numerical expression.
    /// </summary>
    /// <typeparam name="T">The type of the function.</typeparam>
    public struct Function<T> : IEquatable<Function<T>>
        where T : class
    {
        internal Expression Body { get; set; }

        /// <summary>
        /// Compile the numerical function into a delegate.
        /// </summary>
        /// <param name="name">The internal delegate name.</param>
        /// <returns></returns>
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
            // continue looping until we reach a fixed point, ie. expression of last loop == this loop expression
            Expression last, current = Body;
            // allocate enough space for all possible variables
            var bindings = new Expression[Math.Max(Body.nextVar, equalities.Max(x => (int?)x.left.nextVar) ?? 0)];
            do
            {
                // apply every equality to rewrite the term
                last = current;
                foreach (var e in equalities)
                {
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
        /// Implicit convert an expression to the expected function type.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public static implicit operator Function<T>(Expression body)
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
    /// Defines an equality between two expressions.
    /// </summary>
    public struct Identity
    {
        internal Expression left;
        internal Expression right;
        public Identity(Expression left, Expression right)
        {
            if (left.nextVar != right.nextVar)
                throw new ArgumentException("The number of variables in each expression must be equal.");
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
    /// The expression node type.
    /// </summary>
    public enum NodeType
    {
        Add, Sub, Mul, Div, Pow, Neg, Const, Var
    }

    /// <summary>
    /// A variable used to define an expression.
    /// </summary>
    public class Variable : Expression
    {
        internal string name;
        internal int index;

        //FIXME: 1 + index just checks the max variable used, it doesn't ensure that all variables are used
        //Perhaps use a 64-bit bitmap supporting a max of 64 variables?
        internal Variable(string name, int index)
            : base(NodeType.Var, 1 + index)
        {
            this.name = name;
            this.index = index;
        }

        internal protected override void Compile(ILGenerator il)
        {
            il.Emit(OpCodes.Ldarg, index);
        }
        protected internal override Expression Subsitute(Expression[] subs)
        {
            return subs[index];
        }
        protected internal override bool TryUnify(Expression e, Expression[] bindings)
        {
            bindings[index] = e;
            return true;
        }
        public override string ToString()
        {
            return name;
        }
    }

    /// <summary>
    /// A numerical expression.
    /// </summary>
    public abstract class Expression
    {
        protected internal NodeType type;
        protected internal int nextVar;

        protected Expression(NodeType type, int nextVar)
        {
            this.type = type;
            this.nextVar = nextVar;
        }

        sealed class Binary : Expression
        {
            internal Expression left;
            internal Expression right;
            static MethodInfo pow = typeof(Math).GetMethod("Pow", new[] { typeof(double), typeof(double) });

            public Binary(NodeType type, Expression left, Expression right)
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
                    case NodeType.Add: il.Emit(OpCodes.Add); break;
                    case NodeType.Div: il.Emit(OpCodes.Div); break;
                    case NodeType.Pow: il.Emit(OpCodes.Call, pow); break;
                    case NodeType.Mul: il.Emit(OpCodes.Mul); break;
                    case NodeType.Sub: il.Emit(OpCodes.Sub); break;
                    default:
                        throw new NotSupportedException("Unknown operator: " + type);
                }
            }

            protected internal override Expression Subsitute(Expression[] subs)
            {
                return new Binary(type, left.Subsitute(subs), right.Subsitute(subs));
            }

            protected internal override bool TryUnify(Expression e, Expression[] bindings)
            {
                if (e.type != type) return false;
                var eb = e as Binary;
                return left.TryUnify(eb.left, bindings) && right.TryUnify(eb.right, bindings);
            }

            protected internal override Expression Rewrite(Identity e, Expression[] bindings)
            {
                var nleft = left.Rewrite(e, bindings);
                var nright = right.Rewrite(e, bindings);
                return ReferenceEquals(nleft, left) && ReferenceEquals(nright, right)
                     ? base.Rewrite(e, bindings)
                     : new Binary(type, nleft, nright).Rewrite(e, bindings);
            }

            public override string ToString()
            {
                char op;
                switch (type)
                {
                    case NodeType.Add: op = '+'; break;
                    case NodeType.Div: op = '/'; break;
                    case NodeType.Mul: op = '*'; break;
                    case NodeType.Pow: op = '^'; break;
                    case NodeType.Sub: op = '-'; break;
                    default:
                        throw new NotSupportedException("Unknown type: " + type);
                }
                return '(' + left.ToString() + ' ' + op + (type == NodeType.Pow ? " (" + right.ToString() + "))" : ' ' + right.ToString() + ')');
            }
        }

        sealed class Const : Expression
        {
            internal double value;
            public Const(double value) : base(NodeType.Const, 0)
            {
                this.value = value;
            }
            internal protected override void Compile(ILGenerator il)
            {
                il.Emit(OpCodes.Ldc_R8, value);
            }
            protected internal override Expression Subsitute(Expression[] subs)
            {
                return this;
            }
            protected internal override bool TryUnify(Expression e, Expression[] bindings)
            {
                return e.type == NodeType.Const && value == (e as Const).value;
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
        /// Replace the existing expression with the given bindings.
        /// </summary>
        /// <param name="bindings"></param>
        /// <returns></returns>
        internal protected abstract Expression Subsitute(Expression[] bindings);

        /// <summary>
        /// Attempt to unify this expression with the given expression.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="bindings"></param>
        /// <returns></returns>
        internal protected abstract bool TryUnify(Expression e, Expression[] bindings);

        /// <summary>
        /// Rewrite the current expression with the given equality.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="bindings"></param>
        /// <returns></returns>
        internal protected virtual Expression Rewrite(Identity e, Expression[] bindings)
        {
            return e.left.TryUnify(this, bindings) ?  e.right.Subsitute(bindings) : this;
        }
        #endregion

        /// <summary>
        /// Add two expressions.
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public Expression Add(Expression right)
        {
            return new Binary(NodeType.Add, this, right);
        }

        /// <summary>
        /// Subtract two expressions.
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public Expression Subtract(Expression right)
        {
            return new Binary(NodeType.Sub, this, right);
        }

        /// <summary>
        /// Multiply two expressions.
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public Expression Multiply(Expression right)
        {
            return new Binary(NodeType.Mul, this, right);
        }
        
        /// <summary>
        /// Divide two expressions.
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public Expression Divide(Expression right)
        {
            return new Binary(NodeType.Div, this, right);
        }

        /// <summary>
        /// Raise the current expression to the power of <paramref name="exponent"/>.
        /// </summary>
        /// <param name="exponent"></param>
        /// <returns></returns>
        public Expression Pow(Expression exponent)
        {
            return new Binary(NodeType.Pow, this, exponent);
        }
        
        /// <summary>
        /// Negate the current expression.
        /// </summary>
        /// <returns></returns>
        public virtual Expression Negate()
        {
            return new Const(0) - this;
        }
        
        /// <summary>
        /// Construct an expression encapsulating a constant.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Expression Constant(double value)
        {
            return new Const(value);
        }

        /// <summary>
        /// Generate a string representation of the expression.
        /// </summary>
        /// <returns></returns>
        public abstract override string ToString();

        #region Operators
        /// <summary>
        /// Create an expression encapsulating a double.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator Expression(double value)
        {
            return Constant(value);
        }
        
        /// <summary>
        /// Add two expressions.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Expression operator +(Expression left, Expression right)
        {
            return left.Add(right);
        }

        /// <summary>
        /// Subtract two expressions.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Expression operator -(Expression left, Expression right)
        {
            return left.Subtract(right);
        }

        /// <summary>
        /// Negate an expression.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Expression operator -(Expression x)
        {
            return x.Negate();
        }

        /// <summary>
        /// Multiply two expressions.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Expression operator *(Expression left, Expression right)
        {
            return left.Multiply(right);
        }

        /// <summary>
        /// Divide two expressions.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Expression operator /(Expression left, Expression right)
        {
            return left.Divide(right);
        }

        /// <summary>
        /// Generate an identity between two expressions.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Identity operator ==(Expression left, Expression right)
        {
            return new Identity(left, right);
        }

        /// <summary>
        /// ERROR: inequality is not supported.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Identity operator !=(Expression left, Expression right)
        {
            throw new NotSupportedException("Inequalities not supported.");
        }
        #endregion
    }
}
