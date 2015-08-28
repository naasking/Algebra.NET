using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Reflection.Emit;

namespace AlgebraDotNet
{
    /// <summary>
    /// A term in an expression.
    /// </summary>
    public struct Term : IEquatable<Term>
    {
        TermType type;
        Info info;
        Term[] children;

        /// <summary>
        /// Construct a compound term.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="children"></param>
        internal Term(TermType type, params Term[] children)
        {
            this.type = type;
            this.info = new Info { NodeCount = 1, };
            foreach (var x in children)
            {
                info.NodeCount += x.NodeCount;
                info.VMask |= x.VMask;
            }
            this.children = children;
        }

        /// <summary>
        /// Construct a term variable.
        /// </summary>
        /// <param name="variable"></param>
        internal Term(int variable)
            : this()
        {
            this.type = TermType.Var;
            this.info = new Info { Variable = variable, VMask = 1 << variable };
        }

        /// <summary>
        /// Construct a constant term.
        /// </summary>
        /// <param name="value"></param>
        public Term(double value)
            : this()
        {
            this.type = TermType.Const;
            this.info = new Info { Value = value };
        }

        /// <summary>
        /// The list of variables as bit positions.
        /// </summary>
        internal int VMask
        {
            get { return type == TermType.Const ? 0 : info.VMask; }
        }

        /// <summary>
        /// The number of nodes in this term, up to 2^16.
        /// </summary>
        internal int NodeCount
        {
            get
            {
                switch (type)
                {
                    case TermType.Var: return 1;
                    case TermType.Const: return 0;
                    default: return info.NodeCount;
                }
            }
        }

        /// <summary>
        /// Compare terms for syntactic equality.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Term other)
        {
            return type == other.type
                && info.Value == other.info.Value
                && (children == other.children || children.SequenceEqual(other.children));
        }

        static string[] empty = Enumerable.Repeat("x", 32).Select((x, i) => x + i).ToArray();

        /// <summary>
        /// Pretty print a term.
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        public StringBuilder Print(StringBuilder sb, params string[] variables)
        {
            char op;
            switch (type)
            {
                case TermType.Const: return sb.Append(info.Value);
                case TermType.Var: return sb.Append(variables[info.Variable]);
                case TermType.Add: op = '+'; break;
                case TermType.Div: op = '/'; break;
                case TermType.Mul: op = '*'; break;
                case TermType.Pow: op = '^'; break;
                case TermType.Sub: op = '-'; break;
                default:
                    throw new NotSupportedException("Unknown term type: " + type);
            }
            sb.Append('(');
            foreach (var x in children) x.Print(sb, variables).Append(' ').Append(op).Append(' ');
            return sb.Remove(sb.Length - 3, 3).Append(')');
        }

        /// <summary>
        /// Generate a string representation of a term.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Print(new StringBuilder(), empty).ToString();
        }

        #region Rewriting and Compilation
        bool TryMatch(ref Term e, Term[] bindings)
        {
            switch (type)
            {
                case TermType.Var:
                    if (!bindings[info.Variable].Equals(e) && !bindings[info.Variable].Equals(default(Term)) && !bindings[info.Variable].Equals(e))
                        return false;
                    bindings[info.Variable] = e;
                    return true;
                case TermType.Const:
                    return e.type == TermType.Const && info.Value == e.info.Value;
                case TermType.Add:
                case TermType.Div:
                case TermType.Mul:
                case TermType.Pow:
                case TermType.Sub:
                    if (e.type != type || children.Length != e.children.Length) return false;
                    for (int i = 0; i < children.Length; ++i)
                    {
                        if (!children[i].TryMatch(ref e.children[i], bindings))
                            return false;
                    }
                    return true;
                default:
                    throw new NotSupportedException("Unknown term type: " + type);
            }
        }

        Term Substitute(Term[] subs)
        {
            switch (type)
            {
                case TermType.Var:
                    return subs[info.Variable];
                case TermType.Const:
                    return this;
                case TermType.Add:
                case TermType.Div:
                case TermType.Mul:
                case TermType.Pow:
                case TermType.Sub:
                    //FIXME: generalize to n children, which supports operator associativity?
                    //var nchild = new Term[children.Length];
                    //for (int i = 0; i < children.Length; ++i)
                    //{
                    //    nchild[i] = children[i].Substitute(subs);
                    //}
                    //return Nary(type, nchild);
                    return children[0].Substitute(subs)
                                      .Binary(type, children[1].Substitute(subs));
                default:
                    throw new NotSupportedException("Unknown term type: " + type);
            }
        }

        /// <summary>
        /// Rewrite a term given a set of identities and a maximum number of rounds per-node.
        /// </summary>
        internal static Term Rewrite(Term current, int rounds, Identity[] equalities, Term[] bindings)
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
                        var nleft = Rewrite(current.children[0], rounds, equalities, bindings);
                        var nright = Rewrite(current.children[1], rounds, equalities, bindings);
                        if (!nleft.Equals(current.children[0]) || !nright.Equals(current.children[1]))
                            current = nleft.Binary(current.type, nright);
                        // now that sub-terms have been rewritten, try rewriting this term
                        goto case TermType.Var;
                    case TermType.Var:
                    case TermType.Const:
                        foreach (var e in equalities)
                        {
                            Array.Clear(bindings, 0, bindings.Length);
                            if (e.left.TryMatch(ref current, bindings))
                                current = e.right.Substitute(bindings);
                        }
                        break;
                    default:
                        throw new NotSupportedException("Unknown term type: " + current.type);
                }
            } while (--rounds > 0 && !current.Equals(last));
            return current;
        }

        static MethodInfo pow = typeof(Math).GetMethod("Pow", new[] { typeof(double), typeof(double) });

        internal void Compile(ILGenerator il)
        {
            OpCode op;
            MethodInfo call = null;
            switch (type)
            {
                case TermType.Const: il.Emit(OpCodes.Ldc_R8, info.Value); return;
                case TermType.Var: il.Emit(OpCodes.Ldarg, info.Variable); return;
                case TermType.Add: op = OpCodes.Add; break;
                case TermType.Div: op = OpCodes.Div; break;
                case TermType.Pow: op = OpCodes.Call; call = pow; break;
                case TermType.Mul: op = OpCodes.Mul; break;
                case TermType.Sub: op = OpCodes.Sub; break;
                default:
                    throw new NotSupportedException("Unknown operator: " + type);
            }
            children[0].Compile(il);
            for (int i = 1; i < children.Length; ++i)
            {
                children[1].Compile(il);
                if (op == OpCodes.Call)
                    il.Emit(op, call);
                else
                    il.Emit(op);
            }
        }
        #endregion

        #region Operations
        /// <summary>
        /// Create an N-ary operation on N terms.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="children"></param>
        /// <returns></returns>
        public static Term Nary(TermType type, params Term[] children)
        {
            if (children.Length != 2) throw new ArgumentException("Only two children currently supported.");
            return children[0].Binary(type, children[1]);
        }
        
        /// <summary>
        /// Create a binary operation on two terms.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public Term Binary(TermType type, Term right)
        {
            switch (type)
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
                 ? info.Value + right.info.Value
                 : new Term(TermType.Add, this, right);
        }

        /// <summary>
        /// Subtract two terms.
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public Term Subtract(Term right)
        {
            return type == TermType.Const && right.type == TermType.Const
                 ? info.Value - right.info.Value
                 : new Term(TermType.Sub, this, right);
        }

        /// <summary>
        /// Multiply two terms.
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public Term Multiply(Term right)
        {
            return type == TermType.Const && right.type == TermType.Const
                 ? info.Value * right.info.Value
                 : new Term(TermType.Mul, this, right);
        }

        /// <summary>
        /// Divide two terms.
        /// </summary>
        /// <param name="denominator"></param>
        /// <returns></returns>
        public Term Divide(Term denominator)
        {
            return type == TermType.Const && denominator.type == TermType.Const
                 ? info.Value / denominator.info.Value
                 : new Term(TermType.Div, NodeCount + denominator.NodeCount, VMask | denominator.VMask, this, denominator);
        }

        /// <summary>
        /// Negate the current term.
        /// </summary>
        /// <returns></returns>
        public Term Negate()
        {
            return type == TermType.Const ? -info.Value : new Term(0.0) - this;
        }

        /// <summary>
        /// Raise the current term to the power of <paramref name="exponent"/>.
        /// </summary>
        /// <param name="exponent"></param>
        /// <returns></returns>
        public Term Pow(Term exponent)
        {
            return type == TermType.Const && exponent.type == TermType.Const
                 ? Math.Pow(info.Value, exponent.info.Value)
                 : new Term(TermType.Pow, this, exponent);
        }

        /// <summary>
        /// Construct a term for a constant.
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Term Constant(double value)
        {
            return value;
        }
        #endregion

        #region Operators
        /// <summary>
        /// Create an term encapsulating a double.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator Term(double value)
        {
            return new Term(value);
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

        /// <summary>
        /// This is a disjoint union describing the payloads of the term types:
        /// 1. TermType.Var   => Info.Variable
        /// 2. TermType.Const => Info.Value
        /// 3. TermType.*     => Info.NodeCount + Info.VMask
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        struct Info
        {
            [FieldOffset(0)] public int Variable;   // the variable number
            [FieldOffset(0)] public int NodeCount;  // the number of nodes in this term
            [FieldOffset(4)] public int VMask;      // the mask listing this term's variables
            [FieldOffset(0)] public double Value;   // the floating point value
        }
    }
}
